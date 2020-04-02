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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.monocleOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.MonocleOptionsDGV = new System.Windows.Forms.DataGridView();
            this.Options = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cancelButton = new System.Windows.Forms.Button();
            this.mainMenuStrip.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.input_files_dgv)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.monocle_log_tb.SuspendLayout();
            this.monocleOptionsBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MonocleOptionsDGV)).BeginInit();
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
            this.monocleOptionsToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // monocleOptionsToolStripMenuItem
            // 
            this.monocleOptionsToolStripMenuItem.Name = "monocleOptionsToolStripMenuItem";
            this.monocleOptionsToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.monocleOptionsToolStripMenuItem.Text = "Monocle Options";
            // 
            // preferencesToolStripMenuItem
            // 
            this.preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            this.preferencesToolStripMenuItem.Size = new System.Drawing.Size(80, 20);
            this.preferencesToolStripMenuItem.Text = "Preferences";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.remove_dgv_row_button);
            this.groupBox1.Controls.Add(this.add_file_button);
            this.groupBox1.Controls.Add(this.input_files_dgv);
            this.groupBox1.Location = new System.Drawing.Point(12, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(365, 424);
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
            this.input_files_dgv.Dock = System.Windows.Forms.DockStyle.Left;
            this.input_files_dgv.GridColor = System.Drawing.SystemColors.Control;
            this.input_files_dgv.Location = new System.Drawing.Point(3, 16);
            this.input_files_dgv.Name = "input_files_dgv";
            this.input_files_dgv.RowHeadersVisible = false;
            this.input_files_dgv.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.input_files_dgv.RowTemplate.ReadOnly = true;
            this.input_files_dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.input_files_dgv.Size = new System.Drawing.Size(318, 405);
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
            this.remove_dgv_row_button.Location = new System.Drawing.Point(327, 54);
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
            this.add_file_button.Location = new System.Drawing.Point(327, 16);
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
            this.groupBox2.Location = new System.Drawing.Point(383, 273);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(373, 179);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "File Export";
            // 
            // convertOnlyCheckbox
            // 
            this.convertOnlyCheckbox.Appearance = System.Windows.Forms.Appearance.Button;
            this.convertOnlyCheckbox.AutoSize = true;
            this.convertOnlyCheckbox.FlatAppearance.CheckedBackColor = System.Drawing.Color.LightCoral;
            this.convertOnlyCheckbox.Location = new System.Drawing.Point(292, 19);
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
            this.label2.Location = new System.Drawing.Point(9, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(132, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Choose Output File Format";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 126);
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
            this.file_output_format_CLB.Location = new System.Drawing.Point(9, 53);
            this.file_output_format_CLB.MultiColumn = true;
            this.file_output_format_CLB.Name = "file_output_format_CLB";
            this.file_output_format_CLB.Size = new System.Drawing.Size(360, 46);
            this.file_output_format_CLB.TabIndex = 3;
            this.file_output_format_CLB.SelectedIndexChanged += new System.EventHandler(this.File_output_format_CLB_SelectedIndexChanged);
            // 
            // select_output_directory_button
            // 
            this.select_output_directory_button.Location = new System.Drawing.Point(292, 121);
            this.select_output_directory_button.Name = "select_output_directory_button";
            this.select_output_directory_button.Size = new System.Drawing.Size(75, 23);
            this.select_output_directory_button.TabIndex = 1;
            this.select_output_directory_button.Text = "Browse";
            this.select_output_directory_button.UseVisualStyleBackColor = true;
            this.select_output_directory_button.Click += new System.EventHandler(this.select_output_directory_button_Click);
            // 
            // export_folder_maskedTB
            // 
            this.export_folder_maskedTB.Location = new System.Drawing.Point(9, 150);
            this.export_folder_maskedTB.Name = "export_folder_maskedTB";
            this.export_folder_maskedTB.ReadOnly = true;
            this.export_folder_maskedTB.Size = new System.Drawing.Size(357, 20);
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
            this.progressBar1.Location = new System.Drawing.Point(20, 458);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(641, 23);
            this.progressBar1.TabIndex = 4;
            // 
            // start_monocle_button
            // 
            this.start_monocle_button.Location = new System.Drawing.Point(680, 458);
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
            this.log_toggle_checkbox.Location = new System.Drawing.Point(20, 487);
            this.log_toggle_checkbox.Name = "log_toggle_checkbox";
            this.log_toggle_checkbox.Size = new System.Drawing.Size(100, 23);
            this.log_toggle_checkbox.TabIndex = 6;
            this.log_toggle_checkbox.Text = "Show Output Log";
            this.log_toggle_checkbox.UseVisualStyleBackColor = true;
            this.log_toggle_checkbox.CheckedChanged += new System.EventHandler(this.Log_toggle_checkbox_CheckedChanged);
            // 
            // monocleOptionsBox
            // 
            this.monocleOptionsBox.Controls.Add(this.MonocleOptionsDGV);
            this.monocleOptionsBox.Location = new System.Drawing.Point(383, 28);
            this.monocleOptionsBox.Name = "monocleOptionsBox";
            this.monocleOptionsBox.Size = new System.Drawing.Size(373, 239);
            this.monocleOptionsBox.TabIndex = 7;
            this.monocleOptionsBox.TabStop = false;
            this.monocleOptionsBox.Text = "Monocle Options";
            this.mainToolTip.SetToolTip(this.monocleOptionsBox, "Enter Monocle Options, improperly formatted options will be ignored.");
            // 
            // MonocleOptionsDGV
            // 
            this.MonocleOptionsDGV.AllowUserToAddRows = false;
            this.MonocleOptionsDGV.AllowUserToDeleteRows = false;
            this.MonocleOptionsDGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.MonocleOptionsDGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.MonocleOptionsDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MonocleOptionsDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Options,
            this.Value,
            this.Description});
            this.MonocleOptionsDGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MonocleOptionsDGV.Location = new System.Drawing.Point(3, 16);
            this.MonocleOptionsDGV.Name = "MonocleOptionsDGV";
            this.MonocleOptionsDGV.RowHeadersVisible = false;
            this.MonocleOptionsDGV.Size = new System.Drawing.Size(367, 220);
            this.MonocleOptionsDGV.TabIndex = 1;
            // 
            // Options
            // 
            this.Options.HeaderText = "Options";
            this.Options.Name = "Options";
            this.Options.ReadOnly = true;
            // 
            // Value
            // 
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            // 
            // Description
            // 
            this.Description.HeaderText = "Description";
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(680, 487);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 8;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // MonocleUI
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(767, 724);
            this.Controls.Add(this.monocleOptionsBox);
            this.Controls.Add(this.cancelButton);
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
            ((System.ComponentModel.ISupportInitialize)(this.MonocleOptionsDGV)).EndInit();
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
        private System.Windows.Forms.GroupBox monocleOptionsBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.CheckBox convertOnlyCheckbox;
        private System.Windows.Forms.ToolStripMenuItem monocleOptionsToolStripMenuItem;
        private System.Windows.Forms.DataGridView MonocleOptionsDGV;
        private System.Windows.Forms.DataGridViewTextBoxColumn Options;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
    }
}

