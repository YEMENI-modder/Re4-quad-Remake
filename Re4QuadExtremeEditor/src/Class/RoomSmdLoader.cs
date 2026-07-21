using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using OpenTK;
using SHARED_UHD_BIN.ALL;
using SHARED_UHD_BIN.EXTRACT;
using SHARED_UHD_SCENARIO_SMD.SCENARIO;
using Re4QuadExtremeEditor.src.Class.SMX;

namespace Re4QuadExtremeEditor.src.Class
{
    /// <summary>
    /// Loads a room's 3D model from a .SMD file (RE4 UHD "NewAge" format) and its matching
    /// ImagePackHD .pack texture pack, producing the same Room.MeshData layout used by the
    /// existing Xcar/PMD pipeline so it can be rendered with the same shader/GL code.
    ///
    /// This intentionally skips everything the NewAge reference project uses for its editor
    /// Tree/Node/Responsibility system (node registration, dictionaries of live ModelGroup
    /// objects, etc.) since SMD/SMX entries are never shown in this editor's Tree — this is
    /// extraction + mesh conversion only. The actual SMD/SMX/material/render-order LOGIC is
    /// ported in full (see SMX_LOADING and FaceCulling/RenderOrder sections below); only the
    /// editor-node bookkeeping around it is omitted, since it doesn't apply here.
    ///
    /// IMPORTANT (matches NewAge's ObjModelShaderVert.vert order exactly):
    /// Each .BIN is decoded ONCE into local-space "base" meshes (one per material, using
    /// NewAge's own cumulative baseIndex scheme, not a per-material min/max window).
    /// Then for every SMDLine ("placement") that references that BIN, the base mesh's
    /// vertices are transformed as:
    ///     worldPos = ((localPos * Scale) * Rotation) + Position
    /// i.e. scale first, then rotate, then translate — NOT rotate-then-translate as an
    /// earlier version of this loader did (which also omitted Scale entirely, causing
    /// misplaced/doubled/missing objects).
    ///
    /// SHADOW / BAKED-DARKNESS / "REFLECTIVE HOLE COVER" HANDLING:
    /// Confirmed against NewAge's own source AND by testing against real room data. TWO
    /// independent mechanisms gate a fragment's alpha, PLUS one texture-binding fix:
    ///   1) opacity_map alpha (material_flag & 0x04) — a per-MATERIAL alpha map; see
    ///      RoomShaderFrag.frag and the material_flag check in BuildBaseMeshes / AlphaTexKey.
    ///   2) Vertex color (HeaderExtension.ReturnsIsEnableVertexColors, gated by
    ///      header.texture2_flags & 0x4000) — a per-VERTEX RGBA color baked directly into the
    ///      BIN's Vertex_Color_Array (byte order is a,r,g,b). NewAge's RoomShaderFrag.frag
    ///      multiplies this FULL color (not just alpha) into the final result
    ///      (texColor * matColor * color). Finer-grained than (1): tints/fades individual
    ///      vertices/parts of a mesh.
    ///   3) Failed-diffuse-texture-load fix: some materials have a diffuse_map that resolves to a
    ///      technically-valid TPL slot, but the pack ships no actual bitmap for it (confirmed via
    ///      LoadUhdPackCustomQuad.LoadPack: an entry can have a nonzero offset but fail the
    ///      DDS/TGA magic-number check, e.g. because the real game draws it with an environment/
    ///      reflection map instead of a plain diffuse texture — hence no ordinary texture data).
    ///      Confirmed against NewAge's RoomSelectedObj.Render(): it binds DataShader.WhiteTexture
    ///      (OPAQUE white) to texture0 by default and only overwrites it if the real texture is
    ///      actually found in TextureRefDic — so a failed-to-load diffuse texture renders as an
    ///      opaque surface in NewAge, NEVER as transparent. The bug that caused these objects to
    ///      look like a "reflective hole cover" (a flat surface showing whatever fog/clear color
    ///      is configured, hiding the real geometry underneath) was passing
    ///      Consts.TransparentTextureName for these meshes, which IS fully transparent. The fix:
    ///      texturename is always set to the real TexKey (never remapped to
    ///      TransparentTextureName here), so Room.Render()'s texture lookup falls through to its
    ///      opaque DataBase.NoTextureIdGL placeholder when the bitmap is missing — matching
    ///      NewAge's opaque-fallback behavior instead of rendering transparent. Meshes with no TPL
    ///      slot at all (TexKey == null) are still skipped entirely, since there's truly nothing
    ///      to bind for those.
    ///
    /// SMX LOADING (new):
    /// SMX is fully optional. If SmxPath is null/missing, every placement gets the same neutral
    /// defaults NewAge itself falls back to for an unmatched SMX_ID (see RenderOrder.cs's
    /// "smxEmpty" sentinel): OpacityHierarchy=0, AlphaHierarchy=0, FaceCulling=OnlyFront,
    /// SmxColor=(1,1,1,1) (loaded but never applied — see below). When SmxPath IS present, it's
    /// parsed via SmxRawExtract (byte-for-byte port of NewAge's SMX/SMXextract.cs) into a
    /// SmxEntry per record, using the exact same field mapping as NewAge's LoadSMX.cs:
    ///   - FaceCulling: raw byte 1 -> OnlyBack, 2 -> FrontAndBack, anything else -> OnlyFront.
    ///   - SmxColor: ColorRGB/255, snapped to opaque white if it decodes to pure black (matches
    ///     LoadSMX.cs's explicit all-zero check) — loaded for completeness but NOT wired into the
    ///     shader, because NewAge's own RoomShaderFrag.frag never reads it either (its smxColor
    ///     uniform is commented out) — this is NewAge's real current behavior, not a gap here.
    ///
    /// RENDER ORDER (new): after all placements are instanced, the full mesh list is re-sorted
    /// via RoomRenderOrder.ToOrder — a straight port of NewAge's RenderOrder.ToOrder — using each
    /// placement's SMD_ID/SMX_ID/AlphaHierarchy/OpacityHierarchy. See RoomRenderOrder.cs for the
    /// architectural note on why NewAge's separate "NodeOrder" tail-loop doesn't apply here.
    ///
    /// FaceCulling (new): each mesh's SmxEntry.FaceCulling flows straight into
    /// Room.MeshData.faceCulling, which Room.Render() only actually applies when
    /// Room.RenderOnlyFrontFace is on (see Room.cs) — matching NewAge's own default (off) exactly.
    ///
    /// SmxID is explicitly NOT used to gate mesh VISIBILITY — confirmed by checking every use of
    /// SMX records in NewAge (RoomSelectedObj.cs, RenderOrder.cs): they only affect FaceCulling and
    /// AlphaHierarchy/OpacityHierarchy draw-order sorting, never visibility. An earlier version
    /// of this loader skipped placements with SmxID == 0, which incidentally also hid this same
    /// "reflective hole cover" content (since in practice it tends to use SMX ID 0), but has no
    /// basis in NewAge's logic and would incorrectly hide any real object using SMX ID 0. That
    /// skip, and a later (also incorrect) blanket skip of any mesh with a failed texture load,
    /// were both replaced by the more targeted fix (3) above.
    /// </summary>
    public static class RoomSmdLoader
    {
        // NOTE: unlike Xcar/PMD (which multiplies by ExtraScale=10 in Room.LoadFromXcar,
        // compensated by an unknown-to-us PMD skeleton scale factor baked into the .PMD files),
        // AEV/ETS/EMI/etc. objects in this editor are drawn via "mPosition" shader uniforms with
        // NO extra multiplier at all (see TheRender.cs RenderEtcModelETS and friends). NewAge's
        // own RoomShaderVert.vert / ObjModelShaderVert.vert also use identical scaling for both
        // the room and its objects. So SMD vertices should NOT be multiplied by ExtraScale to
        // match AEV/ETS. If sizes still look off after testing, try changing ONLY this constant
        // (e.g. 1f, 0.1f) rather than re-deriving the whole transform.
        private const float ExtraScale = 1f;
        private const float PositionScale = 100f; // same as NewAge's CONSTs.GLOBAL_POSITION_SCALE

