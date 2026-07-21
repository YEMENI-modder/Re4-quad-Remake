using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleEndianBinaryIO;
using Re4QuadExtremeEditor.src.Class.Enums;
using Re4QuadExtremeEditor.src.Class.ObjMethods;
using System.Drawing;
using OpenTK;

namespace Re4QuadExtremeEditor.src.Class.Files
{
    public class File_EFFBLOB_Group
    {
        /// <summary>
        /// Endian do arquivo
        /// </summary>
        public Endianness Endian { get; private set; }

        /// <summary>
        /// Table_0_TPL_Texture_IDs, (byte[8] Line, ushort OrderID)
        /// </summary>
        public EffTable0 _Table0 { get; private set; }

        /// <summary>
        /// Table_1_Ref_Effect_0, (byte[8] Line, ushort OrderID)
        /// </summary>
        public EffTable1 _Table1 { get; private set; }

        /// <summary>
        /// Table_2_EAR_Links, (byte[8] Line, ushort OrderID)
        /// </summary>
        public EffTable2 _Table2 { get; private set; }

        /// <summary>
        /// Table_3_Effect_Path_IDs, (byte[8] Line, ushort OrderID)
        /// </summary>
        public EffTable3 _Table3 { get; private set; }

        /// <summary>
        /// Table_4_BIN_Model_IDs, (byte[8] Line, ushort OrderID)
        /// </summary>
        public EffTable4 _Table4 { get; private set; }

        /// <summary>
        /// Table_6_TextureData, (byte[32] Line, ushort OrderID)
        /// </summary>
        public EffTable6 _Table6 { get; private set; }

        /// <summary>
        /// Table_7_Effect_0_Group, byte[48 -2] //não tem os bytes do campo quantidade;
        /// </summary>
        public EffTableEffectGroup _Table7_Effect0_Group { get; private set; }

        /// <summary>
        /// Table_8_Effect_1_Group, byte[48 -2] //não tem os bytes do campo quantidade;
        /// </summary>
        public EffTableEffectGroup _Table8_Effect1_Group { get; private set; }

        /// <summary>
        /// entries dos groups das tables 7 e 8
        /// <para>Ordem das entrys</para>
        /// <para>internalID, (byte[300] Line, ushort EntryOrder{Indice}, ushort GroupID{Indice}, byte tableID{7 ou 8})</para>
        /// </summary>
        public EffTableEffectEntry _TableEffectEntry { get; private set; }

        /// <summary>
        /// Table_9_Paths_Entry, byte[40], listagem com as entry
        /// <para> Table_9_Paths_Order_Group (groups, e ordem das entry) </para>
        /// <para> ushort internal_Id_da_Table_9_Paths_Entry, (ushort Order_index, ushort GroupID_ordem_dos_grupos) </para>
        /// </summary>
        public EffTable9 _Table9 { get; private set; }

        public Dictionary<EffectEntryTableID, EffTableEffectGroup> _TableEffectGroups { get; private set; }

        public File_EFFBLOB_Group(Endianness Endian)
        {
            this.Endian = Endian;
            _Table0 = new EffTable0(Endian, this);
            _Table1 = new EffTable1(Endian, this);
            _Table2 = new EffTable2(Endian, this);
            _Table3 = new EffTable3(Endian, this);
            _Table4 = new EffTable4(Endian, this);
            _Table6 = new EffTable6(Endian, this);
            _Table9 = new EffTable9(Endian, this);
            _Table7_Effect0_Group = new EffTableEffectGroup(Endian, this, EffectEntryTableID.Table7);
            _Table8_Effect1_Group = new EffTableEffectGroup(Endian, this, EffectEntryTableID.Table8);
            _TableEffectEntry = new EffTableEffectEntry(Endian, this);

            _TableEffectGroups = new Dictionary<EffectEntryTableID, EffTableEffectGroup>
            {
                { EffectEntryTableID.Table7, _Table7_Effect0_Group },
                { EffectEntryTableID.Table8, _Table8_Effect1_Group }
            };
        }

        public abstract class EffTableBase : BaseGroup 
        {
            public Dictionary<ushort, (byte[] Line, ushort OrderID)> TableLines { get; protected set; }
            public ushort IdForNewLine = 0;

            /// <summary>
            /// A classe pai dessa;
            /// </summary>
            protected File_EFFBLOB_Group Parent { get; private set; }

            /// <summary>
            /// Endian do arquivo;
            /// </summary>
            public Endianness Endian { get; private set; }

            public EffTableBase(Endianness endian, File_EFFBLOB_Group parent)
            {
                TableLines = new Dictionary<ushort, (byte[] Line, ushort OrderID)>();

                Parent = parent;
                Endian = endian;

                DisplayMethods = new NodeDisplayMethods();
                DisplayMethods.GetNodeText = GetNodeText;
                DisplayMethods.GetNodeColor = GetNodeColor;

                ChangeAmountMethods = new NodeChangeAmountMethods();
                ChangeAmountMethods.AddNewLineID = () => AddNewLineID(0);
                ChangeAmountMethods.RemoveLineID = RemoveLineID;

                Methods = new NewAge_EFF_Methods();
                SetBaseMethods(Methods);
                Methods.ReturnLine = ReturnLine;
                Methods.SetLine = SetLine;
                Methods.SetEntryOrderID = SetEntryOrderID;
                Methods.GetEntryOrderID = GetEntryOrderID;
            }

            /// <summary>
            /// classe com os metodos responsaveis pelo oque sera exibido no node;
            /// </summary>
            public NodeDisplayMethods DisplayMethods { get; }


            /// <summary>
            /// Classe com os metodos responsaveis para adicinar e remover linhas/lines
            /// </summary>
            public NodeChangeAmountMethods ChangeAmountMethods { get; }

            /// <summary>
            /// Classe com os metodos que serão passados para classe Property;
            /// </summary>
            public NewAge_EFF_Methods Methods { get; }

            protected override Endianness GetEndianness()
            {
                return Endian;
            }

            protected override byte[] GetInternalLine(ushort ID)
            {
                return TableLines[ID].Line;
            }

            protected virtual bool LinesContainsKey(ushort ID)
            {
                return TableLines.ContainsKey(ID);
            }

            protected virtual void SetEntryOrderID(ushort InternalID, ushort EntryOrderID)
            {
                var o = TableLines[InternalID];
                o.OrderID = EntryOrderID;
                TableLines[InternalID] = o;
            }

            protected virtual ushort GetEntryOrderID(ushort InternalID)
            {
                return TableLines[InternalID].OrderID;
            }

            protected virtual byte[] ReturnLine(ushort ID)
            {
                return (byte[])GetInternalLine(ID).Clone();
            }

            protected virtual void SetLine(ushort ID, byte[] value)
            {
                value.CopyTo(GetInternalLine(ID), 0);
            }

            protected ushort GetNewValidEntryOrderID()
            {
                var elements = TableLines.Values.Select(x => x.OrderID).ToArray();
                return elements.Length == 0 ? (ushort)0 : (ushort)(elements.Max() + 1);
            }

            protected abstract void RemoveLineID(ushort ID);
            protected abstract ushort AddNewLineID(byte initType);
            protected abstract Color GetNodeColor(ushort ID);
            protected abstract string GetNodeText(ushort ID);
        }

        public class EffTable0 : EffTableBase
        {
            public EffTable0(Endianness endian, File_EFFBLOB_Group parent) : base(endian, parent)
            {
                Methods0 = new NewAge_EFF_Table0Entry_Methods();
                SetBaseMethods(Methods0);
                Methods0.ReturnLine = ReturnLine;
                Methods0.SetLine = SetLine;
                Methods0.SetEntryOrderID = SetEntryOrderID;
                Methods0.GetEntryOrderID = GetEntryOrderID;

                Methods0.ReturnTextureID = ReturnTextureID;
                Methods0.SetTextureID = SetTextureID;
            }

            public NewAge_EFF_Table0Entry_Methods Methods0 { get; }

            private ushort ReturnTextureID(ushort ID)
            {
                return EndianBitConverter.ToUInt16(TableLines[ID].Line, 0x00, Endian);
            }

            private void SetTextureID(ushort ID, ushort value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(TableLines[ID].Line, 0x00);
            }

            protected override string GetNodeText(ushort ID)
            {
                if (LinesContainsKey(ID))
                {
                    if (Globals.TreeNodeRenderHexValues)
                    {
                        return BitConverter.ToString(GetInternalLine(ID)).Replace("-", "_");
                    }
                    else
                    {
                        var val1 = EndianBitConverter.ToInt16(TableLines[ID].Line, 0, Endian);
                        string r = "EFF Table0 - InID: " + ID + " OrderID: " + GetEntryOrderID(ID) + "  [0x" + val1.ToString("X2") + "]";
                        return r;
                    }
                }

                return "Error Internal Line ID " + ID;
            }

            protected override Color GetNodeColor(ushort ID)
            {
                if (!Globals.RenderFileEFFBLOB)
                {
                    return Globals.NodeColorHided;
                }
                return Globals.NodeColorEntry;
            }

            protected override void RemoveLineID(ushort ID)
            {
                TableLines.Remove(ID);
            }

            protected override ushort AddNewLineID(byte initType)
            {
                ushort newID = IdForNewLine;
                if (IdForNewLine == ushort.MaxValue)
                {
                    var Ushots = Utils.AllUshots();
                    var Useds = TableLines.Keys.ToList();
                    Ushots.RemoveAll(x => Useds.Contains(x));
                    newID = Ushots[0];
                }
                else
                {
                    IdForNewLine++;
                }

                ushort newEntryOrderID = GetNewValidEntryOrderID();

                byte[] content = new byte[8];

                TableLines.Add(newID, (content, newEntryOrderID));
                return newID;
            }

        }

        public class EffTable1 : EffTableBase
        {
            public EffTable1(Endianness endian, File_EFFBLOB_Group parent) : base(endian, parent)
            {
                Methods1 = new NewAge_EFF_Table1Entry_Methods();
                SetBaseMethods(Methods1);
                Methods1.ReturnLine = ReturnLine;
                Methods1.SetLine = SetLine;
                Methods1.SetEntryOrderID = SetEntryOrderID;
                Methods1.GetEntryOrderID = GetEntryOrderID;

                Methods1.ReturnCoreID = ReturnCoreID;
                Methods1.SetCoreID = SetCoreID;
            }

            public NewAge_EFF_Table1Entry_Methods Methods1 { get; }

            private ushort ReturnCoreID(ushort ID)
            {
                return EndianBitConverter.ToUInt16(TableLines[ID].Line, 0x00, Endian);
            }

            private void SetCoreID(ushort ID, ushort value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(TableLines[ID].Line, 0x00);
            }

            protected override string GetNodeText(ushort ID)
            {
                if (LinesContainsKey(ID))
                {
                    if (Globals.TreeNodeRenderHexValues)
                    {
                        return BitConverter.ToString(GetInternalLine(ID)).Replace("-", "_");
                    }
                    else
                    {
                        var val1 = EndianBitConverter.ToInt16(TableLines[ID].Line, 0, Endian);
                        string r = "EFF Table1 - InID: " + ID + " OrderID: " + GetEntryOrderID(ID) + "  [0x" + val1.ToString("X2") + "]";
                        return r;
                    }
                }

                return "Error Internal Line ID " + ID;
            }

            protected override Color GetNodeColor(ushort ID)
            {
                if (!Globals.RenderFileEFFBLOB)
                {
                    return Globals.NodeColorHided;
                }
                return Globals.NodeColorEntry;
            }

            protected override void RemoveLineID(ushort ID)
            {
                TableLines.Remove(ID);
            }

            protected override ushort AddNewLineID(byte initType)
            {
                ushort newID = IdForNewLine;
                if (IdForNewLine == ushort.MaxValue)
                {
                    var Ushots = Utils.AllUshots();
                    var Useds = TableLines.Keys.ToList();
                    Ushots.RemoveAll(x => Useds.Contains(x));
                    newID = Ushots[0];
                }
                else
                {
                    IdForNewLine++;
                }

                ushort newEntryOrderID = GetNewValidEntryOrderID();

                byte[] content = new byte[8];

                TableLines.Add(newID, (content, newEntryOrderID));
                return newID;
            }

        }

        public class EffTable2 : EffTableBase
        {
            public EffTable2(Endianness endian, File_EFFBLOB_Group parent) : base(endian, parent)
            {
                Methods2 = new NewAge_EFF_Table2Entry_Methods();
                SetBaseMethods(Methods2);
                Methods2.ReturnLine = ReturnLine;
                Methods2.SetLine = SetLine;
                Methods2.SetEntryOrderID = SetEntryOrderID;
                Methods2.GetEntryOrderID = GetEntryOrderID;

                Methods2.ReturnEffectGroupID = ReturnEffectGroupID;
                Methods2.SetEffectGroupID = SetEffectGroupID;
                Methods2.ReturnEarLinkID = ReturnEarLinkID;
                Methods2.SetEarLinkID = SetEarLinkID;
            }

