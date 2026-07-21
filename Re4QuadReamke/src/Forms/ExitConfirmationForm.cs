using System;
using System.Windows.Forms;
using Re4QuadExtremeEditor.src;
using Re4QuadExtremeEditor.src.Class.Enums;

namespace Re4QuadExtremeEditor.src.Forms
{
    public partial class ExitConfirmationForm : Form
    {
        public ExitDialogResult Result { get; private set; } = ExitDialogResult.Cancel;

        public ExitConfirmationForm(bool isProjectActive)
        {
            InitializeComponent();

            buttonSaveProject.Enabled = isProjectActive;

            buttonSaveProject.Click += ButtonSaveProject_Click;
            buttonSaveAll.Click += ButtonSaveAll_Click;
            buttonQuitWithoutSaving.Click += ButtonQuitWithoutSaving_Click;
            buttonCancel.Click += ButtonCancel_Click;

            this.Load += ExitConfirmationForm_Load;
        }

        private void ExitConfirmationForm_Load(object sender, EventArgs e)
        {
            if (Globals.DarkMode)
            {
                DarkMode.Apply(this);
            }
        }

        private void ButtonSaveProject_Click(object sender, EventArgs e)
        {
            Result = ExitDialogResult.SaveProject;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ButtonSaveAll_Click(object sender, EventArgs e)
        {
            Result = ExitDialogResult.SaveAll;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ButtonQuitWithoutSaving_Click(object sender, EventArgs e)
        {
            Result = ExitDialogResult.QuitWithoutSaving;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            Result = ExitDialogResult.Cancel;
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
