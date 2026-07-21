using System.Collections.Generic;
using System.Linq;
using OpenTK;
using SHARED_UHD_SCENARIO_SMD.SCENARIO;

namespace Re4QuadExtremeEditor.src.Class
{
    /// <summary>
    /// Port of NewAge's Re4ViewerRender/RenderOrder.cs (RenderOrder.ToOrder), adapted to sort
    /// this editor's flat List&lt;Room.MeshData&gt; instead of NewAge's ModelGroup/GLMeshPart
    /// graph. The sorting rule itself (which meshes count as "special", the multi-key OrderBy
    /// chain, the sentinel int.MaxValue fallback for meshes with no SMD/SMX entry) is copied
    /// as-is; only the container types differ.
    ///
    /// ARCHITECTURAL DIFFERENCE FROM NewAge: NewAge sorts *mesh name references* looked up
    /// against a live ModelGroup (Objects/MeshParts/ScenarioBinList dictionaries) because its
    /// meshes are shared/instanced objects that can also be placed via non-SMD paths (its
    /// "NodeOrder" tail loop). This editor's RoomSmdLoader always produces one already-placed
    /// (world-space) Room.MeshData per SMD line's BIN reference and per BIN material - there is
    /// no separate "non-SMD node order" concept for room meshes here - so the NodeOrder tail
    /// loop is intentionally omitted; every mesh this loader emits always carries a real SMD/SMX
    /// pairing (see RoomSmdLoader.PlacementInfo below), so the "leftover mesh" case NewAge
    /// handles for its editor-only extra models never actually applies to Room's SMD meshes.
    /// </summary>
    public static class RoomRenderOrder
    {
        /// <summary>
        /// Per-mesh placement/material info needed to sort exactly like NewAge: which SMD line
        /// placed it (SMD_ID, SMX_ID, BIN_ID) and which SMX record governs it (AlphaHierarchy,
        /// OpacityHierarchy, FaceCulling). Produced by RoomSmdLoader alongside each MeshData.
        /// </summary>
        public class PlacementInfo
        {
            public int SMD_ID;
            public int SMX_ID;
            public int BIN_ID;
            public int AlphaHierarchy;
            public int OpacityHierarchy;
            public SmxFaceCulling FaceCulling;
        }

        /// <summary>
        /// Sorts (meshes, infos) pairs in place into NewAge's render order and returns the
        /// reordered mesh list (infos are reordered identically alongside, 1:1 by index).
        /// </summary>
        public static List<Room.MeshData> ToOrder(List<Room.MeshData> meshes, List<PlacementInfo> infos, out List<PlacementInfo> orderedInfos)
        {
            // pair them up so a single sort keeps mesh <-> info aligned
            var paired = new List<(Room.MeshData mesh, PlacementInfo info)>(meshes.Count);
            for (int i = 0; i < meshes.Count; i++)
            {
                paired.Add((meshes[i], infos[i]));
            }

            // "special" = AlphaHierarchy >= 0x40, exactly like NewAge's smdSpecial split.
            var special = paired.Where(p => p.info.AlphaHierarchy >= 0x40).ToList();
            var normal = paired.Where(p => p.info.AlphaHierarchy < 0x40).ToList();

            // Matches NewAge's chained OrderByDescending/OrderBy exactly (each call re-sorts the
            // WHOLE sequence using OrderBy's stable-sort semantics - this is NOT the same as
            // ThenBy, but it's what NewAge's source actually does, so it's kept identical here).
            special = special
                .OrderByDescending(p => p.info.SMX_ID)
                .OrderByDescending(p => p.info.AlphaHierarchy)
                .OrderBy(p => p.info.OpacityHierarchy)
                .ToList();

            normal = normal
                .OrderByDescending(p => p.info.SMX_ID)
                .OrderByDescending(p => p.info.SMD_ID)
                .OrderBy(p => p.info.OpacityHierarchy)
                .ToList();

            var result = new List<(Room.MeshData mesh, PlacementInfo info)>(paired.Count);
            result.AddRange(special);
            result.AddRange(normal);

            orderedInfos = result.Select(p => p.info).ToList();
            return result.Select(p => p.mesh).ToList();
        }
    }
}
