using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Re4QuadExtremeEditor.src.Class.CAM
{
    /// <summary>
    /// RE4 UHD CAM File Parser/Writer.
    /// Direct C# port of cam_parser.py (Python tool by the same author).
    /// Based on MaxScript by MarioKart64n, documented by Mr.Curious.
    ///
    /// File layout (little-endian throughout):
    ///   Header (0x10 bytes)
    ///   Table1 (Table1Entry.Size * table1_count)   - links TriggerZone (Table2) + Camera (Table3)
    ///   Table2 (Table2Entry.Size * table1_count)   - TriggerZone definitions (uses table1_count!)
    ///   Table3 (Table3Entry.Size * table3_count)   - Camera setups
    ///   Table4 (Table4Entry.Size * table4_count)   - unknown fixed-size records
    ///   Table2 coord buffers (variable, referenced by DataAddr)
    ///   Table3 data buffers (variable: positions/targets/zoom/fov[/speed for Type 6])
    /// </summary>
    public class Vec3
    {
        public float X;
        public float Y;
        public float Z;

        public Vec3() { }
        public Vec3(float x, float y, float z) { X = x; Y = y; Z = z; }

        public override string ToString() => $"({X:0.000}, {Y:0.000}, {Z:0.000})";
    }

    public class Table1Entry
    {
        public const int Size = 0x10;

        public byte TriggerType = 0x03;
        public byte Unk012 = 0;
        public ushort Unk015 = 0;
        public ushort Unk016 = 0;
        public ushort Unk017 = 0;
        public uint Table2Addr = 0;
        public uint Table3Addr = 0;

        // Runtime-only indices (not stored in the file, resolved from addresses on read)
        public int Table2Index = 0;
        public int Table3Index = 0;
    }

    public class Table2Entry
    {
        // Header is 0x30 bytes; coords are stored separately via DataAddr.
        public const int HeaderSize = 0x30;

        public byte Unk051 = 0;
        public byte Unk052 = 0;
        public byte CamTypeTz = 0;
        public byte Subtype = 0x03;
        public ushort[] Unk055 = new ushort[14];
        public float Height = 1000.0f;
        public float Bottom = 0.0f;
        public uint CoordCount = 0;   // recalculated from Coords.Count on write
        public uint DataAddr = 0;     // recalculated on write

        public List<Vec3> Coords = new List<Vec3>();
    }

    public class Table3Entry
    {
        // Header is 0x34 bytes; buffers are stored separately.
        public const int HeaderSize = 0x34;

        public byte Unk021 = 1;
        public byte CamId = 0;
        public byte CamType = 6;          // 0-8, see CamTypeNames
        public byte Flags = 0;            // bitmask, see FlagBitNames
        public uint Unk025 = 0;
        public float Distance = 1000.0f;
        public float Unk027 = 0.0f;

        /// <summary>
        /// 12 raw bytes read from offset+20..offset+31 of the header.
        /// NOTE: unlike the python dataclass (which pads this to length 16 with
        /// 4 leading zeros purely for bookkeeping), BufAddr0 below is tracked
        /// separately and is always what actually gets written at offset+16.
        /// </summary>
        public byte[] Unk028 = new byte[12];

        public uint BufCount = 0;         // real count written to file; recalculated from Positions.Count on write
        public uint BufAddr1 = 0;
        public uint BufAddr2 = 0;
        public uint BufAddr3 = 0;
        public uint BufAddr4 = 0;

        /// <summary>
        /// The buf0/addr0 field at header offset+16.
        /// For Type 6 this becomes the address of the Data0 (speed) buffer.
        /// For Type 8 this is a runtime pointer written by the game itself at
        /// load time - on write we must restore the exact original value
        /// (RawBufAddr0), never recalculate it.
        /// </summary>
        public uint BufAddr0 = 0;
        public uint RawBufAddr0 = 0;

        /// <summary>
        /// Data0: per-keyframe timeline frame numbers (Type 6 only). First value
        /// must always be 0 and the list must be strictly ascending.
        /// </summary>
        public List<ushort> Data0 = new List<ushort>();

        public List<Vec3> Positions = new List<Vec3>();   // buf1
        public List<Vec3> Targets = new List<Vec3>();     // buf2 (Type 8: 12 entries instead of BufCount)
        public List<float> Zoom = new List<float>();      // buf3
        public List<float> Fov = new List<float>();       // buf4

        /// <summary>Always mirrors Positions.Count after a read; informational only.</summary>
        public int ObjExportCount => Positions.Count;
    }

    public class Table4Entry
    {
        public const int Size = 0x10;

        public byte Unk081, Unk082, Unk083, Unk084;
        public byte Unk085, Unk086, Unk087, Unk088;
        public uint Unk089;
        public uint Unk090;
    }

    public class CamFile
    {
        public const uint MagicB404 = 0x34303442; // 'B404'

        public byte Table3Count = 0;
        public byte Table1Count = 0;
        public byte Table4Count = 0;
        public byte Unk004 = 0;
        public uint Unk005 = 0;
        public uint Unk006 = 0;

        public List<Table1Entry> Table1 = new List<Table1Entry>();
        public List<Table2Entry> Table2 = new List<Table2Entry>();
        public List<Table3Entry> Table3 = new List<Table3Entry>();
        public List<Table4Entry> Table4 = new List<Table4Entry>();

        // ------------------------------------------------------------------
        // Reference tables (for UI / human-readable display only)
        // ------------------------------------------------------------------
        public static readonly Dictionary<byte, string> TriggerTypeNames = new Dictionary<byte, string>
        {
            { 0x03, "Walk Into Zone" },
            { 0x04, "AEV Event (Inspect/Open)" },
            { 0x23, "Walk Into Zone (Alt)" },
            { 0x00, "Unknown" },
        };

        public static readonly Dictionary<byte, string> CamTypeNames = new Dictionary<byte, string>
        {
            { 0, "Locked Position & Rotation" },
            { 1, "Align Behind Player (FOV pull)" },
            { 2, "Overhead Locked (no tracking)" },
            { 3, "Overhead Locked (tracks player)" },
            { 4, "Position & Rotation + Pan" },
            { 5, "Locked Rotation + Player Position" },
            { 6, "Locked + Animated (Inspection)" },
            { 7, "Locked on TriggerZone Entry" },
            { 8, "Over The Shoulder (Shoulder Cam)" },
        };

        public static readonly Dictionary<int, string> FlagBitNames = new Dictionary<int, string>
        {
            { 1, "Normal" }, { 2, "Battle" }, { 4, "Event" }, { 8, "Door" },
            { 16, "Once" }, { 32, "Ahead" }, { 64, "Direct" }, { 128, "Dislgt" },
        };

        // ------------------------------------------------------------------
        // Reading
        // ------------------------------------------------------------------
        public static CamFile Read(string filepath)
        {
            byte[] data = File.ReadAllBytes(filepath);
            return Read(data);
        }

        public static CamFile Read(byte[] data)
        {
            var cam = new CamFile();

            uint fileId = BitConverter.ToUInt32(data, 0);
            if (fileId != MagicB404)
            {
                throw new InvalidDataException(
                    $"Invalid CAM file! Expected magic B404, got {fileId:X8}");
            }

            cam.Table3Count = data[4];
            cam.Table1Count = data[5];
            cam.Table4Count = data[6];
            cam.Unk004 = data[7];
            cam.Unk005 = BitConverter.ToUInt32(data, 8);
            cam.Unk006 = BitConverter.ToUInt32(data, 12);

            int offset = 0x10;

            // ---- Table1 (starts at 0x10) ----
            var table1Positions = new List<int>();
            for (int i = 0; i < cam.Table1Count; i++)
            {
                table1Positions.Add(offset);
                var e = new Table1Entry
                {
                    TriggerType = data[offset],
                    Unk012 = data[offset + 1],
                    Unk015 = BitConverter.ToUInt16(data, offset + 2),
                    Unk016 = BitConverter.ToUInt16(data, offset + 4),
                    Unk017 = BitConverter.ToUInt16(data, offset + 6),
                    Table2Addr = BitConverter.ToUInt32(data, offset + 8),
                    Table3Addr = BitConverter.ToUInt32(data, offset + 12),
                };
                cam.Table1.Add(e);
                offset += Table1Entry.Size;
            }

            // ---- Table2 (follows Table1; NOTE: uses Table1Count entries, not its own count) ----
            var table2Positions = new List<int>();
            for (int i = 0; i < cam.Table1Count; i++)
            {
                int startPos = offset;
                table2Positions.Add(startPos);

                var e = new Table2Entry
                {
                    Unk051 = data[offset],
                    Unk052 = data[offset + 1],
                    CamTypeTz = data[offset + 2],
                    Subtype = data[offset + 3],
                };
                for (int j = 0; j < 14; j++)
                {
                    e.Unk055[j] = BitConverter.ToUInt16(data, offset + 4 + j * 2);
                }
                e.Height = BitConverter.ToSingle(data, offset + 32);
                e.Bottom = BitConverter.ToSingle(data, offset + 36);
                e.CoordCount = BitConverter.ToUInt32(data, offset + 40);
                e.DataAddr = BitConverter.ToUInt32(data, offset + 44);

                if (e.DataAddr > 0 && e.CoordCount > 0)
                {
                    for (int j = 0; j < e.CoordCount; j++)
                    {
                        int addr = (int)e.DataAddr + j * 12;
                        e.Coords.Add(new Vec3(
                            BitConverter.ToSingle(data, addr),
                            BitConverter.ToSingle(data, addr + 4),
                            BitConverter.ToSingle(data, addr + 8)));
                    }
                }

                cam.Table2.Add(e);
                offset = startPos + Table2Entry.HeaderSize;
            }

            // ---- Table3 (follows Table2) ----
            var table3Positions = new List<int>();
            for (int i = 0; i < cam.Table3Count; i++)
            {
                int startPos = offset;
                table3Positions.Add(startPos);

                var e = new Table3Entry
                {
                    Unk021 = data[offset],
                    CamId = data[offset + 1],
                    CamType = data[offset + 2],
                    Flags = data[offset + 3],
                    Unk025 = BitConverter.ToUInt32(data, offset + 4),
                    Distance = BitConverter.ToSingle(data, offset + 8),
                    Unk027 = BitConverter.ToSingle(data, offset + 12),
                };
                e.BufAddr0 = BitConverter.ToUInt32(data, offset + 16);
                e.RawBufAddr0 = e.BufAddr0;

                // unk028 slots [0..3] overlap with BufAddr0 (not stored separately);
                // slots [4..15] = the actual 12 bytes at offset+20..offset+31.
                Array.Copy(data, offset + 20, e.Unk028, 0, 12);

                e.BufCount = BitConverter.ToUInt32(data, offset + 32);
                e.BufAddr1 = BitConverter.ToUInt32(data, offset + 36);
                e.BufAddr2 = BitConverter.ToUInt32(data, offset + 40);
                e.BufAddr3 = BitConverter.ToUInt32(data, offset + 44);
                e.BufAddr4 = BitConverter.ToUInt32(data, offset + 48);

                if (e.BufCount > 0)
                {
                    // buf1: positions
                    if (e.BufAddr1 > 0)
                    {
                        for (int j = 0; j < e.BufCount; j++)
                        {
                            int addr = (int)e.BufAddr1 + j * 12;
                            e.Positions.Add(new Vec3(
                                BitConverter.ToSingle(data, addr),
                                BitConverter.ToSingle(data, addr + 4),
                                BitConverter.ToSingle(data, addr + 8)));
                        }
                    }

                    // buf2: targets. For Type 8, actual stored count is inferred
                    // from the gap to buf3 (it's 12, not BufCount=24).
                    if (e.BufAddr2 > 0)
                    {
                        int tgtCount;
                        if (e.CamType == 8 && e.BufAddr3 > e.BufAddr2)
                        {
                            tgtCount = (int)((e.BufAddr3 - e.BufAddr2) / 12);
                            tgtCount = Math.Max(1, Math.Min(tgtCount, (int)e.BufCount));
                        }
                        else
                        {
                            tgtCount = (int)e.BufCount;
                        }

                        for (int j = 0; j < tgtCount; j++)
                        {
                            int addr = (int)e.BufAddr2 + j * 12;
                            e.Targets.Add(new Vec3(
                                BitConverter.ToSingle(data, addr),
                                BitConverter.ToSingle(data, addr + 4),
                                BitConverter.ToSingle(data, addr + 8)));
                        }
                    }

                    // buf3: zoom
                    if (e.BufAddr3 > 0)
                    {
                        for (int j = 0; j < e.BufCount; j++)
                        {
                            e.Zoom.Add(BitConverter.ToSingle(data, (int)e.BufAddr3 + j * 4));
                        }
                    }

                    // buf4: FOV
                    if (e.BufAddr4 > 0)
                    {
                        for (int j = 0; j < e.BufCount; j++)
                        {
                            e.Fov.Add(BitConverter.ToSingle(data, (int)e.BufAddr4 + j * 4));
                        }
                    }

                    // buf0: Type 6 only - per-keyframe frame numbers
                    if (e.CamType == 6 && e.BufAddr0 > 0)
                    {
                        for (int j = 0; j < e.BufCount; j++)
                        {
                            e.Data0.Add(BitConverter.ToUInt16(data, (int)e.BufAddr0 + j * 2));
                        }
                    }
                }

                cam.Table3.Add(e);
                offset = startPos + Table3Entry.HeaderSize;
            }

            // ---- Table4 ----
            for (int i = 0; i < cam.Table4Count; i++)
            {
                var e = new Table4Entry
                {
                    Unk081 = data[offset], Unk082 = data[offset + 1],
                    Unk083 = data[offset + 2], Unk084 = data[offset + 3],
                    Unk085 = data[offset + 4], Unk086 = data[offset + 5],
                    Unk087 = data[offset + 6], Unk088 = data[offset + 7],
                    Unk089 = BitConverter.ToUInt32(data, offset + 8),
                    Unk090 = BitConverter.ToUInt32(data, offset + 12),
                };
                cam.Table4.Add(e);
                offset += Table4Entry.Size;
            }

            // ---- Link Table1 -> Table2/Table3 by address ----
            foreach (var t1 in cam.Table1)
            {
                int idx2 = table2Positions.IndexOf((int)t1.Table2Addr);
                if (idx2 >= 0) t1.Table2Index = idx2;

                int idx3 = table3Positions.IndexOf((int)t1.Table3Addr);
                if (idx3 >= 0) t1.Table3Index = idx3;
            }

            return cam;
        }

        // ------------------------------------------------------------------
        // Writing
        // ------------------------------------------------------------------

        /// <param name="write2007">
        /// When true, coordinate buffers use a 16-byte stride (xyz + trailing
        /// 1.0f float) instead of the normal 12-byte xyz stride. Matches the
        /// write_2007 flag from the Python tool.
        /// </param>
        public void Write(string filepath, bool write2007 = false)
        {
            byte[] bytes = WriteToBytes(write2007);
            File.WriteAllBytes(filepath, bytes);
        }

        public byte[] WriteToBytes(bool write2007 = false)
        {
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                // ---- Recalculate addresses ----
                uint ptr = 0x10; // header
                ptr += (uint)(Table1Count * Table1Entry.Size);
                uint t2Base = ptr;
                ptr += (uint)(Table1Count * Table2Entry.HeaderSize);
                uint t3Base = ptr;
                ptr += (uint)(Table3Count * Table3Entry.HeaderSize);
                ptr += (uint)(Table4Count * Table4Entry.Size);

                for (int i = 0; i < Table1.Count; i++)
                {
                    var t1 = Table1[i];
                    t1.Table2Addr = (uint)(t2Base + t1.Table2Index * Table2Entry.HeaderSize);
                    t1.Table3Addr = (uint)(t3Base + t1.Table3Index * Table3Entry.HeaderSize);
                }

                // ---- Table2 coord buffer addresses ----
                var t2DataAddrs = new List<uint>();
                foreach (var t2 in Table2)
                {
                    if (t2.Coords.Count > 0)
                    {
                        t2DataAddrs.Add(ptr);
                        ptr += (uint)(t2.Coords.Count * 12);
                    }
                    else
                    {
                        t2DataAddrs.Add(0);
                    }
                }

                // ---- CRITICAL: sync BufCount to actual data before address math ----
                // BufCount drives every stride calculation below; if it drifts from
                // the real list length the file addresses get miscalculated and the
                // resulting CAM is corrupted.
                foreach (var t3 in Table3)
                {
                    int actual = t3.Positions.Count;
                    if (actual > 0 && t3.BufCount != actual)
                    {
                        t3.BufCount = (uint)actual;
                    }
                }

                // ---- Table3 buffer addresses ----
                foreach (var t3 in Table3)
                {
                    if (t3.BufCount > 0 && t3.Positions.Count > 0)
                    {
                        int stride = write2007 ? 16 : 12;

                        t3.BufAddr1 = ptr;
                        ptr += (uint)(t3.BufCount * stride);

                        // buf2 (targets): Type 8 stores 12 targets, not BufCount (24)
                        t3.BufAddr2 = ptr;
                        int tgtCount = t3.Targets.Count > 0 ? t3.Targets.Count : (int)t3.BufCount;
                        ptr += (uint)(tgtCount * stride);

                        t3.BufAddr3 = ptr;
                        ptr += t3.BufCount * 4;

                        t3.BufAddr4 = ptr;
                        ptr += t3.BufCount * 4;
                    }
                    else
                    {
                        t3.BufAddr1 = t3.BufAddr2 = t3.BufAddr3 = t3.BufAddr4 = ptr;
                    }
                }

                // ---- Type 6 buf0 addresses ----
                foreach (var t3 in Table3)
                {
                    if (t3.CamType == 6 && t3.Data0.Count > 0)
                    {
                        t3.BufAddr0 = ptr;
                        ptr += (uint)(t3.Data0.Count * 2);
                    }
                    else if (t3.CamType == 8)
                    {
                        // Type 8: restore the original runtime pointer. The game
                        // writes this at load time - it must NOT be recalculated.
                        t3.BufAddr0 = t3.RawBufAddr0;
                    }
                    else
                    {
                        t3.BufAddr0 = 0;
                    }
                }

                // ---- Write header ----
                bw.Write(MagicB404);
                bw.Write(Table3Count);
                bw.Write(Table1Count);
                bw.Write(Table4Count);
                bw.Write(Unk004);
                bw.Write(Unk005);
                bw.Write(Unk006);

                // ---- Write Table1 ----
                foreach (var t1 in Table1)
                {
                    bw.Write(t1.TriggerType);
                    bw.Write(t1.Unk012);
                    bw.Write(t1.Unk015);
                    bw.Write(t1.Unk016);
                    bw.Write(t1.Unk017);
                    bw.Write(t1.Table2Addr);
                    bw.Write(t1.Table3Addr);
                }

                // ---- Write Table2 ----
                for (int i = 0; i < Table2.Count; i++)
                {
                    var t2 = Table2[i];
                    bw.Write(t2.Unk051);
                    bw.Write(t2.Unk052);
                    bw.Write(t2.CamTypeTz);
                    bw.Write(t2.Subtype);
                    for (int j = 0; j < 14; j++) bw.Write(t2.Unk055[j]);
                    bw.Write(t2.Height);
                    bw.Write(t2.Bottom);
                    bw.Write((uint)t2.Coords.Count);
                    bw.Write(t2DataAddrs[i]);
                }

                // ---- Write Table3 headers ----
                foreach (var t3 in Table3)
                {
                    bw.Write(t3.Unk021);
                    bw.Write(t3.CamId);
                    bw.Write(t3.CamType);
                    bw.Write(t3.Flags);
                    bw.Write(t3.Unk025);
                    bw.Write(t3.Distance);
                    bw.Write(t3.Unk027);
                    // offset+16: must write BufAddr0 here, not a stale unk028 value
                    bw.Write(t3.BufAddr0);
                    // offset+20..31: unk028 tail (12 bytes), guard against short arrays
                    byte[] tail = new byte[12];
                    Array.Copy(t3.Unk028, 0, tail, 0, Math.Min(12, t3.Unk028.Length));
                    bw.Write(tail);
                    bw.Write(t3.BufCount);
                    bw.Write(t3.BufAddr1);
                    bw.Write(t3.BufAddr2);
                    bw.Write(t3.BufAddr3);
                    bw.Write(t3.BufAddr4);
                }

                // ---- Write Table4 ----
                foreach (var t4 in Table4)
                {
                    bw.Write(t4.Unk081); bw.Write(t4.Unk082);
                    bw.Write(t4.Unk083); bw.Write(t4.Unk084);
                    bw.Write(t4.Unk085); bw.Write(t4.Unk086);
                    bw.Write(t4.Unk087); bw.Write(t4.Unk088);
                    bw.Write(t4.Unk089);
                    bw.Write(t4.Unk090);
                }

                // ---- Write Table2 coord buffers ----
                foreach (var t2 in Table2)
                {
                    foreach (var v in t2.Coords)
                    {
                        bw.Write(v.X); bw.Write(v.Y); bw.Write(v.Z);
                    }
                }

                // ---- Write Table3 buffers ----
                foreach (var t3 in Table3)
                {
                    if (t3.BufCount > 0 && t3.Positions.Count > 0)
                    {
                        int n = (int)t3.BufCount;

                        // Positions - always n entries
                        for (int j = 0; j < n; j++)
                        {
                            Vec3 v = j < t3.Positions.Count ? t3.Positions[j] : new Vec3();
                            bw.Write(v.X); bw.Write(v.Y); bw.Write(v.Z);
                            if (write2007) bw.Write(0x3F800000u);
                        }

                        // Targets - exactly tgtWriteCount entries (Type 8 has 12, others have n)
                        // MUST match the space reserved during address calculation above.
                        int tgtWriteCount = t3.Targets.Count > 0 ? t3.Targets.Count : n;
                        for (int j = 0; j < tgtWriteCount; j++)
                        {
                            Vec3 v;
                            if (j < t3.Targets.Count) v = t3.Targets[j];
                            else if (j < t3.Positions.Count) v = t3.Positions[j];
                            else v = new Vec3();
                            bw.Write(v.X); bw.Write(v.Y); bw.Write(v.Z);
                            if (write2007) bw.Write(0x3F800000u);
                        }

                        // Zoom - exactly n floats
                        for (int j = 0; j < n; j++)
                        {
                            bw.Write(j < t3.Zoom.Count ? t3.Zoom[j] : 0.0f);
                        }

                        // FOV - exactly n floats
                        for (int j = 0; j < n; j++)
                        {
                            bw.Write(j < t3.Fov.Count ? t3.Fov[j] : 50.0f);
                        }
                    }
                }

                // ---- Write Type 6 buf0 (speed/frame numbers) ----
                foreach (var t3 in Table3)
                {
                    if (t3.CamType == 6 && t3.Data0.Count > 0)
                    {
                        foreach (var v in t3.Data0) bw.Write(v);
                    }
                }

                return ms.ToArray();
            }
        }

        // ------------------------------------------------------------------
        // Validation (ported from validate_cam)
        // ------------------------------------------------------------------
        private static readonly HashSet<byte> KnownTriggerTypes = new HashSet<byte>
        {
            0x00, 0x01, 0x03, 0x04, 0x23, 0x3B, 0x41, 0x43, 0x63, 0x81, 0x83
        };

        public List<string> Validate()
        {
            var errors = new List<string>();

            if (Table1.Count == 0)
            {
                errors.Add("ERROR: No camera entries found.");
                return errors;
            }

            for (int i = 0; i < Table1.Count; i++)
            {
                var t1 = Table1[i];
                int n = i + 1;
                string prefix = $"Entry #{n}";

                if (t1.Table2Index >= Table2.Count)
                {
                    errors.Add($"{prefix}: table2_index {t1.Table2Index} out of range (max {Table2.Count - 1})");
                }
                if (t1.Table3Index >= Table3.Count)
                {
                    errors.Add($"{prefix}: table3_index {t1.Table3Index} out of range (max {Table3.Count - 1})");
                    continue;
                }

                var t2 = Table2[t1.Table2Index];
                var t3 = Table3[t1.Table3Index];

                if (t2.Unk052 != n)
                {
                    errors.Add($"{prefix}: unk052={t2.Unk052} but should be {n} (camera ID mismatch)");
                }

                // cam_id (Table3) is an internal reference and does NOT have to
                // match the 1-based entry number - original Capcom files can have
                // non-sequential/swapped IDs. Only flag if uninitialized (0).
                if (t3.CamId == 0)
                {
                    errors.Add($"{prefix}: Camera_ID=0 (uninitialized - should be set)");
                }

                if (t3.CamType > 8)
                {
                    errors.Add($"{prefix}: Camera_Type={t3.CamType} is unknown (valid: 0-8)");
                }

                if (!KnownTriggerTypes.Contains(t1.TriggerType))
                {
                    errors.Add($"{prefix}: Trigger_Type=0x{t1.TriggerType:X2} is unrecognised (may still work, verify in-game)");
                }

                if (t3.CamType == 8)
                {
                    if (t3.Positions.Count != 24)
                    {
                        errors.Add($"{prefix}: Type 8 must have exactly 24 Position keyframes, has {t3.Positions.Count}");
                    }
                    if (t3.Targets.Count > 0 && t3.Targets.Count != 12)
                    {
                        errors.Add($"{prefix}: Type 8 must have exactly 12 Target keyframes, has {t3.Targets.Count}");
                    }
                }
                else
                {
                    if (t3.Targets.Count > 0 && t3.Positions.Count != t3.Targets.Count)
                    {
                        errors.Add($"{prefix}: Position count ({t3.Positions.Count}) != Target count ({t3.Targets.Count})");
                    }
                }

                if (t3.Data0.Count > 0 && t3.CamType != 8)
                {
                    if (t3.Data0[0] != 0)
                    {
                        errors.Add($"{prefix}: Speed_01 must be 0, got {t3.Data0[0]}");
                    }
                    for (int k = 1; k < t3.Data0.Count; k++)
                    {
                        if (t3.Data0[k] <= t3.Data0[k - 1])
                        {
                            errors.Add($"{prefix}: Speed_{k + 1}={t3.Data0[k]} must be greater than Speed_{k}={t3.Data0[k - 1]} (must be ascending)");
                        }
                    }
                }

                if (t2.Coords.Count < 3)
                {
                    errors.Add($"{prefix}: TriggerZone has {t2.Coords.Count} coordinate(s), minimum is 3");
                }
            }

            return errors;
        }

        /// <summary>
        /// Auto-fix unk052 (sequential entry counter in Table2) and unk051.
        /// Does NOT touch CamId in Table3 - those are stored IDs from the
        /// original file and must be preserved exactly (changing them shifts
        /// which camera the game actually uses).
        /// Returns the list of changes made.
        /// </summary>
        public List<string> AutoFixIds()
        {
            var changes = new List<string>();

            for (int i = 0; i < Table1.Count; i++)
            {
                var t1 = Table1[i];
                int n = i + 1;
                if (t1.Table2Index < Table2.Count)
                {
                    var t2 = Table2[t1.Table2Index];
                    if (t2.Unk052 != n)
                    {
                        changes.Add($"Entry #{n}: unk052 {t2.Unk052} -> {n}");
                        t2.Unk052 = (byte)n;
                    }
                    if (t2.Unk051 != 1)
                    {
                        t2.Unk051 = 1;
                    }
                }
            }

            Table1Count = (byte)Table1.Count;
            Table3Count = (byte)Table3.Count;

            return changes;
        }

        // ------------------------------------------------------------------
        // Display helpers (ported from _decode_flags / _decode_unk055_03)
        // ------------------------------------------------------------------
        public static string DecodeFlags(byte flags)
        {
            var active = FlagBitNames
                .Where(kv => (flags & kv.Key) != 0)
                .OrderBy(kv => kv.Key)
                .Select(kv => kv.Value);
            string joined = string.Join(" | ", active);
            return joined.Length > 0 ? joined : "None";
        }

        /// <summary>Explains the Ashley/camera-shift flag field (Table2.Unk055[0]).</summary>
        public static string DecodeUnk055_03(ushort val)
        {
            if (val == 65287) return "Ashley shift DISABLED (camera stays same when Ashley follows/stays)";
            if (val == 0) return "Ashley shift ENABLED (default - camera adjusts for Ashley)";
            return $"Custom value: {val}";
        }
    }
}