            public NewAge_EFF_Table2Entry_Methods Methods2 { get; }

            private ushort ReturnEffectGroupID(ushort ID)
            {
                return EndianBitConverter.ToUInt16(TableLines[ID].Line, 0x00, Endian);
            }

            private void SetEffectGroupID(ushort ID, ushort value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(TableLines[ID].Line, 0x00);
            }

            private ushort ReturnEarLinkID(ushort ID)
            {
                return EndianBitConverter.ToUInt16(TableLines[ID].Line, 0x02, Endian);
            }

            private void SetEarLinkID(ushort ID, ushort value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(TableLines[ID].Line, 0x02);
            }

            protected override string GetNodeText(ushort ID)
            {
                if (LinesContainsKey(ID))
                {
                    if (Globals.TreeNodeRenderHexValues)
                    {
                        return BitConverter.ToString(GetInternalLine(ID)).Replace("-", "_");
                    }
                    else
                    {
                        var val1 = EndianBitConverter.ToInt16(TableLines[ID].Line, 0, Endian);
                        var val2 = EndianBitConverter.ToInt16(TableLines[ID].Line, 2, Endian);
                        string r = "EFF Table2 - InID: " + ID + " OrderID: " + GetEntryOrderID(ID) +
                            "  [0x" + val1.ToString("X2") + "][0x" + val2.ToString("X2") + "]";
                        return r;
                    }
                }

                return "Error Internal Line ID " + ID;
            }

            protected override Color GetNodeColor(ushort ID)
            {
                if (!Globals.RenderFileEFFBLOB)
                {
                    return Globals.NodeColorHided;
                }
                return Globals.NodeColorEntry;
            }

            protected override void RemoveLineID(ushort ID)
            {
                TableLines.Remove(ID);
            }

            protected override ushort AddNewLineID(byte initType)
            {
                ushort newID = IdForNewLine;
                if (IdForNewLine == ushort.MaxValue)
                {
                    var Ushots = Utils.AllUshots();
                    var Useds = TableLines.Keys.ToList();
                    Ushots.RemoveAll(x => Useds.Contains(x));
                    newID = Ushots[0];
                }
                else
                {
                    IdForNewLine++;
                }

                ushort newEntryOrderID = GetNewValidEntryOrderID();

                byte[] content = new byte[8];

                TableLines.Add(newID, (content, newEntryOrderID));
                return newID;
            }

        }

        public class EffTable3 : EffTableBase
        {
            public EffTable3(Endianness endian, File_EFFBLOB_Group parent) : base(endian, parent)
            {
                Methods3 = new NewAge_EFF_Table3Entry_Methods();
                SetBaseMethods(Methods3);
                Methods3.ReturnLine = ReturnLine;
                Methods3.SetLine = SetLine;
                Methods3.SetEntryOrderID = SetEntryOrderID;
                Methods3.GetEntryOrderID = GetEntryOrderID;

                Methods3.ReturnPathID = ReturnPathID;
                Methods3.SetPathID = SetPathID;
            }

            public NewAge_EFF_Table3Entry_Methods Methods3 { get; }

            private ushort ReturnPathID(ushort ID)
            {
                return EndianBitConverter.ToUInt16(TableLines[ID].Line, 0x00, Endian);
            }

            private void SetPathID(ushort ID, ushort value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(TableLines[ID].Line, 0x00);
            }

            protected override string GetNodeText(ushort ID)
            {
                if (LinesContainsKey(ID))
                {
                    if (Globals.TreeNodeRenderHexValues)
                    {
                        return BitConverter.ToString(GetInternalLine(ID)).Replace("-", "_");
                    }
                    else
                    {
                        var val1 = EndianBitConverter.ToInt16(TableLines[ID].Line, 0, Endian);
                        string r = "EFF Table3 - InID: " + ID + " OrderID: " + GetEntryOrderID(ID) + "  [0x" + val1.ToString("X2") + "]";
                        return r;
                    }
                }

                return "Error Internal Line ID " + ID;
            }

            protected override Color GetNodeColor(ushort ID)
            {
                if (!Globals.RenderFileEFFBLOB)
                {
                    return Globals.NodeColorHided;
                }
                return Globals.NodeColorEntry;
            }

            protected override void RemoveLineID(ushort ID)
            {
                TableLines.Remove(ID);
            }

            protected override ushort AddNewLineID(byte initType)
            {
                ushort newID = IdForNewLine;
                if (IdForNewLine == ushort.MaxValue)
                {
                    var Ushots = Utils.AllUshots();
                    var Useds = TableLines.Keys.ToList();
                    Ushots.RemoveAll(x => Useds.Contains(x));
                    newID = Ushots[0];
                }
                else
                {
                    IdForNewLine++;
                }

                ushort newEntryOrderID = GetNewValidEntryOrderID();

                byte[] content = new byte[8];

                TableLines.Add(newID, (content, newEntryOrderID));
                return newID;
            }

        }

        public class EffTable4 : EffTableBase
        {
            public EffTable4(Endianness endian, File_EFFBLOB_Group parent) : base(endian, parent)
            {
                Methods4 = new NewAge_EFF_Table4Entry_Methods();
                SetBaseMethods(Methods4);
                Methods4.ReturnLine = ReturnLine;
                Methods4.SetLine = SetLine;
                Methods4.SetEntryOrderID = SetEntryOrderID;
                Methods4.GetEntryOrderID = GetEntryOrderID;

                Methods4.ReturnModelID = ReturnModelID;
                Methods4.SetModelID = SetModelID;
            }

            public NewAge_EFF_Table4Entry_Methods Methods4 { get; }

            private ushort ReturnModelID(ushort ID)
            {
                return EndianBitConverter.ToUInt16(TableLines[ID].Line, 0x00, Endian);
            }

            private void SetModelID(ushort ID, ushort value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(TableLines[ID].Line, 0x00);
            }

            protected override string GetNodeText(ushort ID)
            {
                if (LinesContainsKey(ID))
                {
                    if (Globals.TreeNodeRenderHexValues)
                    {
                        return BitConverter.ToString(GetInternalLine(ID)).Replace("-", "_");
                    }
                    else
                    {
                        var val1 = EndianBitConverter.ToInt16(TableLines[ID].Line, 0, Endian);
                        string r = "EFF Table4 - InID: " + ID + " OrderID: " + GetEntryOrderID(ID) + "  [0x" + val1.ToString("X2") + "]";
                        return r;
                    }
                }

                return "Error Internal Line ID " + ID;
            }

            protected override Color GetNodeColor(ushort ID)
            {
                if (!Globals.RenderFileEFFBLOB)
                {
                    return Globals.NodeColorHided;
                }
                return Globals.NodeColorEntry;
            }

            protected override void RemoveLineID(ushort ID)
            {
                TableLines.Remove(ID);
            }

            protected override ushort AddNewLineID(byte initType)
            {
                ushort newID = IdForNewLine;
                if (IdForNewLine == ushort.MaxValue)
                {
                    var Ushots = Utils.AllUshots();
                    var Useds = TableLines.Keys.ToList();
                    Ushots.RemoveAll(x => Useds.Contains(x));
                    newID = Ushots[0];
                }
                else
                {
                    IdForNewLine++;
                }

                ushort newEntryOrderID = GetNewValidEntryOrderID();

                byte[] content = new byte[8];

                TableLines.Add(newID, (content, newEntryOrderID));
                return newID;
            }

        }

        public class EffTable6 : EffTableBase
        {
            public EffTable6(Endianness endian, File_EFFBLOB_Group parent) : base(endian, parent)
            {
                Methods6 = new NewAge_EFF_Table6Entry_Methods();
                SetBaseMethods(Methods6);
                Methods6.ReturnLine = ReturnLine;
                Methods6.SetLine = SetLine;
                Methods6.SetEntryOrderID = SetEntryOrderID;
                Methods6.GetEntryOrderID = GetEntryOrderID;

                Methods6.ReturnTextureHeight = ReturnTextureHeight;
                Methods6.SetTextureHeight = SetTextureHeight;
                Methods6.ReturnTextureWidth = ReturnTextureWidth;
                Methods6.SetTextureWidth = SetTextureWidth;
                Methods6.ReturnEffectHeight = ReturnEffectHeight;
                Methods6.SetEffectHeight = SetEffectHeight;
                Methods6.ReturnEffectWidth = ReturnEffectWidth;
                Methods6.SetEffectWidth = SetEffectWidth;
                Methods6.ReturnTextureCount = ReturnTextureCount;
                Methods6.SetTextureCount = SetTextureCount;
                Methods6.ReturnUnknown1 = ReturnUnknown1;
                Methods6.SetUnknown1 = SetUnknown1;
                Methods6.ReturnUnknown2 = ReturnUnknown2;
                Methods6.SetUnknown2 = SetUnknown2;
            }

            public NewAge_EFF_Table6Entry_Methods Methods6 { get; }

            private ushort ReturnTextureHeight(ushort ID)
            {
                return EndianBitConverter.ToUInt16(TableLines[ID].Line, 0x00, Endian);
            }

            private void SetTextureHeight(ushort ID, ushort value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(TableLines[ID].Line, 0x00);
            }

            private ushort ReturnTextureWidth(ushort ID)
            {
                return EndianBitConverter.ToUInt16(TableLines[ID].Line, 0x02, Endian);
            }

            private void SetTextureWidth(ushort ID, ushort value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(TableLines[ID].Line, 0x02);
            }

            private ushort ReturnEffectHeight(ushort ID)
            {
                return EndianBitConverter.ToUInt16(TableLines[ID].Line, 0x04, Endian);
            }

            private void SetEffectHeight(ushort ID, ushort value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(TableLines[ID].Line, 0x04);
            }

            private ushort ReturnEffectWidth(ushort ID)
            {
                return EndianBitConverter.ToUInt16(TableLines[ID].Line, 0x06, Endian);
            }

            private void SetEffectWidth(ushort ID, ushort value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(TableLines[ID].Line, 0x06);
            }

            private ushort ReturnTextureCount(ushort ID)
            {
                return EndianBitConverter.ToUInt16(TableLines[ID].Line, 0x08, Endian);
            }

            private void SetTextureCount(ushort ID, ushort value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(TableLines[ID].Line, 0x08);
            }

            private byte ReturnUnknown1(ushort ID)
            {
                return TableLines[ID].Line[0x0A];
            }

            private void SetUnknown1(ushort ID, byte value)
            {
                TableLines[ID].Line[0x0A] = value;
            }

            private byte ReturnUnknown2(ushort ID)
            {
                return TableLines[ID].Line[0x0B];
            }

            private void SetUnknown2(ushort ID, byte value)
            {
                TableLines[ID].Line[0x0B] = value;
            }

            protected override string GetNodeText(ushort ID)
            {
                if (LinesContainsKey(ID))
                {
                    if (Globals.TreeNodeRenderHexValues)
                    {
                        return BitConverter.ToString(GetInternalLine(ID)).Replace("-", "_");
                    }
                    else
                    {
                        string r = "EFF Table6 - InID: " + ID + " OrderID: " + GetEntryOrderID(ID);
                        return r;
                    }
                }

                return "Error Internal Line ID " + ID;
            }

            protected override Color GetNodeColor(ushort ID)
            {
                if (!Globals.RenderFileEFFBLOB)
                {
                    return Globals.NodeColorHided;
                }
                return Globals.NodeColorEntry;
            }

            protected override void RemoveLineID(ushort ID)
            {
                TableLines.Remove(ID);
            }

            protected override ushort AddNewLineID(byte initType)
            {
                ushort newID = IdForNewLine;
                if (IdForNewLine == ushort.MaxValue)
                {
                    var Ushots = Utils.AllUshots();
                    var Useds = TableLines.Keys.ToList();
                    Ushots.RemoveAll(x => Useds.Contains(x));
                    newID = Ushots[0];
                }
                else
                {
                    IdForNewLine++;
                }

                ushort newEntryOrderID = GetNewValidEntryOrderID();

                byte[] content = new byte[32];

                TableLines.Add(newID, (content, newEntryOrderID));
                return newID;
            }

        }

        public class EffTable9 : BaseGroup
        {
            public Dictionary<ushort, (byte[] Line, ushort EntryOrderID, ushort GroupOrderID)> Table9Lines { get; protected set; }
            public ushort IdForNewLine = 0;

            /// <summary>
            /// A classe pai dessa;
            /// </summary>
            protected File_EFFBLOB_Group Parent { get; private set; }

            /// <summary>
            /// Endian do arquivo;
            /// </summary>
            public Endianness Endian { get; private set; }

