using System;
using Re4QuadExtremeEditor.src.Class.ObjMethods;

namespace Re4QuadExtremeEditor.src.Class.Interfaces
{
    public interface IChangeAmountIndexFix
    {
        NodeChangeAmountCallbackMethods ChangeAmountCallbackMethods { get; }
        void OnDeleteNode();
        void OnMoveNode();
    }
}
