namespace MonocleUI.ext
{
    partial class MonocleOptionsForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.MonocleOptionsDGV = new System.Windows.Forms.DataGridView();
            this.SaveMonocleOptionsButton = new System.Windows.Forms.Button();
            this.CancelMonocleOptionsButton = new System.Windows.Forms.Button();
            this.Options = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.MonocleOptionsDGV)).BeginInit();
            this.SuspendLayout();
            // 
            // MonocleOptionsDGV
            // 
            this.MonocleOptionsDGV.AllowUserToAddRows = false;
            this.MonocleOptionsDGV.AllowUserToDeleteRows = false;
            this.MonocleOptionsDGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.MonocleOptionsDGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.MonocleOptionsDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MonocleOptionsDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Options,
            this.Value,
            this.Description});
            this.MonocleOptionsDGV.Dock = System.Windows.Forms.DockStyle.Top;
            this.MonocleOptionsDGV.Location = new System.Drawing.Point(0, 0);
            this.MonocleOptionsDGV.Name = "MonocleOptionsDGV";
            this.MonocleOptionsDGV.RowHeadersVisible = false;
            this.MonocleOptionsDGV.Size = new System.Drawing.Size(413, 400);
            this.MonocleOptionsDGV.TabIndex = 0;
            // 
            // SaveMonocleOptionsButton
            // 
            this.SaveMonocleOptionsButton.Location = new System.Drawing.Point(245, 415);
            this.SaveMonocleOptionsButton.Name = "SaveMonocleOptionsButton";
            this.SaveMonocleOptionsButton.Size = new System.Drawing.Size(75, 23);
            this.SaveMonocleOptionsButton.TabIndex = 1;
            this.SaveMonocleOptionsButton.Text = "Save";
            this.SaveMonocleOptionsButton.UseVisualStyleBackColor = true;
            // 
            // CancelMonocleOptionsButton
            // 
            this.CancelMonocleOptionsButton.Location = new System.Drawing.Point(326, 415);
            this.CancelMonocleOptionsButton.Name = "CancelMonocleOptionsButton";
            this.CancelMonocleOptionsButton.Size = new System.Drawing.Size(75, 23);
            this.CancelMonocleOptionsButton.TabIndex = 2;
            this.CancelMonocleOptionsButton.Text = "Cancel";
            this.CancelMonocleOptionsButton.UseVisualStyleBackColor = true;
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
            // MonocleOptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(413, 450);
            this.Controls.Add(this.CancelMonocleOptionsButton);
            this.Controls.Add(this.SaveMonocleOptionsButton);
            this.Controls.Add(this.MonocleOptionsDGV);
            this.Name = "MonocleOptionsForm";
            this.Text = "MonocleOptions";
            ((System.ComponentModel.ISupportInitialize)(this.MonocleOptionsDGV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView MonocleOptionsDGV;
        private System.Windows.Forms.Button SaveMonocleOptionsButton;
        private System.Windows.Forms.Button CancelMonocleOptionsButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn Options;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
    }
}