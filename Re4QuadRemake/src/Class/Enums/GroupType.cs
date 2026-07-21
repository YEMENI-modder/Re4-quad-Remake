using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re4QuadExtremeEditor.src.Class.Enums
{
   public enum GroupType : byte
    {
        NULL = 0,
        ESL = 1,
        ETS = 2,
        ITA = 3,
        AEV = 4,
        EXTRAS = 5,
        FSE = 0x06,
        SAR = 0x07,
        EAR = 0x08,
        EMI = 0x09,
        ESE = 0x10,
        LIT = 0x20,
        LIT_GROUPS = 0x21,
        LIT_ENTRYS = 0x22,
        EFF = 0x3F,
        EFF_Table0 = 0x30,
        EFF_Table1 = 0x31,
        EFF_Table2 = 0x32,
        EFF_Table3 = 0x33,
        EFF_Table4 = 0x34,
        EFF_Table6 = 0x36,
        EFF_Table7_Effect_0 = 0x37,
        EFF_Table8_Effect_1 = 0x38,
        EFF_EffectEntry = 0x3A,
        EFF_Table9 = 0x39,
        EFF_EffectEntry_ForAdd_Table7 = 0x3B,
        EFF_EffectEntry_ForAdd_Table8 = 0x3C,
        CAM = 0x3D,
        DSE = 0xAA,
    }
}
