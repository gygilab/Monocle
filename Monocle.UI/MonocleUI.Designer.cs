namespace MonocleUI
{
    partial class MonocleUI
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
            this.components = new System.ComponentModel.Container();
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gCCollectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.input_files_dgv = new System.Windows.Forms.DataGridView();
            this.file_column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remove_dgv_row_button = new System.Windows.Forms.Button();
            this.add_file_button = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.convertOnlyCheckbox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.file_output_format_CLB = new System.Windows.Forms.CheckedListBox();
            this.select_output_directory_button = new System.Windows.Forms.Button();
            this.export_folder_maskedTB = new System.Windows.Forms.MaskedTextBox();
            this.monocle_log_tb = new System.Windows.Forms.GroupBox();
            this.monocle_log = new System.Windows.Forms.RichTextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.start_monocle_button = new System.Windows.Forms.Button();
            this.input_file_dialog = new System.Windows.Forms.OpenFileDialog();
            this.export_folder_dialog = new System.Windows.Forms.FolderBrowserDialog();
            this.log_toggle_checkbox = new System.Windows.Forms.CheckBox();
            this.mainToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.monocleOptionsBox = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.polarity_checkBox = new System.Windows.Forms.CheckBox();
            this.toggleChargeDetectionCB = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.highChargeSelectionNUD = new System.Windows.Forms.NumericUpDown();
            this.lowChargeSelectionNUD = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numberOfScansToAverageNUD = new System.Windows.Forms.NumericUpDown();
            this.cancelButton = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.mainMenuStrip.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.input_files_dgv)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.monocle_log_tb.SuspendLayout();
            this.monocleOptionsBox.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.highChargeSelectionNUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lowChargeSelectionNUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfScansToAverageNUD)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.preferencesToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(767, 24);
            this.mainMenuStrip.TabIndex = 0;
            this.mainMenuStrip.Text = "mainMenuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(93, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gCCollectionToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // gCCollectionToolStripMenuItem
            // 
            this.gCCollectionToolStripMenuItem.Name = "gCCollectionToolStripMenuItem";
            this.gCCollectionToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.gCCollectionToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.gCCollectionToolStripMenuItem.Text = "GC Collection";
            // 
            // preferencesToolStripMenuItem
            // 
            this.preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            this.preferencesToolStripMenuItem.Size = new System.Drawing.Size(80, 20);
            this.preferencesToolStripMenuItem.Text = "Preferences";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.input_files_dgv);
            this.groupBox1.Controls.Add(this.remove_dgv_row_button);
            this.groupBox1.Controls.Add(this.add_file_button);
            this.groupBox1.Location = new System.Drawing.Point(13, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(742, 257);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Inputs";
            this.mainToolTip.SetToolTip(this.groupBox1, "Drag and drop files or brows with the \"+\" button.");
            // 
            // input_files_dgv
            // 
            this.input_files_dgv.AllowDrop = true;
            this.input_files_dgv.AllowUserToAddRows = false;
            this.input_files_dgv.AllowUserToDeleteRows = false;
            this.input_files_dgv.AllowUserToResizeColumns = false;
            this.input_files_dgv.AllowUserToResizeRows = false;
            this.input_files_dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.input_files_dgv.BackgroundColor = System.Drawing.SystemColors.Control;
            this.input_files_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.input_files_dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.file_column});
            this.input_files_dgv.GridColor = System.Drawing.SystemColors.Control;
            this.input_files_dgv.Location = new System.Drawing.Point(6, 16);
            this.input_files_dgv.Name = "input_files_dgv";
            this.input_files_dgv.RowHeadersVisible = false;
            this.input_files_dgv.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.input_files_dgv.RowTemplate.ReadOnly = true;
            this.input_files_dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.input_files_dgv.Size = new System.Drawing.Size(692, 231);
            this.input_files_dgv.TabIndex = 5;
            this.mainToolTip.SetToolTip(this.input_files_dgv, "Drag and drop files or brows with the \"+\" button.");
            this.input_files_dgv.DragDrop += new System.Windows.Forms.DragEventHandler(this.Input_files_dgv_DragDrop);
            this.input_files_dgv.DragEnter += new System.Windows.Forms.DragEventHandler(this.Input_files_dgv_DragEnter);
            // 
            // file_column
            // 
            this.file_column.HeaderText = "Files";
            this.file_column.Name = "file_column";
            // 
            // remove_dgv_row_button
            // 
            this.remove_dgv_row_button.Location = new System.Drawing.Point(704, 54);
            this.remove_dgv_row_button.Name = "remove_dgv_row_button";
            this.remove_dgv_row_button.Size = new System.Drawing.Size(32, 32);
            this.remove_dgv_row_button.TabIndex = 4;
            this.remove_dgv_row_button.Text = "-";
            this.mainToolTip.SetToolTip(this.remove_dgv_row_button, "Remove selected row(s).");
            this.remove_dgv_row_button.UseVisualStyleBackColor = true;
            this.remove_dgv_row_button.Click += new System.EventHandler(this.remove_dgv_row_button_Click);
            // 
            // add_file_button
            // 
            this.add_file_button.Location = new System.Drawing.Point(704, 16);
            this.add_file_button.Name = "add_file_button";
            this.add_file_button.Size = new System.Drawing.Size(32, 32);
            this.add_file_button.TabIndex = 3;
            this.add_file_button.Text = "+";
            this.mainToolTip.SetToolTip(this.add_file_button, "Drag and drop files or brows with the \"+\" button.");
            this.add_file_button.UseVisualStyleBackColor = true;
            this.add_file_button.Click += new System.EventHandler(this.add_file_button_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.convertOnlyCheckbox);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.file_output_format_CLB);
            this.groupBox2.Controls.Add(this.select_output_directory_button);
            this.groupBox2.Controls.Add(this.export_folder_maskedTB);
            this.groupBox2.Location = new System.Drawing.Point(372, 291);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(384, 160);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "File Export";
            // 
            // convertOnlyCheckbox
            // 
            this.convertOnlyCheckbox.Appearance = System.Windows.Forms.Appearance.Button;
            this.convertOnlyCheckbox.AutoSize = true;
            this.convertOnlyCheckbox.FlatAppearance.CheckedBackColor = System.Drawing.Color.LightCoral;
            this.convertOnlyCheckbox.Location = new System.Drawing.Point(299, 15);
            this.convertOnlyCheckbox.Name = "convertOnlyCheckbox";
            this.convertOnlyCheckbox.Size = new System.Drawing.Size(78, 23);
            this.convertOnlyCheckbox.TabIndex = 6;
            this.convertOnlyCheckbox.Text = "Convert Only";
            this.convertOnlyCheckbox.UseVisualStyleBackColor = true;
            this.convertOnlyCheckbox.CheckedChanged += new System.EventHandler(this.convertOnlyCheckbox_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(132, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Choose Output File Format";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 109);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Export Data To:";
            // 
            // file_output_format_CLB
            // 
            this.file_output_format_CLB.CheckOnClick = true;
            this.file_output_format_CLB.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.file_output_format_CLB.FormattingEnabled = true;
            this.file_output_format_CLB.Location = new System.Drawing.Point(6, 53);
            this.file_output_format_CLB.MultiColumn = true;
            this.file_output_format_CLB.Name = "file_output_format_CLB";
            this.file_output_format_CLB.Size = new System.Drawing.Size(371, 46);
            this.file_output_format_CLB.TabIndex = 3;
            this.file_output_format_CLB.SelectedIndexChanged += new System.EventHandler(this.File_output_format_CLB_SelectedIndexChanged);
            // 
            // select_output_directory_button
            // 
            this.select_output_directory_button.Location = new System.Drawing.Point(302, 100);
            this.select_output_directory_button.Name = "select_output_directory_button";
            this.select_output_directory_button.Size = new System.Drawing.Size(75, 23);
            this.select_output_directory_button.TabIndex = 1;
            this.select_output_directory_button.Text = "Browse";
            this.select_output_directory_button.UseVisualStyleBackColor = true;
            this.select_output_directory_button.Click += new System.EventHandler(this.select_output_directory_button_Click);
            // 
            // export_folder_maskedTB
            // 
            this.export_folder_maskedTB.Location = new System.Drawing.Point(6, 129);
            this.export_folder_maskedTB.Name = "export_folder_maskedTB";
            this.export_folder_maskedTB.ReadOnly = true;
            this.export_folder_maskedTB.Size = new System.Drawing.Size(371, 20);
            this.export_folder_maskedTB.TabIndex = 0;
            // 
            // monocle_log_tb
            // 
            this.monocle_log_tb.Controls.Add(this.monocle_log);
            this.monocle_log_tb.Location = new System.Drawing.Point(13, 546);
            this.monocle_log_tb.Name = "monocle_log_tb";
            this.monocle_log_tb.Size = new System.Drawing.Size(742, 166);
            this.monocle_log_tb.TabIndex = 3;
            this.monocle_log_tb.TabStop = false;
            this.monocle_log_tb.Text = "Monocle Log";
            // 
            // monocle_log
            // 
            this.monocle_log.Location = new System.Drawing.Point(7, 20);
            this.monocle_log.Name = "monocle_log";
            this.monocle_log.ReadOnly = true;
            this.monocle_log.Size = new System.Drawing.Size(729, 140);
            this.monocle_log.TabIndex = 0;
            this.monocle_log.Text = "";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(20, 474);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(641, 23);
            this.progressBar1.TabIndex = 4;
            // 
            // start_monocle_button
            // 
            this.start_monocle_button.Location = new System.Drawing.Point(680, 474);
            this.start_monocle_button.Name = "start_monocle_button";
            this.start_monocle_button.Size = new System.Drawing.Size(75, 23);
            this.start_monocle_button.TabIndex = 5;
            this.start_monocle_button.Text = "Run!";
            this.start_monocle_button.UseVisualStyleBackColor = true;
            this.start_monocle_button.Click += new System.EventHandler(this.Start_monocle_button_Click);
            // 
            // input_file_dialog
            // 
            this.input_file_dialog.FileName = "Input";
            this.input_file_dialog.Multiselect = true;
            // 
            // log_toggle_checkbox
            // 
            this.log_toggle_checkbox.Appearance = System.Windows.Forms.Appearance.Button;
            this.log_toggle_checkbox.AutoSize = true;
            this.log_toggle_checkbox.Location = new System.Drawing.Point(20, 503);
            this.log_toggle_checkbox.Name = "log_toggle_checkbox";
            this.log_toggle_checkbox.Size = new System.Drawing.Size(100, 23);
            this.log_toggle_checkbox.TabIndex = 6;
            this.log_toggle_checkbox.Text = "Show Output Log";
            this.log_toggle_checkbox.UseVisualStyleBackColor = true;
            this.log_toggle_checkbox.CheckedChanged += new System.EventHandler(this.Log_toggle_checkbox_CheckedChanged);
            // 
            // monocleOptionsBox
            // 
            this.monocleOptionsBox.Controls.Add(this.groupBox3);
            this.monocleOptionsBox.Controls.Add(this.label3);
            this.monocleOptionsBox.Controls.Add(this.numberOfScansToAverageNUD);
            this.monocleOptionsBox.Location = new System.Drawing.Point(14, 291);
            this.monocleOptionsBox.Name = "monocleOptionsBox";
            this.monocleOptionsBox.Size = new System.Drawing.Size(352, 160);
            this.monocleOptionsBox.TabIndex = 7;
            this.monocleOptionsBox.TabStop = false;
            this.monocleOptionsBox.Text = "Monocle Options";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBox2);
            this.groupBox3.Controls.Add(this.checkBox1);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.polarity_checkBox);
            this.groupBox3.Controls.Add(this.toggleChargeDetectionCB);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.highChargeSelectionNUD);
            this.groupBox3.Controls.Add(this.lowChargeSelectionNUD);
            this.groupBox3.Location = new System.Drawing.Point(7, 51);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(339, 98);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Charge Detection";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(246, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 17);
            this.label5.TabIndex = 7;
            this.label5.Text = "Polarity";
            // 
            // polarity_checkBox
            // 
            this.polarity_checkBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.polarity_checkBox.Enabled = false;
            this.polarity_checkBox.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.Control;
            this.polarity_checkBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.polarity_checkBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.polarity_checkBox.ForeColor = System.Drawing.Color.MediumTurquoise;
            this.polarity_checkBox.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.polarity_checkBox.Location = new System.Drawing.Point(304, 61);
            this.polarity_checkBox.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.polarity_checkBox.Name = "polarity_checkBox";
            this.polarity_checkBox.Size = new System.Drawing.Size(32, 30);
            this.polarity_checkBox.TabIndex = 6;
            this.polarity_checkBox.Text = "+";
            this.polarity_checkBox.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.polarity_checkBox.UseVisualStyleBackColor = false;
            this.polarity_checkBox.CheckedChanged += new System.EventHandler(this.Polarity_checkBox_CheckedChanged);
            // 
            // toggleChargeDetectionCB
            // 
            this.toggleChargeDetectionCB.Appearance = System.Windows.Forms.Appearance.Button;
            this.toggleChargeDetectionCB.AutoSize = true;
            this.toggleChargeDetectionCB.BackColor = System.Drawing.Color.White;
            this.toggleChargeDetectionCB.FlatAppearance.CheckedBackColor = System.Drawing.Color.LightSteelBlue;
            this.toggleChargeDetectionCB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.toggleChargeDetectionCB.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toggleChargeDetectionCB.Location = new System.Drawing.Point(205, 17);
            this.toggleChargeDetectionCB.Name = "toggleChargeDetectionCB";
            this.toggleChargeDetectionCB.Size = new System.Drawing.Size(128, 27);
            this.toggleChargeDetectionCB.TabIndex = 2;
            this.toggleChargeDetectionCB.Text = "Charge Detection";
            this.toggleChargeDetectionCB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.mainToolTip.SetToolTip(this.toggleChargeDetectionCB, "Toggle charge detection.");
            this.toggleChargeDetectionCB.UseVisualStyleBackColor = false;
            this.toggleChargeDetectionCB.CheckedChanged += new System.EventHandler(this.ToggleChargeDetectionCB_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 17);
            this.label4.TabIndex = 5;
            this.label4.Text = "Charge Range";
            this.mainToolTip.SetToolTip(this.label4, "Range for Charge Detection, if enabled.");
            // 
            // highChargeSelectionNUD
            // 
            this.highChargeSelectionNUD.Enabled = false;
            this.highChargeSelectionNUD.Location = new System.Drawing.Point(175, 66);
            this.highChargeSelectionNUD.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.highChargeSelectionNUD.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.highChargeSelectionNUD.Name = "highChargeSelectionNUD";
            this.highChargeSelectionNUD.Size = new System.Drawing.Size(60, 20);
            this.highChargeSelectionNUD.TabIndex = 4;
            this.highChargeSelectionNUD.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.mainToolTip.SetToolTip(this.highChargeSelectionNUD, "Range for Charge Detection, if enabled.");
            this.highChargeSelectionNUD.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.highChargeSelectionNUD.ValueChanged += new System.EventHandler(this.HighChargeSelectionNUD_ValueChanged);
            // 
            // lowChargeSelectionNUD
            // 
            this.lowChargeSelectionNUD.Enabled = false;
            this.lowChargeSelectionNUD.Location = new System.Drawing.Point(109, 66);
            this.lowChargeSelectionNUD.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.lowChargeSelectionNUD.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.lowChargeSelectionNUD.Name = "lowChargeSelectionNUD";
            this.lowChargeSelectionNUD.Size = new System.Drawing.Size(60, 20);
            this.lowChargeSelectionNUD.TabIndex = 3;
            this.lowChargeSelectionNUD.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.mainToolTip.SetToolTip(this.lowChargeSelectionNUD, "Range for Charge Detection, if enabled.");
            this.lowChargeSelectionNUD.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.lowChargeSelectionNUD.ValueChanged += new System.EventHandler(this.LowChargeSelectionNUD_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(10, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "Average N Scans";
            // 
            // numberOfScansToAverageNUD
            // 
            this.numberOfScansToAverageNUD.Location = new System.Drawing.Point(151, 25);
            this.numberOfScansToAverageNUD.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numberOfScansToAverageNUD.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numberOfScansToAverageNUD.Name = "numberOfScansToAverageNUD";
            this.numberOfScansToAverageNUD.Size = new System.Drawing.Size(120, 20);
            this.numberOfScansToAverageNUD.TabIndex = 0;
            this.numberOfScansToAverageNUD.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numberOfScansToAverageNUD.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.numberOfScansToAverageNUD.ValueChanged += new System.EventHandler(this.NumberOfScansToAverageNUD_ValueChanged);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(680, 503);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 8;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBox1.BackColor = System.Drawing.Color.White;
            this.checkBox1.FlatAppearance.CheckedBackColor = System.Drawing.Color.LightSteelBlue;
            this.checkBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox1.Location = new System.Drawing.Point(6, 17);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(82, 27);
            this.checkBox1.TabIndex = 8;
            this.checkBox1.Text = "Low Res MS1s";
            this.checkBox1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.mainToolTip.SetToolTip(this.checkBox1, "For low-res scans, output multiple precursors with these charges.");
            this.checkBox1.UseVisualStyleBackColor = false;
            // 
            // checkBox2
            // 
            this.checkBox2.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBox2.BackColor = System.Drawing.Color.White;
            this.checkBox2.FlatAppearance.CheckedBackColor = System.Drawing.Color.LightSteelBlue;
            this.checkBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox2.Location = new System.Drawing.Point(94, 17);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(105, 27);
            this.checkBox2.TabIndex = 9;
            this.checkBox2.Text = "Force Charge";
            this.checkBox2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.mainToolTip.SetToolTip(this.checkBox2, "Output multiple precursors with charges even if charge is known.");
            this.checkBox2.UseVisualStyleBackColor = false;
            // 
            // MonocleUI
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(767, 724);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.monocleOptionsBox);
            this.Controls.Add(this.log_toggle_checkbox);
            this.Controls.Add(this.start_monocle_button);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.monocle_log_tb);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.mainMenuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MainMenuStrip = this.mainMenuStrip;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MonocleUI";
            this.ShowIcon = false;
            this.Text = "Monocle";
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.input_files_dgv)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.monocle_log_tb.ResumeLayout(false);
            this.monocleOptionsBox.ResumeLayout(false);
            this.monocleOptionsBox.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.highChargeSelectionNUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lowChargeSelectionNUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfScansToAverageNUD)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem preferencesToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox monocle_log_tb;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button start_monocle_button;
        private System.Windows.Forms.Button remove_dgv_row_button;
        private System.Windows.Forms.Button add_file_button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button select_output_directory_button;
        private System.Windows.Forms.MaskedTextBox export_folder_maskedTB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckedListBox file_output_format_CLB;
        private System.Windows.Forms.RichTextBox monocle_log;
        private System.Windows.Forms.OpenFileDialog input_file_dialog;
        private System.Windows.Forms.FolderBrowserDialog export_folder_dialog;
        private System.Windows.Forms.DataGridView input_files_dgv;
        private System.Windows.Forms.DataGridViewTextBoxColumn file_column;
        private System.Windows.Forms.CheckBox log_toggle_checkbox;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolTip mainToolTip;
        private System.Windows.Forms.ToolStripMenuItem gCCollectionToolStripMenuItem;
        private System.Windows.Forms.GroupBox monocleOptionsBox;
        private System.Windows.Forms.NumericUpDown numberOfScansToAverageNUD;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown highChargeSelectionNUD;
        private System.Windows.Forms.NumericUpDown lowChargeSelectionNUD;
        private System.Windows.Forms.CheckBox toggleChargeDetectionCB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox polarity_checkBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.CheckBox convertOnlyCheckbox;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}