        private class BaseMesh
        {
            public float[] LocalVertices; // x,y,z,tu,tv per vertex (local/BIN space, NOT yet placed)
            public float[] VertexColor; // r,g,b,a per vertex (0..1), from Vertex_Color_Array (1,1,1,1 if the BIN has no vertex colors)
            public float[] LocalNormals; // nx,ny,nz per vertex (local/BIN space, NOT yet rotated), CPU-normalized. (0,0,0) if the BIN has no normal data.
            public uint[] Indices;
            public string TexKey;

            // Opacity/alpha map texture key (material_flag & 0x04), or null if the material
            // has no opacity map. See the class-level comment for why this matters.
            public string AlphaTexKey;
        }

        /// <summary>
        /// Neutral SMX defaults used for every placement when no .SMX file is found, or when a
        /// placement's SMX_ID has no matching record in the loaded file. Matches NewAge's own
        /// "smxEmpty" sentinel in RenderOrder.cs exactly (OpacityHierarchy=0, AlphaHierarchy=0),
        /// plus LoadSMX.cs's own SmxEntry default for FaceCulling/SmxColor.
        /// </summary>
        private static SmxEntry DefaultSmxEntry()
        {
            return new SmxEntry
            {
                SMX_ID = -1,
                OpacityHierarchy = 0,
                AlphaHierarchy = 0,
                FaceCulling = SmxFaceCulling.OnlyFront,
                SmxColor = Vector4.One
            };
        }

