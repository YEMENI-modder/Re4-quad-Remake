using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Re4QuadExtremeEditor.src.Class.MyProperty.CustomUITypeEditor
{
    public class SelectColorUITypeEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return false;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (provider == null) return value;
            var editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (editorService == null) return value;

            Color initialColor = Color.White;
            if (value is Color c) initialColor = c;

            using (var dlg = new ColorDialog())
            {
                dlg.Color = initialColor;
                dlg.FullOpen = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    return dlg.Color;
                }
            }
            return value;
        }
    }
}
