using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Re4QuadExtremeEditor.src.Class.CustomDelegates;
using Re4QuadExtremeEditor.src.Class.Enums;

namespace Re4QuadExtremeEditor.src.Class.ObjMethods
{
    public class NewAge_EFF_Methods : BaseMethods
    {
        public ReturnByteArray ReturnLine;
        public SetByteArray SetLine;

        public SetUshort SetEntryOrderID;
        public ReturnUshort GetEntryOrderID;
    }

    public class NewAge_EFF_Table2Entry_Methods : BaseMethods
    {
        public ReturnByteArray ReturnLine;
        public SetByteArray SetLine;

        public SetUshort SetEntryOrderID;
        public ReturnUshort GetEntryOrderID;

        public ReturnUshort ReturnEffectGroupID;
        public SetUshort SetEffectGroupID;

        public ReturnUshort ReturnEarLinkID;
        public SetUshort SetEarLinkID;
    }

    public class NewAge_EFF_Table4Entry_Methods : BaseMethods
    {
        public ReturnByteArray ReturnLine;
        public SetByteArray SetLine;

        public SetUshort SetEntryOrderID;
        public ReturnUshort GetEntryOrderID;

        public ReturnUshort ReturnModelID;
        public SetUshort SetModelID;
    }

    public class NewAge_EFF_Table0Entry_Methods : BaseMethods
    {
        public ReturnByteArray ReturnLine;
        public SetByteArray SetLine;

        public SetUshort SetEntryOrderID;
        public ReturnUshort GetEntryOrderID;

        public ReturnUshort ReturnTextureID;
        public SetUshort SetTextureID;
    }

    public class NewAge_EFF_Table1Entry_Methods : BaseMethods
    {
        public ReturnByteArray ReturnLine;
        public SetByteArray SetLine;

        public SetUshort SetEntryOrderID;
        public ReturnUshort GetEntryOrderID;

        public ReturnUshort ReturnCoreID;
        public SetUshort SetCoreID;
    }

    public class NewAge_EFF_Table3Entry_Methods : BaseMethods
    {
        public ReturnByteArray ReturnLine;
        public SetByteArray SetLine;

        public SetUshort SetEntryOrderID;
        public ReturnUshort GetEntryOrderID;

        public ReturnUshort ReturnPathID;
        public SetUshort SetPathID;
    }

    public class NewAge_EFF_Table6Entry_Methods : BaseMethods
    {
        public ReturnByteArray ReturnLine;
        public SetByteArray SetLine;

        public SetUshort SetEntryOrderID;
        public ReturnUshort GetEntryOrderID;

        public ReturnUshort ReturnTextureHeight;
        public SetUshort SetTextureHeight;

        public ReturnUshort ReturnTextureWidth;
        public SetUshort SetTextureWidth;

        public ReturnUshort ReturnEffectHeight;
        public SetUshort SetEffectHeight;

        public ReturnUshort ReturnEffectWidth;
        public SetUshort SetEffectWidth;

        public ReturnUshort ReturnTextureCount;
        public SetUshort SetTextureCount;

        public ReturnByte ReturnUnknown1;
        public SetByte SetUnknown1;

        public ReturnByte ReturnUnknown2;
        public SetByte SetUnknown2;
    }

    public class NewAge_EFF_EffectGroup_Methods : BaseMethods
    {
        public Func<GroupType> GetGrouptype;
        
        public ReturnByteArray ReturnLine;
        public SetByteArray SetLine;

        public SetUshort SetEntryOrderID;
        public ReturnUshort GetEntryOrderID;

        public ReturnInt GetEntryCountInGroup;

        public ReturnFloat ReturnAngleX;
        public SetFloat SetAngleX;

        public ReturnFloat ReturnAngleY;
        public SetFloat SetAngleY;

        public ReturnFloat ReturnAngleZ;
        public SetFloat SetAngleZ;

        public ReturnFloat ReturnPositionX;
        public SetFloat SetPositionX;

        public ReturnFloat ReturnPositionY;
        public SetFloat SetPositionY;

        public ReturnFloat ReturnPositionZ;
        public SetFloat SetPositionZ;

        public ReturnUint ReturnAngleX_Hex;
        public SetUint SetAngleX_Hex;

        public ReturnUint ReturnAngleY_Hex;
        public SetUint SetAngleY_Hex;

