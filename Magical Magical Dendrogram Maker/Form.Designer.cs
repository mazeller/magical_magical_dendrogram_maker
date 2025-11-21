namespace Magical_Magical_Dendrogram_Maker
{
    partial class formMain
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
            this.lblOldFasta = new System.Windows.Forms.Label();
            this.lblNewFasta = new System.Windows.Forms.Label();
            this.txtOldFasta = new System.Windows.Forms.TextBox();
            this.txtNewFasta = new System.Windows.Forms.TextBox();
            this.btnAppend = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnSave = new System.Windows.Forms.Button();
            this.mnuStrip = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.MnuFileSaveFasta = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuCreateAttach = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDendrogram = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAlign = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuTreefile = new System.Windows.Forms.ToolStripMenuItem();
            this.createDendrogramToolStripMenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.mnuTreeDendrogram = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHomology = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHomologyTable = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.lblNewSequences = new System.Windows.Forms.Label();
            this.cbxNewSequences = new System.Windows.Forms.CheckedListBox();
            this.btnAllInOne = new System.Windows.Forms.Button();
            this.mnuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblOldFasta
            // 
            this.lblOldFasta.AutoSize = true;
            this.lblOldFasta.Font = new System.Drawing.Font("Gadugi", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOldFasta.Location = new System.Drawing.Point(12, 31);
            this.lblOldFasta.Name = "lblOldFasta";
            this.lblOldFasta.Size = new System.Drawing.Size(95, 16);
            this.lblOldFasta.TabIndex = 0;
            this.lblOldFasta.Text = "Fasta Workspace";
            // 
            // lblNewFasta
            // 
            this.lblNewFasta.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lblNewFasta.AutoSize = true;
            this.lblNewFasta.Font = new System.Drawing.Font("Gadugi", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNewFasta.Location = new System.Drawing.Point(12, 226);
            this.lblNewFasta.Name = "lblNewFasta";
            this.lblNewFasta.Size = new System.Drawing.Size(174, 16);
            this.lblNewFasta.TabIndex = 1;
            this.lblNewFasta.Text = "New Sequences in Fasta Format";
            // 
            // txtOldFasta
            // 
            this.txtOldFasta.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOldFasta.Location = new System.Drawing.Point(12, 50);
            this.txtOldFasta.Multiline = true;
            this.txtOldFasta.Name = "txtOldFasta";
            this.txtOldFasta.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOldFasta.Size = new System.Drawing.Size(569, 170);
            this.txtOldFasta.TabIndex = 2;
            this.txtOldFasta.TextChanged += new System.EventHandler(this.TextBox1_TextChanged);
            // 
            // txtNewFasta
            // 
            this.txtNewFasta.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNewFasta.Location = new System.Drawing.Point(12, 245);
            this.txtNewFasta.Multiline = true;
            this.txtNewFasta.Name = "txtNewFasta";
            this.txtNewFasta.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtNewFasta.Size = new System.Drawing.Size(569, 170);
            this.txtNewFasta.TabIndex = 3;
            this.txtNewFasta.TextChanged += new System.EventHandler(this.TextBox2_TextChanged);
            // 
            // btnAppend
            // 
            this.btnAppend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAppend.Font = new System.Drawing.Font("Gadugi", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAppend.Location = new System.Drawing.Point(15, 421);
            this.btnAppend.Name = "btnAppend";
            this.btnAppend.Size = new System.Drawing.Size(75, 23);
            this.btnAppend.TabIndex = 5;
            this.btnAppend.Text = "Append";
            this.btnAppend.UseVisualStyleBackColor = true;
            this.btnAppend.Click += new System.EventHandler(this.btnAppend_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.OpenFileDialog1_FileOk);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Font = new System.Drawing.Font("Gadugi", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(96, 421);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // mnuStrip
            // 
            this.mnuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuDendrogram,
            this.mnuHomology,
            this.mnuHelp});
            this.mnuStrip.Location = new System.Drawing.Point(0, 0);
            this.mnuStrip.Name = "mnuStrip";
            this.mnuStrip.Size = new System.Drawing.Size(784, 24);
            this.mnuStrip.TabIndex = 7;
            this.mnuStrip.Text = "menuStrip";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFileOpen,
            this.toolStripMenuItem1,
            this.MnuFileSaveFasta,
            this.toolStripMenuItem2,
            this.mnuCreateAttach,
            this.toolStripMenuItem3,
            this.mnuFileExit});
            this.mnuFile.Font = new System.Drawing.Font("Gadugi", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(37, 20);
            this.mnuFile.Text = "File";
            // 
            // mnuFileOpen
            // 
            this.mnuFileOpen.Name = "mnuFileOpen";
            this.mnuFileOpen.Size = new System.Drawing.Size(184, 22);
            this.mnuFileOpen.Text = "Open";
            this.mnuFileOpen.Click += new System.EventHandler(this.MnuFileOpen_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(181, 6);
            // 
            // MnuFileSaveFasta
            // 
            this.MnuFileSaveFasta.Name = "MnuFileSaveFasta";
            this.MnuFileSaveFasta.Size = new System.Drawing.Size(184, 22);
            this.MnuFileSaveFasta.Text = "Save Fasta";
            this.MnuFileSaveFasta.Click += new System.EventHandler(this.MnuFileSaveFasta_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(181, 6);
            // 
            // mnuCreateAttach
            // 
            this.mnuCreateAttach.Name = "mnuCreateAttach";
            this.mnuCreateAttach.Size = new System.Drawing.Size(184, 22);
            this.mnuCreateAttach.Text = "Save New Sequences";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(181, 6);
            // 
            // mnuFileExit
            // 
            this.mnuFileExit.Name = "mnuFileExit";
            this.mnuFileExit.Size = new System.Drawing.Size(184, 22);
            this.mnuFileExit.Text = "Exit";
            this.mnuFileExit.Click += new System.EventHandler(this.MnuFileExit_Click);
            // 
            // mnuDendrogram
            // 
            this.mnuDendrogram.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAlign,
            this.toolStripMenuItem5,
            this.mnuTreefile,
            this.createDendrogramToolStripMenuItem,
            this.mnuTreeDendrogram});
            this.mnuDendrogram.Font = new System.Drawing.Font("Gadugi", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mnuDendrogram.Name = "mnuDendrogram";
            this.mnuDendrogram.Size = new System.Drawing.Size(86, 20);
            this.mnuDendrogram.Text = "Dendrogram";
            // 
            // mnuAlign
            // 
            this.mnuAlign.Name = "mnuAlign";
            this.mnuAlign.Size = new System.Drawing.Size(178, 22);
            this.mnuAlign.Text = "Alignment";
            this.mnuAlign.Click += new System.EventHandler(this.mnuAlign_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(175, 6);
            // 
            // mnuTreefile
            // 
            this.mnuTreefile.Name = "mnuTreefile";
            this.mnuTreefile.Size = new System.Drawing.Size(178, 22);
            this.mnuTreefile.Text = "Create Treefile";
            this.mnuTreefile.Click += new System.EventHandler(this.MnuTreefile_Click);
            // 
            // createDendrogramToolStripMenuItem
            // 
            this.createDendrogramToolStripMenuItem.Name = "createDendrogramToolStripMenuItem";
            this.createDendrogramToolStripMenuItem.Size = new System.Drawing.Size(175, 6);
            // 
            // mnuTreeDendrogram
            // 
            this.mnuTreeDendrogram.Name = "mnuTreeDendrogram";
            this.mnuTreeDendrogram.Size = new System.Drawing.Size(178, 22);
            this.mnuTreeDendrogram.Text = "Create Dendrogram";
            this.mnuTreeDendrogram.Click += new System.EventHandler(this.MnuTreeDendrogram_Click);
            // 
            // mnuHomology
            // 
            this.mnuHomology.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuHomologyTable});
            this.mnuHomology.Font = new System.Drawing.Font("Gadugi", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mnuHomology.Name = "mnuHomology";
            this.mnuHomology.Size = new System.Drawing.Size(76, 20);
            this.mnuHomology.Text = "Homology";
            // 
            // mnuHomologyTable
            // 
            this.mnuHomologyTable.Name = "mnuHomologyTable";
            this.mnuHomologyTable.Size = new System.Drawing.Size(140, 22);
            this.mnuHomologyTable.Text = "Create Table";
            this.mnuHomologyTable.Click += new System.EventHandler(this.MnuHomologyTable_Click);
            // 
            // mnuHelp
            // 
            this.mnuHelp.Name = "mnuHelp";
            this.mnuHelp.Size = new System.Drawing.Size(44, 20);
            this.mnuHelp.Text = "Help";
            this.mnuHelp.Click += new System.EventHandler(this.MnuHelp_Click);
            // 
            // lblNewSequences
            // 
            this.lblNewSequences.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNewSequences.AutoSize = true;
            this.lblNewSequences.Font = new System.Drawing.Font("Gadugi", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNewSequences.Location = new System.Drawing.Point(594, 34);
            this.lblNewSequences.Name = "lblNewSequences";
            this.lblNewSequences.Size = new System.Drawing.Size(178, 16);
            this.lblNewSequences.TabIndex = 9;
            this.lblNewSequences.Text = "New Sequences for Dendrogram";
            // 
            // cbxNewSequences
            // 
            this.cbxNewSequences.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxNewSequences.Font = new System.Drawing.Font("Gadugi", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxNewSequences.FormattingEnabled = true;
            this.cbxNewSequences.IntegralHeight = false;
            this.cbxNewSequences.Location = new System.Drawing.Point(597, 50);
            this.cbxNewSequences.Name = "cbxNewSequences";
            this.cbxNewSequences.Size = new System.Drawing.Size(175, 365);
            this.cbxNewSequences.TabIndex = 10;
            this.cbxNewSequences.SelectedIndexChanged += new System.EventHandler(this.checkedListBox1_SelectedIndexChanged);
            // 
            // btnAllInOne
            // 
            this.btnAllInOne.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAllInOne.Font = new System.Drawing.Font("Gadugi", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAllInOne.Location = new System.Drawing.Point(177, 421);
            this.btnAllInOne.Name = "btnAllInOne";
            this.btnAllInOne.Size = new System.Drawing.Size(95, 23);
            this.btnAllInOne.TabIndex = 11;
            this.btnAllInOne.Text = "Do All";
            this.btnAllInOne.UseVisualStyleBackColor = true;
            this.btnAllInOne.Click += new System.EventHandler(this.btnAllInOne_Click);
            // 
            // formMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.btnAllInOne);
            this.Controls.Add(this.cbxNewSequences);
            this.Controls.Add(this.lblNewSequences);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnAppend);
            this.Controls.Add(this.txtNewFasta);
            this.Controls.Add(this.txtOldFasta);
            this.Controls.Add(this.lblNewFasta);
            this.Controls.Add(this.lblOldFasta);
            this.Controls.Add(this.mnuStrip);
            this.MainMenuStrip = this.mnuStrip;
            this.Name = "formMain";
            this.Text = "Magical Magical Dendrogram Maker";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.mnuStrip.ResumeLayout(false);
            this.mnuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblOldFasta;
        private System.Windows.Forms.Label lblNewFasta;
        private System.Windows.Forms.TextBox txtOldFasta;
        private System.Windows.Forms.TextBox txtNewFasta;
        private System.Windows.Forms.Button btnAppend;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.MenuStrip mnuStrip;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mnuFileOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mnuFileExit;
        private System.Windows.Forms.ToolStripMenuItem mnuDendrogram;
        private System.Windows.Forms.ToolStripMenuItem mnuTreefile;
        private System.Windows.Forms.ToolStripMenuItem mnuHomology;
        private System.Windows.Forms.ToolStripMenuItem mnuHomologyTable;
        private System.Windows.Forms.ToolStripSeparator createDendrogramToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuTreeDendrogram;
        private System.Windows.Forms.ToolStripMenuItem mnuHelp;
        private System.Windows.Forms.Label lblNewSequences;
        private System.Windows.Forms.CheckedListBox cbxNewSequences;
        private System.Windows.Forms.Button btnAllInOne;
        private System.Windows.Forms.ToolStripMenuItem MnuFileSaveFasta;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem mnuCreateAttach;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem mnuAlign;
    }
}

