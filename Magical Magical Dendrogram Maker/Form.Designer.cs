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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formMain));
            lblOldFasta = new Label();
            lblNewFasta = new Label();
            txtOldFasta = new TextBox();
            txtNewFasta = new TextBox();
            btnAppend = new Button();
            openFileDialog1 = new OpenFileDialog();
            btnSave = new Button();
            mnuStrip = new MenuStrip();
            mnuFile = new ToolStripMenuItem();
            mnuFileOpen = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            MnuFileSaveFasta = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripSeparator();
            mnuCreateAttach = new ToolStripMenuItem();
            toolStripMenuItem3 = new ToolStripSeparator();
            mnuFileExit = new ToolStripMenuItem();
            mnuDendrogram = new ToolStripMenuItem();
            mnuAlign = new ToolStripMenuItem();
            toolStripMenuItem5 = new ToolStripSeparator();
            mnuTreefile = new ToolStripMenuItem();
            createDendrogramToolStripMenuItem = new ToolStripSeparator();
            mnuTreeDendrogram = new ToolStripMenuItem();
            mnuHomology = new ToolStripMenuItem();
            mnuHomologyTable = new ToolStripMenuItem();
            toolStripMenuItem4 = new ToolStripSeparator();
            createAminoAcidTableToolStripMenuItem = new ToolStripMenuItem();
            mnuHelp = new ToolStripMenuItem();
            instructionsToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem7 = new ToolStripSeparator();
            intermediateFileToggleToolStripMenuItem = new ToolStripMenuItem();
            lblNewSequences = new Label();
            cbxNewSequences = new CheckedListBox();
            btnAllInOne = new Button();
            mnuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // lblOldFasta
            // 
            lblOldFasta.AutoSize = true;
            lblOldFasta.Font = new Font("Gadugi", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblOldFasta.Location = new Point(14, 36);
            lblOldFasta.Margin = new Padding(4, 0, 4, 0);
            lblOldFasta.Name = "lblOldFasta";
            lblOldFasta.Size = new Size(95, 16);
            lblOldFasta.TabIndex = 0;
            lblOldFasta.Text = "Fasta Workspace";
            // 
            // lblNewFasta
            // 
            lblNewFasta.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            lblNewFasta.AutoSize = true;
            lblNewFasta.Font = new Font("Gadugi", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblNewFasta.Location = new Point(14, 261);
            lblNewFasta.Margin = new Padding(4, 0, 4, 0);
            lblNewFasta.Name = "lblNewFasta";
            lblNewFasta.Size = new Size(210, 16);
            lblNewFasta.TabIndex = 1;
            lblNewFasta.Text = "New Sequences in Fasta or Seq Format";
            // 
            // txtOldFasta
            // 
            txtOldFasta.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtOldFasta.Location = new Point(14, 58);
            txtOldFasta.Margin = new Padding(4, 3, 4, 3);
            txtOldFasta.MaxLength = 0;
            txtOldFasta.Multiline = true;
            txtOldFasta.Name = "txtOldFasta";
            txtOldFasta.ScrollBars = ScrollBars.Vertical;
            txtOldFasta.Size = new Size(663, 196);
            txtOldFasta.TabIndex = 2;
            txtOldFasta.TextChanged += TextBox1_TextChanged;
            // 
            // txtNewFasta
            // 
            txtNewFasta.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtNewFasta.Location = new Point(14, 283);
            txtNewFasta.Margin = new Padding(4, 3, 4, 3);
            txtNewFasta.MaxLength = 0;
            txtNewFasta.Multiline = true;
            txtNewFasta.Name = "txtNewFasta";
            txtNewFasta.ScrollBars = ScrollBars.Vertical;
            txtNewFasta.Size = new Size(663, 196);
            txtNewFasta.TabIndex = 3;
            txtNewFasta.TextChanged += TextBox2_TextChanged;
            // 
            // btnAppend
            // 
            btnAppend.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnAppend.Font = new Font("Gadugi", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            btnAppend.Location = new Point(18, 486);
            btnAppend.Margin = new Padding(4, 3, 4, 3);
            btnAppend.Name = "btnAppend";
            btnAppend.Size = new Size(88, 27);
            btnAppend.TabIndex = 5;
            btnAppend.Text = "Append";
            btnAppend.UseVisualStyleBackColor = true;
            btnAppend.Click += btnAppend_Click;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnSave.Font = new Font("Gadugi", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            btnSave.Location = new Point(112, 486);
            btnSave.Margin = new Padding(4, 3, 4, 3);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(88, 27);
            btnSave.TabIndex = 6;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // mnuStrip
            // 
            mnuStrip.Items.AddRange(new ToolStripItem[] { mnuFile, mnuDendrogram, mnuHomology, mnuHelp });
            mnuStrip.Location = new Point(0, 0);
            mnuStrip.Name = "mnuStrip";
            mnuStrip.Padding = new Padding(7, 2, 0, 2);
            mnuStrip.Size = new Size(915, 24);
            mnuStrip.TabIndex = 7;
            mnuStrip.Text = "menuStrip";
            // 
            // mnuFile
            // 
            mnuFile.DropDownItems.AddRange(new ToolStripItem[] { mnuFileOpen, toolStripMenuItem1, MnuFileSaveFasta, toolStripMenuItem2, mnuCreateAttach, toolStripMenuItem3, mnuFileExit });
            mnuFile.Font = new Font("Gadugi", 9F, FontStyle.Regular, GraphicsUnit.Point);
            mnuFile.Name = "mnuFile";
            mnuFile.Size = new Size(37, 20);
            mnuFile.Text = "File";
            // 
            // mnuFileOpen
            // 
            mnuFileOpen.Name = "mnuFileOpen";
            mnuFileOpen.Size = new Size(184, 22);
            mnuFileOpen.Text = "Open";
            mnuFileOpen.Click += MnuFileOpen_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(181, 6);
            // 
            // MnuFileSaveFasta
            // 
            MnuFileSaveFasta.Name = "MnuFileSaveFasta";
            MnuFileSaveFasta.Size = new Size(184, 22);
            MnuFileSaveFasta.Text = "Save Fasta";
            MnuFileSaveFasta.Click += MnuFileSaveFasta_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(181, 6);
            // 
            // mnuCreateAttach
            // 
            mnuCreateAttach.Name = "mnuCreateAttach";
            mnuCreateAttach.Size = new Size(184, 22);
            mnuCreateAttach.Text = "Save New Sequences";
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new Size(181, 6);
            // 
            // mnuFileExit
            // 
            mnuFileExit.Name = "mnuFileExit";
            mnuFileExit.Size = new Size(184, 22);
            mnuFileExit.Text = "Exit";
            mnuFileExit.Click += MnuFileExit_Click;
            // 
            // mnuDendrogram
            // 
            mnuDendrogram.DropDownItems.AddRange(new ToolStripItem[] { mnuAlign, toolStripMenuItem5, mnuTreefile, createDendrogramToolStripMenuItem, mnuTreeDendrogram });
            mnuDendrogram.Font = new Font("Gadugi", 9F, FontStyle.Regular, GraphicsUnit.Point);
            mnuDendrogram.Name = "mnuDendrogram";
            mnuDendrogram.Size = new Size(86, 20);
            mnuDendrogram.Text = "Dendrogram";
            // 
            // mnuAlign
            // 
            mnuAlign.Name = "mnuAlign";
            mnuAlign.Size = new Size(178, 22);
            mnuAlign.Text = "Align Fasta";
            mnuAlign.Click += mnuAlign_Click;
            // 
            // toolStripMenuItem5
            // 
            toolStripMenuItem5.Name = "toolStripMenuItem5";
            toolStripMenuItem5.Size = new Size(175, 6);
            // 
            // mnuTreefile
            // 
            mnuTreefile.Name = "mnuTreefile";
            mnuTreefile.Size = new Size(178, 22);
            mnuTreefile.Text = "Create Treefile";
            mnuTreefile.Click += MnuTreefile_Click;
            // 
            // createDendrogramToolStripMenuItem
            // 
            createDendrogramToolStripMenuItem.Name = "createDendrogramToolStripMenuItem";
            createDendrogramToolStripMenuItem.Size = new Size(175, 6);
            // 
            // mnuTreeDendrogram
            // 
            mnuTreeDendrogram.Name = "mnuTreeDendrogram";
            mnuTreeDendrogram.Size = new Size(178, 22);
            mnuTreeDendrogram.Text = "Create Dendrogram";
            mnuTreeDendrogram.Click += MnuTreeDendrogram_Click;
            // 
            // mnuHomology
            // 
            mnuHomology.DropDownItems.AddRange(new ToolStripItem[] { mnuHomologyTable, toolStripMenuItem4, createAminoAcidTableToolStripMenuItem });
            mnuHomology.Font = new Font("Gadugi", 9F, FontStyle.Regular, GraphicsUnit.Point);
            mnuHomology.Name = "mnuHomology";
            mnuHomology.Size = new Size(76, 20);
            mnuHomology.Text = "Homology";
            // 
            // mnuHomologyTable
            // 
            mnuHomologyTable.Name = "mnuHomologyTable";
            mnuHomologyTable.Size = new Size(206, 22);
            mnuHomologyTable.Text = "Create Nucleotide Table";
            mnuHomologyTable.Click += MnuHomologyTable_Click;
            // 
            // toolStripMenuItem4
            // 
            toolStripMenuItem4.Name = "toolStripMenuItem4";
            toolStripMenuItem4.Size = new Size(203, 6);
            // 
            // createAminoAcidTableToolStripMenuItem
            // 
            createAminoAcidTableToolStripMenuItem.Name = "createAminoAcidTableToolStripMenuItem";
            createAminoAcidTableToolStripMenuItem.Size = new Size(206, 22);
            createAminoAcidTableToolStripMenuItem.Text = "Create Amino Acid Table";
            createAminoAcidTableToolStripMenuItem.Click += createAminoAcidTableToolStripMenuItem_Click;
            // 
            // mnuHelp
            // 
            mnuHelp.DropDownItems.AddRange(new ToolStripItem[] { instructionsToolStripMenuItem, toolStripMenuItem7, intermediateFileToggleToolStripMenuItem });
            mnuHelp.Name = "mnuHelp";
            mnuHelp.Size = new Size(44, 20);
            mnuHelp.Text = "Help";
            // 
            // instructionsToolStripMenuItem
            // 
            instructionsToolStripMenuItem.Name = "instructionsToolStripMenuItem";
            instructionsToolStripMenuItem.Size = new Size(200, 22);
            instructionsToolStripMenuItem.Text = "Instructions";
            instructionsToolStripMenuItem.Click += instructionsToolStripMenuItem_Click_1;
            // 
            // toolStripMenuItem7
            // 
            toolStripMenuItem7.Name = "toolStripMenuItem7";
            toolStripMenuItem7.Size = new Size(197, 6);
            // 
            // intermediateFileToggleToolStripMenuItem
            // 
            intermediateFileToggleToolStripMenuItem.Name = "intermediateFileToggleToolStripMenuItem";
            intermediateFileToggleToolStripMenuItem.Size = new Size(200, 22);
            intermediateFileToggleToolStripMenuItem.Text = "Intermediate File Toggle";
            intermediateFileToggleToolStripMenuItem.Click += intermediateFileToggleToolStripMenuItem_Click_1;
            // 
            // lblNewSequences
            // 
            lblNewSequences.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblNewSequences.AutoSize = true;
            lblNewSequences.Font = new Font("Gadugi", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblNewSequences.Location = new Point(693, 39);
            lblNewSequences.Margin = new Padding(4, 0, 4, 0);
            lblNewSequences.Name = "lblNewSequences";
            lblNewSequences.Size = new Size(178, 16);
            lblNewSequences.TabIndex = 9;
            lblNewSequences.Text = "New Sequences for Dendrogram";
            // 
            // cbxNewSequences
            // 
            cbxNewSequences.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cbxNewSequences.Font = new Font("Gadugi", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            cbxNewSequences.FormattingEnabled = true;
            cbxNewSequences.IntegralHeight = false;
            cbxNewSequences.Location = new Point(696, 58);
            cbxNewSequences.Margin = new Padding(4, 3, 4, 3);
            cbxNewSequences.Name = "cbxNewSequences";
            cbxNewSequences.Size = new Size(204, 421);
            cbxNewSequences.TabIndex = 10;
            cbxNewSequences.SelectedIndexChanged += checkedListBox1_SelectedIndexChanged;
            // 
            // btnAllInOne
            // 
            btnAllInOne.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnAllInOne.Font = new Font("Gadugi", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            btnAllInOne.Location = new Point(206, 486);
            btnAllInOne.Margin = new Padding(4, 3, 4, 3);
            btnAllInOne.Name = "btnAllInOne";
            btnAllInOne.Size = new Size(111, 27);
            btnAllInOne.TabIndex = 11;
            btnAllInOne.Text = "Run All";
            btnAllInOne.UseVisualStyleBackColor = true;
            btnAllInOne.Click += btnAllInOne_Click;
            // 
            // formMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(915, 532);
            Controls.Add(txtOldFasta);
            Controls.Add(btnAllInOne);
            Controls.Add(cbxNewSequences);
            Controls.Add(lblNewSequences);
            Controls.Add(btnSave);
            Controls.Add(btnAppend);
            Controls.Add(txtNewFasta);
            Controls.Add(lblNewFasta);
            Controls.Add(lblOldFasta);
            Controls.Add(mnuStrip);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = mnuStrip;
            Margin = new Padding(4, 3, 4, 3);
            Name = "formMain";
            Text = "Magical Magical Dendrogram Maker";
            Load += Form1_Load;
            mnuStrip.ResumeLayout(false);
            mnuStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

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
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem createAminoAcidTableToolStripMenuItem;
        private ToolStripMenuItem instructionsToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem7;
        private ToolStripMenuItem intermediateFileToggleToolStripMenuItem;
    }
}

