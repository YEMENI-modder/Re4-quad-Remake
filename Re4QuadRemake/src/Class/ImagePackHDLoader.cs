using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Re4QuadExtremeEditor.src.Class
{
    /// <summary>
    /// Loads textures from an ImagePackHD .pack file (RE4 UHD).
    /// A .pack contains a table of offsets; each entry is a small header followed by either
    /// a DDS or TGA image. Keys are built as "{PackID:X8}/{TextureID:D4}" to match the TPL's
    /// (PackID, TextureID) references coming from the .SMD/.BIN materials.
    /// </summary>
    public static class ImagePackHDLoader
    {
        /// <summary>
        /// Loads all (or only the requested) textures from a .pack file.
        /// </summary>
        /// <param name="packPath">Full path to the .pack file.</param>
        /// <param name="onlyIndexes">
        /// If provided, only these entry indexes (position inside the pack's offset table) are decoded.
        /// If null, all entries are decoded.
        /// </param>
        /// <returns>Dictionary keyed by "{PackID:X8}/{index:D4}" -> decoded Bitmap.</returns>
        public static Dictionary<string, Bitmap> LoadPack(string packPath, HashSet<int> onlyIndexes = null)
        {
            Dictionary<string, Bitmap> result = new Dictionary<string, Bitmap>();

            if (string.IsNullOrEmpty(packPath) || !File.Exists(packPath))
            {
                return result;
            }

            using (FileStream fs = File.OpenRead(packPath))
            using (BinaryReader pack = new BinaryReader(fs))
            {
                uint PackID = pack.ReadUInt32();
                uint Amount = pack.ReadUInt32();

                List<uint> offsets = new List<uint>();
                for (int i = 0; i < Amount; i++)
                {
                    offsets.Add(pack.ReadUInt32());
                }

                for (int i = 0; i < offsets.Count; i++)
                {
                    if (offsets[i] == 0) { continue; }
                    if (onlyIndexes != null && !onlyIndexes.Contains(i)) { continue; }

                    string texKey = PackID.ToString("X8") + "/" + i.ToString("D4");

                    try
                    {
                        pack.BaseStream.Position = offsets[i];
                        uint fileLength = pack.ReadUInt32();
                        uint ff_ff_ff_ff = pack.ReadUInt32();
                        uint PackID_ = pack.ReadUInt32();
                        uint Type = pack.ReadUInt32();

                        byte[] imageBytes = new byte[fileLength];
                        pack.BaseStream.Read(imageBytes, 0, (int)fileLength);

                        if (imageBytes.Length < 4) { continue; }

                        uint magic = BitConverter.ToUInt32(imageBytes, 0);

                        Bitmap bitmap = null;

                        if (magic == 0x20534444) // "DDS "
                        {
                            try { bitmap = DDSReaderSharp.ToBitmap(imageBytes); }
                            catch (Exception) { }
                        }
                        else if (magic == 0x00020000 || magic == 0x000A0000) // TGA
                        {
                            try
                            {
                                TGASharpLib.TGA tga = new TGASharpLib.TGA(imageBytes);
                                bitmap = tga.ToBitmap(true);
                            }
                            catch (Exception) { }
                        }

                        if (bitmap != null)
                        {
                            result[texKey] = bitmap;
                        }
                    }
                    catch (Exception)
                    {
                        // skip broken entry, keep loading the rest of the pack
                    }
                }
            }

            return result;
        }
    }
}