        /// <summary>
        /// Parses a .SMX file into a SMX_ID-indexed dictionary, matching NewAge's LoadSMX.cs
        /// (LoadSMX.Load) field-for-field. Returns an empty dictionary (never null) if the path
        /// is null/missing/unreadable, so callers can treat "no SMX" and "empty SMX" identically.
        /// </summary>
        private static Dictionary<int, SmxEntry> LoadSmxFile(string smxPath, bool isPs2 = false)
        {
            Dictionary<int, SmxEntry> result = new Dictionary<int, SmxEntry>();

            if (string.IsNullOrEmpty(smxPath) || !File.Exists(smxPath))
            {
                return result;
            }

            List<SmxRaw> rawEntries;
            using (FileStream fs = File.OpenRead(smxPath))
            {
                List<byte[]> lines = SmxRawExtract.Extract(fs);
                rawEntries = SmxRawExtract.ToSmx(lines, isPs2);
            }

            for (int i = 0; i < rawEntries.Count; i++)
            {
                SmxRaw raw = rawEntries[i];

                SmxEntry entry = new SmxEntry
                {
                    SMX_ID = i,
                    OpacityHierarchy = raw.OpacityHierarchy,
                    AlphaHierarchy = raw.AlphaHierarchy
                };

                // Matches NewAge's LoadSMX.cs exactly: 1 -> OnlyBack, 2 -> FrontAndBack, else OnlyFront.
                if (raw.FaceCulling == 1) { entry.FaceCulling = SmxFaceCulling.OnlyBack; }
                else if (raw.FaceCulling == 2) { entry.FaceCulling = SmxFaceCulling.FrontAndBack; }
                else { entry.FaceCulling = SmxFaceCulling.OnlyFront; }

                // Matches NewAge's LoadSMX.cs exactly: ColorRGB/255, snapped to opaque white if
                // the decoded color is pure black.
                Vector4 color = new Vector4(raw.ColorRGB[0] / 255f, raw.ColorRGB[1] / 255f, raw.ColorRGB[2] / 255f, 1f);
                if (color.X == 0 && color.Y == 0 && color.Z == 0)
                {
                    color = new Vector4(1f, 1f, 1f, 1f);
                }
                entry.SmxColor = color;

                result[i] = entry;
            }

            return result;
        }

