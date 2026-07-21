
namespace Re4QuadExtremeEditor.src.Forms
{
    partial class SelectRoomForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectRoomForm));
            this.labelModelSource = new System.Windows.Forms.Label();
            this.comboBoxModelSource = new System.Windows.Forms.ComboBox();
            this.comboBoxRoomList = new System.Windows.Forms.ComboBox();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelInfo = new System.Windows.Forms.Label();
            this.labelSelectVersion = new System.Windows.Forms.Label();
            this.comboBoxSelectVersion = new System.Windows.Forms.ComboBox();
            this.buttonLoadFiles = new System.Windows.Forms.Button();
            this.contextMenuStripLoadFiles = new System.Windows.Forms.ContextMenuStrip();
            this.toolStripMenuItemLoadESL = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemLoadAEV = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemLoadITA = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemLoadETS = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemLoadDSE = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemLoadFSE = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemLoadSAR = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemLoadEAR = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemLoadEMI = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemLoadESE = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemLoadLIT = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemLoadEFFBLOB = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripLoadFiles.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelModelSource
            // 
            this.labelModelSource.AutoSize = true;
            this.labelModelSource.Location = new System.Drawing.Point(12, 15);
            this.labelModelSource.Name = "labelModelSource";
            this.labelModelSource.Size = new System.Drawing.Size(83, 13);
            this.labelModelSource.TabIndex = 7;
            this.labelModelSource.Text = "Load Model:";
            // 
            // comboBoxModelSource
            // 
            this.comboBoxModelSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxModelSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxModelSource.Font = new System.Drawing.Font("Courier New", 9F);
            this.comboBoxModelSource.FormattingEnabled = true;
            this.comboBoxModelSource.Location = new System.Drawing.Point(101, 12);
            this.comboBoxModelSource.Name = "comboBoxModelSource";
            this.comboBoxModelSource.Size = new System.Drawing.Size(494, 23);
            this.comboBoxModelSource.TabIndex = 8;
            // 
            // comboBoxRoomList
            // 
            this.comboBoxRoomList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxRoomList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRoomList.Font = new System.Drawing.Font("Courier New", 9F);
            this.comboBoxRoomList.FormattingEnabled = true;
            this.comboBoxRoomList.Location = new System.Drawing.Point(12, 47);
            this.comboBoxRoomList.Name = "comboBoxRoomList";
            this.comboBoxRoomList.Size = new System.Drawing.Size(583, 23);
            this.comboBoxRoomList.TabIndex = 0;
            // 
            // labelSelectVersion
            // 
            this.labelSelectVersion.AutoSize = true;
            this.labelSelectVersion.Location = new System.Drawing.Point(12, 79);
            this.labelSelectVersion.Name = "labelSelectVersion";
            this.labelSelectVersion.Size = new System.Drawing.Size(83, 13);
            this.labelSelectVersion.TabIndex = 4;
            this.labelSelectVersion.Text = "Select Version:";
            // 
            // comboBoxSelectVersion
            // 
            this.comboBoxSelectVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSelectVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSelectVersion.Font = new System.Drawing.Font("Courier New", 9F);
            this.comboBoxSelectVersion.FormattingEnabled = true;
            this.comboBoxSelectVersion.Location = new System.Drawing.Point(101, 76);
            this.comboBoxSelectVersion.Name = "comboBoxSelectVersion";
            this.comboBoxSelectVersion.Size = new System.Drawing.Size(300, 23);
            this.comboBoxSelectVersion.TabIndex = 5;
            // 
            // buttonLoadFiles
            // 
            this.buttonLoadFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLoadFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLoadFiles.Location = new System.Drawing.Point(419, 76);
            this.buttonLoadFiles.Name = "buttonLoadFiles";
            this.buttonLoadFiles.Size = new System.Drawing.Size(176, 23);
            this.buttonLoadFiles.TabIndex = 6;
            this.buttonLoadFiles.Text = "Load Files (0 selected)";
            this.buttonLoadFiles.UseVisualStyleBackColor = true;
            this.buttonLoadFiles.Click += new System.EventHandler(this.buttonLoadFiles_Click);
            // 
            // contextMenuStripLoadFiles
            // 
            this.contextMenuStripLoadFiles.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemLoadESL,
            this.toolStripMenuItemLoadAEV,
            this.toolStripMenuItemLoadITA,
            this.toolStripMenuItemLoadETS,
            this.toolStripMenuItemLoadDSE,
            this.toolStripMenuItemLoadFSE,
            this.toolStripMenuItemLoadSAR,
            this.toolStripMenuItemLoadEAR,
            this.toolStripMenuItemLoadEMI,
            this.toolStripMenuItemLoadESE,
            this.toolStripMenuItemLoadLIT,
            this.toolStripMenuItemLoadEFFBLOB});
            this.contextMenuStripLoadFiles.Name = "contextMenuStripLoadFiles";
            this.contextMenuStripLoadFiles.Size = new System.Drawing.Size(180, 268);
            this.contextMenuStripLoadFiles.Closing += new System.Windows.Forms.ToolStripDropDownClosingEventHandler(this.contextMenuStripLoadFiles_Closing);
            // 
            // toolStripMenuItemLoadESL
            // 
            this.toolStripMenuItemLoadESL.CheckOnClick = true;
            this.toolStripMenuItemLoadESL.Name = "toolStripMenuItemLoadESL";
            this.toolStripMenuItemLoadESL.Size = new System.Drawing.Size(179, 22);
            this.toolStripMenuItemLoadESL.Text = "ESL FILE (ONLY VILLAGE)";
            this.toolStripMenuItemLoadESL.Tag = "ESL";
            this.toolStripMenuItemLoadESL.Click += new System.EventHandler(this.toolStripMenuItemLoadFileType_Click);
            // 
            // toolStripMenuItemLoadAEV
            // 
            this.toolStripMenuItemLoadAEV.CheckOnClick = true;
            this.toolStripMenuItemLoadAEV.Name = "toolStripMenuItemLoadAEV";
            this.toolStripMenuItemLoadAEV.Size = new System.Drawing.Size(179, 22);
            this.toolStripMenuItemLoadAEV.Text = "AEV FILE";
            this.toolStripMenuItemLoadAEV.Click += new System.EventHandler(this.toolStripMenuItemLoadFileType_Click);
            // 
            // toolStripMenuItemLoadITA
            // 
            this.toolStripMenuItemLoadITA.CheckOnClick = true;
            this.toolStripMenuItemLoadITA.Name = "toolStripMenuItemLoadITA";
            this.toolStripMenuItemLoadITA.Size = new System.Drawing.Size(179, 22);
            this.toolStripMenuItemLoadITA.Text = "ITA FILE";
            this.toolStripMenuItemLoadITA.Click += new System.EventHandler(this.toolStripMenuItemLoadFileType_Click);
            // 
            // toolStripMenuItemLoadETS
            // 
            this.toolStripMenuItemLoadETS.CheckOnClick = true;
            this.toolStripMenuItemLoadETS.Name = "toolStripMenuItemLoadETS";
            this.toolStripMenuItemLoadETS.Size = new System.Drawing.Size(179, 22);
            this.toolStripMenuItemLoadETS.Text = "ETS FILE";
            this.toolStripMenuItemLoadETS.Click += new System.EventHandler(this.toolStripMenuItemLoadFileType_Click);
            // 
            // toolStripMenuItemLoadDSE
            // 
            this.toolStripMenuItemLoadDSE.CheckOnClick = true;
            this.toolStripMenuItemLoadDSE.Name = "toolStripMenuItemLoadDSE";
            this.toolStripMenuItemLoadDSE.Size = new System.Drawing.Size(179, 22);
            this.toolStripMenuItemLoadDSE.Text = "DSE FILE";
            this.toolStripMenuItemLoadDSE.Click += new System.EventHandler(this.toolStripMenuItemLoadFileType_Click);
            // 
            // toolStripMenuItemLoadFSE
            // 
            this.toolStripMenuItemLoadFSE.CheckOnClick = true;
            this.toolStripMenuItemLoadFSE.Name = "toolStripMenuItemLoadFSE";
            this.toolStripMenuItemLoadFSE.Size = new System.Drawing.Size(179, 22);
            this.toolStripMenuItemLoadFSE.Text = "FSE FILE";
            this.toolStripMenuItemLoadFSE.Click += new System.EventHandler(this.toolStripMenuItemLoadFileType_Click);
            // 
            // toolStripMenuItemLoadSAR
            // 
            this.toolStripMenuItemLoadSAR.CheckOnClick = true;
            this.toolStripMenuItemLoadSAR.Name = "toolStripMenuItemLoadSAR";
            this.toolStripMenuItemLoadSAR.Size = new System.Drawing.Size(179, 22);
            this.toolStripMenuItemLoadSAR.Text = "SAR FILE";
            this.toolStripMenuItemLoadSAR.Click += new System.EventHandler(this.toolStripMenuItemLoadFileType_Click);
            // 
            // toolStripMenuItemLoadEAR
            // 
            this.toolStripMenuItemLoadEAR.CheckOnClick = true;
            this.toolStripMenuItemLoadEAR.Name = "toolStripMenuItemLoadEAR";
            this.toolStripMenuItemLoadEAR.Size = new System.Drawing.Size(179, 22);
            this.toolStripMenuItemLoadEAR.Text = "EAR FILE";
            this.toolStripMenuItemLoadEAR.Click += new System.EventHandler(this.toolStripMenuItemLoadFileType_Click);
            // 
            // toolStripMenuItemLoadEMI
            // 
            this.toolStripMenuItemLoadEMI.CheckOnClick = true;
            this.toolStripMenuItemLoadEMI.Name = "toolStripMenuItemLoadEMI";
            this.toolStripMenuItemLoadEMI.Size = new System.Drawing.Size(179, 22);
            this.toolStripMenuItemLoadEMI.Text = "EMI FILE";
            this.toolStripMenuItemLoadEMI.Click += new System.EventHandler(this.toolStripMenuItemLoadFileType_Click);
            // 
            // toolStripMenuItemLoadESE
            // 
            this.toolStripMenuItemLoadESE.CheckOnClick = true;
            this.toolStripMenuItemLoadESE.Name = "toolStripMenuItemLoadESE";
            this.toolStripMenuItemLoadESE.Size = new System.Drawing.Size(179, 22);
            this.toolStripMenuItemLoadESE.Text = "ESE FILE";
            this.toolStripMenuItemLoadESE.Click += new System.EventHandler(this.toolStripMenuItemLoadFileType_Click);
            // 
            // toolStripMenuItemLoadLIT
            // 
            this.toolStripMenuItemLoadLIT.CheckOnClick = true;
            this.toolStripMenuItemLoadLIT.Name = "toolStripMenuItemLoadLIT";
            this.toolStripMenuItemLoadLIT.Size = new System.Drawing.Size(179, 22);
            this.toolStripMenuItemLoadLIT.Text = "LIT FILE";
            this.toolStripMenuItemLoadLIT.Click += new System.EventHandler(this.toolStripMenuItemLoadFileType_Click);
            // 
            // toolStripMenuItemLoadEFFBLOB
            // 
            this.toolStripMenuItemLoadEFFBLOB.CheckOnClick = true;
            this.toolStripMenuItemLoadEFFBLOB.Name = "toolStripMenuItemLoadEFFBLOB";
            this.toolStripMenuItemLoadEFFBLOB.Size = new System.Drawing.Size(179, 22);
            this.toolStripMenuItemLoadEFFBLOB.Text = "EFFBLOB FILE";
            this.toolStripMenuItemLoadEFFBLOB.Click += new System.EventHandler(this.toolStripMenuItemLoadFileType_Click);
            // 
            // buttonLoad
            // 
            this.buttonLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLoad.Location = new System.Drawing.Point(419, 111);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(85, 23);
            this.buttonLoad.TabIndex = 1;
            this.buttonLoad.Text = "LOAD";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCancel.Location = new System.Drawing.Point(510, 111);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(85, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "CANCEL";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // labelInfo
            // 
            this.labelInfo.AutoSize = true;
            this.labelInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfo.Location = new System.Drawing.Point(12, 115);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(323, 15);
            this.labelInfo.TabIndex = 3;
            this.labelInfo.Text = "After click \"LOAD\", wait for the room to be loaded.";
            // 
            // SelectRoomForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(607, 146);
            this.Controls.Add(this.labelModelSource);
            this.Controls.Add(this.comboBoxModelSource);
            this.Controls.Add(this.buttonLoadFiles);
            this.Controls.Add(this.comboBoxSelectVersion);
            this.Controls.Add(this.labelSelectVersion);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonLoad);
            this.Controls.Add(this.comboBoxRoomList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(4000, 177);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(615, 177);
            this.Name = "SelectRoomForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Room";
            this.TopMost = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SelectRoomForm_KeyDown);
            this.contextMenuStripLoadFiles.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelModelSource;
        private System.Windows.Forms.ComboBox comboBoxModelSource;
        private System.Windows.Forms.ComboBox comboBoxRoomList;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.Label labelSelectVersion;
        private System.Windows.Forms.ComboBox comboBoxSelectVersion;
        private System.Windows.Forms.Button buttonLoadFiles;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripLoadFiles;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemLoadESL;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemLoadAEV;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemLoadITA;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemLoadETS;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemLoadDSE;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemLoadFSE;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemLoadSAR;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemLoadEAR;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemLoadEMI;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemLoadESE;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemLoadLIT;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemLoadEFFBLOB;
    }
}
