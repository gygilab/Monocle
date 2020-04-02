using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Monocle;
using MonocleUI.ext;

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

        /// <summary>
        /// Initialize the list of potential output file formats
        /// </summary>
        private void Initiliaze_OutputFormat_CLB()
        {
            foreach(string type in Enum.GetNames(typeof(OutputFileType)))
            {
                file_output_format_CLB.Items.Add(type);
            }
            file_output_format_CLB.SetItemChecked(0, true);
        }

        /// <summary>
        /// Adds files from brows dialog to dgv
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Adds files from drag and drop to dgv
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Selection for where files should be placed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void select_output_directory_button_Click(object sender, EventArgs e)
        {
            if (export_folder_dialog.ShowDialog() == DialogResult.OK)
            {
                export_folder_maskedTB.Text = export_folder_dialog.SelectedPath;
            }
        }

        /// <summary>
        /// Remove row from the file DGV
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Toggle to show the log or not.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Update the Monocle log.
        /// </summary>
        /// <param name="message"></param>
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

        /// <summary>
        /// Update the monocle progress bar
        /// </summary>
        /// <param name="progress"></param>
        public void UpdateProgress(int progress)
        {
            Invoke(new Action(
            () =>
            {
                progressBar1.Value = progress;
            }));
        }

        private static bool FileListenerStarted { get; set; } = false;

        /// <summary>
        /// UI start of Monocle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Start_monocle_button_Click(object sender, EventArgs e)
        {
            EnableRunUI(false);
            if (!FileListenerStarted)
            {
                Processor.FileTracker += FileListener;
                FileListenerStarted = true;
            }

            try
            {
                Processor.Run();
            }
            catch (Exception ex)
            {
                UpdateLog("Run cancelled.");
#if DEBUG
                UpdateLog(ex.ToString());
#endif
            }
        }

        /// <summary>
        /// NUD for changing the number of scans to average in monocle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberOfScansToAverageNUD_ValueChanged(object sender, EventArgs e)
        {
            if(numberOfScansToAverageNUD.Value >= 1 && numberOfScansToAverageNUD.Value <= 20)
            {
                FileProcessor.monocleOptions.Number_Of_Scans_To_Average = (int)numberOfScansToAverageNUD.Value;
            }
        }

        /// <summary>
        /// Enable or disable the UI parameters used while running.
        /// </summary>
        /// <param name="enabled"></param>
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

        /// <summary>
        /// File listener method used for updating the current pipeline progress.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileListener(object sender, FileEventArgs e)
        {
            if (e.FinishedAllFiles)
            {
                EnableRunUI(true);
                return;
            }

            if (!File.Exists(e.FilePath))
            {
                return;
            }

            switch (e.runStatus)
            {
                case RunStatus.Started:
                    UpdateLog("New File started: " + e.FilePath);
                    break;
                case RunStatus.Read:
                    UpdateLog("File Read into Monocle: " + e.FilePath);
                    break;
                case RunStatus.Processed:
                    UpdateLog("Monocle Adjustment Complete: " + e.FilePath);
                    break;
                case RunStatus.Written:
                    UpdateLog("Output File Written: " + e.FilePath);
                    break;
                case RunStatus.Finished:
                    UpdateLog("File Processing Finished: " + e.FilePath);
                    break;
            }
            UpdateProgress((int)e.CurrentProgress);
        }

        /// <summary>
        /// Selector for data output file type.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    FileProcessor.monocleOptions.OutputFileType = (OutputFileType)itemInd;
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

        /// <summary>
        /// Lower of two charge states used for Mono
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LowChargeSelectionNUD_ValueChanged(object sender, EventArgs e)
        {
            FileProcessor.monocleOptions.Charge_Range.Low = (int)lowChargeSelectionNUD.Value;
        }

        /// <summary>
        /// Higher of two charge states used for mono
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HighChargeSelectionNUD_ValueChanged(object sender, EventArgs e)
        {
            FileProcessor.monocleOptions.Charge_Range.Low = (int)highChargeSelectionNUD.Value;
        }

        /// <summary>
        /// Update the polarity used for assessing the mono
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Invoke the cancellation token to stop the run.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            Processor.Cancel();
            EnableRunUI(true);
        }

        /// <summary>
        /// Selector for whether to (1) convert with monocle or (2) convert only
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void convertOnlyCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (convertOnlyCheckbox.Checked)
            {
                FileProcessor.ConvertOnly = true;
                convertOnlyCheckbox.ForeColor = Color.MediumVioletRed;
                convertOnlyCheckbox.BackColor = Color.LightCoral;
            }
            else
            {
                FileProcessor.ConvertOnly = false;
                convertOnlyCheckbox.ForeColor = Color.Black;
                convertOnlyCheckbox.BackColor = Color.Transparent;
            }
        }

        private void lowRes_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            FileProcessor.monocleOptions.ChargeRangeUnknown = new Monocle.Data.ChargeRange("2:3");
        }

        private void forceCharge_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (forceCharge_checkbox.Checked)
            {
                FileProcessor.monocleOptions.ForceCharges = true;
            }
            else
            {
                FileProcessor.monocleOptions.ForceCharges = false;
            }
        }

        private void monocleOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MonocleOptionsForm newOptionsForm = new MonocleOptionsForm();
            newOptionsForm.ShowDialog();
        }
    }

}
