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
            components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            mainMenuStrip = new System.Windows.Forms.MenuStrip();
            fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            monocleOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            groupBox1 = new System.Windows.Forms.GroupBox();
            remove_dgv_row_button = new System.Windows.Forms.Button();
            add_file_button = new System.Windows.Forms.Button();
            input_files_dgv = new System.Windows.Forms.DataGridView();
            file_column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            groupBox2 = new System.Windows.Forms.GroupBox();
            convertOnlyCheckbox = new System.Windows.Forms.CheckBox();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            file_output_format_CLB = new System.Windows.Forms.CheckedListBox();
            select_output_directory_button = new System.Windows.Forms.Button();
            export_folder_maskedTB = new System.Windows.Forms.MaskedTextBox();
            monocle_log_tb = new System.Windows.Forms.GroupBox();
            monocle_log = new System.Windows.Forms.RichTextBox();
            progressBar1 = new System.Windows.Forms.ProgressBar();
            start_monocle_button = new System.Windows.Forms.Button();
            input_file_dialog = new System.Windows.Forms.OpenFileDialog();
            export_folder_dialog = new System.Windows.Forms.FolderBrowserDialog();
            log_toggle_checkbox = new System.Windows.Forms.CheckBox();
            mainToolTip = new System.Windows.Forms.ToolTip(components);
            monocleOptionsBox = new System.Windows.Forms.GroupBox();
            MonocleOptionsDGV = new System.Windows.Forms.DataGridView();
            Options = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            cancelButton = new System.Windows.Forms.Button();
            mainMenuStrip.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)input_files_dgv).BeginInit();
            groupBox2.SuspendLayout();
            monocle_log_tb.SuspendLayout();
            monocleOptionsBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)MonocleOptionsDGV).BeginInit();
            SuspendLayout();
            // 
            // mainMenuStrip
            // 
            mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileToolStripMenuItem, optionsToolStripMenuItem, preferencesToolStripMenuItem });
            mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            mainMenuStrip.Name = "mainMenuStrip";
            mainMenuStrip.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            mainMenuStrip.Size = new System.Drawing.Size(895, 24);
            mainMenuStrip.TabIndex = 0;
            mainMenuStrip.Text = "mainMenuStrip";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new System.Drawing.Size(93, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += ExitToolStripMenuItem_Click;
            // 
            // optionsToolStripMenuItem
            // 
            optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { monocleOptionsToolStripMenuItem });
            optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            optionsToolStripMenuItem.Text = "Options";
            // 
            // monocleOptionsToolStripMenuItem
            // 
            monocleOptionsToolStripMenuItem.Name = "monocleOptionsToolStripMenuItem";
            monocleOptionsToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            monocleOptionsToolStripMenuItem.Text = "Monocle Options";
            // 
            // preferencesToolStripMenuItem
            // 
            preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            preferencesToolStripMenuItem.Size = new System.Drawing.Size(80, 20);
            preferencesToolStripMenuItem.Text = "Preferences";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(remove_dgv_row_button);
            groupBox1.Controls.Add(add_file_button);
            groupBox1.Controls.Add(input_files_dgv);
            groupBox1.Location = new System.Drawing.Point(14, 32);
            groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox1.Size = new System.Drawing.Size(426, 489);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Inputs";
            mainToolTip.SetToolTip(groupBox1, "Drag and drop files or brows with the \"+\" button.");
            // 
            // remove_dgv_row_button
            // 
            remove_dgv_row_button.Location = new System.Drawing.Point(382, 62);
            remove_dgv_row_button.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            remove_dgv_row_button.Name = "remove_dgv_row_button";
            remove_dgv_row_button.Size = new System.Drawing.Size(37, 37);
            remove_dgv_row_button.TabIndex = 4;
            remove_dgv_row_button.Text = "-";
            mainToolTip.SetToolTip(remove_dgv_row_button, "Remove selected row(s).");
            remove_dgv_row_button.UseVisualStyleBackColor = true;
            remove_dgv_row_button.Click += remove_dgv_row_button_Click;
            // 
            // add_file_button
            // 
            add_file_button.Location = new System.Drawing.Point(382, 18);
            add_file_button.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            add_file_button.Name = "add_file_button";
            add_file_button.Size = new System.Drawing.Size(37, 37);
            add_file_button.TabIndex = 3;
            add_file_button.Text = "+";
            mainToolTip.SetToolTip(add_file_button, "Drag and drop files or brows with the \"+\" button.");
            add_file_button.UseVisualStyleBackColor = true;
            add_file_button.Click += add_file_button_Click;
            // 
            // input_files_dgv
            // 
            input_files_dgv.AllowDrop = true;
            input_files_dgv.AllowUserToAddRows = false;
            input_files_dgv.AllowUserToDeleteRows = false;
            input_files_dgv.AllowUserToResizeColumns = false;
            input_files_dgv.AllowUserToResizeRows = false;
            input_files_dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            input_files_dgv.BackgroundColor = System.Drawing.SystemColors.Control;
            input_files_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            input_files_dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { file_column });
            input_files_dgv.Dock = System.Windows.Forms.DockStyle.Left;
            input_files_dgv.GridColor = System.Drawing.SystemColors.Control;
            input_files_dgv.Location = new System.Drawing.Point(4, 19);
            input_files_dgv.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            input_files_dgv.Name = "input_files_dgv";
            input_files_dgv.RowHeadersVisible = false;
            input_files_dgv.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            input_files_dgv.RowTemplate.ReadOnly = true;
            input_files_dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            input_files_dgv.Size = new System.Drawing.Size(371, 467);
            input_files_dgv.TabIndex = 5;
            mainToolTip.SetToolTip(input_files_dgv, "Drag and drop files or brows with the \"+\" button.");
            input_files_dgv.DragDrop += Input_files_dgv_DragDrop;
            input_files_dgv.DragEnter += Input_files_dgv_DragEnter;
            // 
            // file_column
            // 
            file_column.HeaderText = "Files";
            file_column.Name = "file_column";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(convertOnlyCheckbox);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(file_output_format_CLB);
            groupBox2.Controls.Add(select_output_directory_button);
            groupBox2.Controls.Add(export_folder_maskedTB);
            groupBox2.Location = new System.Drawing.Point(447, 315);
            groupBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox2.Size = new System.Drawing.Size(435, 207);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "File Export";
            // 
            // convertOnlyCheckbox
            // 
            convertOnlyCheckbox.Appearance = System.Windows.Forms.Appearance.Button;
            convertOnlyCheckbox.AutoSize = true;
            convertOnlyCheckbox.FlatAppearance.CheckedBackColor = System.Drawing.Color.LightCoral;
            convertOnlyCheckbox.Location = new System.Drawing.Point(341, 22);
            convertOnlyCheckbox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            convertOnlyCheckbox.Name = "convertOnlyCheckbox";
            convertOnlyCheckbox.Size = new System.Drawing.Size(87, 25);
            convertOnlyCheckbox.TabIndex = 6;
            convertOnlyCheckbox.Text = "Convert Only";
            convertOnlyCheckbox.UseVisualStyleBackColor = true;
            convertOnlyCheckbox.CheckedChanged += convertOnlyCheckbox_CheckedChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(10, 42);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(150, 15);
            label2.TabIndex = 5;
            label2.Text = "Choose Output File Format";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(7, 145);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(86, 15);
            label1.TabIndex = 2;
            label1.Text = "Export Data To:";
            // 
            // file_output_format_CLB
            // 
            file_output_format_CLB.CheckOnClick = true;
            file_output_format_CLB.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            file_output_format_CLB.FormattingEnabled = true;
            file_output_format_CLB.Location = new System.Drawing.Point(10, 61);
            file_output_format_CLB.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            file_output_format_CLB.MultiColumn = true;
            file_output_format_CLB.Name = "file_output_format_CLB";
            file_output_format_CLB.Size = new System.Drawing.Size(419, 46);
            file_output_format_CLB.TabIndex = 3;
            file_output_format_CLB.SelectedIndexChanged += File_output_format_CLB_SelectedIndexChanged;
            // 
            // select_output_directory_button
            // 
            select_output_directory_button.Location = new System.Drawing.Point(341, 140);
            select_output_directory_button.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            select_output_directory_button.Name = "select_output_directory_button";
            select_output_directory_button.Size = new System.Drawing.Size(88, 27);
            select_output_directory_button.TabIndex = 1;
            select_output_directory_button.Text = "Browse";
            select_output_directory_button.UseVisualStyleBackColor = true;
            select_output_directory_button.Click += select_output_directory_button_Click;
            // 
            // export_folder_maskedTB
            // 
            export_folder_maskedTB.Location = new System.Drawing.Point(10, 173);
            export_folder_maskedTB.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            export_folder_maskedTB.Name = "export_folder_maskedTB";
            export_folder_maskedTB.ReadOnly = true;
            export_folder_maskedTB.Size = new System.Drawing.Size(416, 23);
            export_folder_maskedTB.TabIndex = 0;
            // 
            // monocle_log_tb
            // 
            monocle_log_tb.Controls.Add(monocle_log);
            monocle_log_tb.Location = new System.Drawing.Point(15, 630);
            monocle_log_tb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            monocle_log_tb.Name = "monocle_log_tb";
            monocle_log_tb.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            monocle_log_tb.Size = new System.Drawing.Size(866, 192);
            monocle_log_tb.TabIndex = 3;
            monocle_log_tb.TabStop = false;
            monocle_log_tb.Text = "Monocle Log";
            // 
            // monocle_log
            // 
            monocle_log.Location = new System.Drawing.Point(8, 23);
            monocle_log.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            monocle_log.Name = "monocle_log";
            monocle_log.ReadOnly = true;
            monocle_log.Size = new System.Drawing.Size(850, 161);
            monocle_log.TabIndex = 0;
            monocle_log.Text = "";
            // 
            // progressBar1
            // 
            progressBar1.Location = new System.Drawing.Point(23, 528);
            progressBar1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new System.Drawing.Size(748, 27);
            progressBar1.TabIndex = 4;
            // 
            // start_monocle_button
            // 
            start_monocle_button.Location = new System.Drawing.Point(793, 528);
            start_monocle_button.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            start_monocle_button.Name = "start_monocle_button";
            start_monocle_button.Size = new System.Drawing.Size(88, 27);
            start_monocle_button.TabIndex = 5;
            start_monocle_button.Text = "Run!";
            start_monocle_button.UseVisualStyleBackColor = true;
            start_monocle_button.Click += Start_monocle_button_Click;
            // 
            // input_file_dialog
            // 
            input_file_dialog.FileName = "Input";
            input_file_dialog.Multiselect = true;
            // 
            // log_toggle_checkbox
            // 
            log_toggle_checkbox.Appearance = System.Windows.Forms.Appearance.Button;
            log_toggle_checkbox.AutoSize = true;
            log_toggle_checkbox.Location = new System.Drawing.Point(23, 562);
            log_toggle_checkbox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            log_toggle_checkbox.Name = "log_toggle_checkbox";
            log_toggle_checkbox.Size = new System.Drawing.Size(110, 25);
            log_toggle_checkbox.TabIndex = 6;
            log_toggle_checkbox.Text = "Show Output Log";
            log_toggle_checkbox.UseVisualStyleBackColor = true;
            log_toggle_checkbox.CheckedChanged += Log_toggle_checkbox_CheckedChanged;
            // 
            // monocleOptionsBox
            // 
            monocleOptionsBox.Controls.Add(MonocleOptionsDGV);
            monocleOptionsBox.Location = new System.Drawing.Point(447, 32);
            monocleOptionsBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            monocleOptionsBox.Name = "monocleOptionsBox";
            monocleOptionsBox.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            monocleOptionsBox.Size = new System.Drawing.Size(435, 276);
            monocleOptionsBox.TabIndex = 7;
            monocleOptionsBox.TabStop = false;
            monocleOptionsBox.Text = "Monocle Options";
            mainToolTip.SetToolTip(monocleOptionsBox, "Enter Monocle Options, improperly formatted options will be ignored.");
            // 
            // MonocleOptionsDGV
            // 
            MonocleOptionsDGV.AllowUserToAddRows = false;
            MonocleOptionsDGV.AllowUserToDeleteRows = false;
            MonocleOptionsDGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            MonocleOptionsDGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            MonocleOptionsDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            MonocleOptionsDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { Options, Value, Description });
            MonocleOptionsDGV.Dock = System.Windows.Forms.DockStyle.Fill;
            MonocleOptionsDGV.Location = new System.Drawing.Point(4, 19);
            MonocleOptionsDGV.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MonocleOptionsDGV.Name = "MonocleOptionsDGV";
            MonocleOptionsDGV.RowHeadersVisible = false;
            MonocleOptionsDGV.Size = new System.Drawing.Size(427, 254);
            MonocleOptionsDGV.TabIndex = 1;
            // 
            // Options
            // 
            Options.HeaderText = "Options";
            Options.Name = "Options";
            Options.ReadOnly = true;
            // 
            // Value
            // 
            Value.HeaderText = "Value";
            Value.Name = "Value";
            // 
            // Description
            // 
            Description.HeaderText = "Description";
            Description.Name = "Description";
            Description.ReadOnly = true;
            // 
            // cancelButton
            // 
            cancelButton.Location = new System.Drawing.Point(793, 562);
            cancelButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(88, 27);
            cancelButton.TabIndex = 8;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += cancelButton_Click;
            // 
            // MonocleUI
            // 
            AllowDrop = true;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new System.Drawing.Size(895, 835);
            Controls.Add(monocleOptionsBox);
            Controls.Add(cancelButton);
            Controls.Add(log_toggle_checkbox);
            Controls.Add(start_monocle_button);
            Controls.Add(progressBar1);
            Controls.Add(monocle_log_tb);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(mainMenuStrip);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MainMenuStrip = mainMenuStrip;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            Name = "MonocleUI";
            ShowIcon = false;
            Text = "Monocle";
            mainMenuStrip.ResumeLayout(false);
            mainMenuStrip.PerformLayout();
            groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)input_files_dgv).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            monocle_log_tb.ResumeLayout(false);
            monocleOptionsBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)MonocleOptionsDGV).EndInit();
            ResumeLayout(false);
            PerformLayout();
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

