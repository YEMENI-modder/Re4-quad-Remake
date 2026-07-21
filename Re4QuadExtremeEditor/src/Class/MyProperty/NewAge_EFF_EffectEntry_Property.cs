using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Drawing.Design;
using Re4QuadExtremeEditor.src.Class.Enums;
using Re4QuadExtremeEditor.src.Class.Interfaces;
using Re4QuadExtremeEditor.src.Class.ObjMethods;
using Re4QuadExtremeEditor.src.Class.MyProperty.CustomAttribute;
using Re4QuadExtremeEditor.src.Class.MyProperty.CustomTypeConverter;
using Re4QuadExtremeEditor.src.Class.MyProperty.CustomUITypeEditor;
using Re4QuadExtremeEditor.src.Class.MyProperty.CustomCollection;

namespace Re4QuadExtremeEditor.src.Class.MyProperty
{
    [DefaultProperty(nameof(InternalLineID))]
    public class NewAge_EFF_EffectEntry_Property : BaseProperty, IInternalID
    {
        public override Type GetClassType()
        {
            return typeof(NewAge_EFF_EffectEntry_Property);
        }

        private const GroupType groupType = GroupType.EFF_EffectEntry;

        private ushort InternalID = ushort.MaxValue;

        private NewAge_EFF_EffectEntry_Methods Methods = null;

        public ushort GetInternalID()
        {
            return InternalID;
        }

        public GroupType GetGroupType()
        {
            return groupType;
        }

        protected override void SetFloatType(bool IsHex)
        {
            ChangePropertyIsBrowsable(nameof(PROP_PositionX), !IsHex);
            ChangePropertyIsBrowsable(nameof(PROP_PositionY), !IsHex);
            ChangePropertyIsBrowsable(nameof(PROP_PositionZ), !IsHex);
            ChangePropertyIsBrowsable(nameof(PROP_AngleX), !IsHex);
            ChangePropertyIsBrowsable(nameof(PROP_AngleY), !IsHex);
            ChangePropertyIsBrowsable(nameof(PROP_AngleZ), !IsHex);
            ChangePropertyIsBrowsable(nameof(PROP_PositionX_Hex), IsHex);
            ChangePropertyIsBrowsable(nameof(PROP_PositionY_Hex), IsHex);
            ChangePropertyIsBrowsable(nameof(PROP_PositionZ_Hex), IsHex);
            ChangePropertyIsBrowsable(nameof(PROP_AngleX_Hex), IsHex);
            ChangePropertyIsBrowsable(nameof(PROP_AngleY_Hex), IsHex);
            ChangePropertyIsBrowsable(nameof(PROP_AngleZ_Hex), IsHex);
        }

        private void SetPropertyTexts()
        {
            ChangePropertyName(nameof(Line), Lang.GetAttributeText(aLang.NewAge_LineArrayDisplayName).Replace("<<Lenght>>", "Variable"));
        }

        public NewAge_EFF_EffectEntry_Property(NewAge_EFF_EffectEntry_Property prop, bool ForMultiSelection = false)
        {
            NewAge_EFF_EffectEntry_PropertyConstructor(prop.InternalID, prop.updateMethods, prop.Methods, ForMultiSelection);
        }

        public NewAge_EFF_EffectEntry_Property(ushort InternalID, UpdateMethods updateMethods, NewAge_EFF_EffectEntry_Methods Methods, bool ForMultiSelection = false) : base()
        {
            NewAge_EFF_EffectEntry_PropertyConstructor(InternalID, updateMethods, Methods, ForMultiSelection);
        }

        private void NewAge_EFF_EffectEntry_PropertyConstructor(ushort InternalID, UpdateMethods updateMethods, NewAge_EFF_EffectEntry_Methods Methods, bool ForMultiSelection = false)
        {
            this.InternalID = InternalID;
            this.updateMethods = updateMethods;
            this.Methods = Methods;

            if (!ForMultiSelection)
            {
                SetThis(this);
            }

            SetFloatType(Globals.PropertyGridUseHexFloat);
            SetPropertyTexts();
        }

        #region Category Ids
        private const int CategoryID_UnderDevelopment = 0;
        private const int CategoryID_InternalLineID = 1;
        private const int CategoryID_LineArray = 2;
        private const int CategoryID_Position = 3;
        private const int CategoryID_Angle = 4;
        private const int CategoryID_EFF_EffectEntry = 5;
        private const int CategoryID_EFF_Basic = 10;
        private const int CategoryID_EFF_Random = 11;
        private const int CategoryID_EFF_Speed = 12;
        private const int CategoryID_EFF_RandomSpeed = 13;
        private const int CategoryID_EFF_Acceleration = 14;
        private const int CategoryID_EFF_RandomAcceleration = 15;
        private const int CategoryID_EFF_RandomRotation = 16;
        private const int CategoryID_EFF_RotationAcceleration = 17;
        private const int CategoryID_EFF_RandomRotationAcceleration = 18;
        private const int CategoryID_EFF_Size = 19;
        private const int CategoryID_EFF_Color = 20;
        private const int CategoryID_EFF_Timing = 21;
        private const int CategoryID_EFF_Simulation = 22;
        private const int CategoryID_EFF_Work = 23;
        private const int CategoryID_EFF_Vector = 24;
        private const int CategoryID_EFF_Spline = 25;
        private const int CategoryID_EFF_Path = 26;
        private const int CategoryID_EFF_Control = 27;
        private const int CategoryID_EFF_ControlPath = 28;
        #endregion