        public ReturnUint ReturnAngleZ_Hex;
        public SetUint SetAngleZ_Hex;

        public ReturnUint ReturnPositionX_Hex;
        public SetUint SetPositionX_Hex;

        public ReturnUint ReturnPositionY_Hex;
        public SetUint SetPositionY_Hex;

        public ReturnUint ReturnPositionZ_Hex;
        public SetUint SetPositionZ_Hex;

        public ReturnUint ReturnPositionW_Hex;
        public SetUint SetPositionW_Hex;
    }

    public class NewAge_EFF_EffectEntry_Methods : BaseMethods
    {
        public ReturnByteArray ReturnLine;
        public SetByteArray SetLine;

        public SetUshort SetEntryOrderID;
        public ReturnUshort GetEntryOrderID;

        public SetUshort SetGroupOrderID;
        public ReturnUshort GetGroupOrderID;

        public SetByte SetTableID;
        public ReturnByte GetTableID;

        public ReturnFloat ReturnAngleX;
        public SetFloat SetAngleX;

        public ReturnFloat ReturnAngleY;
        public SetFloat SetAngleY;

        public ReturnFloat ReturnAngleZ;
        public SetFloat SetAngleZ;

        public ReturnFloat ReturnPositionX;
        public SetFloat SetPositionX;

        public ReturnFloat ReturnPositionY;
        public SetFloat SetPositionY;

        public ReturnFloat ReturnPositionZ;
        public SetFloat SetPositionZ;

        public ReturnUint ReturnAngleX_Hex;
        public SetUint SetAngleX_Hex;

        public ReturnUint ReturnAngleY_Hex;
        public SetUint SetAngleY_Hex;

        public ReturnUint ReturnAngleZ_Hex;
        public SetUint SetAngleZ_Hex;

        public ReturnUint ReturnPositionX_Hex;
        public SetUint SetPositionX_Hex;

        public ReturnUint ReturnPositionY_Hex;
        public SetUint SetPositionY_Hex;

        public ReturnUint ReturnPositionZ_Hex;
        public SetUint SetPositionZ_Hex;

        public ReturnUint ReturnPositionW_Hex;
        public SetUint SetPositionW_Hex;

        public ReturnByte ReturnStateID;
        public SetByte SetStateID;

        public ReturnByte ReturnEspID;
        public SetByte SetEspID;

        public ReturnByte ReturnTextureID;
        public SetByte SetTextureID;

        public ReturnUshort ReturnTime;
        public SetUshort SetTime;

        public ReturnByte ReturnParent;
        public SetByte SetParent;

        public ReturnByte ReturnPart;
        public SetByte SetPart;

        public ReturnUint ReturnFlags;
        public SetUint SetFlags;

        public ReturnFloat ReturnRandomX;
        public SetFloat SetRandomX;

        public ReturnFloat ReturnRandomY;
        public SetFloat SetRandomY;

        public ReturnFloat ReturnRandomZ;
        public SetFloat SetRandomZ;

        public ReturnFloat ReturnSpeedX;
        public SetFloat SetSpeedX;

        public ReturnFloat ReturnSpeedY;
        public SetFloat SetSpeedY;

        public ReturnFloat ReturnSpeedZ;
        public SetFloat SetSpeedZ;

        public ReturnFloat ReturnDeltaSpeed;
        public SetFloat SetDeltaSpeed;

        public ReturnFloat ReturnRandomSpeedX;
        public SetFloat SetRandomSpeedX;

        public ReturnFloat ReturnRandomSpeedY;
        public SetFloat SetRandomSpeedY;

        public ReturnFloat ReturnRandomSpeedZ;
        public SetFloat SetRandomSpeedZ;

        public ReturnFloat ReturnAccelerationX;
        public SetFloat SetAccelerationX;

        public ReturnFloat ReturnAccelerationY;
        public SetFloat SetAccelerationY;

        public ReturnFloat ReturnAccelerationZ;
        public SetFloat SetAccelerationZ;

        public ReturnFloat ReturnRandomAccelerationX;
        public SetFloat SetRandomAccelerationX;

        public ReturnFloat ReturnRandomAccelerationY;
        public SetFloat SetRandomAccelerationY;

        public ReturnFloat ReturnRandomAccelerationZ;
        public SetFloat SetRandomAccelerationZ;

        public ReturnFloat ReturnRandomRotationX;
        public SetFloat SetRandomRotationX;

