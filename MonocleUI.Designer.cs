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
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.file_output_format_CLB = new System.Windows.Forms.CheckedListBox();
            this.button2 = new System.Windows.Forms.Button();
            this.export_folder_maskedTB = new System.Windows.Forms.MaskedTextBox();
            this.monocle_log_tb = new System.Windows.Forms.GroupBox();
            this.monocle_log = new System.Windows.Forms.RichTextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.start_monocle_button = new System.Windows.Forms.Button();
            this.input_file_dialog = new System.Windows.Forms.OpenFileDialog();
            this.export_folder_dialog = new System.Windows.Forms.FolderBrowserDialog();
            this.log_toggle_checkbox = new System.Windows.Forms.CheckBox();
            this.mainToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.mainMenuStrip.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.input_files_dgv)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.monocle_log_tb.SuspendLayout();
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
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
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
            this.gCCollectionToolStripMenuItem.Click += new System.EventHandler(this.GCCollectionToolStripMenuItem_Click);
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
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.button3);
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
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(704, 54);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(32, 32);
            this.button4.TabIndex = 4;
            this.button4.Text = "-";
            this.mainToolTip.SetToolTip(this.button4, "Remove selected row(s).");
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.Button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(704, 16);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(32, 32);
            this.button3.TabIndex = 3;
            this.button3.Text = "+";
            this.mainToolTip.SetToolTip(this.button3, "Drag and drop files or brows with the \"+\" button.");
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Button3_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.file_output_format_CLB);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.export_folder_maskedTB);
            this.groupBox2.Location = new System.Drawing.Point(13, 292);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(742, 152);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Correction Export";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(132, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Choose Output File Format";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(183, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Export Data To:";
            // 
            // file_output_format_CLB
            // 
            this.file_output_format_CLB.CheckOnClick = true;
            this.file_output_format_CLB.FormattingEnabled = true;
            this.file_output_format_CLB.Location = new System.Drawing.Point(9, 35);
            this.file_output_format_CLB.Name = "file_output_format_CLB";
            this.file_output_format_CLB.Size = new System.Drawing.Size(159, 109);
            this.file_output_format_CLB.TabIndex = 3;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(650, 57);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Browse";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // export_folder_maskedTB
            // 
            this.export_folder_maskedTB.Location = new System.Drawing.Point(186, 81);
            this.export_folder_maskedTB.Name = "export_folder_maskedTB";
            this.export_folder_maskedTB.ReadOnly = true;
            this.export_folder_maskedTB.Size = new System.Drawing.Size(539, 20);
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
            this.progressBar1.Location = new System.Drawing.Point(19, 450);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(641, 23);
            this.progressBar1.TabIndex = 4;
            // 
            // start_monocle_button
            // 
            this.start_monocle_button.Location = new System.Drawing.Point(679, 450);
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
            this.log_toggle_checkbox.Location = new System.Drawing.Point(654, 491);
            this.log_toggle_checkbox.Name = "log_toggle_checkbox";
            this.log_toggle_checkbox.Size = new System.Drawing.Size(100, 23);
            this.log_toggle_checkbox.TabIndex = 6;
            this.log_toggle_checkbox.Text = "Show Output Log";
            this.log_toggle_checkbox.UseVisualStyleBackColor = true;
            this.log_toggle_checkbox.CheckedChanged += new System.EventHandler(this.Log_toggle_checkbox_CheckedChanged);
            // 
            // MonocleUI
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(767, 724);
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
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
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
    }
}