            public EffTable9(Endianness endian, File_EFFBLOB_Group parent)
            {
                Table9Lines = new Dictionary<ushort, (byte[] Line, ushort EntryOrderID, ushort GroupOrderID)>();

                Parent = parent;
                Endian = endian;

                DisplayMethods = new NodeDisplayMethods();
                DisplayMethods.GetNodeText = GetNodeText;
                DisplayMethods.GetNodeColor = GetNodeColor;

                ChangeAmountMethods = new NodeChangeAmountMethods();
                ChangeAmountMethods.AddNewLineID = () => AddNewLineID(0);
                ChangeAmountMethods.RemoveLineID = RemoveLineID;

                Methods = new NewAge_EFF_Table9Entry_Methods();
                SetBaseMethods(Methods);
                Methods.ReturnLine = ReturnLine;
                Methods.SetLine = SetLine;
                Methods.SetEntryOrderID = SetEntryOrderID;
                Methods.GetEntryOrderID = GetEntryOrderID;
                Methods.SetGroupOrderID = SetGroupOrderID;
                Methods.GetGroupOrderID = GetGroupOrderID;

                Methods.ReturnPositionX_Hex = ReturnPositionX_Hex;
                Methods.ReturnPositionY_Hex = ReturnPositionY_Hex;
                Methods.ReturnPositionZ_Hex = ReturnPositionZ_Hex;
                Methods.SetPositionX_Hex = SetPositionX_Hex;
                Methods.SetPositionY_Hex = SetPositionY_Hex;
                Methods.SetPositionZ_Hex = SetPositionZ_Hex;
                Methods.ReturnPositionX = ReturnPositionX;
                Methods.ReturnPositionY = ReturnPositionY;
                Methods.ReturnPositionZ = ReturnPositionZ;
                Methods.SetPositionX = SetPositionX;
                Methods.SetPositionY = SetPositionY;
                Methods.SetPositionZ = SetPositionZ;

                MethodsForGL = new NewAge_EFF_Table9Entry_Methods_MethodsForGL();
                MethodsForGL.GetPosition = GetPosition;
                MethodsForGL.GetGroupOrderID = GetGroupOrderID;

                MoveMethods = new NodeMoveMethods();
                MoveMethods.GetObjPostion_ToCamera = GetObjPostion_ToCamera;
                MoveMethods.GetObjAngleY_ToCamera = Utils.GetObjAngleY_ToCamera_Null;
                MoveMethods.GetObjPostion_ToMove_General = GetObjPostion_ToMove_General;
                MoveMethods.SetObjPostion_ToMove_General = SetObjPostion_ToMove_General;
                MoveMethods.GetObjRotationAngles_ToMove = Utils.GetObjRotationAngles_ToMove_Null;
                MoveMethods.SetObjRotationAngles_ToMove = Utils.SetObjRotationAngles_ToMove_Null;
                MoveMethods.GetObjScale_ToMove = Utils.GetObjScale_ToMove_Null;
                MoveMethods.SetObjScale_ToMove = Utils.SetObjScale_ToMove_Null;
                MoveMethods.GetTriggerZoneCategory = Utils.GetTriggerZoneCategory_Null;
            }

            /// <summary>
            /// classe com os metodos responsaveis pelo oque sera exibido no node;
            /// </summary>
            public NodeDisplayMethods DisplayMethods { get; }

            /// <summary>
            /// Classe com os metodos responsaveis para adicinar e remover linhas/lines
            /// </summary>
            public NodeChangeAmountMethods ChangeAmountMethods { get; }

            /// <summary>
            /// Classe com os metodos que serão passados para classe Property;
            /// </summary>
            public NewAge_EFF_Table9Entry_Methods Methods { get; }

            /// <summary>
            ///  classe com os metodos responsaveis pela movimentação dos objetos e da camera
            /// </summary>
            public NodeMoveMethods MoveMethods { get; }

            /// <summary>
            /// classe com os metodos usado para arrumar os index (ordem) das entrys
            /// </summary>
            public NodeChangeAmountCallbackMethods ChangeAmountCallbackMethods { get; set; }

            /// <summary>
            /// MethodsForGL
            /// </summary>
            public NewAge_EFF_Table9Entry_Methods_MethodsForGL MethodsForGL { get; }

            #region parte principal

            protected override Endianness GetEndianness()
            {
                return Endian;
            }

            protected override byte[] GetInternalLine(ushort ID)
            {
                return Table9Lines[ID].Line;
            }

            protected bool LinesContainsKey(ushort ID)
            {
                return Table9Lines.ContainsKey(ID);
            }

            protected void SetEntryOrderID(ushort InternalID, ushort EntryOrderID)
            {
                var o = Table9Lines[InternalID];
                o.EntryOrderID = EntryOrderID;
                Table9Lines[InternalID] = o;
            }

            protected ushort GetEntryOrderID(ushort InternalID)
            {
                return Table9Lines[InternalID].EntryOrderID;
            }

            protected ushort GetGroupOrderID(ushort InternalID)
            {
                return Table9Lines[InternalID].GroupOrderID;
            }

            protected void SetGroupOrderID(ushort InternalID, ushort GroupOrderID)
            {
                if (GroupOrderID < Consts.AmountLimitEFF_Table9_Group)
                {
                    var o = Table9Lines[InternalID];
                    o.GroupOrderID = GroupOrderID;
                    Table9Lines[InternalID] = o;
                    ChangeAmountCallbackMethods.OnMoveNode();
                }
            }

            protected byte[] ReturnLine(ushort ID)
            {
                return (byte[])GetInternalLine(ID).Clone();
            }

            protected void SetLine(ushort ID, byte[] value)
            {
                value.CopyTo(GetInternalLine(ID), 0);
            }

            private ushort GetNewValidEntryOrderID(ushort GroupOrderID)
            {
                var elements = Table9Lines.Values.Where(x => x.GroupOrderID == GroupOrderID).Select(x => x.EntryOrderID).ToArray();
                return elements.Length == 0 ? (ushort)0 : (ushort)(elements.Max() + 1);
            }

            private string GetNodeText(ushort ID)
            {
                if (LinesContainsKey(ID))
                {
                    if (Globals.TreeNodeRenderHexValues)
                    {
                        return BitConverter.ToString(GetInternalLine(ID)).Replace("-", "_");
                    }
                    else
                    {
                        string r = "EFF Table9 - InID: " + ID + "  OrderID: " + GetEntryOrderID(ID) + "  GroupID: " + GetGroupOrderID(ID);
                        return r;
                    }
                }

                return "Error Internal Line ID " + ID;
            }

            private Color GetNodeColor(ushort ID)
            {
                if (!Globals.RenderFileEFFBLOB)
                {
                    return Globals.NodeColorHided;
                }
                else if (!Globals.EFF_RenderTable9)
                {
                    return Globals.NodeColorHided;
                }
                else if (!(Globals.EFF_ShowOnlySelectedGroup == false || (Globals.EFF_ShowOnlySelectedGroup && Globals.EFF_SelectedGroup == GetGroupOrderID(ID))))
                {
                    return Globals.NodeColorHided;
                }
                return Globals.NodeColorEntry;
            }

            private void RemoveLineID(ushort ID)
            {
                Table9Lines.Remove(ID);
            }

            private ushort AddNewLineID(byte initType)
            {
                ushort newID = IdForNewLine;
                if (IdForNewLine == ushort.MaxValue)
                {
                    var Ushots = Utils.AllUshots();
                    var Useds = Table9Lines.Keys.ToList();
                    Ushots.RemoveAll(x => Useds.Contains(x));
                    newID = Ushots[0];
                }
                else
                {
                    IdForNewLine++;
                }

                ushort newEntryOrderID = GetNewValidEntryOrderID(0);

                byte[] content = new byte[40];

                Table9Lines.Add(newID, (content, newEntryOrderID, 0));
                return newID;
            }

            #endregion

            #region propriedades

            private uint ReturnPositionX_Hex(ushort ID)
            {
                return EndianBitConverter.ToUInt32(Table9Lines[ID].Line, 0x00, Endian);
            }

            private void SetPositionX_Hex(ushort ID, uint value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(Table9Lines[ID].Line, 0x00);
            }

            private uint ReturnPositionY_Hex(ushort ID)
            {
                return EndianBitConverter.ToUInt32(Table9Lines[ID].Line, 0x04, Endian);
            }

            private void SetPositionY_Hex(ushort ID, uint value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(Table9Lines[ID].Line, 0x04);
            }

            private uint ReturnPositionZ_Hex(ushort ID)
            {
                return EndianBitConverter.ToUInt32(Table9Lines[ID].Line, 0x08, Endian);
            }

            private void SetPositionZ_Hex(ushort ID, uint value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(Table9Lines[ID].Line, 0x08);
            }

            private float ReturnPositionX(ushort ID)
            {
                return BitConverter.ToSingle(BitConverter.GetBytes(ReturnPositionX_Hex(ID)), 0);
            }

            private float ReturnPositionY(ushort ID)
            {
                return BitConverter.ToSingle(BitConverter.GetBytes(ReturnPositionY_Hex(ID)), 0);
            }

            private float ReturnPositionZ(ushort ID)
            {
                return BitConverter.ToSingle(BitConverter.GetBytes(ReturnPositionZ_Hex(ID)), 0);
            }

            private void SetPositionX(ushort ID, float value)
            {
                SetPositionX_Hex(ID, BitConverter.ToUInt32(BitConverter.GetBytes(value), 0));
            }

            private void SetPositionY(ushort ID, float value)
            {
                SetPositionY_Hex(ID, BitConverter.ToUInt32(BitConverter.GetBytes(value), 0));
            }

            private void SetPositionZ(ushort ID, float value)
            {
                SetPositionZ_Hex(ID, BitConverter.ToUInt32(BitConverter.GetBytes(value), 0));
            }

            #endregion

            #region parte MethodsForGL

            private Vector3 GetPosition(ushort ID)
            {
                return new Vector3(ReturnPositionX(ID) / 100f, ReturnPositionY(ID) / 100f, ReturnPositionZ(ID) / 100f);
            }

            #endregion

            #region metodos move

            private Vector3 GetObjPostion_ToCamera(ushort ID)
            {
                Vector3 position = GetPosition(ID);
                Utils.ToCameraCheckValue(ref position);
                return position;
            }

            private Vector3[] GetObjPostion_ToMove_General(ushort ID)
            {
                Vector3[] pos = new Vector3[1];
                pos[0] = new Vector3(ReturnPositionX(ID), ReturnPositionY(ID), ReturnPositionZ(ID));
                Utils.ToMoveCheckLimits(ref pos);
                return pos;
            }


            private void SetObjPostion_ToMove_General(ushort ID, Vector3[] value)
            {
                if (value != null && value.Length >= 1)
                {
                    SetPositionX(ID, value[0].X);
                    SetPositionY(ID, value[0].Y);
                    SetPositionZ(ID, value[0].Z);
                }
            }
            #endregion
        }

        public class EffTableEffectGroup : BaseGroup
        {
            public Dictionary<ushort, (byte[] Line, ushort OrderID)> Table_Effect_Group { get; private set; }
            public ushort IdForNewLine = 0;

            /// <summary>
            /// verifica qual é a tabela;
            /// </summary>
            private EffectEntryTableID EffectEntryTableID { get; }

            /// <summary>
            /// a classe pai dessa;
            /// </summary>
            private File_EFFBLOB_Group Parent { get; }

            /// <summary>
            /// Endian do arquivo;
            /// </summary>
            public Endianness Endian { get; private set; }