        public ReturnFloat ReturnRandomRotationY;
        public SetFloat SetRandomRotationY;

        public ReturnFloat ReturnRandomRotationZ;
        public SetFloat SetRandomRotationZ;

        public ReturnFloat ReturnRotationAccelerationX;
        public SetFloat SetRotationAccelerationX;

        public ReturnFloat ReturnRotationAccelerationY;
        public SetFloat SetRotationAccelerationY;

        public ReturnFloat ReturnRotationAccelerationZ;
        public SetFloat SetRotationAccelerationZ;

        public ReturnFloat ReturnRandomRotationAccelerationX;
        public SetFloat SetRandomRotationAccelerationX;

        public ReturnFloat ReturnRandomRotationAccelerationY;
        public SetFloat SetRandomRotationAccelerationY;

        public ReturnFloat ReturnRandomRotationAccelerationZ;
        public SetFloat SetRandomRotationAccelerationZ;

        public ReturnFloat ReturnWidth;
        public SetFloat SetWidth;

        public ReturnFloat ReturnHeight;
        public SetFloat SetHeight;

        public ReturnFloat ReturnRandomSize;
        public SetFloat SetRandomSize;

        public ReturnFloat ReturnGrow;
        public SetFloat SetGrow;

        public ReturnFloat ReturnDeltaGrow;
        public SetFloat SetDeltaGrow;

        public ReturnByte ReturnColorR;
        public SetByte SetColorR;

        public ReturnByte ReturnColorG;
        public SetByte SetColorG;

        public ReturnByte ReturnColorB;
        public SetByte SetColorB;

        public ReturnByte ReturnColorA;
        public SetByte SetColorA;

        public ReturnFloat ReturnDeltaColorR;
        public SetFloat SetDeltaColorR;

        public ReturnFloat ReturnDeltaColorG;
        public SetFloat SetDeltaColorG;

        public ReturnFloat ReturnDeltaColorB;
        public SetFloat SetDeltaColorB;

        public ReturnFloat ReturnDeltaColorA;
        public SetFloat SetDeltaColorA;

        public ReturnUshort ReturnDeltaColorAttack;
        public SetUshort SetDeltaColorAttack;

        public ReturnUshort ReturnDeltaColorStartFrame;
        public SetUshort SetDeltaColorStartFrame;

        public ReturnUshort ReturnDeltaSizeStartFrame;
        public SetUshort SetDeltaSizeStartFrame;

        public ReturnUshort ReturnLifeTime;
        public SetUshort SetLifeTime;

        public ReturnUint ReturnAnimationSpeed;
        public SetUint SetAnimationSpeed;

        public ReturnByte ReturnReleaseTime;
        public SetByte SetReleaseTime;

        public ReturnUshort ReturnBlend;
        public SetUshort SetBlend;

        public ReturnByte ReturnSimulationType;
        public SetByte SetSimulationType;

        public ReturnByte ReturnSimulationPower;
        public SetByte SetSimulationPower;

        public ReturnByte ReturnMaskTextureID;
        public SetByte SetMaskTextureID;

        public ReturnByte ReturnValueIn;
        public SetByte SetValueIn;

        public ReturnByte ReturnValueOut;
        public SetByte SetValueOut;

        public ReturnByte ReturnWork0;
        public SetByte SetWork0;

        public ReturnByte ReturnWork1;
        public SetByte SetWork1;

        public ReturnByte ReturnWork2;
        public SetByte SetWork2;

        public ReturnByte ReturnWork3;
        public SetByte SetWork3;

        public ReturnUint ReturnWork4;
        public SetUint SetWork4;

        public ReturnUint ReturnWork5;
        public SetUint SetWork5;

        public ReturnUint ReturnWork6;
        public SetUint SetWork6;

        public ReturnFloat ReturnVector0X;
        public SetFloat SetVector0X;

        public ReturnFloat ReturnVector0Y;
        public SetFloat SetVector0Y;

        public ReturnFloat ReturnVector0Z;
        public SetFloat SetVector0Z;

        public ReturnFloat ReturnVector1X;
        public SetFloat SetVector1X;

        public ReturnFloat ReturnVector1Y;
        public SetFloat SetVector1Y;

        public ReturnFloat ReturnVector1Z;
        public SetFloat SetVector1Z;

        public ReturnFloat ReturnVector2X;
        public SetFloat SetVector2X;

