using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Re4QuadExtremeEditor.src.Class.Enums;
using Re4QuadExtremeEditor.src.Class.ObjMethods;
using SimpleEndianBinaryIO;

namespace Re4QuadExtremeEditor.src.Class.Files
{
    public abstract class BaseGroup
    {
        protected abstract byte[] GetInternalLine(ushort ID);
        protected abstract Endianness GetEndianness();

        protected virtual void SetBaseMethods(BaseMethods methods)
        {
            // Base implementation - subclasses assign specific delegates after calling this
        }
    }
}
