using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Windows.Forms.VisualStyles;
using System.Windows.Navigation;


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
            this.AllowDrop = true;

            this.DragEnter += FormMain_DragEnter;
            this.DragDrop += FormMain_DragDrop;
        }

        // --- Drag and Drop
        // save fasta location for pipeline
        string fastaPath = "";

        private void FormMain_DragEnter(object sender, DragEventArgs e)
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

        private void FormMain_DragDrop(object sender, DragEventArgs e)
        {
            string[] paths = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (paths != null && paths.Length > 0)
            {
                // For example, load the first file
                string filePath = paths[0];
                fastaPath = filePath;
                txtOldFasta.Text = File.ReadAllText(filePath);

                MessageBox.Show($"Selected file: {filePath}");
            }
        }

        // --- Fasta editing ---

        // Append typed contents to the fasta contents and save new fasta
        private void btnAppend_Click(object sender, EventArgs e)
        {
            txtOldFasta.Text += txtNewFasta.Text;

            MessageBox.Show("Sequences appended successfully!",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

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
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "FASTA files (*.fasta)|*.fasta|All files (*.*)|*.*",
                Title = "Save FASTA file"
            };

            using (sfd)
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    // Build FASTA contents
                    StringBuilder fastaBuilder = new StringBuilder();

                    // Add text in first textbox to new fasta
                    if (!string.IsNullOrWhiteSpace(txtOldFasta.Text))
                    {
                        fastaBuilder.AppendLine(txtOldFasta.Text);
                    }

                    // Save to file
                    System.IO.File.WriteAllText(sfd.FileName, fastaBuilder.ToString());


                    MessageBox.Show("Fasta saved successfully!",
                                "Success",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                }
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void OpenFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        // Check for required tools on load
        private void Form1_Load(object sender, EventArgs e)
        {
            string appBase = AppDomain.CurrentDomain.BaseDirectory;
            string dir = Path.GetFullPath(Path.Combine(appBase, "..", ".."));

            string iqtreePath = Path.Combine(dir, "iqtree", "iqtree3.exe");
            string mafftPath = Path.Combine(dir, "mafft-win", "mafft.bat");
            string psScript = Path.Combine(dir, "iqtree", "safe_iqtree_powershell.ps1");

            CheckTool(iqtreePath, "IQ-TREE");
            CheckTool(mafftPath, "MAFFT");
            CheckTool(psScript, "PowerShell wrapper script");
        }

        // Helper function that returns whether tool is found or not
        private void CheckTool(string path, string toolName)
        {
            if (!File.Exists(path))
            {
                MessageBox.Show(
                    $"{toolName} was not found at:\n{path}\n\nPlease verify installation or update the path.",
                    "Missing Tool",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            else
            {
                Console.WriteLine($"{toolName} found: {path}");
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
            txtOldFasta.Height = 170 + boxHeight;
            txtNewFasta.Height = 170 + boxHeight;
            cbxNewSequences.Height = 365 + boxHeight * 2;
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
                }
            }
        }

        // Handles the "Save" menu click event
        private void MnuFileSaveFasta_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "FASTA files (*.fasta)|*.fasta|All files (*.*)|*.*",
                Title = "Save FASTA file"
            };
            using (sfd)
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    // Build FASTA contents
                    StringBuilder fastaBuilder = new StringBuilder();

                    // Add text in first textbox to new fasta
                    if (!string.IsNullOrWhiteSpace(txtOldFasta.Text))
                    {
                        fastaBuilder.AppendLine(txtOldFasta.Text);
                    }

                    // Save to file
                    System.IO.File.WriteAllText(sfd.FileName, fastaBuilder.ToString());


                    MessageBox.Show("Fasta saved successfully!",
                                "Success",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                }
            }
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
            string command = $"/C call \"{mafft}\" --localpair --out \"{alnFile}\" \"{file}\"";

            // mafft process settings
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = command,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            // begin mafft process
            using (Process proc = new Process { StartInfo = psi })
            {
                proc.Start();
                string stdout = proc.StandardOutput.ReadToEnd();
                string stderr = proc.StandardError.ReadToEnd();

                proc.WaitForExit();

                if (proc.ExitCode != 0)
                {
                    ShowErrorForm("MAFFT failed: \n\nSTDOUT:\n" + stdout + "\n\nSTDERR:\n" + stderr);
                }
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
                return foundFiles[0];
            }
            else
            {
                ShowErrorForm("IQ-TREE finished but no treefile was created.\n\nSTDOUT:\n" + stdout + "\n\nSTDERR:\n" + stderr);
                return null;
            }
        }

        // handles "Tree Treefile" menu click event
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

        // --- Dendrogram Section ---

        // handles treeview commands and creates the dendrogram
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

        private string RunDendrogramRedirected(string file, string treePath, string attachmentPath)
        {
            if (!File.Exists(treePath))
            {
                MessageBox.Show($"TreeViewer not found at {treePath}");
                return null;
            }

            string outputImage = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + "_tree.png");

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

        // handles "Dendrogram" menu click event
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
                    /*
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
                    */
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


        /*private void MnuTreeDendrogram_Click(object sender, EventArgs e)
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
                // Get iqtree paths to create treefile
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string homologyStatus = "Homology failed to run.";
                    string homologyPath = "";
                    await LoadingForm.RunWithLoading(this, "Creating Homology Table CSV...", async () =>
                    {
                        await Task.Run(() =>
                        {
                            homologyPath = RunHomology(ofd.FileName);
                        });
                        homologyStatus = "Table created at: " + homologyPath;
                        if (homologyStatus == "Table created at: ")
                        {
                            homologyStatus = "Homology ran but failed to produce new Table";
                        }
                    });
                    MessageBox.Show(homologyStatus);
                }
            }
        }

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
            string outputFile = fileName.Replace(".fasta", "_homology.csv");
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
                            output = "100%";
                        else
                            output = value.ToString("F2") + "%";

                        writer.Write(output);
                        if (j < sequences.Count - 1)
                            writer.Write(",");
                    }
                    writer.WriteLine();
                }
            }

            return outputFile;
        }

        private async void btnAllInOne_Click(object sender, EventArgs e)
        {
            // paths for pipelining
            string alnPath = "";
            string treePath = "";
            string dendrogramPath = "";
            string attachPath = "";
            string homologyPath = "";
            string mafftStatus = "MAFFT failed to run.";

            // Create process
            // Check whether fasta is currently in use; if not, use file dialog
            if (string.IsNullOrEmpty(fastaPath))
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
                        fastaPath = ofd.FileName;
                    }
                }
            }
            // fasta is already selected and in textbox: use that fasta for process
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

                //run homology
                string homologyStatus = "Homology failed to run.";
                await LoadingForm.RunWithLoading(this, "Creating Homology Table CSV...", async () =>
                {
                    await Task.Run(() =>
                    {
                        homologyPath = RunHomology(fastaPath);
                    });
                    homologyStatus = "Table created at: " + homologyPath;
                    if (homologyStatus == "Table created at: ")
                    {
                        homologyStatus = "Homology ran but failed to produce new Table";
                    }
                });
                fastaPath = "";
                txtOldFasta.Text = "";
                MessageBox.Show("All done" + "\n" + mafftStatus + "\n" + iqtreeStatus + "\n" + treeViewerStatus + "\n" + homologyStatus);
            }
        }


        // Instructions
        private void MnuHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Quick help:\n\n" +
                "Drag a fasta and click Magic button to create both a dendrogram image and a homology table.\n\n" +
                "File: \n\n" +
                "   Open -> Opens fasta for modification or examination\n\n" +
                "Dendrogram: \n\n" +
                "   Align Fasta -> Uses mafft to align fasta for treefile creation \n\n" +
                "   Create Treefile -> Uses IQ-TREE to create treefile from aligned fasta\n\n" +
                "   Create Dendrogram -> Uses TreeViewer to create dendrogram image from treefile\n\n" +
                "Homology: \n\n" +
                "   Create Homology Table -> Creates a homology table CSV from \n\n",
                "Help",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //--- helper functions ---

        // outputs scrollable error windows
        private void ShowErrorForm(string text)
        {
            Form errorForm = new Form
            {
                Text = "Output",
                Width = 800,
                Height = 600
            };

            TextBox textBox = new TextBox
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

        // finds the tool path
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
                // Installed elsewhere → assume tools are in same folder as .exe
                root = exeDir;
            }

            // Now build the final path: binRoot → toolFolder → executable
            string fullPath = Path.GetFullPath(Path.Combine(root, toolFolder, executable));

            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"Tool not found: {fullPath}", fullPath);

            return fullPath;
        }

        internal static class MatrixInterop
        {
            [DllImport("kernel32", SetLastError = true)]
            private static extern bool SetDllDirectory(string lpPathName);

            // Ensure the homology folder is added to the native DLL search path
            static MatrixInterop()
            {
                try
                {
                    string exeDir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar);
                    string homologyDir = Path.Combine(exeDir, "Homology");
                    string root = Directory.GetParent(Directory.GetParent(exeDir).FullName).FullName;
                    homologyDir = Path.GetFullPath(Path.Combine(root, "Homology"));

                    if (Directory.Exists(homologyDir))
                    {
                        SetDllDirectory(homologyDir);
                        Debug.WriteLine($"MatrixInterop: added homology folder to DLL search path: {homologyDir}");
                    }
                    else
                    {
                        Debug.WriteLine($"MatrixInterop: homology folder not found at {homologyDir}");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("MatrixInterop static constructor failed: " + ex);
                }
            }

            [DllImport("matrix_builder.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr buildSimilarityMatrix(
                IntPtr sequences,
                int count,
                int match,
                int mismatch,
                int gap
            );

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
    }
}
