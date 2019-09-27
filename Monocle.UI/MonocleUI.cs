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
            foreach(string type in Enum.GetNames(typeof(OutputFileType)))
            {
                file_output_format_CLB.Items.Add(type);
            }
            file_output_format_CLB.SetItemChecked(0, true);
        }

        private void add_file_button_Click(object sender, EventArgs e)
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

        private void select_output_directory_button_Click(object sender, EventArgs e)
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
            if (monocle_log.IsHandleCreated)
            {
                Invoke(new Action(
                () =>
                {
                    monocle_log.AppendText(string.Format("[{0}]\t{1}\n", DateTime.Now.ToLongTimeString(), message));
                    monocle_log.SelectionStart = monocle_log.Text.Length;
                    monocle_log.ScrollToCaret();
                }));
            }
        }

        public void UpdateProgress(int progress)
        {
            Invoke(new Action(
            () =>
            {
                progressBar1.Value = progress;
            }));
        }

        private void Start_monocle_button_Click(object sender, EventArgs e)
        {
            EnableRunUI(false);
            Processor.FileTracker += FileListener;
            Processor.Run();
        }

        private void GCCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //GC.Collect();
        }

        private void NumberOfScansToAverageNUD_ValueChanged(object sender, EventArgs e)
        {
            if(numberOfScansToAverageNUD.Value >= 1 && numberOfScansToAverageNUD.Value <= 20)
            {
                FileProcessor.monocleOptions.Number_Of_Scans_To_Average = (int)numberOfScansToAverageNUD.Value;
            }
        }

        private void EnableRunUI(bool enabled)
        {
            Invoke(new Action(
            () =>
            {
                start_monocle_button.Enabled = enabled;
                input_files_dgv.Enabled = enabled;
                file_output_format_CLB.Enabled = enabled;
                lowChargeSelectionNUD.Enabled = enabled;
                highChargeSelectionNUD.Enabled = enabled;
                add_file_button.Enabled = enabled;
                remove_dgv_row_button.Enabled = enabled;
                toggleChargeDetectionCB.Enabled = enabled;
                numberOfScansToAverageNUD.Enabled = enabled;
            }));
        }

        private void FileListener(object sender, FileEventArgs e)
        {
            if (e.FilePath != "" && e.Finished)
            {
                UpdateLog("File Finished: " + e.FilePath);
            }
            else if (e.FilePath != "" && e.Written)
            {
                UpdateLog("Writing Complete: " + e.FilePath);
            }
            else if (e.FilePath != "" && e.Processed)
            {
                UpdateLog("Processing Complete: " + e.FilePath);
            }
            else if (e.FilePath != "" && e.Read)
            {
                UpdateLog("File Read Complete: " + e.FilePath);
            }
            UpdateProgress((int)e.CurrentProgress);
            if (e.FinishedAllFiles)
            {
                EnableRunUI(true);
            }
        }

        private void File_output_format_CLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int itemInd = 0; itemInd < file_output_format_CLB.Items.Count; itemInd++)
            {
                if (file_output_format_CLB.GetItemChecked(itemInd) && itemInd != file_output_format_CLB.SelectedIndex)
                {
                    file_output_format_CLB.SetItemCheckState(itemInd, CheckState.Unchecked);
                }
                else if (itemInd == file_output_format_CLB.SelectedIndex)
                {
                    file_output_format_CLB.SetItemCheckState(itemInd, CheckState.Checked);
                    Processor.outputFileType = (OutputFileType)itemInd;
                }
            }
        }

        /// <summary>
        /// Toggle charge detection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleChargeDetectionCB_CheckedChanged(object sender, EventArgs e)
        {
            if (toggleChargeDetectionCB.Checked)
            {
                FileProcessor.monocleOptions.Charge_Detection = true;
                polarity_checkBox.Enabled = true;
                lowChargeSelectionNUD.Enabled = true;
                highChargeSelectionNUD.Enabled = true;
            }
            else
            {
                FileProcessor.monocleOptions.Charge_Detection = false;
                polarity_checkBox.Enabled = false;
                lowChargeSelectionNUD.Enabled = false;
                highChargeSelectionNUD.Enabled = false;
            }
        }

        private void LowChargeSelectionNUD_ValueChanged(object sender, EventArgs e)
        {
            FileProcessor.monocleOptions.Charge_Range.Low = (int)lowChargeSelectionNUD.Value;
        }

        private void HighChargeSelectionNUD_ValueChanged(object sender, EventArgs e)
        {
            FileProcessor.monocleOptions.Charge_Range.Low = (int)highChargeSelectionNUD.Value;
        }

        private void Polarity_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!polarity_checkBox.Checked)
            {
                polarity_checkBox.ForeColor = Color.MediumTurquoise;
                polarity_checkBox.Text = "+";
            }
            else
            {
                polarity_checkBox.ForeColor = Color.MediumVioletRed;
                polarity_checkBox.Text = "-";
            }
        }
    }
}
