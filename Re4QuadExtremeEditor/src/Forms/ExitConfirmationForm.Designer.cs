namespace Re4QuadExtremeEditor.src.Forms
{
    partial class ExitConfirmationForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.labelMessage = new System.Windows.Forms.Label();
            this.buttonSaveProject = new System.Windows.Forms.Button();
            this.buttonSaveAll = new System.Windows.Forms.Button();
            this.buttonQuitWithoutSaving = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // labelMessage
            //
            this.labelMessage.AutoSize = false;
            this.labelMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMessage.Location = new System.Drawing.Point(12, 15);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(340, 40);
            this.labelMessage.TabIndex = 0;
            this.labelMessage.Text = "Do you want to save your changes before exiting?";
            this.labelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // buttonSaveProject
            //
            this.buttonSaveProject.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSaveProject.Location = new System.Drawing.Point(12, 65);
            this.buttonSaveProject.Name = "buttonSaveProject";
            this.buttonSaveProject.Size = new System.Drawing.Size(340, 32);
            this.buttonSaveProject.TabIndex = 1;
            this.buttonSaveProject.Text = "Save Project";
            this.buttonSaveProject.UseVisualStyleBackColor = true;
            //
            // buttonSaveAll
            //
            this.buttonSaveAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSaveAll.Location = new System.Drawing.Point(12, 103);
            this.buttonSaveAll.Name = "buttonSaveAll";
            this.buttonSaveAll.Size = new System.Drawing.Size(340, 32);
            this.buttonSaveAll.TabIndex = 2;
            this.buttonSaveAll.Text = "Save All";
            this.buttonSaveAll.UseVisualStyleBackColor = true;
            //
            // buttonQuitWithoutSaving
            //
            this.buttonQuitWithoutSaving.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonQuitWithoutSaving.Location = new System.Drawing.Point(12, 141);
            this.buttonQuitWithoutSaving.Name = "buttonQuitWithoutSaving";
            this.buttonQuitWithoutSaving.Size = new System.Drawing.Size(340, 32);
            this.buttonQuitWithoutSaving.TabIndex = 3;
            this.buttonQuitWithoutSaving.Text = "Quit Without Saving";
            this.buttonQuitWithoutSaving.UseVisualStyleBackColor = true;
            //
            // buttonCancel
            //
            this.buttonCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCancel.Location = new System.Drawing.Point(12, 179);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(340, 32);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            //
            // ExitConfirmationForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 223);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonQuitWithoutSaving);
            this.Controls.Add(this.buttonSaveAll);
            this.Controls.Add(this.buttonSaveProject);
            this.Controls.Add(this.labelMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExitConfirmationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Exit";
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.Button buttonSaveProject;
        private System.Windows.Forms.Button buttonSaveAll;
        private System.Windows.Forms.Button buttonQuitWithoutSaving;
        private System.Windows.Forms.Button buttonCancel;
    }
}
