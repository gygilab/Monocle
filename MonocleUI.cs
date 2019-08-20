using System;
using System.Windows.Forms;

namespace MonocleUI
{
    public partial class MonocleUI : Form
    {
        public MonocleUI()
        {
            InitializeComponent();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if(input_file_dialog.ShowDialog() == DialogResult.OK)
            {
                foreach(string file in input_file_dialog.FileNames)
                {
                    input_rtb.AppendText(file + Environment.NewLine);
                }
            }
        }
    }
}
