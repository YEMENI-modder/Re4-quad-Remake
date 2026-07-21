using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Re4QuadExtremeEditor.src.Class;
using Re4QuadExtremeEditor.src.Class.Enums;
using Re4QuadExtremeEditor.src.Class.TreeNodeObj;

namespace Re4QuadExtremeEditor.src.Forms
{
    public partial class AddNewObjForm : Form
    {
        public event Class.CustomDelegates.ActivateMethod OnButtonOk_Click;

        public AddNewObjForm()
        {
            InitializeComponent();

            KeyPreview = true;
            if (DataBase.FileETS != null && DataBase.FileETS.Lines.Count < 10000)
            {
                comboBoxType.Items.Add(new GroupTypeObjForListBox(GroupType.ETS, Lang.GetText(eLang.AddNewETS)));
            }
            if (DataBase.FileITA != null && DataBase.FileITA.Lines.Count < 10000)
            {
                comboBoxType.Items.Add(new GroupTypeObjForListBox(GroupType.ITA, Lang.GetText(eLang.AddNewITA)));
            }
            if (DataBase.FileAEV != null && DataBase.FileAEV.Lines.Count < 10000)
            {
                comboBoxType.Items.Add(new GroupTypeObjForListBox(GroupType.AEV, Lang.GetText(eLang.AddNewAEV)));
            }
            if (DataBase.FileDSE != null && DataBase.FileDSE.Lines.Count < 10000)
            {
                comboBoxType.Items.Add(new GroupTypeObjForListBox(GroupType.DSE, Lang.GetText(eLang.AddNewDSE)));
            }
            if (DataBase.FileFSE != null && DataBase.FileFSE.Lines.Count < 10000)
            {
                comboBoxType.Items.Add(new GroupTypeObjForListBox(GroupType.FSE, Lang.GetText(eLang.AddNewFSE)));
            }
            if (DataBase.FileSAR != null && DataBase.FileSAR.Lines.Count < 10000)
            {
                comboBoxType.Items.Add(new GroupTypeObjForListBox(GroupType.SAR, Lang.GetText(eLang.AddNewSAR)));
            }
            if (DataBase.FileEAR != null && DataBase.FileEAR.Lines.Count < 10000)
            {
                comboBoxType.Items.Add(new GroupTypeObjForListBox(GroupType.EAR, Lang.GetText(eLang.AddNewEAR)));
            }
            if (DataBase.FileEMI != null && DataBase.FileEMI.Lines.Count < 10000)
            {
                comboBoxType.Items.Add(new GroupTypeObjForListBox(GroupType.EMI, Lang.GetText(eLang.AddNewEMI)));
            }
            if (DataBase.FileESE != null && DataBase.FileESE.Lines.Count < 10000)
            {
                comboBoxType.Items.Add(new GroupTypeObjForListBox(GroupType.ESE, Lang.GetText(eLang.AddNewESE)));
            }
            if (DataBase.FileLIT != null && DataBase.FileLIT.LightGroups.Lines.Count < 10000)
            {
                comboBoxType.Items.Add(new GroupTypeObjForListBox(GroupType.LIT_GROUPS, Lang.GetText(eLang.AddNewLITGroup)));
            }
            if (DataBase.FileLIT != null && DataBase.FileLIT.LightEntrys.Lines.Count < 10000)
            {
                comboBoxType.Items.Add(new GroupTypeObjForListBox(GroupType.LIT_ENTRYS, Lang.GetText(eLang.AddNewLITEntry)));
            }
            if (DataBase.FileEFF != null && DataBase.FileEFF._TableEffectEntry.EffectEntry.Count < 10000)
            {
                comboBoxType.Items.Add(new GroupTypeObjForListBox(GroupType.EFF_EffectEntry_ForAdd_Table7, "EFF Entry Table 7 (Group 0)"));
                comboBoxType.Items.Add(new GroupTypeObjForListBox(GroupType.EFF_EffectEntry_ForAdd_Table8, "EFF Entry Table 8 (Group 1)"));
            }
            if (DataBase.FileEFF != null && DataBase.FileEFF._Table7_Effect0_Group.Table_Effect_Group.Count < 10000)
            {
                comboBoxType.Items.Add(new GroupTypeObjForListBox(GroupType.EFF_Table7_Effect_0, "Add EFF Table 7 (Effect Group)"));
            }
            if (DataBase.FileEFF != null && DataBase.FileEFF._Table8_Effect1_Group.Table_Effect_Group.Count < 10000)
            {
                comboBoxType.Items.Add(new GroupTypeObjForListBox(GroupType.EFF_Table8_Effect_1, "Add EFF Table 8 (Effect Group)"));
            }
            if (DataBase.FileEFF != null && DataBase.FileEFF._Table9.Table9Lines.Count < 10000)
            {
                comboBoxType.Items.Add(new GroupTypeObjForListBox(GroupType.EFF_Table9, "Add EFF Table 9 (Path)"));
            }
            if (DataBase.FileEFF != null && DataBase.FileEFF._Table0.TableLines.Count < 10000)
            {
                comboBoxType.Items.Add(new GroupTypeObjForListBox(GroupType.EFF_Table0, "Add EFF Table 0"));
            }
            if (DataBase.FileEFF != null && DataBase.FileEFF._Table1.TableLines.Count < 10000)
            {
                comboBoxType.Items.Add(new GroupTypeObjForListBox(GroupType.EFF_Table1, "Add EFF Table 1"));
            }
            if (DataBase.FileEFF != null && DataBase.FileEFF._Table2.TableLines.Count < 10000)
            {
                comboBoxType.Items.Add(new GroupTypeObjForListBox(GroupType.EFF_Table2, "Add EFF Table 2"));
            }
            if (DataBase.FileEFF != null && DataBase.FileEFF._Table3.TableLines.Count < 10000)
            {
                comboBoxType.Items.Add(new GroupTypeObjForListBox(GroupType.EFF_Table3, "Add EFF Table 3"));
            }
            if (DataBase.FileEFF != null && DataBase.FileEFF._Table4.TableLines.Count < 10000)
            {
                comboBoxType.Items.Add(new GroupTypeObjForListBox(GroupType.EFF_Table4, "Add EFF Table 4"));
            }
            if (DataBase.FileEFF != null && DataBase.FileEFF._Table6.TableLines.Count < 10000)
            {
                comboBoxType.Items.Add(new GroupTypeObjForListBox(GroupType.EFF_Table6, "Add EFF Table 6"));
            }

            if (comboBoxType.Items.Count == 0)
            {
                comboBoxType.Items.Add(new GroupTypeObjForListBox(GroupType.NULL, Lang.GetText(eLang.AddNewNull)));
                comboBoxType.Enabled = false;
                numericUpDownAmount.Enabled = false;
                buttonOK.Enabled = false;
            }
            comboBoxType.SelectedIndex = 0;

            if (Lang.LoadedTranslation)
            {
                StartUpdateTranslation();
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            buttonCancel.Enabled = false;
            buttonOK.Enabled = false;
            if (comboBoxType.SelectedItem is GroupTypeObjForListBox gt)
            {
                if (gt.ID == GroupType.ETS)
                {
                    List<Object3D> nodes = new List<Object3D>();
                    for (int i = 0; i < numericUpDownAmount.Value; i++)
                    {
                        if (DataBase.FileETS.Lines.Count < 10000)
                        {
                            ushort NewId = DataBase.NodeETS.ChangeAmountMethods.AddNewLineID();
                            Object3D o = new Object3D();
                            o.Name = NewId.ToString();
                            o.Text = "";
                            o.Group = GroupType.ETS;
                            o.ObjLineRef = NewId;
                            nodes.Add(o);
                        }
                        else 
                        {
                            break;
                        }
                    }
                    DataBase.NodeETS.Nodes.AddRange(nodes.ToArray());
                    DataBase.NodeETS.Expand();
                }

                if (gt.ID == GroupType.ITA)
                {
                    List<Object3D> nodes = new List<Object3D>();
                    for (ushort i = 0; i < numericUpDownAmount.Value; i++)
                    {
                        if (DataBase.FileITA.Lines.Count < 10000)
                        {
                            ushort NewId = DataBase.NodeITA.ChangeAmountMethods.AddNewLineID();
                            Object3D o = new Object3D();
                            o.Name = NewId.ToString();
                            o.Text = "";
                            o.Group = GroupType.ITA;
                            o.ObjLineRef = NewId;
                            nodes.Add(o);
                        }
                        else
                        {
                            break;
                        }
                    }
                    DataBase.NodeITA.Nodes.AddRange(nodes.ToArray());
                    DataBase.NodeITA.Expand();
                }

                if (gt.ID == GroupType.AEV)
                {
                    List<Object3D> nodes = new List<Object3D>();
                    for (ushort i = 0; i < numericUpDownAmount.Value; i++)
                    {
                        if (DataBase.FileAEV.Lines.Count < 10000)
                        {
                            ushort NewId = DataBase.NodeAEV.ChangeAmountMethods.AddNewLineID();
                            Object3D o = new Object3D();
                            o.Name = NewId.ToString();
                            o.Text = "";
                            o.Group = GroupType.AEV;
                            o.ObjLineRef = NewId;
                            nodes.Add(o);
                        }
                        else
                        {
                            break;
                        }
                    }
                    DataBase.NodeAEV.Nodes.AddRange(nodes.ToArray());
                    DataBase.NodeAEV.Expand();
                }

                if (gt.ID == GroupType.DSE)
                {
                    List<Object3D> nodes = new List<Object3D>();
                    for (ushort i = 0; i < numericUpDownAmount.Value; i++)
                    {
                        if (DataBase.FileDSE.Lines.Count < 10000)
                        {
                            ushort NewId = DataBase.NodeDSE.ChangeAmountMethods.AddNewLineID();
                            Object3D o = new Object3D();
                            o.Name = NewId.ToString();
                            o.Text = "";
                            o.Group = GroupType.DSE;
                            o.ObjLineRef = NewId;
                            nodes.Add(o);
                        }
                        else
                        {
                            break;
                        }
                    }
                    DataBase.NodeDSE.Nodes.AddRange(nodes.ToArray());
                    DataBase.NodeDSE.Expand();
                }

                if (gt.ID == GroupType.FSE)
                {
                    List<Object3D> nodes = new List<Object3D>();
                    for (ushort i = 0; i < numericUpDownAmount.Value; i++)
                    {
                        if (DataBase.FileFSE.Lines.Count < 10000)
                        {
                            ushort NewId = DataBase.NodeFSE.ChangeAmountMethods.AddNewLineID();
                            Object3D o = new Object3D();
                            o.Name = NewId.ToString();
                            o.Text = "";
                            o.Group = GroupType.FSE;
                            o.ObjLineRef = NewId;
                            nodes.Add(o);
                        }
                        else
                        {
                            break;
                        }
                    }
                    DataBase.NodeFSE.Nodes.AddRange(nodes.ToArray());
                    DataBase.NodeFSE.Expand();
                }

                if (gt.ID == GroupType.SAR)
                {
                    List<Object3D> nodes = new List<Object3D>();
                    for (ushort i = 0; i < numericUpDownAmount.Value; i++)
                    {
                        if (DataBase.FileSAR.Lines.Count < 10000)
                        {
                            ushort NewId = DataBase.NodeSAR.ChangeAmountMethods.AddNewLineID();
                            Object3D o = new Object3D();
                            o.Name = NewId.ToString();
                            o.Text = "";
                            o.Group = GroupType.SAR;
                            o.ObjLineRef = NewId;
                            nodes.Add(o);
                        }
                        else
                        {
                            break;
                        }
                    }
                    DataBase.NodeSAR.Nodes.AddRange(nodes.ToArray());
                    DataBase.NodeSAR.Expand();
                }

                if (gt.ID == GroupType.EAR)
                {
                    List<Object3D> nodes = new List<Object3D>();
                    for (ushort i = 0; i < numericUpDownAmount.Value; i++)
                    {
                        if (DataBase.FileEAR.Lines.Count < 10000)
                        {
                            ushort NewId = DataBase.NodeEAR.ChangeAmountMethods.AddNewLineID();
                            Object3D o = new Object3D();
                            o.Name = NewId.ToString();
                            o.Text = "";
                            o.Group = GroupType.EAR;
                            o.ObjLineRef = NewId;
                            nodes.Add(o);
                        }
                        else
                        {
                            break;
                        }
                    }
                    DataBase.NodeEAR.Nodes.AddRange(nodes.ToArray());
                    DataBase.NodeEAR.Expand();
                }

                if (gt.ID == GroupType.EMI)
                {
                    List<Object3D> nodes = new List<Object3D>();
                    for (ushort i = 0; i < numericUpDownAmount.Value; i++)
                    {
                        if (DataBase.FileEMI.Lines.Count < 10000)
                        {
                            ushort NewId = DataBase.NodeEMI.ChangeAmountMethods.AddNewLineID();
                            Object3D o = new Object3D();
                            o.Name = NewId.ToString();
                            o.Text = "";
                            o.Group = GroupType.EMI;
                            o.ObjLineRef = NewId;
                            nodes.Add(o);
                        }
                        else
                        {
                            break;
                        }
                    }
                    DataBase.NodeEMI.Nodes.AddRange(nodes.ToArray());
                    DataBase.NodeEMI.Expand();
                }

                if (gt.ID == GroupType.ESE)
                {
                    List<Object3D> nodes = new List<Object3D>();
                    for (ushort i = 0; i < numericUpDownAmount.Value; i++)
                    {
                        if (DataBase.FileESE.Lines.Count < 10000)
                        {
                            ushort NewId = DataBase.NodeESE.ChangeAmountMethods.AddNewLineID();
                            Object3D o = new Object3D();
                            o.Name = NewId.ToString();
                            o.Text = "";
                            o.Group = GroupType.ESE;
                            o.ObjLineRef = NewId;
                            nodes.Add(o);
                        }
                        else
                        {
                            break;
                        }
                    }
                    DataBase.NodeESE.Nodes.AddRange(nodes.ToArray());
                    DataBase.NodeESE.Expand();
                }

                if (gt.ID == GroupType.LIT_GROUPS)
                {
                    List<Object3D> nodes = new List<Object3D>();
                    for (ushort i = 0; i < numericUpDownAmount.Value; i++)
                    {
                        if (DataBase.FileLIT.LightGroups.Lines.Count < 10000)
                        {
                            ushort NewId = DataBase.NodeLIT_Groups.ChangeAmountMethods.AddNewLineID();
                            Object3D o = new Object3D();
                            o.Name = NewId.ToString();
                            o.Text = "";
                            o.Group = GroupType.LIT_GROUPS;
                            o.ObjLineRef = NewId;
                            nodes.Add(o);
                        }
                        else
                        {
                            break;
                        }
                    }
                    DataBase.NodeLIT_Groups.Nodes.AddRange(nodes.ToArray());
                    DataBase.NodeLIT_Groups.Expand();
                }

                if (gt.ID == GroupType.LIT_ENTRYS)
                {
                    List<Object3D> nodes = new List<Object3D>();
                    for (ushort i = 0; i < numericUpDownAmount.Value; i++)
                    {
                        if (DataBase.FileLIT.LightEntrys.Lines.Count < 10000)
                        {
                            ushort NewId = DataBase.NodeLIT_Entrys.ChangeAmountMethods.AddNewLineID();
                            Object3D o = new Object3D();
                            o.Name = NewId.ToString();
                            o.Text = "";
                            o.Group = GroupType.LIT_ENTRYS;
                            o.ObjLineRef = NewId;
                            nodes.Add(o);
                        }
                        else
                        {
                            break;
                        }
                    }
                    DataBase.NodeLIT_Entrys.Nodes.AddRange(nodes.ToArray());
                    DataBase.NodeLIT_Entrys.Expand();
                }

                if (gt.ID == GroupType.EFF_EffectEntry_ForAdd_Table7 || gt.ID == GroupType.EFF_EffectEntry_ForAdd_Table8)
                {
                    byte tableID = gt.ID == GroupType.EFF_EffectEntry_ForAdd_Table7 ? (byte)EffectEntryTableID.Table7 : (byte)EffectEntryTableID.Table8;

                    List<Object3D> nodes = new List<Object3D>();
                    for (ushort i = 0; i < numericUpDownAmount.Value; i++)
                    {
                        if (DataBase.FileEFF._TableEffectEntry.EffectEntry.Count < 10000)
                        {
                            ushort NewId = DataBase.NodeEFF_EffectEntry.ChangeAmountMethods.AddNewLineID();
                            DataBase.NodeEFF_EffectEntry.PropertyMethods.SetTableID(NewId, tableID);
                            Object3D o = new Object3D();
                            o.Name = NewId.ToString();
                            o.Text = "";
                            o.Group = GroupType.EFF_EffectEntry;
                            o.ObjLineRef = NewId;
                            nodes.Add(o);
                        }
                        else
                        {
                            break;
                        }
                    }
                    DataBase.NodeEFF_EffectEntry.Nodes.AddRange(nodes.ToArray());
                    DataBase.NodeEFF_EffectEntry.Expand();
                }

                if (gt.ID == GroupType.EFF_Table7_Effect_0)
                {
                    List<Object3D> nodes = new List<Object3D>();
                    for (ushort i = 0; i < numericUpDownAmount.Value; i++)
                    {
                        if (DataBase.FileEFF._Table7_Effect0_Group.Table_Effect_Group.Count < 10000)
                        {
                            ushort NewId = DataBase.NodeEFF_Table7_Effect_0.ChangeAmountMethods.AddNewLineID();
                            Object3D o = new Object3D();
                            o.Name = NewId.ToString();
                            o.Text = "";
                            o.Group = GroupType.EFF_Table7_Effect_0;
                            o.ObjLineRef = NewId;
                            nodes.Add(o);
                        }
                        else
                        {
                            break;
                        }
                    }
                    DataBase.NodeEFF_Table7_Effect_0.Nodes.AddRange(nodes.ToArray());
                    DataBase.NodeEFF_Table7_Effect_0.Expand();
                }

                if (gt.ID == GroupType.EFF_Table8_Effect_1)
                {
                    List<Object3D> nodes = new List<Object3D>();
                    for (ushort i = 0; i < numericUpDownAmount.Value; i++)
                    {
                        if (DataBase.FileEFF._Table8_Effect1_Group.Table_Effect_Group.Count < 10000)
                        {
                            ushort NewId = DataBase.NodeEFF_Table8_Effect_1.ChangeAmountMethods.AddNewLineID();
                            Object3D o = new Object3D();
                            o.Name = NewId.ToString();
                            o.Text = "";
                            o.Group = GroupType.EFF_Table8_Effect_1;
                            o.ObjLineRef = NewId;
                            nodes.Add(o);
                        }
                        else
                        {
                            break;
                        }
                    }
                    DataBase.NodeEFF_Table8_Effect_1.Nodes.AddRange(nodes.ToArray());
                    DataBase.NodeEFF_Table8_Effect_1.Expand();
                }

                if (gt.ID == GroupType.EFF_Table9)
                {
                    List<Object3D> nodes = new List<Object3D>();
                    for (ushort i = 0; i < numericUpDownAmount.Value; i++)
                    {
                        if (DataBase.FileEFF._Table9.Table9Lines.Count < 10000)
                        {
                            ushort NewId = DataBase.NodeEFF_Table9.ChangeAmountMethods.AddNewLineID();
                            Object3D o = new Object3D();
                            o.Name = NewId.ToString();
                            o.Text = "";
                            o.Group = GroupType.EFF_Table9;
                            o.ObjLineRef = NewId;
                            nodes.Add(o);
                        }
                        else
                        {
                            break;
                        }
                    }
                    DataBase.NodeEFF_Table9.Nodes.AddRange(nodes.ToArray());
                    DataBase.NodeEFF_Table9.Expand();
                }

                if (gt.ID == GroupType.EFF_Table0)
                {
                    List<Object3D> nodes = new List<Object3D>();
                    for (ushort i = 0; i < numericUpDownAmount.Value; i++)
                    {
                        if (DataBase.FileEFF._Table0.TableLines.Count < 10000)
                        {
                            ushort NewId = DataBase.NodeEFF_Table0.ChangeAmountMethods.AddNewLineID();
                            Object3D o = new Object3D();
                            o.Name = NewId.ToString();
                            o.Text = "";
                            o.Group = GroupType.EFF_Table0;
                            o.ObjLineRef = NewId;
                            nodes.Add(o);
                        }
                        else
                        {
                            break;
                        }
                    }
                    DataBase.NodeEFF_Table0.Nodes.AddRange(nodes.ToArray());
                    DataBase.NodeEFF_Table0.Expand();
                }

                if (gt.ID == GroupType.EFF_Table1)
                {
                    List<Object3D> nodes = new List<Object3D>();
                    for (ushort i = 0; i < numericUpDownAmount.Value; i++)
                    {
                        if (DataBase.FileEFF._Table1.TableLines.Count < 10000)
                        {
                            ushort NewId = DataBase.NodeEFF_Table1.ChangeAmountMethods.AddNewLineID();
                            Object3D o = new Object3D();
                            o.Name = NewId.ToString();
                            o.Text = "";
                            o.Group = GroupType.EFF_Table1;
                            o.ObjLineRef = NewId;
                            nodes.Add(o);
                        }
                        else
                        {
                            break;
                        }
                    }
                    DataBase.NodeEFF_Table1.Nodes.AddRange(nodes.ToArray());
                    DataBase.NodeEFF_Table1.Expand();
                }

                if (gt.ID == GroupType.EFF_Table2)
                {
                    List<Object3D> nodes = new List<Object3D>();
                    for (ushort i = 0; i < numericUpDownAmount.Value; i++)
                    {
                        if (DataBase.FileEFF._Table2.TableLines.Count < 10000)
                        {
                            ushort NewId = DataBase.NodeEFF_Table2.ChangeAmountMethods.AddNewLineID();
                            Object3D o = new Object3D();
                            o.Name = NewId.ToString();
                            o.Text = "";
                            o.Group = GroupType.EFF_Table2;
                            o.ObjLineRef = NewId;
                            nodes.Add(o);
                        }
                        else
                        {
                            break;
                        }
                    }
                    DataBase.NodeEFF_Table2.Nodes.AddRange(nodes.ToArray());
                    DataBase.NodeEFF_Table2.Expand();
                }

                if (gt.ID == GroupType.EFF_Table3)
                {
                    List<Object3D> nodes = new List<Object3D>();
                    for (ushort i = 0; i < numericUpDownAmount.Value; i++)
                    {
                        if (DataBase.FileEFF._Table3.TableLines.Count < 10000)
                        {
                            ushort NewId = DataBase.NodeEFF_Table3.ChangeAmountMethods.AddNewLineID();
                            Object3D o = new Object3D();
                            o.Name = NewId.ToString();
                            o.Text = "";
                            o.Group = GroupType.EFF_Table3;
                            o.ObjLineRef = NewId;
                            nodes.Add(o);
                        }
                        else
                        {
                            break;
                        }
                    }
                    DataBase.NodeEFF_Table3.Nodes.AddRange(nodes.ToArray());
                    DataBase.NodeEFF_Table3.Expand();
                }

                if (gt.ID == GroupType.EFF_Table4)
                {
                    List<Object3D> nodes = new List<Object3D>();
                    for (ushort i = 0; i < numericUpDownAmount.Value; i++)
                    {
                        if (DataBase.FileEFF._Table4.TableLines.Count < 10000)
                        {
                            ushort NewId = DataBase.NodeEFF_Table4.ChangeAmountMethods.AddNewLineID();
                            Object3D o = new Object3D();
                            o.Name = NewId.ToString();
                            o.Text = "";
                            o.Group = GroupType.EFF_Table4;
                            o.ObjLineRef = NewId;
                            nodes.Add(o);
                        }
                        else
                        {
                            break;
                        }
                    }
                    DataBase.NodeEFF_Table4.Nodes.AddRange(nodes.ToArray());
                    DataBase.NodeEFF_Table4.Expand();
                }

                if (gt.ID == GroupType.EFF_Table6)
                {
                    List<Object3D> nodes = new List<Object3D>();
                    for (ushort i = 0; i < numericUpDownAmount.Value; i++)
                    {
                        if (DataBase.FileEFF._Table6.TableLines.Count < 10000)
                        {
                            ushort NewId = DataBase.NodeEFF_Table6.ChangeAmountMethods.AddNewLineID();
                            Object3D o = new Object3D();
                            o.Name = NewId.ToString();
                            o.Text = "";
                            o.Group = GroupType.EFF_Table6;
                            o.ObjLineRef = NewId;
                            nodes.Add(o);
                        }
                        else
                        {
                            break;
                        }
                    }
                    DataBase.NodeEFF_Table6.Nodes.AddRange(nodes.ToArray());
                    DataBase.NodeEFF_Table6.Expand();
                }
            }
            OnButtonOk_Click?.Invoke();
            Close();
        }

        private void AddNewObjForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private void StartUpdateTranslation() 
        {
            this.Text = Lang.GetText(eLang.AddNewObjForm);
            buttonCancel.Text = Lang.GetText(eLang.buttonCancel);
            buttonOK.Text = Lang.GetText(eLang.buttonOK);
            labelAmountInfo.Text = Lang.GetText(eLang.labelAmountInfo);
            labelTypeInfo.Text = Lang.GetText(eLang.labelTypeInfo);
        }

        /// <summary>
        /// Custom-draws comboBoxType's dropdown list so it follows dark mode (matching
        /// GridComboBox's approach). Without this, the dropdown list always renders with
        /// the OS-default light background regardless of dark mode, making the text
        /// unreadable when dark mode is on.
        /// </summary>
        private void comboBoxType_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) { return; }

            Color backColor = Globals.DarkMode ? Color.FromArgb(0x2D, 0x2D, 0x30) : System.Drawing.SystemColors.Window;
            bool isSelected = e.State.HasFlag(DrawItemState.Selected);

            e.Graphics.FillRectangle(new SolidBrush(isSelected ? Color.FromArgb(0x70, 0xBB, 0xDB) : backColor), e.Bounds);

            Color textColor = isSelected ? Color.Black : (Globals.DarkMode ? Color.White : Color.Black);
            e.Graphics.DrawString(comboBoxType.Items[e.Index].ToString(), e.Font, new SolidBrush(textColor), e.Bounds.Left, e.Bounds.Top);

            e.DrawFocusRectangle();
        }
    }
}
