using System;

namespace Re4QuadExtremeEditor.src.Class.Enums
{
    /// <summary>
    /// which DLL/patch layout the RE4 UHD game folder is using, this changes where
    /// the room's NewAge files (AEV, ITA, ETS, EAR, SAR, FSE, EMI, ESE, LIT, EFFBLOB)
    /// are located on disk.
    /// </summary>
    public enum LoadRoomDllVersion
    {
        Qingsheng = 0,
        Raz0r = 1,
        WithoutAnyDLL = 2
    }
}
