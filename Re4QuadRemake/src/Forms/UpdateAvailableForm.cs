using System;
using System.Drawing;
using System.Windows.Forms;

namespace Re4QuadExtremeEditor.src.Forms
{
    /// <summary>
    /// Simple "update available" dialog: shows current version, new version, and the download
    /// size, with Update/Cancel buttons. Built entirely in code (no .Designer.cs) to keep this
    /// self-contained and avoid touching the WinForms designer surface for a one-off dialog.
    /// </summary>
    public class UpdateAvailableForm : Form
    {
        public bool UserChoseUpdate { get; private set; }

        private readonly Label labelMessage;
        private readonly Button buttonUpdate;
        private readonly Button buttonCancel;

        public UpdateAvailableForm(string currentVersion, string newVersion, long downloadSizeBytes)
        {
            Text = "Update Available";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            ClientSize = new Size(380, 160);

            labelMessage = new Label
            {
                AutoSize = false,
                Location = new Point(15, 15),
                Size = new Size(350, 90),
                Text =
                    "A new version of RE4 Quad Extreme Editor [Remake] is available." + Environment.NewLine + Environment.NewLine +
                    "Current version: " + currentVersion + Environment.NewLine +
                    "New version: " + newVersion + Environment.NewLine +
                    "Download size: " + FormatSize(downloadSizeBytes)
            };

            buttonUpdate = new Button
            {
                Text = "Update",
                Location = new Point(190, 115),
                Size = new Size(80, 28),
                DialogResult = DialogResult.OK
            };
            buttonUpdate.Click += (s, e) => { UserChoseUpdate = true; Close(); };

            buttonCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(280, 115),
                Size = new Size(80, 28),
                DialogResult = DialogResult.Cancel
            };
            buttonCancel.Click += (s, e) => { UserChoseUpdate = false; Close(); };

            Controls.Add(labelMessage);
            Controls.Add(buttonUpdate);
            Controls.Add(buttonCancel);

            AcceptButton = buttonUpdate;
            CancelButton = buttonCancel;
        }

        private static string FormatSize(long bytes)
        {
            if (bytes <= 0) return "unknown";
            double mb = bytes / (1024.0 * 1024.0);
            return mb.ToString("0.0") + " MB";
        }
    }
}
