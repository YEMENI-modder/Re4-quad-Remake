using System;
using System.Drawing;
using System.Windows.Forms;

namespace Re4QuadExtremeEditor.src.Forms
{
    /// <summary>
    /// Small modeless progress dialog shown while the update is downloaded and installed.
    /// Built entirely in code (no .Designer.cs), same as UpdateAvailableForm.
    /// </summary>
    public class UpdateProgressForm : Form
    {
        private readonly Label labelStatus;
        private readonly ProgressBar progressBar;

        public UpdateProgressForm()
        {
            Text = "Updating...";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ControlBox = false;
            ShowInTaskbar = false;
            ClientSize = new Size(360, 90);

            labelStatus = new Label
            {
                AutoSize = false,
                Location = new Point(15, 15),
                Size = new Size(330, 20),
                Text = "Downloading update..."
            };

            progressBar = new ProgressBar
            {
                Location = new Point(15, 45),
                Size = new Size(330, 22),
                Minimum = 0,
                Maximum = 100,
                Value = 0
            };

            Controls.Add(labelStatus);
            Controls.Add(progressBar);
        }

        public void SetStatus(string text)
        {
            if (IsHandleCreated)
            {
                BeginInvoke((MethodInvoker)(() => labelStatus.Text = text));
            }
            else
            {
                labelStatus.Text = text;
            }
        }

        public void SetProgress(int percent)
        {
            if (percent < 0) percent = 0;
            if (percent > 100) percent = 100;

            if (IsHandleCreated)
            {
                BeginInvoke((MethodInvoker)(() => progressBar.Value = percent));
            }
            else
            {
                progressBar.Value = percent;
            }
        }
    }
}
