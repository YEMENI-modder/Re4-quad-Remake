using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Re4QuadExtremeEditor.src
{
    /// <summary>
    /// Provides Dark Mode support for Windows Forms controls.
    /// Automatically applies on macOS/Linux via cross-platform styling.
    /// </summary>
    public static class DarkMode
    {
        public static readonly Color BackColor       = Color.FromArgb(30, 30, 30);
        public static readonly Color ForeColor       = Color.FromArgb(220, 220, 220);
        public static readonly Color PanelColor      = Color.FromArgb(40, 40, 40);
        public static readonly Color MenuColor       = Color.FromArgb(35, 35, 35);
        public static readonly Color MenuItemColor   = Color.FromArgb(50, 50, 50);
        public static readonly Color BorderColor     = Color.FromArgb(70, 70, 70);
        public static readonly Color AccentColor     = Color.FromArgb(0x70, 0xBB, 0xDB);
        public static readonly Color ButtonBack      = Color.FromArgb(55, 55, 55);
        public static readonly Color ButtonFore      = Color.FromArgb(220, 220, 220);
        public static readonly Color TreeViewBack    = Color.FromArgb(25, 25, 25);
        public static readonly Color PropertyBack    = Color.FromArgb(28, 28, 28);

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
        public static void EnableDarkTitleBar(Form form)
        {
            try { int d = 1; DwmSetWindowAttribute(form.Handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref d, sizeof(int)); }
            catch { }
        }

        public static void DisableDarkTitleBar(Form form)
        {
            try { int d = 0; DwmSetWindowAttribute(form.Handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref d, sizeof(int)); }
            catch { }
        }

        public static void ApplyLight(Form form)
        {
            DisableDarkTitleBar(form);
            ApplyLightToControl(form);
            foreach (Control ctrl in GetAllControls(form))
                ApplyLightToControl(ctrl);
        }

        private static void ApplyLightToControl(Control ctrl)
        {
            ctrl.BackColor = SystemColors.Control;
            ctrl.ForeColor = SystemColors.ControlText;
            if (ctrl is MenuStrip ms) { ms.BackColor = SystemColors.Control; ms.ForeColor = SystemColors.ControlText; ms.Renderer = new ToolStripProfessionalRenderer(); }
            else if (ctrl is TreeView tv) { tv.BackColor = SystemColors.Window; tv.ForeColor = SystemColors.ControlText; }
            else if (ctrl is PropertyGrid pg) { pg.BackColor = SystemColors.Window; pg.ViewBackColor = SystemColors.Window; pg.ViewForeColor = SystemColors.ControlText; pg.LineColor = SystemColors.ControlDark; pg.CategoryForeColor = SystemColors.ControlText; pg.HelpBackColor = SystemColors.Control; pg.HelpForeColor = SystemColors.ControlText; }
            else if (ctrl is Button btn) { btn.BackColor = SystemColors.Control; btn.ForeColor = SystemColors.ControlText; btn.FlatStyle = FlatStyle.Standard; }
            else if (ctrl is TextBox tb) { tb.BackColor = SystemColors.Window; tb.ForeColor = SystemColors.ControlText; }
            else if (ctrl is ComboBox cb) { cb.BackColor = SystemColors.Window; cb.ForeColor = SystemColors.ControlText; }
            else if (ctrl is Label) { ctrl.BackColor = Color.Transparent; ctrl.ForeColor = SystemColors.ControlText; }
            else if (ctrl is CheckBox) { ctrl.BackColor = Color.Transparent; ctrl.ForeColor = SystemColors.ControlText; }
            else if (ctrl is NumericUpDown nud) { nud.BackColor = SystemColors.Window; nud.ForeColor = SystemColors.ControlText; }
        }

        public static void Apply(Form form)
        {
            EnableDarkTitleBar(form);
            ApplyToControl(form);
            foreach (Control ctrl in GetAllControls(form))
                ApplyToControl(ctrl);
        }

        private static void ApplyToControl(Control ctrl)
        {
            ctrl.BackColor = BackColor;
            ctrl.ForeColor = ForeColor;
            if (ctrl is MenuStrip ms) { ms.BackColor = MenuColor; ms.ForeColor = ForeColor; ms.Renderer = new DarkMenuRenderer(); }
            else if (ctrl is Panel) ctrl.BackColor = PanelColor;
            else if (ctrl is SplitContainer sc) { sc.BackColor = PanelColor; sc.Panel1.BackColor = PanelColor; sc.Panel2.BackColor = PanelColor; }
            else if (ctrl is TreeView tv) { tv.BackColor = TreeViewBack; tv.ForeColor = ForeColor; }
            else if (ctrl is PropertyGrid pg) { pg.BackColor = PropertyBack; pg.ViewBackColor = PropertyBack; pg.ViewForeColor = Color.White; pg.LineColor = BorderColor; pg.CategoryForeColor = AccentColor; pg.HelpBackColor = Color.FromArgb(35,35,35); pg.HelpForeColor = Color.White; }
            else if (ctrl is Button btn) { btn.BackColor = ButtonBack; btn.ForeColor = ButtonFore; btn.FlatStyle = FlatStyle.Flat; btn.FlatAppearance.BorderColor = BorderColor; }
            else if (ctrl is TextBox tb) { tb.BackColor = Color.FromArgb(45,45,45); tb.ForeColor = ForeColor; }
            else if (ctrl is ComboBox cb) { cb.BackColor = Color.FromArgb(45,45,45); cb.ForeColor = ForeColor; }
            else if (ctrl is Label) { ctrl.BackColor = Color.Transparent; ctrl.ForeColor = ForeColor; }
            else if (ctrl is CheckBox) { ctrl.BackColor = Color.Transparent; ctrl.ForeColor = ForeColor; }
            else if (ctrl is NumericUpDown nud) { nud.BackColor = Color.FromArgb(45,45,45); nud.ForeColor = ForeColor; }
        }

        private static System.Collections.Generic.IEnumerable<Control> GetAllControls(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                yield return ctrl;
                foreach (Control child in GetAllControls(ctrl))
                    yield return child;
            }
        }

        private class DarkMenuRenderer : ToolStripProfessionalRenderer
        {
            public DarkMenuRenderer() : base(new DarkColorTable()) { }
            protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
            {
                var rect = new Rectangle(Point.Empty, e.Item.Size);
                using (var brush = new SolidBrush(e.Item.Selected ? Color.FromArgb(70,70,70) : MenuItemColor))
                    e.Graphics.FillRectangle(brush, rect);
            }
            protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
            {
                using (var brush = new SolidBrush(MenuColor))
                    e.Graphics.FillRectangle(brush, e.AffectedBounds);
            }
            protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
            {
                // Garante texto claro em todos os itens de menu, independente do ForeColor
                // individual de cada ToolStripMenuItem (que por padrão herda preto).
                e.TextColor = e.Item.Enabled ? ForeColor : Color.FromArgb(120, 120, 120);
                base.OnRenderItemText(e);
            }
        }

        private class DarkColorTable : ProfessionalColorTable
        {
            public override Color MenuStripGradientBegin => MenuColor;
            public override Color MenuStripGradientEnd => MenuColor;
            public override Color MenuBorder => BorderColor;
            public override Color MenuItemBorder => BorderColor;
            public override Color MenuItemSelected => Color.FromArgb(70,70,70);
            public override Color MenuItemSelectedGradientBegin => Color.FromArgb(70,70,70);
            public override Color MenuItemSelectedGradientEnd => Color.FromArgb(70,70,70);
            public override Color ToolStripDropDownBackground => MenuItemColor;
            public override Color ImageMarginGradientBegin => MenuItemColor;
            public override Color ImageMarginGradientMiddle => MenuItemColor;
            public override Color ImageMarginGradientEnd => MenuItemColor;
            public override Color SeparatorDark => BorderColor;
            public override Color SeparatorLight => BorderColor;
        }
    }
}