        #region firt propertys

        [CustomCategory(aLang.UnderDevelopment_Category)]
        [CustomDisplayName(aLang.UnderDevelopment_DisplayName)]
        [CustomDescription(aLang.UnderDevelopment_Description)]
        [DefaultValue(null)]
        [ReadOnly(true)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(0, CategoryID_UnderDevelopment)]
        public string UnderDevelopment { get => Lang.GetAttributeText(aLang.UnderDevelopment_Value); }

        [CustomCategory(aLang.NewAge_InternalLineIDCategory)]
        [CustomDisplayName(aLang.NewAge_InternalLineIDDisplayName)]
        [CustomDescription(aLang.NewAge_InternalLineIDDescription)]
        [DefaultValue(null)]
        [ReadOnly(true)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(1, CategoryID_InternalLineID)]
        public string InternalLineID { get => GetInternalID().ToString(); }

        [CustomCategory(aLang.EFF_EffectEntry_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_GroupOrderID_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(true)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(2, CategoryID_InternalLineID)]
        public ushort PROP_GroupOrderID { get => Methods.GetGroupOrderID(InternalID); }

        [CustomCategory(aLang.EFF_EffectEntry_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_TableID_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(true)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3, CategoryID_InternalLineID)]
        public byte PROP_TableID { get => Methods.GetTableID(InternalID); }

        [CustomCategory(aLang.NewAge_LineArrayCategory)]
        [CustomDisplayName(aLang.NewAge_LineArrayDisplayName)]
        [CustomDescription(aLang.NewAge_LineArrayDescription)]
        [TypeConverter(typeof(ByteArrayTypeConverter))]
        [Editor(typeof(NoneUITypeEditor), typeof(UITypeEditor))]
        [HexNumber()]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(8, CategoryID_LineArray)]
        public byte[] Line
        {
            get => Methods.ReturnLine(InternalID);
            set
            {
                byte[] current = Methods.ReturnLine(InternalID);
                byte[] _set = new byte[current.Length];
                byte[] insert = value.Take(current.Length).ToArray();
                current.CopyTo(_set, 0);
                insert.CopyTo(_set, 0);
                Methods.SetLine(InternalID, _set);

                updateMethods.UpdateMoveObjSelection();
                updateMethods.UpdateOrbitCamera();
                updateMethods.UpdateGL();
            }
        }

        #endregion

        #region Position Angle