        /// <summary>
        /// Loads all meshes and textures for a room from the given .SMD file, automatically
        /// pulling in the matching ImagePackHD .pack referenced by the SMD's materials/TPL, and
        /// (optionally) the matching .SMX file for FaceCulling + AlphaHierarchy/OpacityHierarchy
        /// draw-order sorting.
        /// </summary>
        /// <param name="smdPath">Full path to the .SMD file (either the "SMD", "SMD (Data)" or "SMD (0000)" variant).</param>
        /// <param name="packPath">Full path to the room's ImagePackHD .pack file (may be null/missing).</param>
        /// <param name="smxPath">Full path to the room's .SMX file (may be null/missing — SMX is fully optional).</param>
        /// <param name="meshes">Output: meshes in Room.MeshData format (vertices/indices/texturename/faceCulling only; GL fields are left at 0 for the caller to fill in). Already sorted into NewAge's render order.</param>
        /// <param name="textures">Output: texture name -> Bitmap, keyed the same way as MeshData.texturename.</param>
        /// <param name="isPS4NS">Whether the target .SMD/.BIN uses the PS4/NS variant layout. Defaults to false (PC UHD).</param>
        public static void Load(
            string smdPath,
            string packPath,
            string smxPath,
            out List<Room.MeshData> meshes,
            out Dictionary<string, Bitmap> textures,
            bool isPS4NS = false)
        {
            meshes = new List<Room.MeshData>();
            textures = new Dictionary<string, Bitmap>();

            if (string.IsNullOrEmpty(smdPath) || !File.Exists(smdPath))
            {
                return;
            }

            Dictionary<int, UhdBIN> uhdBinDic;
            UhdTPL uhdTpl;
            SmdMagic smdMagic;
            int binAmount = 0;
            SMDLine[] smdLines;

            using (FileStream fs = File.OpenRead(smdPath))
            {
                UhdSmdExtract extract = new UhdSmdExtract();
                smdLines = extract.Extract(fs, out uhdBinDic, out uhdTpl, out smdMagic, ref binAmount, isPS4NS);
            }

            if (smdLines == null || smdLines.Length == 0 || uhdBinDic == null)
            {
                return;
            }

            // SMX is optional: an empty dictionary here means every placement falls back to
            // DefaultSmxEntry() below, with no error and no change in program behavior otherwise.
            Dictionary<int, SmxEntry> smxDic = LoadSmxFile(smxPath, isPS4NS);

            // texKey (PackID:X8/TextureID:D4) for each TPL slot index, so materials can resolve their diffuse_map.
            string[] tplTexKeys = new string[0];
            HashSet<int> neededPackIndexes = new HashSet<int>();
            if (uhdTpl != null && uhdTpl.TplArray != null)
            {
                tplTexKeys = new string[uhdTpl.TplArray.Length];
                for (int i = 0; i < uhdTpl.TplArray.Length; i++)
                {
                    var info = uhdTpl.TplArray[i];
                    tplTexKeys[i] = info.PackID.ToString("X8") + "/" + info.TextureID.ToString("D4");
                    neededPackIndexes.Add((int)info.TextureID);
                }
            }

            // load only the textures actually referenced, if we know the pack path
            if (!string.IsNullOrEmpty(packPath) && File.Exists(packPath))
            {
                var loaded = ImagePackHDLoader.LoadPack(packPath, neededPackIndexes.Count > 0 ? neededPackIndexes : null);
                foreach (var kv in loaded)
                {
                    textures[kv.Key] = kv.Value;
                }
            }

            // Step 1: decode every referenced BIN exactly once into local-space base meshes,
            // using NewAge's cumulative baseIndex scheme (LoadUhdBinModel.PopulateTreatedModel).
            Dictionary<int, List<BaseMesh>> baseMeshesByBin = new Dictionary<int, List<BaseMesh>>();

            HashSet<int> referencedBinIds = new HashSet<int>();
            foreach (SMDLine l in smdLines)
            {
                referencedBinIds.Add(l.BinID);
            }

            foreach (int binId in referencedBinIds)
            {
                if (!uhdBinDic.TryGetValue(binId, out UhdBIN uhdBin) || uhdBin == null) { continue; }
                if (uhdBin.Vertex_Position_Array == null || uhdBin.Materials == null) { continue; }

                baseMeshesByBin[binId] = BuildBaseMeshes(uhdBin, tplTexKeys);
            }

            // Step 2: for every placement (SMDLine), instance the BIN's base meshes at that
            // line's Position/Angle/Scale, matching ObjModelShaderVert.vert:
            //   temp1 = pos * scale
            //   temp2 = temp1 * rotation
            //   temp3 = temp2 + position
            //
            // Baked shadow/dark/hole-cover meshes are hidden purely via opacity_map alpha
            // (material_flag & 0x04), handled per-mesh in the shader — see MeshData.alphaTexturename
            // below. Confirmed against NewAge's own source that SmxID never gates whether a
            // placement is rendered (SMX records only affect FaceCulling and draw-order sorting),
            // so every placement is instanced here regardless of its SmxID.
            List<RoomRenderOrder.PlacementInfo> placementInfos = new List<RoomRenderOrder.PlacementInfo>();

            for (int lineIndex = 0; lineIndex < smdLines.Length; lineIndex++)
            {
                SMDLine line = smdLines[lineIndex];
                if (!baseMeshesByBin.TryGetValue(line.BinID, out List<BaseMesh> baseMeshes)) { continue; }

                // Resolve this placement's SMX record (optional — falls back to neutral defaults).
                SmxEntry smxEntry;
                if (!smxDic.TryGetValue(line.SmxID, out smxEntry) || smxEntry == null)
                {
                    smxEntry = DefaultSmxEntry();
                }

                Matrix4 rotation = Matrix4.CreateRotationX(line.angleX)
                                  * Matrix4.CreateRotationY(line.angleY)
                                  * Matrix4.CreateRotationZ(line.angleZ);

                Vector3 scale = new Vector3(
                    line.scaleX == 0 ? 1f : line.scaleX,
                    line.scaleY == 0 ? 1f : line.scaleY,
                    line.scaleZ == 0 ? 1f : line.scaleZ);

                Vector3 position = new Vector3(
                    line.positionX / PositionScale,
                    line.positionY / PositionScale,
                    line.positionZ / PositionScale);

                foreach (BaseMesh baseMesh in baseMeshes)
                {
                    int vertexCount = baseMesh.LocalVertices.Length / 5;
                    // Layout matches Room.MeshData / NewAge's GLMeshPart.cs: pos(3), normal(3), uv(2), color(4) = 12.
                    float[] worldVertices = new float[vertexCount * 12];

                    for (int vi = 0; vi < vertexCount; vi++)
                    {
                        int srcOff = vi * 5;
                        int dstOff = vi * 12;
                        int nOff = vi * 3;
                        int cOff = vi * 4;

                        Vector4 local = new Vector4(
                            baseMesh.LocalVertices[srcOff + 0],
                            baseMesh.LocalVertices[srcOff + 1],
                            baseMesh.LocalVertices[srcOff + 2],
                            1f);

                        Vector4 scaled = local * new Vector4(scale, 1f);
                        Vector4 rotated = scaled * rotation;
                        Vector4 world = rotated + new Vector4(position, 0f);

                        worldVertices[dstOff + 0] = world.X * ExtraScale;
                        worldVertices[dstOff + 1] = world.Y * ExtraScale;
                        worldVertices[dstOff + 2] = world.Z * ExtraScale;

                        // Normals are directions, not points: rotate only (no scale, no translation),
                        // matching NewAge's vertex shader (normal_matrix derived from mRotation only).
                        // Also CPU-normalized here (matches NewAge's LoadUhdBinModel, which divides by
                        // NORMAL_FIX on the CPU before upload) — a zero normal stays zero on purpose,
                        // so the shader's NormalIsZero check still triggers its unlit fallback.
                        Vector3 localNormal = new Vector3(
                            baseMesh.LocalNormals[nOff + 0],
                            baseMesh.LocalNormals[nOff + 1],
                            baseMesh.LocalNormals[nOff + 2]);
                        Vector3 worldNormal = Vector3.Zero;
                        if (localNormal != Vector3.Zero)
                        {
                            worldNormal = Vector3.TransformVector(localNormal, rotation);
                            worldNormal.Normalize();
                        }

                        worldVertices[dstOff + 3] = worldNormal.X;
                        worldVertices[dstOff + 4] = worldNormal.Y;
                        worldVertices[dstOff + 5] = worldNormal.Z;

                        worldVertices[dstOff + 6] = baseMesh.LocalVertices[srcOff + 3];
                        worldVertices[dstOff + 7] = baseMesh.LocalVertices[srcOff + 4];

                        worldVertices[dstOff + 8] = baseMesh.VertexColor[cOff + 0];
                        worldVertices[dstOff + 9] = baseMesh.VertexColor[cOff + 1];
                        worldVertices[dstOff + 10] = baseMesh.VertexColor[cOff + 2];
                        worldVertices[dstOff + 11] = baseMesh.VertexColor[cOff + 3];
                    }

                    // Meshes whose diffuse_map didn't resolve to ANY valid TPL slot (TexKey == null)
                    // are skipped entirely — there's no texture reference to bind at all.
                    if (baseMesh.TexKey == null)
                    {
                        continue;
                    }

                    // IMPORTANT: if TexKey IS valid but its bitmap failed to load from the
                    // ImagePackHD pack (texturename below won't be in texturesGL), do NOT map it
                    // to Consts.TransparentTextureName. Confirmed against NewAge's own
                    // RoomSelectedObj.Render(): it binds DataShader.WhiteTexture (opaque white) to
                    // texture0 BEFORE trying to look up the real texture, and only overwrites it if
                    // the texture is actually found in TextureRefDic. So a failed-to-load diffuse
                    // texture renders as an OPAQUE WHITE surface in NewAge, never as transparent.
                    // Passing the real TexKey through here (even though it's known-missing from
                    // `textures`) makes Room.Render()'s texture-binding fall through to its final
                    // `else` branch (DataBase.NoTextureIdGL, an opaque placeholder) instead of its
                    // `Consts.TransparentTextureName` branch (DataBase.TransparentTextureIdGL, which
                    // is fully transparent and was the actual cause of the "reflective hole cover"
                    // bug: it let the fog/clear color show through as if it were a mirror).
                    Room.MeshData mesh = new Room.MeshData();
                    mesh.vertices = worldVertices;
                    mesh.indices = (uint[])baseMesh.Indices.Clone();
                    mesh.texturename = baseMesh.TexKey;
                    mesh.alphaTexturename = (baseMesh.AlphaTexKey != null && textures.ContainsKey(baseMesh.AlphaTexKey))
                        ? baseMesh.AlphaTexKey
                        : null;
                    mesh.faceCulling = smxEntry.FaceCulling;

                    meshes.Add(mesh);
                    placementInfos.Add(new RoomRenderOrder.PlacementInfo
                    {
                        SMD_ID = lineIndex,
                        SMX_ID = line.SmxID,
                        BIN_ID = line.BinID,
                        AlphaHierarchy = smxEntry.AlphaHierarchy,
                        OpacityHierarchy = smxEntry.OpacityHierarchy,
                        FaceCulling = smxEntry.FaceCulling
                    });
                }
            }

            // Step 3: re-sort the whole mesh list into NewAge's render order
            // (Re4ViewerRender/RenderOrder.cs), based on each placement's SMX/AlphaHierarchy/
            // OpacityHierarchy/SMD_ID — see RoomRenderOrder.cs.
            List<RoomRenderOrder.PlacementInfo> orderedInfos;
            meshes = RoomRenderOrder.ToOrder(meshes, placementInfos, out orderedInfos);
        }

