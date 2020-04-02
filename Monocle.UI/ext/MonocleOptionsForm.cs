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
            PropertyInfo[] myPropertyInfo;
            // Get the properties of 'Type' class object.
            myPropertyInfo = FileProcessor.monocleOptions.GetType().GetProperties();
            Console.WriteLine("Properties of System.Type are:");

            for (int i = 0; i < myPropertyInfo.Length; i++)
            {
                MonocleOptionsDGV.Rows.Add(myPropertyInfo[i].Name.ToString());
            }
        }

        public void InitializeOptionsTable()
        {
            MonocleOptionsDGV.ColumnCount = 3;
            MonocleOptionsDGV.Columns[0].Name = "Options";
        }
    }
}
