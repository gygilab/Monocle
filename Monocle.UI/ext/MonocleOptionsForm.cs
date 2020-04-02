using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MonocleUI.ext
{
    public partial class MonocleOptionsForm : Form
    {
        public MonocleOptionsForm()
        {
            InitializeComponent();
            LoadOptions();
        }

        public void LoadOptions()
        {
            PropertyInfo[] propertyInfo = FileProcessor.monocleOptions.GetType().GetProperties();
            Console.WriteLine("Properties of System.Type are:");

            for (int i = 0; i < propertyInfo.Length; i++)
            {
                string[] newRow = new string[3]
                {
                    propertyInfo[i].Name,
                    propertyInfo[i].GetValue(FileProcessor.monocleOptions).ToString(),
                    propertyInfo[i].GetValue(FileProcessor.monocleOptions).ToString()
                };
                MonocleOptionsDGV.Rows.Add(newRow);
            }
        }
    }
}
