using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Injector
{
    public partial class AppForm : Form
    {
        private string selectedDllPath;
        private Random random = new Random();

        public AppForm()
        {
            InitializeComponent();
        }

        private void selectButton1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                fileDialog.InitialDirectory = Directory.GetCurrentDirectory();
                fileDialog.Filter = "dll file (*.dll)|*.dll|all files (*.*)|*.*";
                fileDialog.FilterIndex = 1;
                fileDialog.RestoreDirectory = true;
                fileDialog.Title = "select dll";
                fileDialog.CheckFileExists = true;
                fileDialog.CheckPathExists = true;

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedDllPath = fileDialog.FileName;

                    if (File.Exists(selectedDllPath) && new FileInfo(selectedDllPath).Length > 0)
                    {
                        status1.Text = $"dll chosen: {Path.GetFileName(selectedDllPath)}";
                        status1.ForeColor = Color.Lime;
                        injectButton1.Enabled = true;

                        // Beep de confirmation
                        Console.Beep(1000, 300);
                    }
                    else
                    {
                        status1.Text = "invalid file";
                        status1.ForeColor = Color.Red;
                        selectedDllPath = string.Empty;
                        injectButton1.Enabled = false;

                        Console.Beep(400, 500);
                    }
                }
            }
        }

        private async Task AnimateSkidProgress()
        {
            progressBar1.Value = 0;
            string[] messages = {
                "step1",
                "step2"
            };

            for (int i = 0; i <= 100; i += 2)
            {
                progressBar1.Value = i;
                if (i % 15 == 0 && i / 15 < messages.Length)
                {
                    status1.Text = $"[LOADING] {messages[i / 15]}";
                }
                await Task.Delay(50);
            }
        }

        private async void injectButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedDllPath))
            {
                MessageBox.Show("no dll selected", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            progressBar1.Visible = true;
            try
            {
                await AnimateSkidProgress();

                bool success = bypass.Run(selectedDllPath);

                if (success)
                {
                    MessageBox.Show("injected",
                                  "",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("failed",
                                  "",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}",

                              "",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
            finally
            {
                progressBar1.Visible = false;
            }
        }
    }
}