            public EffTableEffectGroup(Endianness endian, File_EFFBLOB_Group parent, EffectEntryTableID effectEntryTableID)
            {
                Table_Effect_Group = new Dictionary<ushort, (byte[] Line, ushort OrderID)>();

                Parent = parent;
                Endian = endian;
                EffectEntryTableID = effectEntryTableID;

                DisplayMethods = new NodeDisplayMethods();
                DisplayMethods.GetNodeText = GetNodeText;
                DisplayMethods.GetNodeColor = GetNodeColor;

                ChangeAmountMethods = new NodeChangeAmountMethods();
                ChangeAmountMethods.AddNewLineID = () => AddNewLineID(0);
                ChangeAmountMethods.RemoveLineID = RemoveLineID;

                Methods = new NewAge_EFF_EffectGroup_Methods();
                SetBaseMethods(Methods);
                Methods.ReturnLine = ReturnLine;
                Methods.SetLine = SetLine;
                Methods.SetEntryOrderID = SetEntryOrderID;
                Methods.GetEntryOrderID = GetEntryOrderID;
                Methods.GetGrouptype = GetGrouptype;
                Methods.GetEntryCountInGroup = GetEntryCountInGroup;

                Methods.ReturnAngleX_Hex = ReturnAngleX_Hex;
                Methods.ReturnAngleY_Hex = ReturnAngleY_Hex;
                Methods.ReturnAngleZ_Hex = ReturnAngleZ_Hex;
                Methods.ReturnPositionX_Hex = ReturnPositionX_Hex;
                Methods.ReturnPositionY_Hex = ReturnPositionY_Hex;
                Methods.ReturnPositionZ_Hex = ReturnPositionZ_Hex;
                Methods.SetAngleX_Hex = SetAngleX_Hex;
                Methods.SetAngleY_Hex = SetAngleY_Hex;
                Methods.SetAngleZ_Hex = SetAngleZ_Hex;
                Methods.SetPositionX_Hex = SetPositionX_Hex;
                Methods.SetPositionY_Hex = SetPositionY_Hex;
                Methods.SetPositionZ_Hex = SetPositionZ_Hex;
                Methods.ReturnAngleX = ReturnAngleX;
                Methods.ReturnAngleY = ReturnAngleY;
                Methods.ReturnAngleZ = ReturnAngleZ;
                Methods.ReturnPositionX = ReturnPositionX;
                Methods.ReturnPositionY = ReturnPositionY;
                Methods.ReturnPositionZ = ReturnPositionZ;
                Methods.SetAngleX = SetAngleX;
                Methods.SetAngleY = SetAngleY;
                Methods.SetAngleZ = SetAngleZ;
                Methods.SetPositionX = SetPositionX;
                Methods.SetPositionY = SetPositionY;
                Methods.SetPositionZ = SetPositionZ;

                MethodsForGL = new NewAge_EFF_EffectGroup_Methods_MethodsForGL();
                MethodsForGL.GetPosition = GetPosition;
                MethodsForGL.GetAngle = GetAngle;

                MoveMethods = new NodeMoveMethods();
                MoveMethods.GetObjPostion_ToCamera = GetObjPostion_ToCamera;
                MoveMethods.GetObjAngleY_ToCamera = GetObjAngleY_ToCamera;
                MoveMethods.GetObjPostion_ToMove_General = GetObjPostion_ToMove_General;
                MoveMethods.SetObjPostion_ToMove_General = SetObjPostion_ToMove_General;
                MoveMethods.GetObjRotationAngles_ToMove = GetObjRotationAngles_ToMove;
                MoveMethods.SetObjRotationAngles_ToMove = SetObjRotationAngles_ToMove;
                MoveMethods.GetObjScale_ToMove = Utils.GetObjScale_ToMove_Null;
                MoveMethods.SetObjScale_ToMove = Utils.SetObjScale_ToMove_Null;
                MoveMethods.GetTriggerZoneCategory = Utils.GetTriggerZoneCategory_Null;
            }

            /// <summary>
            /// classe com os metodos responsaveis pelo oque sera exibido no node;
            /// </summary>
            public NodeDisplayMethods DisplayMethods { get; }

            /// <summary>
            ///  classe com os metodos responsaveis pela movimentação dos objetos e da camera
            /// </summary>
            public NodeMoveMethods MoveMethods { get; }

            /// <summary>
            /// Classe com os metodos responsaveis para adicinar e remover linhas/lines
            /// </summary>
            public NodeChangeAmountMethods ChangeAmountMethods { get; }

            /// <summary>
            /// Classe com os metodos que serão passados para classe Property;
            /// </summary>
            public NewAge_EFF_EffectGroup_Methods Methods { get; }

            /// <summary>
            /// MethodsForGL
            /// </summary>
            public NewAge_EFF_EffectGroup_Methods_MethodsForGL MethodsForGL { get; }

            #region parte principal

            protected override Endianness GetEndianness()
            {
                return Endian;
            }

            private GroupType GetGrouptype()
            {
                if (EffectEntryTableID == EffectEntryTableID.Table7)
                {
                    return GroupType.EFF_Table7_Effect_0;
                }
                else if (EffectEntryTableID == EffectEntryTableID.Table8)
                {
                    return GroupType.EFF_Table8_Effect_1;
                }
                else
                {
                    return GroupType.NULL;
                }
            }

            private int GetEntryCountInGroup(ushort InternalID)
            {
                return Parent._TableEffectEntry.GetEntryCountInGroup(GetEntryOrderID(InternalID), EffectEntryTableID);
            }

            protected void SetEntryOrderID(ushort InternalID, ushort EntryOrderID)
            {
                var o = Table_Effect_Group[InternalID];
                o.OrderID = EntryOrderID;
                Table_Effect_Group[InternalID] = o;
            }

            protected ushort GetEntryOrderID(ushort InternalID)
            {
                return Table_Effect_Group[InternalID].OrderID;
            }

            protected override byte[] GetInternalLine(ushort ID)
            {
               return Table_Effect_Group[ID].Line;
            }

            protected bool LinesContainsKey(ushort ID)
            {
                return Table_Effect_Group.ContainsKey(ID);
            }

            protected byte[] ReturnLine(ushort ID)
            {
                return (byte[])GetInternalLine(ID).Clone();
            }

            protected void SetLine(ushort ID, byte[] value)
            {
                value.CopyTo(GetInternalLine(ID), 0);
            }

            protected ushort GetNewValidEntryOrderID()
            {
                var elements = Table_Effect_Group.Values.Select(x => x.OrderID).ToArray();
                return elements.Length == 0 ? (ushort)0 : (ushort)(elements.Max() + 1);
            }

            public bool GetExistGroupID(ushort GroupOrderID)
            {
                return Table_Effect_Group.Where(x => x.Value.OrderID == GroupOrderID).Count() != 0;
            }

            public ushort Get_InternalID_From_GroupOrderID(ushort GroupOrderID) 
            {
                var where = Table_Effect_Group.Where(x => x.Value.OrderID == GroupOrderID);
                return where.Count() != 0 ? where.FirstOrDefault().Key : (ushort)0xFFFF;
            }

            private string GetNodeText(ushort ID)
            {
                if (LinesContainsKey(ID))
                {
                    if (Globals.TreeNodeRenderHexValues)
                    {
                        return BitConverter.ToString(GetInternalLine(ID)).Replace("-", "_");
                    }
                    else
                    {
                        string table = "Error";
                        if (EffectEntryTableID == EffectEntryTableID.Table7)
                        {
                            table = "Table7";
                        }
                        else if (EffectEntryTableID == EffectEntryTableID.Table8)
                        {
                            table = "Table8";
                        }

                        string r = $"EFF {table} - InID: {ID} OrderID: {GetEntryOrderID(ID)}";
                        return r;
                    }
                }

                return "Error Internal Line ID " + ID;
            }

            private Color GetNodeColor(ushort ID)
            {
                if (!Globals.RenderFileEFFBLOB)
                {
                    return Globals.NodeColorHided;
                }
                else if (!Globals.EFF_RenderTable7 && EffectEntryTableID == EffectEntryTableID.Table7)
                {
                    return Globals.NodeColorHided;
                }
                else if (!Globals.EFF_RenderTable8 && EffectEntryTableID == EffectEntryTableID.Table8)
                {
                    return Globals.NodeColorHided;
                }
                else if (!(Globals.EFF_ShowOnlySelectedGroup == false || (Globals.EFF_ShowOnlySelectedGroup && Globals.EFF_SelectedGroup == GetEntryOrderID(ID))))
                {
                    return Globals.NodeColorHided;
                }
                return Globals.NodeColorEntry;
            }

            private void RemoveLineID(ushort ID)
            {
                Table_Effect_Group.Remove(ID);
            }

            private ushort AddNewLineID(byte initType)
            {
                ushort newID = IdForNewLine;
                if (IdForNewLine == ushort.MaxValue)
                {
                    var Ushots = Utils.AllUshots();
                    var Useds = Table_Effect_Group.Keys.ToList();
                    Ushots.RemoveAll(x => Useds.Contains(x));
                    newID = Ushots[0];
                }
                else
                {
                    IdForNewLine++;
                }

                ushort newEntryOrderID = GetNewValidEntryOrderID();

                byte[] content = new byte[46];
                content[0x08] = 0xFE;
                content[0x22] = 0x10;

                Table_Effect_Group.Add(newID, (content, newEntryOrderID));
                return newID;
            }

            #endregion

            #region propriedades

            private uint ReturnPositionX_Hex(ushort ID)
            {
                return EndianBitConverter.ToUInt32(Table_Effect_Group[ID].Line, 0x0C -2, Endian);
            }

            private void SetPositionX_Hex(ushort ID, uint value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(Table_Effect_Group[ID].Line, 0x0C -2);
            }

            private uint ReturnPositionY_Hex(ushort ID)
            {
                return EndianBitConverter.ToUInt32(Table_Effect_Group[ID].Line, 0x10 -2, Endian);
            }

            private void SetPositionY_Hex(ushort ID, uint value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(Table_Effect_Group[ID].Line, 0x10 -2);
            }

            private uint ReturnPositionZ_Hex(ushort ID)
            {
                return EndianBitConverter.ToUInt32(Table_Effect_Group[ID].Line, 0x14 -2, Endian);
            }

            private void SetPositionZ_Hex(ushort ID, uint value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(Table_Effect_Group[ID].Line, 0x14 -2);
            }

            private float ReturnPositionX(ushort ID)
            {
                return BitConverter.ToSingle(BitConverter.GetBytes(ReturnPositionX_Hex(ID)), 0);
            }

            private float ReturnPositionY(ushort ID)
            {
                return BitConverter.ToSingle(BitConverter.GetBytes(ReturnPositionY_Hex(ID)), 0);
            }

            private float ReturnPositionZ(ushort ID)
            {
                return BitConverter.ToSingle(BitConverter.GetBytes(ReturnPositionZ_Hex(ID)), 0);
            }

            private void SetPositionX(ushort ID, float value)
            {
                SetPositionX_Hex(ID, BitConverter.ToUInt32(BitConverter.GetBytes(value), 0));
            }

            private void SetPositionY(ushort ID, float value)
            {
                SetPositionY_Hex(ID, BitConverter.ToUInt32(BitConverter.GetBytes(value), 0));
            }

            private void SetPositionZ(ushort ID, float value)
            {
                SetPositionZ_Hex(ID, BitConverter.ToUInt32(BitConverter.GetBytes(value), 0));
            }

            private uint ReturnAngleX_Hex(ushort ID)
            {
                return EndianBitConverter.ToUInt32(Table_Effect_Group[ID].Line, 0x18 -2, Endian);
            }

            private void SetAngleX_Hex(ushort ID, uint value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(Table_Effect_Group[ID].Line, 0x18 -2);
            }

            private uint ReturnAngleY_Hex(ushort ID)
            {
                return EndianBitConverter.ToUInt32(Table_Effect_Group[ID].Line, 0x1C -2, Endian);
            }

            private void SetAngleY_Hex(ushort ID, uint value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(Table_Effect_Group[ID].Line, 0x1C -2);
            }

            private uint ReturnAngleZ_Hex(ushort ID)
            {
                return EndianBitConverter.ToUInt32(Table_Effect_Group[ID].Line, 0x20 -2, Endian);
            }

            private void SetAngleZ_Hex(ushort ID, uint value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(Table_Effect_Group[ID].Line, 0x20 -2);
            }

            private float ReturnAngleX(ushort ID)
            {
                return BitConverter.ToSingle(BitConverter.GetBytes(ReturnAngleX_Hex(ID)), 0);
            }

            private float ReturnAngleY(ushort ID)
            {
                return BitConverter.ToSingle(BitConverter.GetBytes(ReturnAngleY_Hex(ID)), 0);
            }

            private float ReturnAngleZ(ushort ID)
            {
                return BitConverter.ToSingle(BitConverter.GetBytes(ReturnAngleZ_Hex(ID)), 0);
            }

            private void SetAngleX(ushort ID, float value)
            {
                SetAngleX_Hex(ID, BitConverter.ToUInt32(BitConverter.GetBytes(value), 0));
            }

            private void SetAngleY(ushort ID, float value)
            {
                SetAngleY_Hex(ID, BitConverter.ToUInt32(BitConverter.GetBytes(value), 0));
            }

            private void SetAngleZ(ushort ID, float value)
            {
                SetAngleZ_Hex(ID, BitConverter.ToUInt32(BitConverter.GetBytes(value), 0));
            }

            #endregion

            #region parte MethodsForGL
            private Matrix4 GetAngle(ushort ID)
            {
                return Matrix4.CreateRotationX(MathHelper.DegreesToRadians(ReturnAngleX(ID))) * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(ReturnAngleY(ID))) * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(ReturnAngleZ(ID)));
            }

            private Vector3 GetPosition(ushort ID)
            {
                return new Vector3(ReturnPositionX(ID) / 100f, ReturnPositionY(ID) / 100f, ReturnPositionZ(ID) / 100f);
            }

            #endregion

            #region metodos move

            private Vector3 GetObjPostion_ToCamera(ushort ID)
            {
                Vector3 position = GetPosition(ID);
                Utils.ToCameraCheckValue(ref position);
                return position;
            }

            private float GetObjAngleY_ToCamera(ushort ID)
            {
                float AngleY = MathHelper.DegreesToRadians(ReturnAngleY(ID));
                if (float.IsNaN(AngleY) || float.IsInfinity(AngleY)) { AngleY = 0; }
                return AngleY;
            }


            private Vector3[] GetObjPostion_ToMove_General(ushort ID)
            {
                Vector3[] pos = new Vector3[1];
                pos[0] = new Vector3(ReturnPositionX(ID), ReturnPositionY(ID), ReturnPositionZ(ID));
                Utils.ToMoveCheckLimits(ref pos);
                return pos;
            }


