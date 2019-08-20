using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace MonocleUI
{
    public partial class MonocleUI : Form
    {
        Files InputFiles = new Files();

        public MonocleUI()
        {
            InitializeComponent();
            input_rtb.AllowDrop = true;
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

        private void Input_rtb_TextChanged(object sender, EventArgs e)
        {

        }

        private void Input_rtb_DragEnter(object sender, DragEventArgs e)
        {
            Debug.WriteLine("ENTER" + e.Data.GetDataPresent(DataFormats.FileDrop));
        }

        private void Input_rtb_DragDrop(object sender, DragEventArgs e)
        {
            string[] fileArray;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                fileArray = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string filePath in fileArray)
                {
                    Debug.WriteLine(filePath);
                    if (InputFiles.Add(filePath))
                    {
                        input_rtb.AppendText(filePath + Environment.NewLine);
                    }
                }
            }
        }
    }
}
