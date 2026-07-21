using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Re4QuadExtremeEditor.src.Class.Enums;
using Re4QuadExtremeEditor.src.Class.ObjMethods;

namespace Re4QuadExtremeEditor.src.Class.MyProperty
{
    public abstract class BaseProperty : GenericProperty
    {
        public BaseProperty() : base() { }

        protected UpdateMethods updateMethods = null;

        public override Type GetClassType() { return null; }

        protected abstract void SetFloatType(bool IsHex);

        public void RefreshFloatType(bool IsHex)
        {
            SetFloatType(IsHex);
        }
    }
}
