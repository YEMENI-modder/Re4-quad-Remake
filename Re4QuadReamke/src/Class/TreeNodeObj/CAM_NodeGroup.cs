using System.Windows.Forms;
using Re4QuadExtremeEditor.src.Class.Enums;

namespace Re4QuadExtremeEditor.src.Class.TreeNodeObj
{
    public enum CamPickKind
    {
        TriggerZone,
        Camera
    }

    /// <summary>Tag attached to CAM tree nodes so the renderer/picker knows
    /// which CamFile table+index a given node refers to.</summary>
    public class CamPickTag
    {
        public CamPickKind Kind;
        public int Index;
    }

    /// <summary>
    /// Minimal Node container for CAM entries, used for the initial standalone
    /// Open/Save test. Deliberately does not implement INodeChangeAmount/Move
    /// yet - those get added once round-trip loading/saving is confirmed to
    /// work correctly, and CAM gets wired into the normal room LoadFile flow.
    /// </summary>
    public class CAM_NodeGroup : TreeNodeGroup
    {
        public CAM_NodeGroup() : base() { }
        public CAM_NodeGroup(string text) : base(text) { }
        public CAM_NodeGroup(string text, TreeNode[] children) : base(text, children) { }
    }
}