        /// <summary>
        /// Decodes a single UhdBIN into one local-space BaseMesh per material, using the same
        /// cumulative baseIndex scheme as NewAge's LoadUhdBinModel.PopulateTreatedModel
        /// (baseIndex starts at 0, and after each material becomes maxIndex + 1 for the next one) —
        /// NOT a per-material min/max window, which can misalign vertices when index ranges
        /// aren't perfectly contiguous per material.
        /// </summary>
        private static List<BaseMesh> BuildBaseMeshes(UhdBIN uhdBin, string[] tplTexKeys)
        {
            List<BaseMesh> result = new List<BaseMesh>();

            int baseIndex = 0;
            int maxIndex = 0;

            bool hasUV = uhdBin.Vertex_UV_Array != null;

            // Matches NewAge's LoadUhdBinModel.PopulateTreatedModel: vertex colors are only
            // present/valid when the BIN header's vertex-colors bit is set. Vertex_Color_Array
            // format is (a, r, g, b) — alpha is the FIRST byte, not the last. Both the RGB tint
            // and the alpha are used (matches NewAge's RoomShaderFrag.frag: texColor * matColor *
            // color, using the full color, not just its alpha).
            bool hasVertexColors = uhdBin.Header.ReturnsIsEnableVertexColors()
                && uhdBin.Vertex_Color_Array != null
                && uhdBin.Vertex_Color_Array.Length > 0;

            for (int im = 0; im < uhdBin.Materials.Length; im++)
            {
                MaterialBin materialBin = uhdBin.Materials[im];
                if (materialBin.face_index_array == null || materialBin.face_index_array.Length == 0)
                {
                    continue;
                }

                int localMax = maxIndex;
                for (int f = 0; f < materialBin.face_index_array.Length; f++)
                {
                    var face = materialBin.face_index_array[f];
                    int tempI = face.i1;
                    if (face.i2 > tempI) { tempI = face.i2; }
                    if (face.i3 > tempI) { tempI = face.i3; }
                    if (tempI > localMax) { localMax = tempI; }
                }
                maxIndex = localMax;

                if (maxIndex >= uhdBin.Vertex_Position_Array.Length)
                {
                    // guard against malformed data; skip this material rather than throw
                    baseIndex = maxIndex + 1;
                    continue;
                }

                int count = maxIndex - baseIndex + 1;
                if (count <= 0)
                {
                    baseIndex = maxIndex + 1;
                    continue;
                }

                float[] vertices = new float[count * 5];
                float[] vertexColor = new float[count * 4];
                float[] normals = new float[count * 3];
                bool hasNormals = uhdBin.Vertex_Normal_Array != null;

                for (int vi = baseIndex; vi <= maxIndex; vi++)
                {
                    var pos = uhdBin.Vertex_Position_Array[vi];
                    int vOffset = (vi - baseIndex) * 5;

                    vertices[vOffset + 0] = pos.vx / PositionScale;
                    vertices[vOffset + 1] = pos.vy / PositionScale;
                    vertices[vOffset + 2] = pos.vz / PositionScale;

                    if (hasUV && vi < uhdBin.Vertex_UV_Array.Length)
                    {
                        vertices[vOffset + 3] = uhdBin.Vertex_UV_Array[vi].tu;
                        vertices[vOffset + 4] = uhdBin.Vertex_UV_Array[vi].tv;
                    }

                    int cOffset = (vi - baseIndex) * 4;
                    if (hasVertexColors && vi < uhdBin.Vertex_Color_Array.Length)
                    {
                        var c = uhdBin.Vertex_Color_Array[vi];
                        vertexColor[cOffset + 0] = c.r / 255f;
                        vertexColor[cOffset + 1] = c.g / 255f;
                        vertexColor[cOffset + 2] = c.b / 255f;
                        vertexColor[cOffset + 3] = c.a / 255f;
                    }
                    else
                    {
                        vertexColor[cOffset + 0] = 1f;
                        vertexColor[cOffset + 1] = 1f;
                        vertexColor[cOffset + 2] = 1f;
                        vertexColor[cOffset + 3] = 1f;
                    }

                    int nOffset = (vi - baseIndex) * 3;
                    if (hasNormals && vi < uhdBin.Vertex_Normal_Array.Length)
                    {
                        var n = uhdBin.Vertex_Normal_Array[vi];
                        // Matches NewAge's LoadUhdBinModel: normals are normalized on the CPU
                        // before upload, not left for the shader to normalize post-transform.
                        Vector3 normal = new Vector3(n.nx, n.ny, n.nz);
                        if (normal != Vector3.Zero)
                        {
                            normal.Normalize();
                        }
                        normals[nOffset + 0] = normal.X;
                        normals[nOffset + 1] = normal.Y;
                        normals[nOffset + 2] = normal.Z;
                    }
                    // else left at (0,0,0) — matches NewAge's NormalIsZero check, which falls back
                    // to unlit (ambient-only) rendering for vertices with no real normal data.
                }

                uint[] indices = new uint[materialBin.face_index_array.Length * 3];
                int iOffset = 0;
                for (int f = 0; f < materialBin.face_index_array.Length; f++)
                {
                    var face = materialBin.face_index_array[f];
                    indices[iOffset + 0] = (uint)(face.i1 - baseIndex);
                    indices[iOffset + 1] = (uint)(face.i2 - baseIndex);
                    indices[iOffset + 2] = (uint)(face.i3 - baseIndex);
                    iOffset += 3;
                }

                string texKey = null;
                string alphaTexKey = null;
                byte diffuseMap = materialBin.material?.diffuse_map ?? 0;
                if (diffuseMap < tplTexKeys.Length)
                {
                    texKey = tplTexKeys[diffuseMap];
                }
                // texKey stays null if diffuse_map didn't resolve to any valid TPL slot — this
                // material has no real diffuse texture at all. See the skip in the caller's loop
                // (Load()) for why meshes using this material are hidden instead of drawn with a
                // placeholder.

                // Matches NewAge's LoadUhdBinModel.PopulateMaterialGroup exactly: bit 0x04 of
                // material_flag means this material carries a separate opacity/alpha map whose
                // alpha channel (not the diffuse texture's) should gate visibility. This is the
                // mechanism that hides the room's baked shadow/dark mesh.
                byte materialFlag = materialBin.material?.material_flag ?? 0;
                if ((materialFlag & 0x04) == 0x04)
                {
                    byte opacityMap = materialBin.material.opacity_map;
                    if (opacityMap < tplTexKeys.Length)
                    {
                        alphaTexKey = tplTexKeys[opacityMap];
                    }
                }

                result.Add(new BaseMesh
                {
                    LocalVertices = vertices,
                    VertexColor = vertexColor,
                    LocalNormals = normals,
                    Indices = indices,
                    TexKey = texKey,
                    AlphaTexKey = alphaTexKey
                });

                baseIndex = maxIndex + 1;
            }

            return result;
        }
    }
}