        public ReturnFloat ReturnVector2Y;
        public SetFloat SetVector2Y;

        public ReturnFloat ReturnVector2Z;
        public SetFloat SetVector2Z;

        public ReturnByte ReturnSpline0;
        public SetByte SetSpline0;

        public ReturnByte ReturnSpline1;
        public SetByte SetSpline1;

        public ReturnByte ReturnSpline2;
        public SetByte SetSpline2;

        public ReturnByte ReturnSpline3;
        public SetByte SetSpline3;

        public ReturnByte ReturnPathOwn;
        public SetByte SetPathOwn;

        public ReturnByte ReturnPathNumber;
        public SetByte SetPathNumber;

        public ReturnByte ReturnPathStart;
        public SetByte SetPathStart;

        public ReturnByte ReturnPathRandom;
        public SetByte SetPathRandom;

        public ReturnByte ReturnEffType;
        public SetByte SetEffType;

        public ReturnByte ReturnControlID;
        public SetByte SetControlID;

        public ReturnUshort ReturnControlFlag;
        public SetUshort SetControlFlag;

        public ReturnByte ReturnControlInterval;
        public SetByte SetControlInterval;

        public ReturnByte ReturnControlNumber;
        public SetByte SetControlNumber;

        public ReturnByte ReturnControlRP;
        public SetByte SetControlRP;

        public ReturnUshort ReturnControlLife;
        public SetUshort SetControlLife;

        public ReturnFloat ReturnControlPathScaleX;
        public SetFloat SetControlPathScaleX;

        public ReturnFloat ReturnControlPathScaleY;
        public SetFloat SetControlPathScaleY;

        public ReturnFloat ReturnControlPathScaleZ;
        public SetFloat SetControlPathScaleZ;

        public ReturnByte ReturnControlPathDeltaSize;
        public SetByte SetControlPathDeltaSize;

        public ReturnByte ReturnControlPathDeltaSpeed;
        public SetByte SetControlPathDeltaSpeed;

        public ReturnByte ReturnControlPathDeltaAlpha;
        public SetByte SetControlPathDeltaAlpha;

        public ReturnByte ReturnControlPathDeltaInterval;
        public SetByte SetControlPathDeltaInterval;

        public ReturnByte ReturnControlPathRandomInterval;
        public SetByte SetControlPathRandomInterval;

        public ReturnByte ReturnControlPathRotationX;
        public SetByte SetControlPathRotationX;

        public ReturnByte ReturnControlPathRotationY;
        public SetByte SetControlPathRotationY;

        public ReturnByte ReturnControlPathFlag;
        public SetByte SetControlPathFlag;

    }

    public class NewAge_EFF_Table9Entry_Methods : BaseMethods
    {
        public ReturnByteArray ReturnLine;
        public SetByteArray SetLine;

        public SetUshort SetEntryOrderID;
        public ReturnUshort GetEntryOrderID;

        public SetUshort SetGroupOrderID;
        public ReturnUshort GetGroupOrderID;

        public ReturnFloat ReturnPositionX;
        public SetFloat SetPositionX;

        public ReturnFloat ReturnPositionY;
        public SetFloat SetPositionY;

        public ReturnFloat ReturnPositionZ;
        public SetFloat SetPositionZ;

        public ReturnUint ReturnPositionX_Hex;
        public SetUint SetPositionX_Hex;

        public ReturnUint ReturnPositionY_Hex;
        public SetUint SetPositionY_Hex;

        public ReturnUint ReturnPositionZ_Hex;
        public SetUint SetPositionZ_Hex;

        public ReturnUint ReturnPositionW_Hex;
        public SetUint SetPositionW_Hex;
    }

    public class NewAge_EFF_EffectGroup_Methods_MethodsForGL
    {
        public ReturnVector3 GetPosition;

        public ReturnMatrix4 GetAngle;
    }

    public class NewAge_EFF_EffectEntry_Methods_MethodsForGL
    {
        public ReturnVector3 GetPosition;

        public ReturnMatrix4 GetAngle;

        public ReturnUshort GetGroupOrderID;

        public ReturnEffectEntryTableID GetTableID;
    }

    public class NewAge_EFF_Table9Entry_Methods_MethodsForGL
    {
        public ReturnVector3 GetPosition;

        public ReturnUshort GetGroupOrderID;
    }

}
