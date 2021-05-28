using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComharDesign.Command
{
    public partial class ExcelImport_Form : Form
    {
        public ExcelImport_Form()
        {
            InitializeComponent();
        }

        
        private void browserbtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string strfilename = openFileDialog1.FileName;
                xlstbox.Text = strfilename;
            }
        }

        public string getExcel()
        {
            string s;
            s = xlstbox.Text;
            return s;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
