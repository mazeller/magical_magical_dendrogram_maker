using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace Magical_Magical_Dendrogram_Maker
{

    public partial class formMain : Form
    {
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_RESTORE = 9;

        public formMain()
        {
            InitializeComponent();
            this.Resize += Form1_Resize;
            txtOldFasta.AllowDrop = true;
            txtNewFasta.AllowDrop = true;

            txtOldFasta.DragEnter += CheckDragEnter;
            txtOldFasta.DragDrop += TextBox1_DragDrop;
            txtNewFasta.DragEnter += CheckDragEnter;
            txtNewFasta.DragDrop += TextBox2_DragDrop;
        }

        // --- Drag and Drop ---
        // save fasta location for pipeline
        string fastaPath = "";
        string seqText = "";
        bool deleteIntermediateFile = true;

        private void CheckDragEnter(object sender, DragEventArgs e)
        {
            // Check if the data being dragged is one or more files
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        // Fasta Workplace takes only one fasta
        private void TextBox1_DragDrop(object sender, DragEventArgs e)
        {
            string[] paths = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (paths != null && paths.Length > 0)
            {
                if (ValidateFasta(paths[0]))
                {
                    if (!string.IsNullOrEmpty(txtOldFasta.Text))
                    {
                        MessageBox.Show($"Selected file: {paths[0]}");
                    }
                    txtOldFasta.Text = File.ReadAllText(fastaPath).Trim();
                    cbxNewSequences.Items.Clear();
                }
                else
                {
                    MessageBox.Show("Invalid format, please choose a fasta file.");
                }
            }
        }

        // New Sequences takes both fasta and seq files and additively appends them to existing text
        private void TextBox2_DragDrop(object sender, DragEventArgs e)
        {
            string[] paths = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (paths != null && paths.Length > 0)
            {
                if (ValidateFasta(paths[0]))
                {
                    fastaPath = paths[0];
                    // first item in textbox 2 trimmed for consistency when fasta have trailing newlines
                    if (string.IsNullOrEmpty(txtNewFasta.Text))
                    {
                        txtNewFasta.Text += File.ReadAllText(fastaPath).Trim();
                    }
                    // ensures proper spacing between each element
                    else
                    {
                        txtNewFasta.Text += "\r\n" + File.ReadAllText(fastaPath).Trim();
                        txtNewFasta.SelectionStart = txtNewFasta.Text.Length;
                        txtNewFasta.ScrollToCaret();
                    }
                }
                else if (ParseSeq(paths[0]))
                {
                    // first item in textbox 2 trimmed for consistency when fasta have trailing newlines
                    if (string.IsNullOrEmpty(txtNewFasta.Text))
                    {
                        txtNewFasta.Text += seqText;
                    }
                    // ensures proper spacing between each element
                    else
                    {
                        txtNewFasta.Text += "\r\n" + seqText;
                        txtNewFasta.SelectionStart = txtNewFasta.Text.Length;
                        txtNewFasta.ScrollToCaret();
                    }
                }
                else
                {
                    MessageBox.Show("Invalid format, please choose a fasta file.");
                }
            }
        }

        // --- Fasta editing ---

        // Append typed contents to the fasta contents and save new fasta
        private void btnAppend_Click(object sender, EventArgs e)
        {
            txtOldFasta.Text += "\r\n" + txtNewFasta.Text;

            /*MessageBox.Show("Sequences appended successfully!",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);*/

            txtOldFasta.SelectionStart = txtOldFasta.Text.Length;
            txtOldFasta.ScrollToCaret();

            // Add new sequences to new sequence check list
            string[] newLines = txtNewFasta.Lines;
            int i = 0;
            foreach (string line in newLines)
            {
                if (!string.IsNullOrWhiteSpace(line) && line.StartsWith(">"))
                {
                    cbxNewSequences.Items.Add(line.Trim().Replace(">", ""));
                    cbxNewSequences.SetItemChecked(i, true);
                    i++;

                }
            }
            txtNewFasta.Clear();
        }

        // Save fasta
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveMethod();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e) { }

        private void TextBox2_TextChanged(object sender, EventArgs e) { }

        // Check for required tools on load
        private void Form1_Load(object sender, EventArgs e)
        {
            string appBase = AppDomain.CurrentDomain.BaseDirectory;
            string dir = Path.GetFullPath(Path.Combine(appBase, "..", ".."));
            bool mafftExists = false;
            bool iqtreeExists = false;
            bool psScriptExists = false;
            bool treeviewerExists = false;

            // This is for the debug/release/project version
            string mafftPath = Path.Combine(dir, "mafft-win", "mafft.bat");
            string iqtreePath = Path.Combine(dir, "iqtree", "iqtree3.exe");
            string psScript = Path.Combine(dir, "iqtree", "safe_iqtree_powershell.ps1");
            string treeviewerPath = Path.Combine(dir, "TreeViewer", "TreeViewerCommandLine.exe");
            // This is for the published version
            string mafftPath2 = Path.Combine(appBase, "mafft-win", "mafft.bat");
            string iqtreePath2 = Path.Combine(appBase, "iqtree", "iqtree3.exe");
            string psScript2 = Path.Combine(appBase, "iqtree", "safe_iqtree_powershell.ps1");
            string treeviewerPath2 = Path.Combine(appBase, "TreeViewer", "TreeViewerCommandLine.exe");

            mafftExists = File.Exists(mafftPath);
            iqtreeExists = File.Exists(iqtreePath);
            psScriptExists = File.Exists(psScript);
            treeviewerExists = File.Exists(treeviewerPath);

            if (!mafftExists)
            {
                mafftPath = mafftPath2;
                mafftExists = File.Exists(mafftPath2);
            }
            if (!iqtreeExists)
            {
                iqtreePath = iqtreePath2;
                iqtreeExists = File.Exists(iqtreePath2);
            }
            if (!psScriptExists)
            {
                psScript = psScript2;
                psScriptExists = File.Exists(psScript2);
            }
            if (!treeviewerExists)
            {
                treeviewerPath = treeviewerPath2;
                treeviewerExists = File.Exists(treeviewerPath2);
            }

            if (!mafftExists)
            {
                MessageBox.Show("MAFFT not found. Please install MAFFT and place it in the application directory.",
                "Missing Tool",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
            else
            {
                Console.WriteLine("MAFFT found: " + mafftPath);
            }
            if (!iqtreeExists)
            {
                MessageBox.Show("IQ-TREE not found. Please install IQ-TREE and place it in the application directory.",
                "Missing Tool",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
            else
            {
                Console.WriteLine("IQ-TREE found: " + iqtreePath);
            }
            if (!psScriptExists)
            {
                MessageBox.Show("PowerShell wrapper script for IQ-TREE not found. Please ensure it is in the iqtree directory.",
                "Missing Tool",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
            else
            {
                Console.WriteLine("PowerShell wrapper script found: " + psScript);
            }
            if (!treeviewerExists)
            {
                MessageBox.Show("TreeViewer not found. Please install TreeViewer and place it in the application directory.",
                "Missing Tool",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
            else
            {
                Console.WriteLine("TreeViewer found: " + treeviewerPath);
            }
        }

        // Handles the form resize event
        private void Form1_Resize(object sender, EventArgs e)
        {
            int height = this.ClientSize.Height;
            int change = (460 - height) / 2;

            // rate of box size change
            int boxHeight = (height - 460) / (500 / 180);

            // Resize boxes, reponsition label
            txtOldFasta.Height = 160 + boxHeight;
            txtNewFasta.Height = 160 + boxHeight;
            cbxNewSequences.Height = 350 + boxHeight * 2;
            lblNewFasta.Top = 226 - change;
        }

        // Handles the "Open" menu click event
        private void MnuFileOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Fasta files (*.fasta)|*.fasta|All files (*.*)|*.*",
                Title = "Select a fasta file"
            };
            //Load file
            using (ofd)
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtOldFasta.Text = File.ReadAllText(ofd.FileName);
                    cbxNewSequences.Items.Clear();
                }
            }
        }

        private void SaveMethod()
        {
            // Checks if there is text to save
            if (!string.IsNullOrEmpty(txtOldFasta.Text))
            {
                // Checks if a file path is already set
                if (!string.IsNullOrEmpty(fastaPath))
                {
                    // preparing to put new fasta in location of old fasta, and find location for archive
                    string fastaDir = Path.GetDirectoryName(fastaPath);
                    string archiveDir = Directory.EnumerateDirectories(fastaDir, "*", SearchOption.TopDirectoryOnly).FirstOrDefault(d => string.Equals(Path.GetFileName(d), "archive", StringComparison.OrdinalIgnoreCase));

                    // Create if not found
                    if (archiveDir == null)
                    {
                        archiveDir = Path.Combine(fastaDir, "archive");
                        Directory.CreateDirectory(archiveDir);
                    }

                    // archive old fasta
                    string fastaName = Path.GetFileName(fastaPath);
                    string archivedPath = Path.Combine(archiveDir, fastaName);
                    // handle archive filename conflicts
                    int count = 1;
                    string archivedNameNoExt = Path.GetFileNameWithoutExtension(fastaName);
                    while (File.Exists(archivedPath))
                    {
                        archivedPath = Path.Combine(archiveDir, archivedNameNoExt + " (" + count + ").fasta");
                        count++;
                    }
                    File.Move(fastaPath, archivedPath);
                }
                //prompt for save location
                SaveFileDialog sfd = new SaveFileDialog()
                {
                    Filter = "Fasta files (*.fasta)|*.fasta|All files (*.*)|*.*",
                    Title = "Save location for fasta",
                    FileName = Path.GetFileName(fastaPath),
                    DefaultExt = "fasta",
                    RestoreDirectory = true
                };

                using (sfd)
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        fastaPath = sfd.FileName;
                    }
                    else
                    {
                        return;
                    }
                }

                // Build new FASTA
                StringBuilder fastaBuilder = new StringBuilder();

                fastaBuilder.AppendLine(txtOldFasta.Text + "\r\n");

                // Save new file
                File.WriteAllText(fastaPath, fastaBuilder.ToString());

                MessageBox.Show("Fasta saved successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Make a fasta file");
            }
        }

        // Handles the "Save" menu click event
        private void MnuFileSaveFasta_Click(object sender, EventArgs e)
        {
            SaveMethod();
        }

        // Handles the "Exit" menu click event
        private void MnuFileExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // --- Alignment Section ---

        // Handles mafft cmds
        public String RunMafft(string file, string mafft)
        {

            // Format paths and commands for process
            string alnFile = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + "_aligned.fasta");
            string command = $"--auto \"{file}\"";
            var mafftDir = Path.GetDirectoryName(mafft);

            // mafft process settings
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = mafft,
                Arguments = command,
                WorkingDirectory = mafftDir,
                RedirectStandardInput = false,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            // begin mafft process
            using var proc = Process.Start(psi);

            var stderrTask = proc.StandardError.ReadToEndAsync(); //prevent mafft block by draining

            // write file
            using (var fs = File.Create(alnFile))
            {
                proc.StandardOutput.BaseStream.CopyTo(fs);
            }

            proc.WaitForExit();
            string stderr = stderrTask.Result;

            if (proc.ExitCode != 0)
            {
                throw new Exception("MAFFT failed:\n" + stderr);
            }

            return alnFile;
        }


        // Handles the "Edit Align" menu click event
        private async void mnuAlign_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Fasta files (*.fasta)|*.fasta|All files (*.*)|*.*",
                Title = "Select a fasta file"
            };
            using (ofd)
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (ValidateFasta(ofd.FileName))
                    {
                        string mafftPath = ResolveToolPath("mafft-win", "mafft.bat");
                        string mafftStatus = "MAFFT failed to run.";
                        string alnPath = ofd.FileName;
                        await LoadingForm.RunWithLoading(this, "Aligning sequences with MAFFT...", async () =>
                        {
                            await Task.Run(() =>
                            {
                                alnPath = RunMafft(alnPath, mafftPath);
                                Invoke(new Action(() =>
                                {
                                    txtOldFasta.Text = File.ReadAllText(alnPath).Replace("\n", "\r\n").ToUpper();
                                }));
                            });
                            mafftStatus = "Aligned file created at: " + alnPath;
                            if (mafftStatus == "Aligned file created at: ")
                            {
                                mafftStatus = "MAFFT ran but failed to produce aligned fasta";
                            }
                        });
                        MessageBox.Show(mafftStatus);
                    }
                }
            }
        }

        // Handles attachment creation events for dendrogram styles
        private void mnuAttach_Click(object sender, EventArgs e)
        {

            throw new NotImplementedException();

            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*",
                Title = "Save Text file"
            };
            using (sfd)
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    // Build attachment contents
                    StringBuilder stringBuilder = new StringBuilder();

                    stringBuilder.AppendLine("Name Color");

                    // Add new sequences to attachment
                    foreach (var item in cbxNewSequences.CheckedItems)
                    {
                        string sequenceName = item.ToString().TrimStart('>');
                        stringBuilder.AppendLine($"{sequenceName}\t#FF0000");
                    }

                    // Save to stringBuilder
                    System.IO.File.WriteAllText(sfd.FileName, stringBuilder.ToString());


                    MessageBox.Show("Attachment saved successfully!",
                                "Success",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                }
            }
        }

        // --- Treefile Section ---

        // Handles safe_iqtree commands
        private string RunSafeIqtree(string file, string iqtreeScript)
        {
            // Ensure the PowerShell script exists
            if (!File.Exists(iqtreeScript))
            {
                ShowErrorForm($"Safe Iqtree script not found at: {iqtreeScript}");
                return null;
            }

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-ExecutionPolicy Bypass -File \"{iqtreeScript}\" \"{file}\" -nt AUTO -m GTR+G",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = Path.GetDirectoryName(file)
            };

            StringBuilder stdout = new StringBuilder();
            StringBuilder stderr = new StringBuilder();

            using (Process proc = new Process())
            {
                proc.StartInfo = psi;
                proc.OutputDataReceived += (s, e) => { if (e.Data != null) stdout.AppendLine(e.Data); };
                proc.ErrorDataReceived += (s, e) => { if (e.Data != null) stderr.AppendLine(e.Data); };

                proc.Start();
                proc.BeginOutputReadLine();
                proc.BeginErrorReadLine();
                proc.WaitForExit();

                if (proc.ExitCode != 0)
                {
                    ShowErrorForm("IQ-TREE failed.\n\nSTDOUT:\n" + stdout + "\n\nSTDERR:\n" + stderr);
                    return null;
                }
            }

            string fastaDir = Path.GetDirectoryName(file);
            string searchPattern = Path.GetFileNameWithoutExtension(file) + "*_restored.treefile";
            string[] foundFiles = Directory.GetFiles(fastaDir, searchPattern);

            if (foundFiles.Length > 0)
            {
                foreach (var dir in Directory.GetDirectories(
                             fastaDir,
                             "tmp_" + Path.GetFileNameWithoutExtension(file)))
                {
                    Directory.Delete(dir, recursive: true);
                }
                return foundFiles[0];
            }
            else
            {
                ShowErrorForm("IQ-TREE finished but no treefile was created.\n\nSTDOUT:\n" + stdout + "\n\nSTDERR:\n" + stderr);
                return null;
            }
        }

        // Handles "Tree Treefile" menu click event
        private async void MnuTreefile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Fasta files (*.fasta)|*.fasta|All files (*.*)|*.*",
                Title = "Select a fasta file"
            };
            // Get iqtree paths to create treefile
            using (ofd)
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (ValidateFasta(ofd.FileName))
                    {
                        string safeIqtreePath = ResolveToolPath("iqtree", "safe_iqtree_powershell.ps1");
                        string iqtreeStatus = "IQTREE failed to run.";
                        string treefilePath = ofd.FileName;
                        await LoadingForm.RunWithLoading(this, "Creating Treefile with IQTREE...", async () =>
                        {
                            await Task.Run(() =>
                            {
                                treefilePath = RunSafeIqtree(treefilePath, safeIqtreePath);
                            });
                            iqtreeStatus = "Treefile created at: " + treefilePath;
                            if (iqtreeStatus == "Treefile created at: ")
                            {
                                iqtreeStatus = "IQTREE ran but failed to produce new treefile";
                            }
                        });
                        MessageBox.Show(iqtreeStatus);
                    }
                }
            }
        }

        // --- Dendrogram Section ---

        // Handles treeview commands via opening window and pasting and creates the dendrogram
        private string RunDendrogram(string file, string treePath, string attachmentPath)
        {
            file = Path.GetFullPath(file);
            attachmentPath = Path.GetFullPath(attachmentPath);

            if (!File.Exists(treePath))
            {
                MessageBox.Show($"TreeView not found at {treePath}");
                return null;
            }

            string outputImage = Path.Combine(Path.GetDirectoryName(file),
                Path.GetFileNameWithoutExtension(file) + "_tree.png");

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = treePath,
                UseShellExecute = true,
                CreateNoWindow = false,
                WindowStyle = ProcessWindowStyle.Normal
            };

            Process proc = Process.Start(psi);
            Thread.Sleep(10000); // give TreeViewer time to load

            string[] commands = new[]
            {
    $"file {file}",
    "open",
    "load",
    "y",
    "module enable Reroot tree",
    "module select Reroot tree",
    "option select Rooting mode",
    "option set Mid-point",
    "update all",
    "module enable Sort nodes",
    "module select Sort nodes",
    "option select Order",
    "option set Ascending",
    "update all",
    "module enable Rectangular",
    "module select Rectangular",
    "option select Width",
    "option set 1200",
    "option select Height",
    "option set 1200",
    "update all",
    "module enable Scale tree",
    "module select Scale tree",
    "option select Scaling factor",
    "option set 1000",
    "update all",
    "module enable Branches",
    "module select Branches",
    "option select Root branch",
    "option set true",
    "module enable Labels",
    "module select Labels",
    "option select Show on",
    "option set Leaves",
    "node select root",
    $"attachment add {attachmentPath}",
    "attach",
    "y",
    "y",
    "module enable parse node states",
    "module select parse node states",
    "option select #1",
    "option set attach",
    "option select #9",
    "option set true",
    "update all",
    $"png {outputImage}",
    "exit"
};

            string joinedCommands = string.Join("\r\n", commands) + "\r\n";
            Clipboard.SetText(joinedCommands);
            Thread.Sleep(100);

            SetForegroundWindow(proc.MainWindowHandle);
            ShowWindow(proc.MainWindowHandle, SW_RESTORE);
            Thread.Sleep(100);

            SendKeys.SendWait("^{v}");
            SendKeys.SendWait("{ENTER}");

            MessageBox.Show("Dendrogram generation completed.");
            return outputImage;
        }

        // Handles treeview commands via redirected process and creates the dendrogram
        private string RunDendrogramRedirected(string file, string treePath, string attachmentPath)
        {
            if (!File.Exists(treePath))
            {
                MessageBox.Show($"TreeViewer not found at {treePath}");
                return null;
            }

            // increment 10 per extra 50 sequences from here
            int imgHeight = 1200;
            int imgWidth = 1200;
            int seqCount = 0;
            if (!string.IsNullOrEmpty(fastaPath))
            {
                foreach (var line in File.ReadLines(fastaPath))
                {
                    if (line.Length > 0 && line[0] == '>')
                        seqCount++;
                }

                imgHeight += (seqCount - 50) * 15;
                imgWidth += (seqCount - 50) * 2;
                if (seqCount <= 50)
                {
                    imgHeight = 1200;
                    imgWidth = 1200;
                }
            }

            string outputImage = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + "_tree.pdf");

            string[] commands = new[]
            {
    $"file {file}",
    "open",
    "load",
    "y",
    "module enable Reroot tree",
    "module select Reroot tree",
    "option select Rooting mode",
    "option set Mid-point",
    "update all",
    "module enable Sort nodes",
    "module select Sort nodes",
    "option select Order",
    "option set Ascending",
    "update all",
    "module enable Rectangular",
    "module select Rectangular",
    "option select Width",
    $"option set {imgWidth}",
    "option select Height",
    $"option set {imgHeight}",
    "update all",
    "module enable Scale tree",
    "module select Scale tree",
    "option select Scaling factor",
    "option set 1",
    "update all",
    "module enable Branches",
    "module select Branches",
    "option select Root branch",
    "option set true",
    "module enable Labels",
    "module select Labels",
    "option select Show on",
    "option set Leaves",
    "node select root",
    $"attachment add {attachmentPath}",
    "attach",
    "y",
    "y",
    "module enable parse node states",
    "module select parse node states",
    "option select #1",
    "option set attach",
    "option select #9",
    "option set true",
    "update all",
    $"pdf {outputImage}",
    "exit"
};

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = treePath,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            psi.StandardOutputEncoding = System.Text.Encoding.UTF8;
            psi.StandardErrorEncoding = System.Text.Encoding.UTF8;

            Process proc = Process.Start(psi);
            if (proc != null)
            {
                using (StreamWriter sw = proc.StandardInput)
                {
                    foreach (var line in commands)
                    {
                        sw.WriteLine(line);
                        sw.Flush();
                    }
                }

                string stdErr = proc.StandardError.ReadToEnd();
                //ShowErrorForm(stdErr);
                proc.WaitForExit();
            }

            return outputImage;
        }

        // Handles "Dendrogram" menu click event
        private async void MnuTreeDendrogram_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Treefile files (*.treefile)|*.treefile|All files (*.*)|*.*",
                Title = "Select a treefile"
            };

            // Get treeview paths to create treefile
            using (ofd)
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string treeViewerPath = ResolveToolPath("TreeViewer", "TreeViewerCommandLine.exe");
                    string treeViewerStatus = "TREEVIEWER failed to run.";
                    string dendrogramPath = ofd.FileName;
                    // Ask user for attachment file
                    string attachmentPath = "";

                    using (OpenFileDialog dialog = new OpenFileDialog())
                    {
                        dialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                        dialog.Title = "Select a Text File";

                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            attachmentPath = dialog.FileName;
                            MessageBox.Show($"Attachment file at {attachmentPath}");
                        }
                        else
                        {
                            MessageBox.Show("No attachment file selected");
                        }
                    }

                    await LoadingForm.RunWithLoading(this, "Creating Dendrogram with TREEVIEWER...", async () =>
                    {
                        await Task.Run(() =>
                        {
                            dendrogramPath = RunDendrogramRedirected(dendrogramPath, treeViewerPath, attachmentPath);
                        });
                        treeViewerStatus = "Dendrogram created at: " + dendrogramPath;
                        if (treeViewerStatus == "Dendrogram created at: ")
                        {
                            treeViewerStatus = "TREEVIEWER ran but failed to produce dendrogram";
                        }
                    });
                    MessageBox.Show(treeViewerStatus);
                }
            }
        }

        /*
        private void MnuTreeDendrogram_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Treefile files (*.treefile)|*.treefile|All files (*.*)|*.*",
                Title = "Select a treefile"
            };

            using (ofd)
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string treeViewPath = ResolveToolPath("TreeViewer", "TreeViewerCommandLine.exe");
                    MessageBox.Show(treeViewPath);
                    try
                    {
                        // Ask user for attachment file
                        string attachmentPath = "";
                        using (OpenFileDialog dialog = new OpenFileDialog())
                        {
                            dialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                            dialog.Title = "Select a Text File";

                            if (dialog.ShowDialog() == DialogResult.OK)
                            {
                                attachmentPath = dialog.FileName;
                                MessageBox.Show($"Attachment file at {attachmentPath}");
                            }
                            else
                            {
                                MessageBox.Show("No attachment file selected");
                            }
                        }

                        //Run TreeViewer
                        string dendrogramFile = RunDendrogram(ofd.FileName, treeViewPath, attachmentPath);
                        if (dendrogramFile != null)
                        {
                            MessageBox.Show($"Dendrogram generated: {dendrogramFile}", "Success");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Unexpected error after dendrogram creation:\n{ex}");
                    }
                }
            }
        }
        */

        // --- Homology Section ---

        // Creates Homology table
        private async void MnuHomologyTable_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Fasta files (*.fasta)|*.fasta|All files (*.*)|*.*",
                Title = "Select a fasta file"
            };
            using (ofd)
            {
                // use fasta for homology process
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (ValidateFasta(ofd.FileName))
                    {
                        string homologyStatus = "Nucleotide Homology failed to run.";
                        string homologyPath = "";
                        await LoadingForm.RunWithLoading(this, "Creating Nucleotide Homology Table CSV...", async () =>
                        {
                            await Task.Run(() =>
                            {
                                homologyPath = RunHomology(ofd.FileName);
                            });
                            homologyStatus = "Table created at: " + homologyPath;
                            if (homologyStatus == "Table created at: ")
                            {
                                homologyStatus = "Nucleotide Homology ran but failed to produce new Table";
                            }
                        });
                        MessageBox.Show(homologyStatus);
                    }
                }
            }
        }

        // Handles alignment, nucleotide matrix, and csv formatting processes
        private string RunHomology(string fileName)
        {
            // parse fasta into array
            var fasta = File.ReadAllText(fileName);
            var lines = fasta
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.None)
                .Select(l => l.Trim())
                .Where(l => l != "")
                .ToList();

            var sequences = new List<(string Header, string Sequence)>();

            string currentHeader = null;
            var currentSequence = new StringBuilder();
            foreach (var line in lines)
            {
                if (line.StartsWith(">"))
                {
                    if (currentHeader != null)
                        sequences.Add((currentHeader, currentSequence.ToString()));

                    currentHeader = line.Substring(1);
                    currentSequence.Clear();
                }
                else
                {
                    currentSequence.Append(line);
                }
            }
            if (currentHeader != null)
                sequences.Add((currentHeader, currentSequence.ToString()));

            // Extract sequences
            string[] seqArray = sequences.Select(s => s.Sequence).ToArray();

            // Call native similarity matrix
            double[,] matrix = MatrixInterop.ComputeMatrix(seqArray);

            // Save results as CSV
            string outputFile = fileName.Replace(".fasta", "_nucleotide_homology.csv");
            using (var writer = new StreamWriter(outputFile))
            {
                writer.Write(",");
                writer.WriteLine(string.Join(",", sequences.Select(s => s.Header)));

                for (int i = 0; i < sequences.Count; i++)
                {
                    writer.Write(sequences[i].Header + ",");
                    for (int j = 0; j < sequences.Count; j++)
                    {
                        double value = matrix[i, j];
                        string output;

                        if (i == j)
                            output = "--";
                        else if (value == 100.00)
                            output = "100";
                        else
                        {
                            output = value.ToString("0.0");
                        }

                        writer.Write(output);
                        if (j < sequences.Count - 1)
                            writer.Write(",");
                    }
                    writer.Write(",");
                    writer.Write(sequences[i].Header);
                    writer.WriteLine();
                }
                writer.Write(",");
                writer.WriteLine(string.Join(",", sequences.Select(s => s.Header)));
            }

            return outputFile;
        }

        // Handles amino acid similarity table creation
        private async void createAminoAcidTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Fasta files (*.fasta)|*.fasta|All files (*.*)|*.*",
                Title = "Select a fasta file"
            };
            using (ofd)
            {
                // use fasta for homology process
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (ValidateFasta(ofd.FileName))
                    {
                        string aminoacidStatus = "Amino Acid Homology failed to run.";
                        string aminoacidPath = "";
                        await LoadingForm.RunWithLoading(this, "Creating Amino Acid Homology Table CSV...", async () =>
                        {
                            await Task.Run(() =>
                            {
                                aminoacidPath = RunAminoAcidHomology(ofd.FileName);
                            });
                            aminoacidStatus = "Table created at: " + aminoacidPath;
                            if (aminoacidStatus == "Table created at: ")
                            {
                                aminoacidStatus = "Amino Acid Homology ran but failed to produce new Table";
                            }
                        });
                        MessageBox.Show(aminoacidStatus);
                    }
                }
            }
        }

        // Handles amino acid translation, similarity table creation, and csv formatting processes
        private string RunAminoAcidHomology(string fileName)
        {
            if (ValidateFasta(fileName) == false)
            {
                return fileName + " is not a valid fasta file.";
            }

            // read fasta info
            var fasta = new List<(string Header, string Seq)>();
            string header = "";
            var sb = new StringBuilder();

            // format fasta
            foreach (var line in File.ReadLines(fileName))
            {
                if (line.StartsWith(">"))
                {
                    if (!string.IsNullOrEmpty(header))
                    {
                        fasta.Add((header, sb.ToString()));
                    }

                    header = line.Substring(1).Trim();
                    sb.Clear();
                }
                else
                {
                    sb.Append(line.Trim());
                }
            }
            if (string.IsNullOrEmpty(header))
            {
                fasta.Add((header, sb.ToString()));
                sb.Clear();
            }

            // Translate sequences to amino acids
            var aminoAcidSeq = new List<(string Header, string Sequence)>();
            foreach (var line in fasta)
            {
                aminoAcidSeq.Add((line.Header, GetAminoAcidSeq(line.Seq)));
            }

            // create amino acid file to align
            var fastaBuilder = new StringBuilder();

            foreach (var line in aminoAcidSeq)
            {
                fastaBuilder.AppendLine(">" + line.Header);
                fastaBuilder.AppendLine(line.Sequence);
            }

            // Location for aligned AAfasta to go
            var fastaDir = Path.GetDirectoryName(fileName);
            string fastaName = Path.GetFileNameWithoutExtension(fileName);
            string fullPath = Path.Combine(fastaDir, fastaName + "_amino_acid.fasta");
            File.WriteAllText(fullPath, fastaBuilder.ToString());

            // align with mafft
            string alnFile = RunMafft(fullPath, ResolveToolPath("mafft-win", "mafft.bat"));

            // format aligned fasta
            var alnFasta = new List<(string Header, string Seq)>();
            header = "";
            foreach (var line in File.ReadLines(alnFile))
            {
                if (line.StartsWith(">"))
                {
                    if (!string.IsNullOrEmpty(header))
                    {
                        alnFasta.Add((header, sb.ToString()));
                    }

                    header = line.Substring(1).Trim();
                    sb.Clear();
                }
                else
                {
                    sb.Append(line.Trim());
                }
            }
            if (string.IsNullOrEmpty(header))
            {
                alnFasta.Add((header, sb.ToString()));
            }

            // Create similarity matrix
            string[] seqArray = alnFasta.Select(s => s.Seq).ToArray();

            double[,] matrix = MatrixInterop.ComputeMatrix(seqArray);

            // Save results as CSV
            string outputFile = fileName.Replace(".fasta", "_amino_acid_homology.csv");
            using (var writer = new StreamWriter(outputFile))
            {
                writer.Write(",");
                writer.WriteLine(string.Join(",", alnFasta.Select(s => s.Header)));

                for (int i = 0; i < alnFasta.Count; i++)
                {
                    writer.Write(alnFasta[i].Header + ",");
                    for (int j = 0; j < alnFasta.Count; j++)
                    {
                        double value = matrix[i, j];
                        string output;

                        if (i == j)
                            output = "--";
                        else if (value == 100.00)
                            output = "100";
                        else
                        {
                            output = value.ToString("0.0");
                        }

                        writer.Write(output);
                        if (j < alnFasta.Count - 1)
                            writer.Write(",");
                    }
                    writer.Write(",");
                    writer.Write(alnFasta[i].Header);
                    writer.WriteLine();
                }
                writer.Write(",");
                writer.WriteLine(string.Join(",", alnFasta.Select(s => s.Header)));
            }
            // clean up temp files
            if (deleteIntermediateFile)
            {
                File.Delete(fullPath);
                File.Delete(alnFile);
            }

            return outputFile;
        }

        // Run All button method
        private async void btnAllInOne_Click(object sender, EventArgs e)
        {
            // paths for pipelining
            string alnPath = "";
            string treePath = "";
            string dendrogramPath = "";
            string attachPath = "";
            string homologyPath = "";
            string aminoacidPath = "";
            string mafftStatus = "MAFFT failed to run.";

            // Check whether fasta is currently in use; if not, use file dialog
            if (string.IsNullOrEmpty(fastaPath))
            {
                DialogResult result = MessageBox.Show(
                    "New sequences were not added to the fasta. Continue with processes?",
                    "Confirmation",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                    );
                if (result == DialogResult.No)
                {
                    return;
                }
                else
                {
                    OpenFileDialog ofd = new OpenFileDialog
                    {
                        Filter = "Fasta files (*.fasta)|*.fasta|All files (*.*)|*.*",
                        Title = "Select a fasta file"
                    };
                    using (ofd)
                    {
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            if (ValidateFasta(ofd.FileName))
                            {
                                fastaPath = ofd.FileName;
                            }
                        }
                    }
                }
            }

            // Checks that a fasta was actually selected; ensures cancellations don't create errors
            if (!string.IsNullOrEmpty(fastaPath))
            {

                //run mafft
                string mafftPath = ResolveToolPath("mafft-win", "mafft.bat");
                await LoadingForm.RunWithLoading(this, "Aligning sequences with MAFFT...", async () =>
                {
                    await Task.Run(() =>
                    {
                        alnPath = RunMafft(fastaPath, mafftPath);
                        Invoke(new Action(() =>
                        {
                            txtOldFasta.Text = File.ReadAllText(alnPath).Replace("\n", "\r\n").ToUpper();
                        }));
                    });
                    mafftStatus = "Aligned file created at: " + alnPath;
                    if (mafftStatus == "Aligned file created at: ")
                    {
                        mafftStatus = "MAFFT ran but failed to produce aligned fasta";
                    }
                });

                //run iqtree
                string safeIqtreePath = ResolveToolPath("iqtree", "safe_iqtree_powershell.ps1");
                string iqtreeStatus = "IQTREE failed to run.";
                await LoadingForm.RunWithLoading(this, "Creating Treefile with IQTREE...", async () =>
                {
                    await Task.Run(() =>
                    {
                        treePath = RunSafeIqtree(alnPath, safeIqtreePath);
                    });
                    iqtreeStatus = "Treefile created at: " + treePath;
                    if (iqtreeStatus == "Treefile created at: ")
                    {
                        iqtreeStatus = "IQTREE ran but failed to produce new treefile";
                    }
                });

                //run treeview
                string treeViewerPath = ResolveToolPath("TreeViewer", "TreeViewerCommandLine.exe");
                string treeViewerStatus = "TREEVIEWER failed to run.";
                await LoadingForm.RunWithLoading(this, "Creating Dendrogram with TREEVIEWER...", async () =>
                {
                    await Task.Run(() =>
                    {
                        dendrogramPath = RunDendrogramRedirected(treePath, treeViewerPath, attachPath);
                    });
                    treeViewerStatus = "Dendrogram created at: " + dendrogramPath;
                    if (treeViewerStatus == "Dendrogram created at: ")
                    {
                        treeViewerStatus = "TREEVIEWER ran but failed to produce dendrogram";
                    }
                });

                //run nucleotide homology
                string homologyStatus = "Nucleotide Homology failed to run.";
                await LoadingForm.RunWithLoading(this, "Creating Nucleotide Homology Table CSV...", async () =>
                {
                    await Task.Run(() =>
                    {
                        homologyPath = RunHomology(fastaPath);
                    });
                    homologyStatus = "Table created at: " + homologyPath;
                    if (homologyStatus == "Table created at: ")
                    {
                        homologyStatus = "Nucleotide Homology ran but failed to produce new Table";
                    }
                });

                //run amino acid homology
                string aminoacidStatus = "Amino Acid Homology failed to run.";
                await LoadingForm.RunWithLoading(this, "Creating Amino Acid Homology Table CSV...", async () =>
                {
                    await Task.Run(() =>
                    {
                        aminoacidPath = RunAminoAcidHomology(fastaPath);
                    });
                    aminoacidStatus = " Table created at: " + aminoacidPath;
                    if (aminoacidStatus == "Table created at: ")
                    {
                        aminoacidStatus = "Amino Acid Homology ran but failed to produce new Table";
                    }
                });

                //create homology workbook
                string workbookStatus = "Failed to create Homology Workbook.";
                string workbookPath = Path.Combine(Path.GetDirectoryName(fastaPath), Path.GetFileNameWithoutExtension(fastaPath) + "_homology.xlsx"); ;
                await LoadingForm.RunWithLoading(this, "Creating Homology Workbook XLSX...", async () =>
                {
                    await Task.Run(() =>
                    {
                        workbookPath = CSVtoXLSX(homologyPath, aminoacidPath, workbookPath);
                    });
                    workbookStatus = "Workbook created at: " + workbookPath;
                    if (workbookStatus == "Workbook created at: ")
                    {
                        workbookStatus = "Homology Workbook attempted but failed to produce new Workbook";
                    }
                });

                // clear temporary files
                if (new FileInfo(workbookPath).Length > 100)
                {
                    File.Delete(homologyPath);
                    File.Delete(aminoacidPath);
                }
                if (new FileInfo(dendrogramPath).Length > 100 && deleteIntermediateFile)
                {
                    File.Delete(alnPath);
                    File.Delete(treePath);
                }

                fastaPath = "";
                txtOldFasta.Text = "";
                MessageBox.Show("All done" + "\n" + mafftStatus + "\n" + iqtreeStatus + "\n" + treeViewerStatus + "\n" + workbookStatus);
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //--- helper functions ---

        // Translates nucleotide sequence to amino acid sequence
        private static string GetAminoAcidSeq(string dna)
        {
            // formatting
            dna = dna.ToUpper().Replace("\n", "").Replace("\r", "").Replace(" ", "");

            // nucleotide to codon
            var codonTable = new Dictionary<string, char>
            {
                {"TTT",'F'}, {"TTC",'F'}, {"TTA",'L'}, {"TTG",'L'},
                {"TCT",'S'}, {"TCC",'S'}, {"TCA",'S'}, {"TCG",'S'},
                {"TAT",'Y'}, {"TAC",'Y'}, {"TAA",'X'}, {"TAG",'X'},
                {"TGT",'C'}, {"TGC",'C'}, {"TGA",'X'}, {"TGG",'W'},

                {"CTT",'L'}, {"CTC",'L'}, {"CTA",'L'}, {"CTG",'L'},
                {"CCT",'P'}, {"CCC",'P'}, {"CCA",'P'}, {"CCG",'P'},
                {"CAT",'H'}, {"CAC",'H'}, {"CAA",'Q'}, {"CAG",'Q'},
                {"CGT",'R'}, {"CGC",'R'}, {"CGA",'R'}, {"CGG",'R'},

                {"ATT",'I'}, {"ATC",'I'}, {"ATA",'I'}, {"ATG",'M'},
                {"ACT",'T'}, {"ACC",'T'}, {"ACA",'T'}, {"ACG",'T'},
                {"AAT",'N'}, {"AAC",'N'}, {"AAA",'K'}, {"AAG",'K'},
                {"AGT",'S'}, {"AGC",'S'}, {"AGA",'R'}, {"AGG",'R'},

                {"GTT",'V'}, {"GTC",'V'}, {"GTA",'V'}, {"GTG",'V'},
                {"GCT",'A'}, {"GCC",'A'}, {"GCA",'A'}, {"GCG",'A'},
                {"GAT",'D'}, {"GAC",'D'}, {"GAA",'E'}, {"GAG",'E'},
                {"GGT",'G'}, {"GGC",'G'}, {"GGA",'G'}, {"GGG",'G'},
            };

            var sb = new StringBuilder();

            // translate 3 bases to codon
            for (int pos = 0; pos + 2 < dna.Length; pos += 3)
            {
                string codon = dna.Substring(pos, 3);
                if (codonTable.TryGetValue(codon, out char aa))
                {
                    sb.Append(aa);

                }
                else
                {
                    sb.Append('?'); // invalid codon/gap
                }
            }

            return sb.ToString();
        }


        // Outputs scrollable error windows
        private void ShowErrorForm(string text)
        {
            Form errorForm = new Form
            {
                Text = "Output",
                Width = 800,
                Height = 600
            };

            System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox
            {
                Multiline = true,
                ScrollBars = ScrollBars.Both,
                WordWrap = false,
                Dock = DockStyle.Fill,
                Text = text
            };

            errorForm.Controls.Add(textBox);
            errorForm.ShowDialog();
        }

        // Finds the tool path
        private string ResolveToolPath(string toolFolder, string executable)
        {
            // Start from where the .exe is running
            string exeDir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar);

            // Detect if we're in bin\Debug or bin\Release
            string root;
            if (exeDir.EndsWith(@"\bin\Debug", StringComparison.OrdinalIgnoreCase) ||
                exeDir.EndsWith(@"\bin\Release", StringComparison.OrdinalIgnoreCase) ||
                    exeDir.EndsWith(@"\obj\Debug", StringComparison.OrdinalIgnoreCase) ||
                        exeDir.EndsWith(@"\obj\Release", StringComparison.OrdinalIgnoreCase))
            {
                // Go up two level to reach base project directory
                root = Directory.GetParent(Directory.GetParent(exeDir).FullName).FullName;

            }
            else
            {
                // Installed elsewhere → assume tool folder is in same folder as .exe
                root = exeDir;
            }

            // Now build the final path: binRoot → toolFolder → executable
            string fullPath = Path.GetFullPath(Path.Combine(root, toolFolder, executable));

            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"Tool not found: {fullPath}", fullPath);

            return fullPath;
        }

        // Checks input file validity
        private bool ValidateFasta(string filePath)
        {
            // checks if is fasta
            string ext = Path.GetExtension(filePath).ToLower();
            if (ext != ".fasta" && ext != ".fa" && ext != ".fas")
            {
                return false;
            }

            // valid fasta alphabet
            var validChars = @"ACGTUI-RYKMSWBDHVN".ToCharArray();
            string[] lines = File.ReadAllLines(filePath);
            bool expectingHeader = true;

            // checks each line in fasta
            foreach (var rawLine in lines)
            {
                string line = rawLine.Trim();
                if (line == "") continue;

                // validates header
                if (expectingHeader)
                {
                    if (!line.StartsWith(">"))
                    {
                        MessageBox.Show("Incorrect header formatting");
                        return false;
                    }
                    expectingHeader = false;
                }
                // validates probably not header
                else
                {
                    // is header
                    if (line.StartsWith(">"))
                    {
                        expectingHeader = false;
                        continue;
                    }
                    // contains invalid chars
                    else if (line.Any(c => !validChars.Contains(char.ToUpper(c))))
                    {
                        char bad = line.First(c => !validChars.Contains(char.ToUpper(c)));
                        ShowErrorForm("Sequences aren't valid because of " + bad);
                        return false;
                    }
                }
            }
            fastaPath = filePath;
            return true;
        }

        // Validates and translates seq file
        private bool ParseSeq(string filePath)
        {
            // checks if is seq file
            string ext = Path.GetExtension(filePath).ToLower();
            if (ext != ".seq")
            {
                return false;
            }

            // changes seq file info to fasta compatible format
            string[] lines = File.ReadAllLines(filePath);
            string seqName = lines[0].Split('"')[1];
            string sequence = lines[4];
            seqText = ">" + seqName + "\r\n" + sequence;
            return true;
        }

        // create excel workbook
        private string CSVtoXLSX(string nCSV, string aaCSV, string outputXlsx)
        {
            using (var wb = new XLWorkbook())
            {
                // Sheet 1
                var ws1 = wb.Worksheets.Add("Nucleotide");
                var lines1 = File.ReadAllLines(nCSV);
                for (int r = 0; r < lines1.Length; r++)
                {
                    var cells = lines1[r].Split(',');
                    for (int c = 0; c < cells.Length; c++)
                        ws1.Cell(r + 1, c + 1).Value = cells[c];
                }

                // Sheet 2
                var ws2 = wb.Worksheets.Add("Amino Acid");
                var lines2 = File.ReadAllLines(aaCSV);
                for (int r = 0; r < lines2.Length; r++)
                {
                    var cells = lines2[r].Split(',');
                    for (int c = 0; c < cells.Length; c++)
                        ws2.Cell(r + 1, c + 1).Value = cells[c];
                }

                wb.SaveAs(outputXlsx);
            }
            return outputXlsx;
        }

        // Translates C methods
        internal static class MatrixInterop
        {

            static MatrixInterop()
            {
                NativeLibrary.SetDllImportResolver(
                    typeof(MatrixInterop).Assembly,
                    (name, assembly, path) =>
                    {
                        if (name == "matrix_builder.dll")
                        {
                            string dllPath = Path.Combine(
                                AppContext.BaseDirectory,
                                "homology",
                                "matrix_builder.dll"
                            );
                            return NativeLibrary.Load(dllPath);
                        }
                        return IntPtr.Zero;
                    });
            }

            [DllImport("matrix_builder.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr buildSimilarityMatrix(
                IntPtr sequences,
                int count,
                int match,
                int mismatch,
                int gap
            );

            // Create homology matrix
            public static double[,] ComputeMatrix(string[] sequences, int match = 1, int mismatch = -1, int gap = -12)
            {
                int count = sequences.Length;
                int size = count * count;

                // Convert managed strings to unmanaged array
                IntPtr[] seqPtrs = new IntPtr[count];
                for (int i = 0; i < count; i++)
                    seqPtrs[i] = Marshal.StringToHGlobalAnsi(sequences[i]);

                // Allocate unmanaged array of pointers
                IntPtr seqArrayPtr = Marshal.AllocHGlobal(IntPtr.Size * count);
                Marshal.Copy(seqPtrs, 0, seqArrayPtr, count);

                // Call the C function
                IntPtr matrixPtr = buildSimilarityMatrix(seqArrayPtr, count, match, mismatch, gap);

                if (matrixPtr == IntPtr.Zero)
                {
                    // Provide clearer diagnostic than an opaque DllNotFound/AccessViolation
                    Marshal.FreeHGlobal(seqArrayPtr);
                    foreach (var ptr in seqPtrs)
                        Marshal.FreeHGlobal(ptr);

                    throw new DllNotFoundException("Native function buildSimilarityMatrix returned NULL. Confirm matrix_builder.dll exists in the homology folder and that its dependencies and architecture match the process.");
                }

                // Copy flat result back into managed memory
                double[] flat = new double[size];
                Marshal.Copy(matrixPtr, flat, 0, size);

                // Free native allocations
                Marshal.FreeHGlobal(seqArrayPtr);
                foreach (var ptr in seqPtrs)
                    Marshal.FreeHGlobal(ptr);

                // Convert to 2D array
                double[,] matrix = new double[count, count];
                for (int i = 0; i < count; i++)
                    for (int j = 0; j < count; j++)
                        matrix[i, j] = flat[i * count + j];

                return matrix;
            }
        }

        private void instructionsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Main Function:\n" +
                "   Append -> Add new sequences onto the selected fasta. \r\n" +
                "   Save -> Save the current version of the selected fasta, putting the previous version into an archive folder in the same directory.\r\n" +
                "   Run All -> Drag a fasta and click \"Run All\" button to create both a dendrogram image and a homology table.\n\n" +
                "File:\n" +
                "   Open -> Opens fasta for modification or examination\n" +
                "   Save Fasta -> Save the current version of the selected fasta, putting the previous version into an archive folder in the same directory.\r\n" +
                "Dendrogram:\n" +
                "   Align Fasta -> Uses mafft to align fasta for treefile creation\n" +
                "   Create Treefile -> Uses IQ-TREE to create treefile from aligned fasta\n" +
                "   Create Dendrogram -> Uses TreeViewer to create dendrogram image from treefile\n\n" +
                "Homology:\n" +
                "   Create Homology Table -> Creates a homology table CSV from\n\n",
                "Help",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void intermediateFileToggleToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (deleteIntermediateFile)
            {
                deleteIntermediateFile = false;
                MessageBox.Show("Intermediate files will be kept.");
            }
            else
            {
                deleteIntermediateFile = true;
                MessageBox.Show("Intermediate files will be deleted.");
            }
        }
    }
}
