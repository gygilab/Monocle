using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Monocle;
using Monocle.Data;
using MonocleUI.lib;

namespace MonocleUI
{
    public partial class MonocleUI : Form
    {
        FileProcessor Processor = new FileProcessor();

        public MonocleUI()
        {
            InitializeComponent();
            Initiliaze_OutputFormat_CLB();
            LoadOptions();
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
                UpdateMonocleOptions();
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
        /// Load Monocle Options DGV
        /// </summary>
        public void LoadOptions()
        {
            PropertyInfo[] propertyInfo = FileProcessor.monocleOptions.GetType().GetProperties();
            for (int i = 0; i < propertyInfo.Length - 1; i++)
            {
                if(propertyInfo[i].Name == "WriteSps" || propertyInfo[i].Name == "ConvertOnly" || 
                    propertyInfo[i].Name == "WriteDebugString" || propertyInfo[i].Name == "OutputFileType")
                {
                    continue;
                }

                string[] newRow = new string[3]
                {
                    propertyInfo[i].Name,
                    FileProcessor.monocleOptions[propertyInfo[i].Name].ToString(),
                    OptionDescriptions.Descriptions[propertyInfo[i].Name]
                };
                //Invoke(new Action(
                //() =>
                //{
                    MonocleOptionsDGV.Rows.Add(newRow);
                //}));
            }
        }

        /// <summary>
        /// Update Monocle Options from Options DGV
        /// </summary>
        public void UpdateMonocleOptions()
        {
            foreach(DataGridViewRow row in MonocleOptionsDGV.Rows)
            {
                try
                {
                    Type tempType = FileProcessor.monocleOptions[row.Cells[0].Value.ToString()].GetType();

                    if(tempType == typeof(bool))
                    {
                        FileProcessor.monocleOptions[row.Cells[0].Value.ToString()] = bool.Parse(row.Cells[1].Value.ToString());
                    }
                    else if (tempType == typeof(ChargeRange))
                    {
                        FileProcessor.monocleOptions[row.Cells[0].Value.ToString()] = new ChargeRange(row.Cells[1].Value.ToString());
                    }
                    else if (tempType == typeof(OutputFileType))
                    {
                        FileProcessor.monocleOptions[row.Cells[0].Value.ToString()] = Enum.Parse(typeof(OutputFileType), row.Cells[1].Value.ToString());
                    }
                    else if (tempType == typeof(Polarity))
                    {
                        FileProcessor.monocleOptions[row.Cells[0].Value.ToString()] = Enum.Parse(typeof(Polarity), row.Cells[1].Value.ToString());
                    }
                    else if (tempType == typeof(int))
                    {
                        FileProcessor.monocleOptions[row.Cells[0].Value.ToString()] = int.Parse(row.Cells[1].Value.ToString());
                    }
                    else if (tempType == typeof(AveragingVector))
                    {
                        FileProcessor.monocleOptions[row.Cells[0].Value.ToString()] = Enum.Parse(typeof(AveragingVector), row.Cells[1].Value.ToString());
                    }


                    Debug.WriteLine(row.Cells[1].Value.ToString());
                    Debug.WriteLine(FileProcessor.monocleOptions[row.Cells[0].Value.ToString()].GetType() == Type.GetType("double"));
                }
                catch
                {
                    Debug.WriteLine(row.Cells[0].Value.ToString());
                    Debug.WriteLine(FileProcessor.monocleOptions[row.Cells[0].Value.ToString()].GetType() == typeof(bool));
                    continue;
                }
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
                add_file_button.Enabled = enabled;
                remove_dgv_row_button.Enabled = enabled;
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
                Invoke(new Action(
                () =>
                {
                    progressBar1.Value = 0;
                }));
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

    }

}