            private void SetObjPostion_ToMove_General(ushort ID, Vector3[] value)
            {
                if (value != null && value.Length >= 1)
                {
                    SetPositionX(ID, value[0].X);
                    SetPositionY(ID, value[0].Y);
                    SetPositionZ(ID, value[0].Z);
                }
            }

            private Vector3[] GetObjRotationAngles_ToMove(ushort ID)
            {
                Vector3[] v = new Vector3[1];
                v[0] = new Vector3(MathHelper.DegreesToRadians(ReturnAngleX(ID)), MathHelper.DegreesToRadians(ReturnAngleY(ID)), MathHelper.DegreesToRadians(ReturnAngleZ(ID)));
                Utils.ToMoveCheckLimits(ref v);
                return v;
            }

            private void SetObjRotationAngles_ToMove(ushort ID, Vector3[] value)
            {
                if (value != null && value.Length >= 1)
                {
                    SetAngleX(ID, MathHelper.Clamp(MathHelper.RadiansToDegrees(value[0].X), -360, 360));
                    SetAngleY(ID, MathHelper.Clamp(MathHelper.RadiansToDegrees(value[0].Y), -360, 360));
                    SetAngleZ(ID, MathHelper.Clamp(MathHelper.RadiansToDegrees(value[0].Z), -360, 360));
                }
            }

