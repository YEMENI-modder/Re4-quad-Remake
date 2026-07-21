using System.Collections.Generic;
using OpenTK;

namespace Re4QuadExtremeEditor.src.Class
{
    /// <summary>
    /// Byte-for-byte port of NewAge's ViewerBase/SmxEntry.cs. Represents one processed SMX
    /// record (the "used SMX ID" indexed dictionary key, not the SMD placement's SMX_ID).
    /// </summary>
    public class SmxEntry
    {
        public int SMX_ID { get; set; }
        public int OpacityHierarchy { get; set; }
        public int AlphaHierarchy { get; set; }
        public Vector4 SmxColor { get; set; }

        public SmxFaceCulling FaceCulling { get; set; }

        public SmxEntry()
        {
            SMX_ID = -1;
            OpacityHierarchy = 0;
            AlphaHierarchy = 0;
            SmxColor = Vector4.One;
            FaceCulling = SmxFaceCulling.OnlyFront;
        }
    }

    public enum SmxFaceCulling
    {
        OnlyFront = 0,
        OnlyBack = 1,
        FrontAndBack = 2
    }

    public class SmxGroup
    {
        public string SmxName { get; private set; }
        public Dictionary<int, SmxEntry> SmxEntries { get; set; }

        public SmxGroup(string smxName)
        {
            SmxName = smxName;
            SmxEntries = new Dictionary<int, SmxEntry>();
        }
    }
}
