using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Monocle;

namespace MonocleUI
{
    public partial class MonocleUI : Form
    {
        FileProcessor Processor = new FileProcessor();

        public MonocleUI()
        {
            InitializeComponent();
            Initiliaze_OutputFormat_CLB();
            Size = new Size(783, 563);
        }

        private void Initiliaze_OutputFormat_CLB()
        {
            foreach(string type in Enum.GetNames(typeof(InputFileType)))
            {
                file_output_format_CLB.Items.Add(type);
            }
            file_output_format_CLB.SetItemChecked(0, true);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if(input_file_dialog.ShowDialog() == DialogResult.OK)
            {
                foreach(string filePath in input_file_dialog.FileNames)
                {
                    if (Processor.files.Add(filePath))
                    {
                        export_folder_maskedTB.Text = Path.GetFullPath(filePath).Replace(Path.GetFileName(filePath), "");
                        Files.ExportPath = Path.GetFullPath(filePath).Replace(Path.GetFileName(filePath), "");
                        input_files_dgv.Rows.Add(filePath);
                    }
                }
            }
        }
        private void Input_files_dgv_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void Input_files_dgv_DragDrop(object sender, DragEventArgs e)
        {
            string[] fileArray;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                fileArray = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string filePath in fileArray)
                {
                    if (Processor.files.Add(filePath))
                    {
                        export_folder_maskedTB.Text = Path.GetFullPath(filePath).Replace(Path.GetFileName(filePath), "");
                        Files.ExportPath = Path.GetFullPath(filePath).Replace(Path.GetFileName(filePath), "");
                        input_files_dgv.Rows.Add(filePath);
                    }
                }
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (export_folder_dialog.ShowDialog() == DialogResult.OK)
            {
                export_folder_maskedTB.Text = export_folder_dialog.SelectedPath;
            }
        }

        private void remove_dgv_row_button_Click(object sender, EventArgs e)
        {
            if(input_files_dgv.SelectedRows.Count > 0)
            {
                foreach(DataGridViewRow row in input_files_dgv.Rows)
                {
                    if (row.Selected)
                    {
                        Processor.files.Remove(row.Cells[0].Value.ToString());
                        input_files_dgv.Rows.Remove(row);
                    }
                }
                
            }
        }

        private void Log_toggle_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (log_toggle_checkbox.Checked)
            {
                Size = new Size(783, 763); 
            }
            else
            {
                Size = new Size(783, 563);
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void UpdateLog(string message)
        {
            Invoke(new Action(
            () =>
            {
                monocle_log.AppendText(string.Format("[{0}]\t{1}\n", DateTime.Now.ToLongTimeString(), message));
                monocle_log.SelectionStart = monocle_log.Text.Length;
                monocle_log.ScrollToCaret();
            }));
        }

        private void Start_monocle_button_Click(object sender, EventArgs e)
        {
            Processor.Run();
        }

        private void GCCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GC.Collect();
        }
    }
}