            #endregion
        }

        public class EffTableEffectEntry : BaseGroup
        {
            public Dictionary<ushort, (byte[] Line, ushort EntryOrderID, ushort GroupOrderID, EffectEntryTableID TableID)> EffectEntry { get; private set; }
            public ushort IdForNewLine = 0;

            /// <summary>
            /// a classe pai dessa;
            /// </summary>
            private File_EFFBLOB_Group Parent { get; }

            /// <summary>
            /// Endian do arquivo;
            /// </summary>
            public Endianness Endian { get; private set; }

            public EffTableEffectEntry(Endianness endian, File_EFFBLOB_Group parent)
            {
                EffectEntry = new Dictionary<ushort, (byte[] Line, ushort EntryOrderID, ushort GroupOrderID, EffectEntryTableID TableID)>();
                Parent = parent;
                Endian = endian;

                DisplayMethods = new NodeDisplayMethods();
                DisplayMethods.GetNodeText = GetNodeText;
                DisplayMethods.GetNodeColor = GetNodeColor;

                ChangeAmountMethods = new NodeChangeAmountMethods();
                ChangeAmountMethods.AddNewLineID = () => AddNewLineID(0);
                ChangeAmountMethods.RemoveLineID = RemoveLineID;

                Methods = new NewAge_EFF_EffectEntry_Methods();
                SetBaseMethods(Methods);
                Methods.ReturnLine = ReturnLine;
                Methods.SetLine = SetLine;
                Methods.SetEntryOrderID = SetEntryOrderID;
                Methods.GetEntryOrderID = GetEntryOrderID;
                Methods.SetGroupOrderID = SetGroupOrderID;
                Methods.GetGroupOrderID = GetGroupOrderID;
                Methods.SetTableID = SetTableID;
                Methods.GetTableID = GetTableID;

                Methods.ReturnAngleX_Hex = ReturnAngleX_Hex;
                Methods.ReturnAngleY_Hex = ReturnAngleY_Hex;
                Methods.ReturnAngleZ_Hex = ReturnAngleZ_Hex;
                Methods.ReturnPositionX_Hex = ReturnPositionX_Hex;
                Methods.ReturnPositionY_Hex = ReturnPositionY_Hex;
                Methods.ReturnPositionZ_Hex = ReturnPositionZ_Hex;
                Methods.SetAngleX_Hex = SetAngleX_Hex;
                Methods.SetAngleY_Hex = SetAngleY_Hex;
                Methods.SetAngleZ_Hex = SetAngleZ_Hex;
                Methods.SetPositionX_Hex = SetPositionX_Hex;
                Methods.SetPositionY_Hex = SetPositionY_Hex;
                Methods.SetPositionZ_Hex = SetPositionZ_Hex;
                Methods.ReturnAngleX = ReturnAngleX;
                Methods.ReturnAngleY = ReturnAngleY;
                Methods.ReturnAngleZ = ReturnAngleZ;
                Methods.ReturnPositionX = ReturnPositionX;
                Methods.ReturnPositionY = ReturnPositionY;
                Methods.ReturnPositionZ = ReturnPositionZ;
                Methods.SetAngleX = SetAngleX;
                Methods.SetAngleY = SetAngleY;
                Methods.SetAngleZ = SetAngleZ;
                Methods.SetPositionX = SetPositionX;
                Methods.SetPositionY = SetPositionY;
                Methods.SetPositionZ = SetPositionZ;

                Methods.ReturnStateID = ReturnStateID;
                Methods.SetStateID = SetStateID;

                Methods.ReturnEspID = ReturnEspID;
                Methods.SetEspID = SetEspID;

                Methods.ReturnTextureID = ReturnTextureID;
                Methods.SetTextureID = SetTextureID;

                Methods.ReturnTime = ReturnTime;
                Methods.SetTime = SetTime;

                Methods.ReturnParent = ReturnParent;
                Methods.SetParent = SetParent;

                Methods.ReturnPart = ReturnPart;
                Methods.SetPart = SetPart;

                Methods.ReturnFlags = ReturnFlags;
                Methods.SetFlags = SetFlags;

                Methods.ReturnRandomX = ReturnRandomX;
                Methods.SetRandomX = SetRandomX;

                Methods.ReturnRandomY = ReturnRandomY;
                Methods.SetRandomY = SetRandomY;

                Methods.ReturnRandomZ = ReturnRandomZ;
                Methods.SetRandomZ = SetRandomZ;

                Methods.ReturnSpeedX = ReturnSpeedX;
                Methods.SetSpeedX = SetSpeedX;

                Methods.ReturnSpeedY = ReturnSpeedY;
                Methods.SetSpeedY = SetSpeedY;

                Methods.ReturnSpeedZ = ReturnSpeedZ;
                Methods.SetSpeedZ = SetSpeedZ;

                Methods.ReturnDeltaSpeed = ReturnDeltaSpeed;
                Methods.SetDeltaSpeed = SetDeltaSpeed;

                Methods.ReturnRandomSpeedX = ReturnRandomSpeedX;
                Methods.SetRandomSpeedX = SetRandomSpeedX;

                Methods.ReturnRandomSpeedY = ReturnRandomSpeedY;
                Methods.SetRandomSpeedY = SetRandomSpeedY;

                Methods.ReturnRandomSpeedZ = ReturnRandomSpeedZ;
                Methods.SetRandomSpeedZ = SetRandomSpeedZ;

                Methods.ReturnAccelerationX = ReturnAccelerationX;
                Methods.SetAccelerationX = SetAccelerationX;

                Methods.ReturnAccelerationY = ReturnAccelerationY;
                Methods.SetAccelerationY = SetAccelerationY;

                Methods.ReturnAccelerationZ = ReturnAccelerationZ;
                Methods.SetAccelerationZ = SetAccelerationZ;

                Methods.ReturnRandomAccelerationX = ReturnRandomAccelerationX;
                Methods.SetRandomAccelerationX = SetRandomAccelerationX;

                Methods.ReturnRandomAccelerationY = ReturnRandomAccelerationY;
                Methods.SetRandomAccelerationY = SetRandomAccelerationY;

                Methods.ReturnRandomAccelerationZ = ReturnRandomAccelerationZ;
                Methods.SetRandomAccelerationZ = SetRandomAccelerationZ;

                Methods.ReturnRandomRotationX = ReturnRandomRotationX;
                Methods.SetRandomRotationX = SetRandomRotationX;

                Methods.ReturnRandomRotationY = ReturnRandomRotationY;
                Methods.SetRandomRotationY = SetRandomRotationY;

                Methods.ReturnRandomRotationZ = ReturnRandomRotationZ;
                Methods.SetRandomRotationZ = SetRandomRotationZ;

                Methods.ReturnRotationAccelerationX = ReturnRotationAccelerationX;
                Methods.SetRotationAccelerationX = SetRotationAccelerationX;

                Methods.ReturnRotationAccelerationY = ReturnRotationAccelerationY;
                Methods.SetRotationAccelerationY = SetRotationAccelerationY;

                Methods.ReturnRotationAccelerationZ = ReturnRotationAccelerationZ;
                Methods.SetRotationAccelerationZ = SetRotationAccelerationZ;

                Methods.ReturnRandomRotationAccelerationX = ReturnRandomRotationAccelerationX;
                Methods.SetRandomRotationAccelerationX = SetRandomRotationAccelerationX;

                Methods.ReturnRandomRotationAccelerationY = ReturnRandomRotationAccelerationY;
                Methods.SetRandomRotationAccelerationY = SetRandomRotationAccelerationY;

                Methods.ReturnRandomRotationAccelerationZ = ReturnRandomRotationAccelerationZ;
                Methods.SetRandomRotationAccelerationZ = SetRandomRotationAccelerationZ;

                Methods.ReturnWidth = ReturnWidth;
                Methods.SetWidth = SetWidth;

                Methods.ReturnHeight = ReturnHeight;
                Methods.SetHeight = SetHeight;

                Methods.ReturnRandomSize = ReturnRandomSize;
                Methods.SetRandomSize = SetRandomSize;

                Methods.ReturnGrow = ReturnGrow;
                Methods.SetGrow = SetGrow;

                Methods.ReturnDeltaGrow = ReturnDeltaGrow;
                Methods.SetDeltaGrow = SetDeltaGrow;

                Methods.ReturnColorR = ReturnColorR;
                Methods.SetColorR = SetColorR;

                Methods.ReturnColorG = ReturnColorG;
                Methods.SetColorG = SetColorG;

                Methods.ReturnColorB = ReturnColorB;
                Methods.SetColorB = SetColorB;

                Methods.ReturnColorA = ReturnColorA;
                Methods.SetColorA = SetColorA;

                Methods.ReturnDeltaColorR = ReturnDeltaColorR;
                Methods.SetDeltaColorR = SetDeltaColorR;

                Methods.ReturnDeltaColorG = ReturnDeltaColorG;
                Methods.SetDeltaColorG = SetDeltaColorG;

                Methods.ReturnDeltaColorB = ReturnDeltaColorB;
                Methods.SetDeltaColorB = SetDeltaColorB;

                Methods.ReturnDeltaColorA = ReturnDeltaColorA;
                Methods.SetDeltaColorA = SetDeltaColorA;

                Methods.ReturnDeltaColorAttack = ReturnDeltaColorAttack;
                Methods.SetDeltaColorAttack = SetDeltaColorAttack;

                Methods.ReturnDeltaColorStartFrame = ReturnDeltaColorStartFrame;
                Methods.SetDeltaColorStartFrame = SetDeltaColorStartFrame;

                Methods.ReturnDeltaSizeStartFrame = ReturnDeltaSizeStartFrame;
                Methods.SetDeltaSizeStartFrame = SetDeltaSizeStartFrame;

                Methods.ReturnLifeTime = ReturnLifeTime;
                Methods.SetLifeTime = SetLifeTime;

                Methods.ReturnAnimationSpeed = ReturnAnimationSpeed;
                Methods.SetAnimationSpeed = SetAnimationSpeed;

                Methods.ReturnReleaseTime = ReturnReleaseTime;
                Methods.SetReleaseTime = SetReleaseTime;

                Methods.ReturnBlend = ReturnBlend;
                Methods.SetBlend = SetBlend;

                Methods.ReturnSimulationType = ReturnSimulationType;
                Methods.SetSimulationType = SetSimulationType;

                Methods.ReturnSimulationPower = ReturnSimulationPower;
                Methods.SetSimulationPower = SetSimulationPower;

                Methods.ReturnMaskTextureID = ReturnMaskTextureID;
                Methods.SetMaskTextureID = SetMaskTextureID;

                Methods.ReturnValueIn = ReturnValueIn;
                Methods.SetValueIn = SetValueIn;

                Methods.ReturnValueOut = ReturnValueOut;
                Methods.SetValueOut = SetValueOut;

                Methods.ReturnWork0 = ReturnWork0;
                Methods.SetWork0 = SetWork0;

                Methods.ReturnWork1 = ReturnWork1;
                Methods.SetWork1 = SetWork1;

                Methods.ReturnWork2 = ReturnWork2;
                Methods.SetWork2 = SetWork2;

                Methods.ReturnWork3 = ReturnWork3;
                Methods.SetWork3 = SetWork3;

                Methods.ReturnWork4 = ReturnWork4;
                Methods.SetWork4 = SetWork4;

                Methods.ReturnWork5 = ReturnWork5;
                Methods.SetWork5 = SetWork5;

                Methods.ReturnWork6 = ReturnWork6;
                Methods.SetWork6 = SetWork6;

                Methods.ReturnVector0X = ReturnVector0X;
                Methods.SetVector0X = SetVector0X;

                Methods.ReturnVector0Y = ReturnVector0Y;
                Methods.SetVector0Y = SetVector0Y;

                Methods.ReturnVector0Z = ReturnVector0Z;
                Methods.SetVector0Z = SetVector0Z;

                Methods.ReturnVector1X = ReturnVector1X;
                Methods.SetVector1X = SetVector1X;

                Methods.ReturnVector1Y = ReturnVector1Y;
                Methods.SetVector1Y = SetVector1Y;

                Methods.ReturnVector1Z = ReturnVector1Z;
                Methods.SetVector1Z = SetVector1Z;

                Methods.ReturnVector2X = ReturnVector2X;
                Methods.SetVector2X = SetVector2X;

                Methods.ReturnVector2Y = ReturnVector2Y;
                Methods.SetVector2Y = SetVector2Y;

                Methods.ReturnVector2Z = ReturnVector2Z;
                Methods.SetVector2Z = SetVector2Z;

                Methods.ReturnSpline0 = ReturnSpline0;
                Methods.SetSpline0 = SetSpline0;

                Methods.ReturnSpline1 = ReturnSpline1;
                Methods.SetSpline1 = SetSpline1;

                Methods.ReturnSpline2 = ReturnSpline2;
                Methods.SetSpline2 = SetSpline2;

                Methods.ReturnSpline3 = ReturnSpline3;
                Methods.SetSpline3 = SetSpline3;

                Methods.ReturnPathOwn = ReturnPathOwn;
                Methods.SetPathOwn = SetPathOwn;

                Methods.ReturnPathNumber = ReturnPathNumber;
                Methods.SetPathNumber = SetPathNumber;

                Methods.ReturnPathStart = ReturnPathStart;
                Methods.SetPathStart = SetPathStart;

                Methods.ReturnPathRandom = ReturnPathRandom;
                Methods.SetPathRandom = SetPathRandom;

                Methods.ReturnEffType = ReturnEffType;
                Methods.SetEffType = SetEffType;

                Methods.ReturnControlID = ReturnControlID;
                Methods.SetControlID = SetControlID;

                Methods.ReturnControlFlag = ReturnControlFlag;
                Methods.SetControlFlag = SetControlFlag;

                Methods.ReturnControlInterval = ReturnControlInterval;
                Methods.SetControlInterval = SetControlInterval;

                Methods.ReturnControlNumber = ReturnControlNumber;
                Methods.SetControlNumber = SetControlNumber;

                Methods.ReturnControlRP = ReturnControlRP;
                Methods.SetControlRP = SetControlRP;

                Methods.ReturnControlLife = ReturnControlLife;
                Methods.SetControlLife = SetControlLife;

                Methods.ReturnControlPathScaleX = ReturnControlPathScaleX;
                Methods.SetControlPathScaleX = SetControlPathScaleX;

                Methods.ReturnControlPathScaleY = ReturnControlPathScaleY;
                Methods.SetControlPathScaleY = SetControlPathScaleY;

                Methods.ReturnControlPathScaleZ = ReturnControlPathScaleZ;
                Methods.SetControlPathScaleZ = SetControlPathScaleZ;

                Methods.ReturnControlPathDeltaSize = ReturnControlPathDeltaSize;
                Methods.SetControlPathDeltaSize = SetControlPathDeltaSize;

                Methods.ReturnControlPathDeltaSpeed = ReturnControlPathDeltaSpeed;
                Methods.SetControlPathDeltaSpeed = SetControlPathDeltaSpeed;

                Methods.ReturnControlPathDeltaAlpha = ReturnControlPathDeltaAlpha;
                Methods.SetControlPathDeltaAlpha = SetControlPathDeltaAlpha;

                Methods.ReturnControlPathDeltaInterval = ReturnControlPathDeltaInterval;
                Methods.SetControlPathDeltaInterval = SetControlPathDeltaInterval;

                Methods.ReturnControlPathRandomInterval = ReturnControlPathRandomInterval;
                Methods.SetControlPathRandomInterval = SetControlPathRandomInterval;

                Methods.ReturnControlPathRotationX = ReturnControlPathRotationX;
                Methods.SetControlPathRotationX = SetControlPathRotationX;

                Methods.ReturnControlPathRotationY = ReturnControlPathRotationY;
                Methods.SetControlPathRotationY = SetControlPathRotationY;

                Methods.ReturnControlPathFlag = ReturnControlPathFlag;
                Methods.SetControlPathFlag = SetControlPathFlag;


                MethodsForGL = new NewAge_EFF_EffectEntry_Methods_MethodsForGL();
                MethodsForGL.GetPosition = GetPosition;
                MethodsForGL.GetAngle = GetAngle;
                MethodsForGL.GetGroupOrderID = GetGroupOrderID;
                MethodsForGL.GetTableID = GetEffectEntryTableID;

                MoveMethods = new NodeMoveMethods();
                MoveMethods.GetObjPostion_ToCamera = GetObjPostion_ToCamera;
                MoveMethods.GetObjAngleY_ToCamera = GetObjAngleY_ToCamera;
                MoveMethods.GetObjPostion_ToMove_General = GetObjPostion_ToMove_General;
                MoveMethods.SetObjPostion_ToMove_General = SetObjPostion_ToMove_General;
                MoveMethods.GetObjRotationAngles_ToMove = GetObjRotationAngles_ToMove;
                MoveMethods.SetObjRotationAngles_ToMove = SetObjRotationAngles_ToMove;
                MoveMethods.GetObjScale_ToMove = Utils.GetObjScale_ToMove_Null;
                MoveMethods.SetObjScale_ToMove = Utils.SetObjScale_ToMove_Null;
                MoveMethods.GetTriggerZoneCategory = Utils.GetTriggerZoneCategory_Null;
            }

            /// <summary>
            /// classe com os metodos responsaveis pelo oque sera exibido no node;
            /// </summary>
            public NodeDisplayMethods DisplayMethods { get; }

            /// <summary>
            /// Classe com os metodos responsaveis para adicinar e remover linhas/lines
            /// </summary>
            public NodeChangeAmountMethods ChangeAmountMethods { get; }

            /// <summary>
            /// Classe com os metodos que serão passados para classe Property;
            /// </summary>
            public NewAge_EFF_EffectEntry_Methods Methods { get; }

            /// <summary>
            /// MethodsForGL
            /// </summary>
            public NewAge_EFF_EffectEntry_Methods_MethodsForGL MethodsForGL { get; }

            /// <summary>
            ///  classe com os metodos responsaveis pela movimentação dos objetos e da camera
            /// </summary>
            public NodeMoveMethods MoveMethods { get; }

            /// <summary>
            /// classe com os metodos usado para arrumar os index (ordem) das entrys
            /// </summary>
            public NodeChangeAmountCallbackMethods ChangeAmountCallbackMethods { get; set; }

            #region parte principal

            protected override Endianness GetEndianness()
            {
                return Endian;
            }

            protected override byte[] GetInternalLine(ushort ID)
            {
                return EffectEntry[ID].Line;
            }

            private bool LinesContainsKey(ushort ID)
            {
                return EffectEntry.ContainsKey(ID);
            }

            protected void SetEntryOrderID(ushort InternalID, ushort EntryOrderID)
            {
                var o = EffectEntry[InternalID];
                o.EntryOrderID = EntryOrderID;
                EffectEntry[InternalID] = o;
            }

            protected ushort GetEntryOrderID(ushort InternalID)
            {
                return EffectEntry[InternalID].EntryOrderID;
            }

            protected ushort GetGroupOrderID(ushort InternalID)
            {
                return EffectEntry[InternalID].GroupOrderID;
            }

            protected void SetGroupOrderID(ushort InternalID, ushort GroupOrderID)
            {
                if (GroupOrderID < Consts.AmountLimitEFF_Table7and8)
                {
                    var o = EffectEntry[InternalID];
                    o.GroupOrderID = GroupOrderID;
                    EffectEntry[InternalID] = o;
                    ChangeAmountCallbackMethods.OnMoveNode();
                }
            }

            private byte GetTableID(ushort ID)
            {
                return (byte)EffectEntry[ID].TableID;
            }

            private void SetTableID(ushort ID, byte value)
            {
                if (value == 8 || value == 7)
                {
                    var o = EffectEntry[ID];
                    o.TableID = (EffectEntryTableID)value;
                    EffectEntry[ID] = o;
                    ChangeAmountCallbackMethods.OnMoveNode();
                }
            }

            private ushort GetNewValidEntryOrderID(ushort GroupOrderID, EffectEntryTableID table)
            {
                var elements = EffectEntry.Values.Where(x => x.GroupOrderID == GroupOrderID && x.TableID == table).Select(x => x.EntryOrderID).ToArray();
                return elements.Length == 0 ? (ushort)0 : (ushort)(elements.Max() + 1);
            }

            private EffectEntryTableID GetEffectEntryTableID(ushort ID) 
            {
                return EffectEntry[ID].TableID;
            }

            private byte[] ReturnLine(ushort ID)
            {
                return (byte[])GetInternalLine(ID).Clone();
            }

            private void SetLine(ushort ID, byte[] value)
            {
                value.CopyTo(GetInternalLine(ID), 0);
            }

            private bool GetExistGroupID(ushort InternalID)
            {
                EffectEntryTableID tableID = GetEffectEntryTableID(InternalID);
                if (Parent._TableEffectGroups.ContainsKey(tableID))
                {
                    return Parent._TableEffectGroups[tableID].GetExistGroupID(GetGroupOrderID(InternalID));
                }
                return false;
            }

            public int GetEntryCountInGroup(ushort GroupOrderID, EffectEntryTableID TableID)
            {
                return EffectEntry.Values.Where(x => x.GroupOrderID == GroupOrderID && x.TableID == TableID).Count();
            }

            private string GetNodeText(ushort ID)
            {
                if (LinesContainsKey(ID))
                {
                    if (Globals.TreeNodeRenderHexValues)
                    {
                        return BitConverter.ToString(GetInternalLine(ID)).Replace("-", "_");
                    }
                    else
                    {
                        string r = "EFF EffectEntry - InID: " + ID + "  OrderID: " + GetEntryOrderID(ID) + "  GroupID: " + GetGroupOrderID(ID);
                        if (GetEffectEntryTableID(ID) == EffectEntryTableID.Table7)
                        {
                            r += "  Table7 Effect0";
                        }
                        else if (GetEffectEntryTableID(ID) == EffectEntryTableID.Table8)
                        {
                            r += "  Table8 Effect1";
                        }
                        else 
                        {
                            r += "  ERROR";
                        }

                        byte EspID = GetInternalLine(ID)[1];
                        byte TexID = GetInternalLine(ID)[2];

                        r += "  EspID: 0x" + EspID.ToString("X2") + "  TexID: 0x" + TexID.ToString("X2");  

                        if (!GetExistGroupID(ID))
                        {
                            r += "  This group does not exist!";
                        }
                        return r;
                    }
                }

                return "Error Internal Line ID " + ID;
            }

            private Color GetNodeColor(ushort ID)
            {
                if (!Globals.RenderFileEFFBLOB)
                {
                    return Globals.NodeColorHided;
                }
                else if (!Globals.EFF_RenderTable7 && GetEffectEntryTableID(ID) == EffectEntryTableID.Table7)
                {
                    return Globals.NodeColorHided;
                }
                else if (!Globals.EFF_RenderTable8 && GetEffectEntryTableID(ID) == EffectEntryTableID.Table8)
                {
                    return Globals.NodeColorHided;
                }
                else if (!(Globals.EFF_ShowOnlySelectedGroup == false || (Globals.EFF_ShowOnlySelectedGroup && Globals.EFF_SelectedGroup == GetGroupOrderID(ID))))
                {
                    return Globals.NodeColorHided;
                }
                return Globals.NodeColorEntry;
            }

            private void RemoveLineID(ushort ID)
            {
                EffectEntry.Remove(ID);
            }

            private ushort AddNewLineID(byte initType)
            {
                ushort newID = IdForNewLine;
                if (IdForNewLine == ushort.MaxValue)
                {
                    var Ushots = Utils.AllUshots();
                    var Useds = EffectEntry.Keys.ToList();
                    Ushots.RemoveAll(x => Useds.Contains(x));
                    newID = Ushots[0];
                }
                else
                {
                    IdForNewLine++;
                }

                ushort newEntryOrderID = GetNewValidEntryOrderID(0, (EffectEntryTableID)initType);

                byte[] content = new byte[300];

                EffectEntry.Add(newID, (content, newEntryOrderID, 0, (EffectEntryTableID)initType));
                return newID;
            }

            #endregion

            #region propriedades

            private uint ReturnPositionX_Hex(ushort ID)
            {
                return EndianBitConverter.ToUInt32(EffectEntry[ID].Line, 0x0C, Endian);
            }

            private void SetPositionX_Hex(ushort ID, uint value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x0C);
            }

            private uint ReturnPositionY_Hex(ushort ID)
            {
                return EndianBitConverter.ToUInt32(EffectEntry[ID].Line, 0x10, Endian);
            }

            private void SetPositionY_Hex(ushort ID, uint value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x10);
            }

            private uint ReturnPositionZ_Hex(ushort ID)
            {
                return EndianBitConverter.ToUInt32(EffectEntry[ID].Line, 0x14, Endian);
            }

            private void SetPositionZ_Hex(ushort ID, uint value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x14);
            }

            private float ReturnPositionX(ushort ID)
            {
                return BitConverter.ToSingle(BitConverter.GetBytes(ReturnPositionX_Hex(ID)), 0);
            }

            private float ReturnPositionY(ushort ID)
            {
                return BitConverter.ToSingle(BitConverter.GetBytes(ReturnPositionY_Hex(ID)), 0);
            }

            private float ReturnPositionZ(ushort ID)
            {
                return BitConverter.ToSingle(BitConverter.GetBytes(ReturnPositionZ_Hex(ID)), 0);
            }

            private void SetPositionX(ushort ID, float value)
            {
                SetPositionX_Hex(ID, BitConverter.ToUInt32(BitConverter.GetBytes(value), 0));
            }

            private void SetPositionY(ushort ID, float value)
            {
                SetPositionY_Hex(ID, BitConverter.ToUInt32(BitConverter.GetBytes(value), 0));
            }

            private void SetPositionZ(ushort ID, float value)
            {
                SetPositionZ_Hex(ID, BitConverter.ToUInt32(BitConverter.GetBytes(value), 0));
            }

            private uint ReturnAngleX_Hex(ushort ID)
            {
                return EndianBitConverter.ToUInt32(EffectEntry[ID].Line, 0x58, Endian);
            }

            private void SetAngleX_Hex(ushort ID, uint value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x58);
            }

            private uint ReturnAngleY_Hex(ushort ID)
            {
                return EndianBitConverter.ToUInt32(EffectEntry[ID].Line, 0x5C, Endian);
            }

            private void SetAngleY_Hex(ushort ID, uint value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x5C);
            }

            private uint ReturnAngleZ_Hex(ushort ID)
            {
                return EndianBitConverter.ToUInt32(EffectEntry[ID].Line, 0x60, Endian);
            }

            private void SetAngleZ_Hex(ushort ID, uint value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x60);
            }

            private float ReturnAngleX(ushort ID)
            {
                return BitConverter.ToSingle(BitConverter.GetBytes(ReturnAngleX_Hex(ID)), 0);
            }

            private float ReturnAngleY(ushort ID)
            {
                return BitConverter.ToSingle(BitConverter.GetBytes(ReturnAngleY_Hex(ID)), 0);
            }

            private float ReturnAngleZ(ushort ID)
            {
                return BitConverter.ToSingle(BitConverter.GetBytes(ReturnAngleZ_Hex(ID)), 0);
            }

            private void SetAngleX(ushort ID, float value)
            {
                SetAngleX_Hex(ID, BitConverter.ToUInt32(BitConverter.GetBytes(value), 0));
            }

            private void SetAngleY(ushort ID, float value)
            {
                SetAngleY_Hex(ID, BitConverter.ToUInt32(BitConverter.GetBytes(value), 0));
            }

            private void SetAngleZ(ushort ID, float value)
            {
                SetAngleZ_Hex(ID, BitConverter.ToUInt32(BitConverter.GetBytes(value), 0));
            }

            private byte ReturnStateID(ushort ID)
            {
                return EffectEntry[ID].Line[0x0];
            }

            private void SetStateID(ushort ID, byte value)
            {
                EffectEntry[ID].Line[0x0] = value;
            }

            private byte ReturnEspID(ushort ID)
            {
                return EffectEntry[ID].Line[0x1];
            }

            private void SetEspID(ushort ID, byte value)
            {
                EffectEntry[ID].Line[0x1] = value;
            }

            private byte ReturnTextureID(ushort ID)
            {
                return EffectEntry[ID].Line[0x2];
            }

            private void SetTextureID(ushort ID, byte value)
            {
                EffectEntry[ID].Line[0x2] = value;
            }

            private ushort ReturnTime(ushort ID)
            {
                return EndianBitConverter.ToUInt16(EffectEntry[ID].Line, 0x4, Endian);
            }

            private void SetTime(ushort ID, ushort value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x4);
            }

            private byte ReturnParent(ushort ID)
            {
                return EffectEntry[ID].Line[0x6];
            }

            private void SetParent(ushort ID, byte value)
            {
                EffectEntry[ID].Line[0x6] = value;
            }

            private byte ReturnPart(ushort ID)
            {
                return EffectEntry[ID].Line[0x7];
            }

            private void SetPart(ushort ID, byte value)
            {
                EffectEntry[ID].Line[0x7] = value;
            }

            private uint ReturnFlags(ushort ID)
            {
                return EndianBitConverter.ToUInt32(EffectEntry[ID].Line, 0x8, Endian);
            }

            private void SetFlags(ushort ID, uint value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x8);
            }

            private float ReturnRandomX(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0x18, Endian);
            }

            private void SetRandomX(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x18);
            }

            private float ReturnRandomY(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0x1c, Endian);
            }

            private void SetRandomY(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x1c);
            }

            private float ReturnRandomZ(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0x20, Endian);
            }

            private void SetRandomZ(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x20);
            }

            private float ReturnSpeedX(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0x24, Endian);
            }

            private void SetSpeedX(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x24);
            }

            private float ReturnSpeedY(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0x28, Endian);
            }

            private void SetSpeedY(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x28);
            }

            private float ReturnSpeedZ(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0x2c, Endian);
            }

            private void SetSpeedZ(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x2c);
            }

            private float ReturnDeltaSpeed(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0x30, Endian);
            }

            private void SetDeltaSpeed(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x30);
            }

            private float ReturnRandomSpeedX(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0x34, Endian);
            }

            private void SetRandomSpeedX(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x34);
            }

            private float ReturnRandomSpeedY(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0x38, Endian);
            }

            private void SetRandomSpeedY(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x38);
            }

            private float ReturnRandomSpeedZ(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0x3c, Endian);
            }

            private void SetRandomSpeedZ(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x3c);
            }

            private float ReturnAccelerationX(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0x40, Endian);
            }

            private void SetAccelerationX(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x40);
            }

            private float ReturnAccelerationY(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0x44, Endian);
            }

            private void SetAccelerationY(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x44);
            }

            private float ReturnAccelerationZ(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0x48, Endian);
            }

            private void SetAccelerationZ(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x48);
            }

            private float ReturnRandomAccelerationX(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0x4c, Endian);
            }

            private void SetRandomAccelerationX(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x4c);
            }

            private float ReturnRandomAccelerationY(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0x50, Endian);
            }

            private void SetRandomAccelerationY(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x50);
            }

            private float ReturnRandomAccelerationZ(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0x54, Endian);
            }

            private void SetRandomAccelerationZ(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x54);
            }

            private float ReturnRandomRotationX(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0x64, Endian);
            }

            private void SetRandomRotationX(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x64);
            }

            private float ReturnRandomRotationY(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0x68, Endian);
            }

            private void SetRandomRotationY(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x68);
            }

            private float ReturnRandomRotationZ(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0x6c, Endian);
            }

            private void SetRandomRotationZ(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x6c);
            }

            private float ReturnRotationAccelerationX(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0x70, Endian);
            }

            private void SetRotationAccelerationX(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x70);
            }

            private float ReturnRotationAccelerationY(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0x74, Endian);
            }

            private void SetRotationAccelerationY(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x74);
            }

            private float ReturnRotationAccelerationZ(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0x78, Endian);
            }

            private void SetRotationAccelerationZ(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x78);
            }

            private float ReturnRandomRotationAccelerationX(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0x7c, Endian);
            }

            private void SetRandomRotationAccelerationX(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x7c);
            }

            private float ReturnRandomRotationAccelerationY(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0x80, Endian);
            }

            private void SetRandomRotationAccelerationY(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x80);
            }

            private float ReturnRandomRotationAccelerationZ(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0x84, Endian);
            }

            private void SetRandomRotationAccelerationZ(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x84);
            }

            private float ReturnWidth(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0x88, Endian);
            }

            private void SetWidth(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x88);
            }

            private float ReturnHeight(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0x8c, Endian);
            }

            private void SetHeight(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x8c);
            }

            private float ReturnRandomSize(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0x90, Endian);
            }

            private void SetRandomSize(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x90);
            }

            private float ReturnGrow(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0x94, Endian);
            }

            private void SetGrow(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x94);
            }

            private float ReturnDeltaGrow(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0x98, Endian);
            }

            private void SetDeltaGrow(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x98);
            }

            private byte ReturnColorR(ushort ID)
            {
                return EffectEntry[ID].Line[0x9c];
            }

            private void SetColorR(ushort ID, byte value)
            {
                EffectEntry[ID].Line[0x9c] = value;
            }

            private byte ReturnColorG(ushort ID)
            {
                return EffectEntry[ID].Line[0x9d];
            }

            private void SetColorG(ushort ID, byte value)
            {
                EffectEntry[ID].Line[0x9d] = value;
            }

            private byte ReturnColorB(ushort ID)
            {
                return EffectEntry[ID].Line[0x9e];
            }

            private void SetColorB(ushort ID, byte value)
            {
                EffectEntry[ID].Line[0x9e] = value;
            }

            private byte ReturnColorA(ushort ID)
            {
                return EffectEntry[ID].Line[0x9f];
            }

            private void SetColorA(ushort ID, byte value)
            {
                EffectEntry[ID].Line[0x9f] = value;
            }

            private float ReturnDeltaColorR(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0xa0, Endian);
            }

            private void SetDeltaColorR(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0xa0);
            }

            private float ReturnDeltaColorG(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0xa4, Endian);
            }

            private void SetDeltaColorG(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0xa4);
            }

            private float ReturnDeltaColorB(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0xa8, Endian);
            }

            private void SetDeltaColorB(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0xa8);
            }

            private float ReturnDeltaColorA(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0xac, Endian);
            }

            private void SetDeltaColorA(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0xac);
            }

            private ushort ReturnDeltaColorAttack(ushort ID)
            {
                return EndianBitConverter.ToUInt16(EffectEntry[ID].Line, 0xb0, Endian);
            }

            private void SetDeltaColorAttack(ushort ID, ushort value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0xb0);
            }

            private ushort ReturnDeltaColorStartFrame(ushort ID)
            {
                return EndianBitConverter.ToUInt16(EffectEntry[ID].Line, 0xb2, Endian);
            }

            private void SetDeltaColorStartFrame(ushort ID, ushort value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0xb2);
            }

            private ushort ReturnDeltaSizeStartFrame(ushort ID)
            {
                return EndianBitConverter.ToUInt16(EffectEntry[ID].Line, 0xb6, Endian);
            }

            private void SetDeltaSizeStartFrame(ushort ID, ushort value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0xb6);
            }

            private ushort ReturnLifeTime(ushort ID)
            {
                return EndianBitConverter.ToUInt16(EffectEntry[ID].Line, 0xb8, Endian);
            }

            private void SetLifeTime(ushort ID, ushort value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0xb8);
            }

            private uint ReturnAnimationSpeed(ushort ID)
            {
                return EndianBitConverter.ToUInt32(EffectEntry[ID].Line, 0xba, Endian);
            }

            private void SetAnimationSpeed(ushort ID, uint value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0xba);
            }

            private byte ReturnReleaseTime(ushort ID)
            {
                return EffectEntry[ID].Line[0xc0];
            }

            private void SetReleaseTime(ushort ID, byte value)
            {
                EffectEntry[ID].Line[0xc0] = value;
            }

            private ushort ReturnBlend(ushort ID)
            {
                return EndianBitConverter.ToUInt16(EffectEntry[ID].Line, 0xc1, Endian);
            }

            private void SetBlend(ushort ID, ushort value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0xc1);
            }

            private byte ReturnSimulationType(ushort ID)
            {
                return EffectEntry[ID].Line[0xc3];
            }

            private void SetSimulationType(ushort ID, byte value)
            {
                EffectEntry[ID].Line[0xc3] = value;
            }

            private byte ReturnSimulationPower(ushort ID)
            {
                return EffectEntry[ID].Line[0xc4];
            }

            private void SetSimulationPower(ushort ID, byte value)
            {
                EffectEntry[ID].Line[0xc4] = value;
            }

            private byte ReturnMaskTextureID(ushort ID)
            {
                return EffectEntry[ID].Line[0xc5];
            }

            private void SetMaskTextureID(ushort ID, byte value)
            {
                EffectEntry[ID].Line[0xc5] = value;
            }

            private byte ReturnValueIn(ushort ID)
            {
                return EffectEntry[ID].Line[0xc6];
            }

            private void SetValueIn(ushort ID, byte value)
            {
                EffectEntry[ID].Line[0xc6] = value;
            }

            private byte ReturnValueOut(ushort ID)
            {
                return EffectEntry[ID].Line[0xc7];
            }

            private void SetValueOut(ushort ID, byte value)
            {
                EffectEntry[ID].Line[0xc7] = value;
            }

            private byte ReturnWork0(ushort ID)
            {
                return EffectEntry[ID].Line[0xc8];
            }

            private void SetWork0(ushort ID, byte value)
            {
                EffectEntry[ID].Line[0xc8] = value;
            }

            private byte ReturnWork1(ushort ID)
            {
                return EffectEntry[ID].Line[0xc9];
            }

            private void SetWork1(ushort ID, byte value)
            {
                EffectEntry[ID].Line[0xc9] = value;
            }

            private byte ReturnWork2(ushort ID)
            {
                return EffectEntry[ID].Line[0xca];
            }

            private void SetWork2(ushort ID, byte value)
            {
                EffectEntry[ID].Line[0xca] = value;
            }

            private byte ReturnWork3(ushort ID)
            {
                return EffectEntry[ID].Line[0xcb];
            }

            private void SetWork3(ushort ID, byte value)
            {
                EffectEntry[ID].Line[0xcb] = value;
            }

            private uint ReturnWork4(ushort ID)
            {
                return EndianBitConverter.ToUInt32(EffectEntry[ID].Line, 0xcc, Endian);
            }

            private void SetWork4(ushort ID, uint value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0xcc);
            }

            private uint ReturnWork5(ushort ID)
            {
                return EndianBitConverter.ToUInt32(EffectEntry[ID].Line, 0xd0, Endian);
            }

            private void SetWork5(ushort ID, uint value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0xd0);
            }

            private uint ReturnWork6(ushort ID)
            {
                return EndianBitConverter.ToUInt32(EffectEntry[ID].Line, 0xd4, Endian);
            }

            private void SetWork6(ushort ID, uint value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0xd4);
            }

            private float ReturnVector0X(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0xd8, Endian);
            }

            private void SetVector0X(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0xd8);
            }

            private float ReturnVector0Y(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0xdc, Endian);
            }

            private void SetVector0Y(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0xdc);
            }

            private float ReturnVector0Z(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0xe0, Endian);
            }

            private void SetVector0Z(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0xe0);
            }

            private float ReturnVector1X(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0xe4, Endian);
            }

            private void SetVector1X(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0xe4);
            }

            private float ReturnVector1Y(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0xe8, Endian);
            }

            private void SetVector1Y(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0xe8);
            }

            private float ReturnVector1Z(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0xec, Endian);
            }

            private void SetVector1Z(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0xec);
            }

            private float ReturnVector2X(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0xf0, Endian);
            }

            private void SetVector2X(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0xf0);
            }

            private float ReturnVector2Y(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0xf4, Endian);
            }

            private void SetVector2Y(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0xf4);
            }

            private float ReturnVector2Z(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0xf8, Endian);
            }

            private void SetVector2Z(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0xf8);
            }

            private byte ReturnSpline0(ushort ID)
            {
                return EffectEntry[ID].Line[0xfc];
            }

            private void SetSpline0(ushort ID, byte value)
            {
                EffectEntry[ID].Line[0xfc] = value;
            }

            private byte ReturnSpline1(ushort ID)
            {
                return EffectEntry[ID].Line[0xfd];
            }

            private void SetSpline1(ushort ID, byte value)
            {
                EffectEntry[ID].Line[0xfd] = value;
            }

            private byte ReturnSpline2(ushort ID)
            {
                return EffectEntry[ID].Line[0xfe];
            }

            private void SetSpline2(ushort ID, byte value)
            {
                EffectEntry[ID].Line[0xfe] = value;
            }

            private byte ReturnSpline3(ushort ID)
            {
                return EffectEntry[ID].Line[0xff];
            }

            private void SetSpline3(ushort ID, byte value)
            {
                EffectEntry[ID].Line[0xff] = value;
            }

            private byte ReturnPathOwn(ushort ID)
            {
                return EffectEntry[ID].Line[0x104];
            }

            private void SetPathOwn(ushort ID, byte value)
            {
                EffectEntry[ID].Line[0x104] = value;
            }

            private byte ReturnPathNumber(ushort ID)
            {
                return EffectEntry[ID].Line[0x105];
            }

            private void SetPathNumber(ushort ID, byte value)
            {
                EffectEntry[ID].Line[0x105] = value;
            }

            private byte ReturnPathStart(ushort ID)
            {
                return EffectEntry[ID].Line[0x106];
            }

            private void SetPathStart(ushort ID, byte value)
            {
                EffectEntry[ID].Line[0x106] = value;
            }

            private byte ReturnPathRandom(ushort ID)
            {
                return EffectEntry[ID].Line[0x107];
            }

            private void SetPathRandom(ushort ID, byte value)
            {
                EffectEntry[ID].Line[0x107] = value;
            }

            private byte ReturnEffType(ushort ID)
            {
                return EffectEntry[ID].Line[0x108];
            }

            private void SetEffType(ushort ID, byte value)
            {
                EffectEntry[ID].Line[0x108] = value;
            }

            private byte ReturnControlID(ushort ID)
            {
                return EffectEntry[ID].Line[0x109];
            }

            private void SetControlID(ushort ID, byte value)
            {
                EffectEntry[ID].Line[0x109] = value;
            }

            private ushort ReturnControlFlag(ushort ID)
            {
                return EndianBitConverter.ToUInt16(EffectEntry[ID].Line, 0x10a, Endian);
            }

            private void SetControlFlag(ushort ID, ushort value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x10a);
            }

            private byte ReturnControlInterval(ushort ID)
            {
                return EffectEntry[ID].Line[0x10c];
            }

            private void SetControlInterval(ushort ID, byte value)
            {
                EffectEntry[ID].Line[0x10c] = value;
            }

            private byte ReturnControlNumber(ushort ID)
            {
                return EffectEntry[ID].Line[0x10d];
            }

            private void SetControlNumber(ushort ID, byte value)
            {
                EffectEntry[ID].Line[0x10d] = value;
            }

            private byte ReturnControlRP(ushort ID)
            {
                return EffectEntry[ID].Line[0x10e];
            }

            private void SetControlRP(ushort ID, byte value)
            {
                EffectEntry[ID].Line[0x10e] = value;
            }

            private ushort ReturnControlLife(ushort ID)
            {
                return EndianBitConverter.ToUInt16(EffectEntry[ID].Line, 0x110, Endian);
            }

            private void SetControlLife(ushort ID, ushort value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x110);
            }

            private float ReturnControlPathScaleX(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0x118, Endian);
            }

            private void SetControlPathScaleX(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x118);
            }

            private float ReturnControlPathScaleY(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0x11c, Endian);
            }

            private void SetControlPathScaleY(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x11c);
            }

            private float ReturnControlPathScaleZ(ushort ID)
            {
                return EndianBitConverter.ToSingle(EffectEntry[ID].Line, 0x120, Endian);
            }

            private void SetControlPathScaleZ(ushort ID, float value)
            {
                EndianBitConverter.GetBytes(value, Endian).CopyTo(EffectEntry[ID].Line, 0x120);
            }

            private byte ReturnControlPathDeltaSize(ushort ID)
            {
                return EffectEntry[ID].Line[0x124];
            }

            private void SetControlPathDeltaSize(ushort ID, byte value)
            {
                EffectEntry[ID].Line[0x124] = value;
            }

            private byte ReturnControlPathDeltaSpeed(ushort ID)
            {
                return EffectEntry[ID].Line[0x125];
            }

            private void SetControlPathDeltaSpeed(ushort ID, byte value)
            {
                EffectEntry[ID].Line[0x125] = value;
            }

            private byte ReturnControlPathDeltaAlpha(ushort ID)
            {
                return EffectEntry[ID].Line[0x126];
            }

            private void SetControlPathDeltaAlpha(ushort ID, byte value)
            {
                EffectEntry[ID].Line[0x126] = value;
            }

            private byte ReturnControlPathDeltaInterval(ushort ID)
            {
                return EffectEntry[ID].Line[0x127];
            }

            private void SetControlPathDeltaInterval(ushort ID, byte value)
            {
                EffectEntry[ID].Line[0x127] = value;
            }

            private byte ReturnControlPathRandomInterval(ushort ID)
            {
                return EffectEntry[ID].Line[0x128];
            }

            private void SetControlPathRandomInterval(ushort ID, byte value)
            {
                EffectEntry[ID].Line[0x128] = value;
            }

            private byte ReturnControlPathRotationX(ushort ID)
            {
                return EffectEntry[ID].Line[0x129];
            }

            private void SetControlPathRotationX(ushort ID, byte value)
            {
                EffectEntry[ID].Line[0x129] = value;
            }

            private byte ReturnControlPathRotationY(ushort ID)
            {
                return EffectEntry[ID].Line[0x12a];
            }

            private void SetControlPathRotationY(ushort ID, byte value)
            {
                EffectEntry[ID].Line[0x12a] = value;
            }

            private byte ReturnControlPathFlag(ushort ID)
            {
                return EffectEntry[ID].Line[0x12b];
            }

            private void SetControlPathFlag(ushort ID, byte value)
            {
                EffectEntry[ID].Line[0x12b] = value;
            }

            #endregion

            #region parte MethodsForGL
            private Matrix4 GetAngle(ushort ID)
            {
                return Matrix4.CreateRotationX(MathHelper.DegreesToRadians(ReturnAngleX(ID))) * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(ReturnAngleY(ID))) * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(ReturnAngleZ(ID)));
            }

            private Vector3 GetPosition(ushort ID)
            {
                return new Vector3(ReturnPositionX(ID) / 100f, ReturnPositionY(ID) / 100f, ReturnPositionZ(ID) / 100f);
            }

            #endregion

            #region metodos move

            private Vector3 GetObjPostion_ToCamera(ushort InternalID)
            {
                EffectEntryTableID tableID = GetEffectEntryTableID(InternalID);
                EffTableEffectGroup tableGroup = null;
                if (Parent._TableEffectGroups.ContainsKey(tableID))
                {
                    tableGroup = Parent._TableEffectGroups[tableID];
                }

                Vector3 GroupPos = Vector3.Zero;
                Matrix4 GroupRot = Matrix4.Identity;
                if (GetExistGroupID(InternalID) && tableGroup != null && Globals.EFF_Use_Group_Position)
                {
                    GroupPos = tableGroup.MethodsForGL.GetPosition(tableGroup.Get_InternalID_From_GroupOrderID(GetGroupOrderID(InternalID)));
                    GroupRot = tableGroup.MethodsForGL.GetAngle(tableGroup.Get_InternalID_From_GroupOrderID(GetGroupOrderID(InternalID)));
                }

                Vector4 position = new Vector4(GetPosition(InternalID));
                position *= GroupRot;
                position += new Vector4(GroupPos);
                Vector3 pos = position.Xyz;

                Utils.ToCameraCheckValue(ref pos);
                return pos;
            }

            private float GetObjAngleY_ToCamera(ushort ID)
            {
                float AngleY = MathHelper.DegreesToRadians(ReturnAngleY(ID));
                if (float.IsNaN(AngleY) || float.IsInfinity(AngleY)) { AngleY = 0; }
                return AngleY;
            }


            private Vector3[] GetObjPostion_ToMove_General(ushort ID)
            {
                Vector3[] pos = new Vector3[1];
                pos[0] = new Vector3(ReturnPositionX(ID), ReturnPositionY(ID), ReturnPositionZ(ID));
                Utils.ToMoveCheckLimits(ref pos);
                return pos;
            }


            private void SetObjPostion_ToMove_General(ushort ID, Vector3[] value)
            {
                if (value != null && value.Length >= 1)
                {
                    SetPositionX(ID, value[0].X);
                    SetPositionY(ID, value[0].Y);
                    SetPositionZ(ID, value[0].Z);
                }
            }

            private Vector3[] GetObjRotationAngles_ToMove(ushort ID)
            {
                Vector3[] v = new Vector3[1];
                v[0] = new Vector3(MathHelper.DegreesToRadians(ReturnAngleX(ID)), MathHelper.DegreesToRadians(ReturnAngleY(ID)), MathHelper.DegreesToRadians(ReturnAngleZ(ID)));
                Utils.ToMoveCheckLimits(ref v);
                return v;
            }

            private void SetObjRotationAngles_ToMove(ushort ID, Vector3[] value)
            {
                if (value != null && value.Length >= 1)
                {
                    SetAngleX(ID, MathHelper.Clamp(MathHelper.RadiansToDegrees(value[0].X), -360, 360));
                    SetAngleY(ID, MathHelper.Clamp(MathHelper.RadiansToDegrees(value[0].Y), -360, 360));
                    SetAngleZ(ID, MathHelper.Clamp(MathHelper.RadiansToDegrees(value[0].Z), -360, 360));
                }
            }

            #endregion
        }
    }
}
