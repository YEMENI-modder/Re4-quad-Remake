using System;

namespace Re4QuadExtremeEditor.src.Class.Enums
{
    /// <summary>
    /// Which source/format the room's 3D model should be loaded from.
    /// Smd automatically also loads the matching ImagePackHD textures (the map's texture pack) —
    /// there is no standalone "ImagePackHD" option.
    /// </summary>
    public enum RoomModelLoadSource
    {
        Xcar = 0,
        Smd = 1,
        SmdData = 2,
        Smd0000 = 3,
        /// <summary>
        /// Load no model at all (no meshes, no textures). Used by the "None" option in Select Room.
        /// </summary>
        None = 4
    }
}