        [CustomCategory(aLang.EFF_EffectEntry_Position_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_PositionX_Float_DisplayName)]
        [TypeConverter(typeof(FloatNumberTypeConverter))]
        [FloatNumber()]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [AllowInMultiSelect()]
        [DynamicTypeDescriptor.Id(1900, CategoryID_Position)]
        public float PROP_PositionX
        {
            get => Methods.ReturnPositionX(InternalID);
            set
            {
                Methods.SetPositionX(InternalID, value);
                updateMethods.UpdateOrbitCamera();
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Position_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_PositionY_Float_DisplayName)]
        [TypeConverter(typeof(FloatNumberTypeConverter))]
        [FloatNumber()]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [AllowInMultiSelect()]
        [DynamicTypeDescriptor.Id(2000, CategoryID_Position)]
        public float PROP_PositionY
        {
            get => Methods.ReturnPositionY(InternalID);
            set
            {
                Methods.SetPositionY(InternalID, value);
                updateMethods.UpdateOrbitCamera();
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Position_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_PositionZ_Float_DisplayName)]
        [TypeConverter(typeof(FloatNumberTypeConverter))]
        [FloatNumber()]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [AllowInMultiSelect()]
        [DynamicTypeDescriptor.Id(2100, CategoryID_Position)]
        public float PROP_PositionZ
        {
            get => Methods.ReturnPositionZ(InternalID);
            set
            {
                Methods.SetPositionZ(InternalID, value);
                updateMethods.UpdateOrbitCamera();
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Position_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_PositionX_Hex_DisplayName)]
        [TypeConverter(typeof(HexNumberTypeConverter))]
        [HexNumber()]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(false)]
        [DynamicTypeDescriptor.Id(1000, CategoryID_Position)]
        public uint PROP_PositionX_Hex
        {
            get => Methods.ReturnPositionX_Hex(InternalID);
            set
            {
                Methods.SetPositionX_Hex(InternalID, value);
                updateMethods.UpdateOrbitCamera();
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Position_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_PositionY_Hex_DisplayName)]
        [TypeConverter(typeof(HexNumberTypeConverter))]
        [HexNumber()]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(false)]
        [DynamicTypeDescriptor.Id(1100, CategoryID_Position)]
        public uint PROP_PositionY_Hex
        {
            get => Methods.ReturnPositionY_Hex(InternalID);
            set
            {
                Methods.SetPositionY_Hex(InternalID, value);
                updateMethods.UpdateOrbitCamera();
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Position_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_PositionZ_Hex_DisplayName)]
        [TypeConverter(typeof(HexNumberTypeConverter))]
        [HexNumber()]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(false)]
        [DynamicTypeDescriptor.Id(1200, CategoryID_Position)]
        public uint PROP_PositionZ_Hex
        {
            get => Methods.ReturnPositionZ_Hex(InternalID);
            set
            {
                Methods.SetPositionZ_Hex(InternalID, value);
                updateMethods.UpdateOrbitCamera();
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Angle_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_AngleX_Float_DisplayName)]
        [TypeConverter(typeof(FloatNumberTypeConverter))]
        [FloatNumber()]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [AllowInMultiSelect()]
        [DynamicTypeDescriptor.Id(2200, CategoryID_Angle)]
        public float PROP_AngleX
        {
            get => Methods.ReturnAngleX(InternalID);
            set
            {
                Methods.SetAngleX(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Angle_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_AngleY_Float_DisplayName)]
        [TypeConverter(typeof(FloatNumberTypeConverter))]
        [FloatNumber()]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [AllowInMultiSelect()]
        [DynamicTypeDescriptor.Id(2300, CategoryID_Angle)]
        public float PROP_AngleY
        {
            get => Methods.ReturnAngleY(InternalID);
            set
            {
                Methods.SetAngleY(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Angle_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_AngleZ_Float_DisplayName)]
        [TypeConverter(typeof(FloatNumberTypeConverter))]
        [FloatNumber()]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [AllowInMultiSelect()]
        [DynamicTypeDescriptor.Id(2400, CategoryID_Angle)]
        public float PROP_AngleZ
        {
            get => Methods.ReturnAngleZ(InternalID);
            set
            {
                Methods.SetAngleZ(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Angle_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_AngleX_Hex_DisplayName)]
        [TypeConverter(typeof(HexNumberTypeConverter))]
        [HexNumber()]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(false)]
        [DynamicTypeDescriptor.Id(2500, CategoryID_Angle)]
        public uint PROP_AngleX_Hex
        {
            get => Methods.ReturnAngleX_Hex(InternalID);
            set
            {
                Methods.SetAngleX_Hex(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Angle_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_AngleY_Hex_DisplayName)]
        [TypeConverter(typeof(HexNumberTypeConverter))]
        [HexNumber()]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(false)]
        [DynamicTypeDescriptor.Id(2600, CategoryID_Angle)]
        public uint PROP_AngleY_Hex
        {
            get => Methods.ReturnAngleY_Hex(InternalID);
            set
            {
                Methods.SetAngleY_Hex(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Angle_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_AngleZ_Hex_DisplayName)]
        [TypeConverter(typeof(HexNumberTypeConverter))]
        [HexNumber()]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(false)]
        [DynamicTypeDescriptor.Id(2700, CategoryID_Angle)]
        public uint PROP_AngleZ_Hex
        {
            get => Methods.ReturnAngleZ_Hex(InternalID);
            set
            {
                Methods.SetAngleZ_Hex(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        #endregion

        #region Additional Effect Properties (from mod.rs)

        [CustomCategory(aLang.EFF_EffectEntry_Basic_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_StateID_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3000, CategoryID_EFF_Basic)]
        public byte PROP_StateID
        {
            get => Methods.ReturnStateID(InternalID);
            set
            {
                Methods.SetStateID(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Basic_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_EspID_DisplayName)]
        [TypeConverter(typeof(HexNumberTypeConverter))]
        [HexNumber()]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3010, CategoryID_EFF_Basic)]
        public byte PROP_EspID
        {
            get => Methods.ReturnEspID(InternalID);
            set
            {
                Methods.SetEspID(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Basic_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_TextureID_DisplayName)]
        [TypeConverter(typeof(HexNumberTypeConverter))]
        [HexNumber()]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3020, CategoryID_EFF_Basic)]
        public byte PROP_TextureID
        {
            get => Methods.ReturnTextureID(InternalID);
            set
            {
                Methods.SetTextureID(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Basic_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_Time_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3030, CategoryID_EFF_Basic)]
        public ushort PROP_Time
        {
            get => Methods.ReturnTime(InternalID);
            set
            {
                Methods.SetTime(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Basic_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_Parent_DisplayName)]
        [TypeConverter(typeof(HexNumberTypeConverter))]
        [HexNumber()]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3040, CategoryID_EFF_Basic)]
        public byte PROP_Parent
        {
            get => Methods.ReturnParent(InternalID);
            set
            {
                Methods.SetParent(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Basic_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_Part_DisplayName)]
        [TypeConverter(typeof(HexNumberTypeConverter))]
        [HexNumber()]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3050, CategoryID_EFF_Basic)]
        public byte PROP_Part
        {
            get => Methods.ReturnPart(InternalID);
            set
            {
                Methods.SetPart(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Basic_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_Flags_DisplayName)]
        [TypeConverter(typeof(HexNumberTypeConverter))]
        [HexNumber()]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3060, CategoryID_EFF_Basic)]
        public uint PROP_Flags
        {
            get => Methods.ReturnFlags(InternalID);
            set
            {
                Methods.SetFlags(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Random_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_RandomX_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3070, CategoryID_EFF_Random)]
        public float PROP_RandomX
        {
            get => Methods.ReturnRandomX(InternalID);
            set
            {
                Methods.SetRandomX(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Random_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_RandomY_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3080, CategoryID_EFF_Random)]
        public float PROP_RandomY
        {
            get => Methods.ReturnRandomY(InternalID);
            set
            {
                Methods.SetRandomY(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Random_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_RandomZ_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3090, CategoryID_EFF_Random)]
        public float PROP_RandomZ
        {
            get => Methods.ReturnRandomZ(InternalID);
            set
            {
                Methods.SetRandomZ(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Speed_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_SpeedX_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3100, CategoryID_EFF_Speed)]
        public float PROP_SpeedX
        {
            get => Methods.ReturnSpeedX(InternalID);
            set
            {
                Methods.SetSpeedX(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Speed_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_SpeedY_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3110, CategoryID_EFF_Speed)]
        public float PROP_SpeedY
        {
            get => Methods.ReturnSpeedY(InternalID);
            set
            {
                Methods.SetSpeedY(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Speed_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_SpeedZ_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3120, CategoryID_EFF_Speed)]
        public float PROP_SpeedZ
        {
            get => Methods.ReturnSpeedZ(InternalID);
            set
            {
                Methods.SetSpeedZ(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Speed_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_DeltaSpeed_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3130, CategoryID_EFF_Speed)]
        public float PROP_DeltaSpeed
        {
            get => Methods.ReturnDeltaSpeed(InternalID);
            set
            {
                Methods.SetDeltaSpeed(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_RandomSpeed_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_RandomSpeedX_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3140, CategoryID_EFF_RandomSpeed)]
        public float PROP_RandomSpeedX
        {
            get => Methods.ReturnRandomSpeedX(InternalID);
            set
            {
                Methods.SetRandomSpeedX(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_RandomSpeed_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_RandomSpeedY_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3150, CategoryID_EFF_RandomSpeed)]
        public float PROP_RandomSpeedY
        {
            get => Methods.ReturnRandomSpeedY(InternalID);
            set
            {
                Methods.SetRandomSpeedY(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_RandomSpeed_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_RandomSpeedZ_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3160, CategoryID_EFF_RandomSpeed)]
        public float PROP_RandomSpeedZ
        {
            get => Methods.ReturnRandomSpeedZ(InternalID);
            set
            {
                Methods.SetRandomSpeedZ(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Acceleration_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_AccelerationX_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3170, CategoryID_EFF_Acceleration)]
        public float PROP_AccelerationX
        {
            get => Methods.ReturnAccelerationX(InternalID);
            set
            {
                Methods.SetAccelerationX(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Acceleration_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_AccelerationY_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3180, CategoryID_EFF_Acceleration)]
        public float PROP_AccelerationY
        {
            get => Methods.ReturnAccelerationY(InternalID);
            set
            {
                Methods.SetAccelerationY(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Acceleration_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_AccelerationZ_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3190, CategoryID_EFF_Acceleration)]
        public float PROP_AccelerationZ
        {
            get => Methods.ReturnAccelerationZ(InternalID);
            set
            {
                Methods.SetAccelerationZ(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_RandomAcceleration_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_RandomAccelerationX_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3200, CategoryID_EFF_RandomAcceleration)]
        public float PROP_RandomAccelerationX
        {
            get => Methods.ReturnRandomAccelerationX(InternalID);
            set
            {
                Methods.SetRandomAccelerationX(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_RandomAcceleration_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_RandomAccelerationY_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3210, CategoryID_EFF_RandomAcceleration)]
        public float PROP_RandomAccelerationY
        {
            get => Methods.ReturnRandomAccelerationY(InternalID);
            set
            {
                Methods.SetRandomAccelerationY(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_RandomAcceleration_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_RandomAccelerationZ_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3220, CategoryID_EFF_RandomAcceleration)]
        public float PROP_RandomAccelerationZ
        {
            get => Methods.ReturnRandomAccelerationZ(InternalID);
            set
            {
                Methods.SetRandomAccelerationZ(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_RandomRotation_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_RandomRotationX_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3230, CategoryID_EFF_RandomRotation)]
        public float PROP_RandomRotationX
        {
            get => Methods.ReturnRandomRotationX(InternalID);
            set
            {
                Methods.SetRandomRotationX(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_RandomRotation_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_RandomRotationY_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3240, CategoryID_EFF_RandomRotation)]
        public float PROP_RandomRotationY
        {
            get => Methods.ReturnRandomRotationY(InternalID);
            set
            {
                Methods.SetRandomRotationY(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_RandomRotation_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_RandomRotationZ_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3250, CategoryID_EFF_RandomRotation)]
        public float PROP_RandomRotationZ
        {
            get => Methods.ReturnRandomRotationZ(InternalID);
            set
            {
                Methods.SetRandomRotationZ(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_RotationAcceleration_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_RotationAccelerationX_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3260, CategoryID_EFF_RotationAcceleration)]
        public float PROP_RotationAccelerationX
        {
            get => Methods.ReturnRotationAccelerationX(InternalID);
            set
            {
                Methods.SetRotationAccelerationX(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_RotationAcceleration_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_RotationAccelerationY_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3270, CategoryID_EFF_RotationAcceleration)]
        public float PROP_RotationAccelerationY
        {
            get => Methods.ReturnRotationAccelerationY(InternalID);
            set
            {
                Methods.SetRotationAccelerationY(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_RotationAcceleration_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_RotationAccelerationZ_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3280, CategoryID_EFF_RotationAcceleration)]
        public float PROP_RotationAccelerationZ
        {
            get => Methods.ReturnRotationAccelerationZ(InternalID);
            set
            {
                Methods.SetRotationAccelerationZ(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_RandomRotationAcceleration_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_RandomRotationAccelerationX_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3290, CategoryID_EFF_RandomRotationAcceleration)]
        public float PROP_RandomRotationAccelerationX
        {
            get => Methods.ReturnRandomRotationAccelerationX(InternalID);
            set
            {
                Methods.SetRandomRotationAccelerationX(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_RandomRotationAcceleration_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_RandomRotationAccelerationY_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3300, CategoryID_EFF_RandomRotationAcceleration)]
        public float PROP_RandomRotationAccelerationY
        {
            get => Methods.ReturnRandomRotationAccelerationY(InternalID);
            set
            {
                Methods.SetRandomRotationAccelerationY(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_RandomRotationAcceleration_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_RandomRotationAccelerationZ_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3310, CategoryID_EFF_RandomRotationAcceleration)]
        public float PROP_RandomRotationAccelerationZ
        {
            get => Methods.ReturnRandomRotationAccelerationZ(InternalID);
            set
            {
                Methods.SetRandomRotationAccelerationZ(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Size_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_Width_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3320, CategoryID_EFF_Size)]
        public float PROP_Width
        {
            get => Methods.ReturnWidth(InternalID);
            set
            {
                Methods.SetWidth(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Size_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_Height_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3330, CategoryID_EFF_Size)]
        public float PROP_Height
        {
            get => Methods.ReturnHeight(InternalID);
            set
            {
                Methods.SetHeight(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Size_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_RandomSize_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3340, CategoryID_EFF_Size)]
        public float PROP_RandomSize
        {
            get => Methods.ReturnRandomSize(InternalID);
            set
            {
                Methods.SetRandomSize(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Size_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_Grow_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3350, CategoryID_EFF_Size)]
        public float PROP_Grow
        {
            get => Methods.ReturnGrow(InternalID);
            set
            {
                Methods.SetGrow(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Size_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_DeltaGrow_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3360, CategoryID_EFF_Size)]
        public float PROP_DeltaGrow
        {
            get => Methods.ReturnDeltaGrow(InternalID);
            set
            {
                Methods.SetDeltaGrow(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Color_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_ColorR_DisplayName)]
        [TypeConverter(typeof(HexNumberTypeConverter))]
        [HexNumber()]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3370, CategoryID_EFF_Color)]
        public byte PROP_ColorR
        {
            get => Methods.ReturnColorR(InternalID);
            set
            {
                Methods.SetColorR(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Color_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_ColorG_DisplayName)]
        [TypeConverter(typeof(HexNumberTypeConverter))]
        [HexNumber()]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3380, CategoryID_EFF_Color)]
        public byte PROP_ColorG
        {
            get => Methods.ReturnColorG(InternalID);
            set
            {
                Methods.SetColorG(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Color_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_ColorB_DisplayName)]
        [TypeConverter(typeof(HexNumberTypeConverter))]
        [HexNumber()]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3390, CategoryID_EFF_Color)]
        public byte PROP_ColorB
        {
            get => Methods.ReturnColorB(InternalID);
            set
            {
                Methods.SetColorB(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Color_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_ColorA_DisplayName)]
        [TypeConverter(typeof(HexNumberTypeConverter))]
        [HexNumber()]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3400, CategoryID_EFF_Color)]
        public byte PROP_ColorA
        {
            get => Methods.ReturnColorA(InternalID);
            set
            {
                Methods.SetColorA(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Color_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_DeltaColorR_DisplayName)]
        [TypeConverter(typeof(FloatNumberTypeConverter))]
        [FloatNumber()]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3410, CategoryID_EFF_Color)]
        public float PROP_DeltaColorR
        {
            get => Methods.ReturnDeltaColorR(InternalID);
            set
            {
                Methods.SetDeltaColorR(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Color_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_DeltaColorG_DisplayName)]
        [TypeConverter(typeof(FloatNumberTypeConverter))]
        [FloatNumber()]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3420, CategoryID_EFF_Color)]
        public float PROP_DeltaColorG
        {
            get => Methods.ReturnDeltaColorG(InternalID);
            set
            {
                Methods.SetDeltaColorG(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Color_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_DeltaColorB_DisplayName)]
        [TypeConverter(typeof(FloatNumberTypeConverter))]
        [FloatNumber()]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3430, CategoryID_EFF_Color)]
        public float PROP_DeltaColorB
        {
            get => Methods.ReturnDeltaColorB(InternalID);
            set
            {
                Methods.SetDeltaColorB(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Color_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_DeltaColorA_DisplayName)]
        [TypeConverter(typeof(FloatNumberTypeConverter))]
        [FloatNumber()]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3440, CategoryID_EFF_Color)]
        public float PROP_DeltaColorA
        {
            get => Methods.ReturnDeltaColorA(InternalID);
            set
            {
                Methods.SetDeltaColorA(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Color_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_DeltaColorAttack_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3450, CategoryID_EFF_Color)]
        public ushort PROP_DeltaColorAttack
        {
            get => Methods.ReturnDeltaColorAttack(InternalID);
            set
            {
                Methods.SetDeltaColorAttack(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Color_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_DeltaColorStartFrame_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3460, CategoryID_EFF_Color)]
        public ushort PROP_DeltaColorStartFrame
        {
            get => Methods.ReturnDeltaColorStartFrame(InternalID);
            set
            {
                Methods.SetDeltaColorStartFrame(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Timing_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_DeltaSizeStartFrame_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3470, CategoryID_EFF_Timing)]
        public ushort PROP_DeltaSizeStartFrame
        {
            get => Methods.ReturnDeltaSizeStartFrame(InternalID);
            set
            {
                Methods.SetDeltaSizeStartFrame(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Timing_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_LifeTime_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3480, CategoryID_EFF_Timing)]
        public ushort PROP_LifeTime
        {
            get => Methods.ReturnLifeTime(InternalID);
            set
            {
                Methods.SetLifeTime(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Timing_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_AnimationSpeed_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3490, CategoryID_EFF_Timing)]
        public uint PROP_AnimationSpeed
        {
            get => Methods.ReturnAnimationSpeed(InternalID);
            set
            {
                Methods.SetAnimationSpeed(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Timing_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_ReleaseTime_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3500, CategoryID_EFF_Timing)]
        public byte PROP_ReleaseTime
        {
            get => Methods.ReturnReleaseTime(InternalID);
            set
            {
                Methods.SetReleaseTime(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Timing_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_Blend_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3510, CategoryID_EFF_Timing)]
        public ushort PROP_Blend
        {
            get => Methods.ReturnBlend(InternalID);
            set
            {
                Methods.SetBlend(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Simulation_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_SimulationType_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3520, CategoryID_EFF_Simulation)]
        public byte PROP_SimulationType
        {
            get => Methods.ReturnSimulationType(InternalID);
            set
            {
                Methods.SetSimulationType(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Simulation_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_SimulationPower_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3530, CategoryID_EFF_Simulation)]
        public byte PROP_SimulationPower
        {
            get => Methods.ReturnSimulationPower(InternalID);
            set
            {
                Methods.SetSimulationPower(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Simulation_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_MaskTextureID_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3540, CategoryID_EFF_Simulation)]
        public byte PROP_MaskTextureID
        {
            get => Methods.ReturnMaskTextureID(InternalID);
            set
            {
                Methods.SetMaskTextureID(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Simulation_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_ValueIn_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3550, CategoryID_EFF_Simulation)]
        public byte PROP_ValueIn
        {
            get => Methods.ReturnValueIn(InternalID);
            set
            {
                Methods.SetValueIn(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Simulation_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_ValueOut_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3560, CategoryID_EFF_Simulation)]
        public byte PROP_ValueOut
        {
            get => Methods.ReturnValueOut(InternalID);
            set
            {
                Methods.SetValueOut(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Work_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_Work0_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3570, CategoryID_EFF_Work)]
        public byte PROP_Work0
        {
            get => Methods.ReturnWork0(InternalID);
            set
            {
                Methods.SetWork0(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Work_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_Work1_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3580, CategoryID_EFF_Work)]
        public byte PROP_Work1
        {
            get => Methods.ReturnWork1(InternalID);
            set
            {
                Methods.SetWork1(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Work_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_Work2_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3590, CategoryID_EFF_Work)]
        public byte PROP_Work2
        {
            get => Methods.ReturnWork2(InternalID);
            set
            {
                Methods.SetWork2(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Work_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_Work3_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3600, CategoryID_EFF_Work)]
        public byte PROP_Work3
        {
            get => Methods.ReturnWork3(InternalID);
            set
            {
                Methods.SetWork3(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Work_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_Work4_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3610, CategoryID_EFF_Work)]
        public uint PROP_Work4
        {
            get => Methods.ReturnWork4(InternalID);
            set
            {
                Methods.SetWork4(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Work_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_Work5_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3620, CategoryID_EFF_Work)]
        public uint PROP_Work5
        {
            get => Methods.ReturnWork5(InternalID);
            set
            {
                Methods.SetWork5(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Work_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_Work6_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3630, CategoryID_EFF_Work)]
        public uint PROP_Work6
        {
            get => Methods.ReturnWork6(InternalID);
            set
            {
                Methods.SetWork6(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Vector_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_Vector0X_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3640, CategoryID_EFF_Vector)]
        public float PROP_Vector0X
        {
            get => Methods.ReturnVector0X(InternalID);
            set
            {
                Methods.SetVector0X(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Vector_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_Vector0Y_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3650, CategoryID_EFF_Vector)]
        public float PROP_Vector0Y
        {
            get => Methods.ReturnVector0Y(InternalID);
            set
            {
                Methods.SetVector0Y(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Vector_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_Vector0Z_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3660, CategoryID_EFF_Vector)]
        public float PROP_Vector0Z
        {
            get => Methods.ReturnVector0Z(InternalID);
            set
            {
                Methods.SetVector0Z(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Vector_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_Vector1X_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3670, CategoryID_EFF_Vector)]
        public float PROP_Vector1X
        {
            get => Methods.ReturnVector1X(InternalID);
            set
            {
                Methods.SetVector1X(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Vector_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_Vector1Y_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3680, CategoryID_EFF_Vector)]
        public float PROP_Vector1Y
        {
            get => Methods.ReturnVector1Y(InternalID);
            set
            {
                Methods.SetVector1Y(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Vector_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_Vector1Z_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3690, CategoryID_EFF_Vector)]
        public float PROP_Vector1Z
        {
            get => Methods.ReturnVector1Z(InternalID);
            set
            {
                Methods.SetVector1Z(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Vector_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_Vector2X_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3700, CategoryID_EFF_Vector)]
        public float PROP_Vector2X
        {
            get => Methods.ReturnVector2X(InternalID);
            set
            {
                Methods.SetVector2X(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Vector_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_Vector2Y_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3710, CategoryID_EFF_Vector)]
        public float PROP_Vector2Y
        {
            get => Methods.ReturnVector2Y(InternalID);
            set
            {
                Methods.SetVector2Y(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Vector_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_Vector2Z_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3720, CategoryID_EFF_Vector)]
        public float PROP_Vector2Z
        {
            get => Methods.ReturnVector2Z(InternalID);
            set
            {
                Methods.SetVector2Z(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Spline_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_Spline0_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3730, CategoryID_EFF_Spline)]
        public byte PROP_Spline0
        {
            get => Methods.ReturnSpline0(InternalID);
            set
            {
                Methods.SetSpline0(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Spline_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_Spline1_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3740, CategoryID_EFF_Spline)]
        public byte PROP_Spline1
        {
            get => Methods.ReturnSpline1(InternalID);
            set
            {
                Methods.SetSpline1(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Spline_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_Spline2_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3750, CategoryID_EFF_Spline)]
        public byte PROP_Spline2
        {
            get => Methods.ReturnSpline2(InternalID);
            set
            {
                Methods.SetSpline2(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Spline_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_Spline3_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3760, CategoryID_EFF_Spline)]
        public byte PROP_Spline3
        {
            get => Methods.ReturnSpline3(InternalID);
            set
            {
                Methods.SetSpline3(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Path_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_PathOwn_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3770, CategoryID_EFF_Path)]
        public byte PROP_PathOwn
        {
            get => Methods.ReturnPathOwn(InternalID);
            set
            {
                Methods.SetPathOwn(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Path_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_PathNumber_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3780, CategoryID_EFF_Path)]
        public byte PROP_PathNumber
        {
            get => Methods.ReturnPathNumber(InternalID);
            set
            {
                Methods.SetPathNumber(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Path_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_PathStart_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3790, CategoryID_EFF_Path)]
        public byte PROP_PathStart
        {
            get => Methods.ReturnPathStart(InternalID);
            set
            {
                Methods.SetPathStart(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Path_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_PathRandom_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3800, CategoryID_EFF_Path)]
        public byte PROP_PathRandom
        {
            get => Methods.ReturnPathRandom(InternalID);
            set
            {
                Methods.SetPathRandom(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Control_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_EffType_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3810, CategoryID_EFF_Control)]
        public byte PROP_EffType
        {
            get => Methods.ReturnEffType(InternalID);
            set
            {
                Methods.SetEffType(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Control_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_ControlID_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3820, CategoryID_EFF_Control)]
        public byte PROP_ControlID
        {
            get => Methods.ReturnControlID(InternalID);
            set
            {
                Methods.SetControlID(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Control_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_ControlFlag_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3830, CategoryID_EFF_Control)]
        public ushort PROP_ControlFlag
        {
            get => Methods.ReturnControlFlag(InternalID);
            set
            {
                Methods.SetControlFlag(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Control_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_ControlInterval_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3840, CategoryID_EFF_Control)]
        public byte PROP_ControlInterval
        {
            get => Methods.ReturnControlInterval(InternalID);
            set
            {
                Methods.SetControlInterval(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Control_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_ControlNumber_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3850, CategoryID_EFF_Control)]
        public byte PROP_ControlNumber
        {
            get => Methods.ReturnControlNumber(InternalID);
            set
            {
                Methods.SetControlNumber(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Control_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_ControlRP_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3860, CategoryID_EFF_Control)]
        public byte PROP_ControlRP
        {
            get => Methods.ReturnControlRP(InternalID);
            set
            {
                Methods.SetControlRP(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_Control_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_ControlLife_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3870, CategoryID_EFF_Control)]
        public ushort PROP_ControlLife
        {
            get => Methods.ReturnControlLife(InternalID);
            set
            {
                Methods.SetControlLife(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_ControlPath_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_ControlPathScaleX_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3880, CategoryID_EFF_ControlPath)]
        public float PROP_ControlPathScaleX
        {
            get => Methods.ReturnControlPathScaleX(InternalID);
            set
            {
                Methods.SetControlPathScaleX(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_ControlPath_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_ControlPathScaleY_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3890, CategoryID_EFF_ControlPath)]
        public float PROP_ControlPathScaleY
        {
            get => Methods.ReturnControlPathScaleY(InternalID);
            set
            {
                Methods.SetControlPathScaleY(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_ControlPath_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_ControlPathScaleZ_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3900, CategoryID_EFF_ControlPath)]
        public float PROP_ControlPathScaleZ
        {
            get => Methods.ReturnControlPathScaleZ(InternalID);
            set
            {
                Methods.SetControlPathScaleZ(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_ControlPath_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_ControlPathDeltaSize_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3910, CategoryID_EFF_ControlPath)]
        public byte PROP_ControlPathDeltaSize
        {
            get => Methods.ReturnControlPathDeltaSize(InternalID);
            set
            {
                Methods.SetControlPathDeltaSize(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_ControlPath_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_ControlPathDeltaSpeed_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3920, CategoryID_EFF_ControlPath)]
        public byte PROP_ControlPathDeltaSpeed
        {
            get => Methods.ReturnControlPathDeltaSpeed(InternalID);
            set
            {
                Methods.SetControlPathDeltaSpeed(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_ControlPath_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_ControlPathDeltaAlpha_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3930, CategoryID_EFF_ControlPath)]
        public byte PROP_ControlPathDeltaAlpha
        {
            get => Methods.ReturnControlPathDeltaAlpha(InternalID);
            set
            {
                Methods.SetControlPathDeltaAlpha(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_ControlPath_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_ControlPathDeltaInterval_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3940, CategoryID_EFF_ControlPath)]
        public byte PROP_ControlPathDeltaInterval
        {
            get => Methods.ReturnControlPathDeltaInterval(InternalID);
            set
            {
                Methods.SetControlPathDeltaInterval(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_ControlPath_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_ControlPathRandomInterval_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3950, CategoryID_EFF_ControlPath)]
        public byte PROP_ControlPathRandomInterval
        {
            get => Methods.ReturnControlPathRandomInterval(InternalID);
            set
            {
                Methods.SetControlPathRandomInterval(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_ControlPath_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_ControlPathRotationX_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3960, CategoryID_EFF_ControlPath)]
        public byte PROP_ControlPathRotationX
        {
            get => Methods.ReturnControlPathRotationX(InternalID);
            set
            {
                Methods.SetControlPathRotationX(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_ControlPath_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_ControlPathRotationY_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3970, CategoryID_EFF_ControlPath)]
        public byte PROP_ControlPathRotationY
        {
            get => Methods.ReturnControlPathRotationY(InternalID);
            set
            {
                Methods.SetControlPathRotationY(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        [CustomCategory(aLang.EFF_EffectEntry_ControlPath_Category)]
        [CustomDisplayName(aLang.EFF_EffectEntry_ControlPathFlag_DisplayName)]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(true)]
        [DynamicTypeDescriptor.Id(3980, CategoryID_EFF_ControlPath)]
        public byte PROP_ControlPathFlag
        {
            get => Methods.ReturnControlPathFlag(InternalID);
            set
            {
                Methods.SetControlPathFlag(InternalID, value);
                updateMethods.UpdateGL();
            }
        }

        #endregion
    }
}
