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
            this.MonocleOptionsDGV = new System.Windows.Forms.DataGridView();
            this.SaveMonocleOptionsButton = new System.Windows.Forms.Button();
            this.CancelMonocleOptionsButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.MonocleOptionsDGV)).BeginInit();
            this.SuspendLayout();
            // 
            // MonocleOptionsDGV
            // 
            this.MonocleOptionsDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MonocleOptionsDGV.Dock = System.Windows.Forms.DockStyle.Top;
            this.MonocleOptionsDGV.Location = new System.Drawing.Point(0, 0);
            this.MonocleOptionsDGV.Name = "MonocleOptionsDGV";
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
    }
}