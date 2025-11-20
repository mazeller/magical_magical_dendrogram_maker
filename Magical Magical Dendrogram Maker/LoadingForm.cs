using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Magical_Magical_Dendrogram_Maker
{
    public partial class LoadingForm : Form
    {
        public LoadingForm(string message = "Processing, please wait...")
        {
            // Form appearance
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            Width = 320;
            Height = 130;
            BackColor = Color.White;
            TopMost = true;
            ShowInTaskbar = false;

            // Panel for better spacing
            var panel = new Panel()
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
            };
            Controls.Add(panel);

            // Message label
            var lbl = new Label()
            {
                Text = message,
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                AutoSize = false,
                Height = 40
            };
            panel.Controls.Add(lbl);

            // Progress Bar
            var progress = new ProgressBar()
            {
                Style = ProgressBarStyle.Marquee,
                Dock = DockStyle.Bottom,
                Height = 25,
                MarqueeAnimationSpeed = 30
            };
            panel.Controls.Add(progress);
        }

        public static async Task RunWithLoading(Form parent, string message, Func<Task> action)
        {
            using (LoadingForm loading = new LoadingForm(message))
            {
                loading.Show(parent);
                loading.Refresh();

                try
                {
                    await action();
                }
                finally
                {
                    loading.Close();
                }
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadingForm));
            this.SuspendLayout();
            // 
            // LoadingForm
            // 
            resources.ApplyResources(this, "$this");
            this.Name = "LoadingForm";
            this.ResumeLayout(false);

        }
    }
}
