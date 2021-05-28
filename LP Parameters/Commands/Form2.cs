using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LP_Parameters.Commands
{
    public partial class Form2 : Form
    {
        public Form2(int matrSize)
        {
            InitializeComponent();
            //int counter = 0;
            //TextBox[] MatrixNodes = new TextBox[matrSize * matrSize];
            //for (int i = 0; i < matrSize; i++)
            //{
            //    Random r = new Random();
            //    for (int j = 0; j < matrSize; j++)
            //    {
            //        var tb = new TextBox();                    
            //        int num = r.Next(1, 1000);
            //        MatrixNodes[counter] = tb;
            //        tb.Name = "Node_" + MatrixNodes[counter];
            //        tb.Text = num.ToString();
            //        tb.Location = new Point(172 + (j * 28), 32 + (i * 28));
            //        tb.Visible = true;
            //        this.Controls.Add(tb);
            //        counter++;
            //    }
            //}
            //Debug.Write(counter);
        }

        public void SetBtn_Click(object sender, EventArgs e)
        {
            int n = int.Parse(textBox1.Text.ToString());
            for (int i = 0; i < n; i++)
            {
                flowLayoutPanel1.Controls.Add(btn(i));
                flowLayoutPanel1.Controls.Add(btn1(i));
                flowLayoutPanel1.Controls.Add(btn2(i));
            }


        }

        Button btn(int i)
        {
            Button b = new Button();
            b.Name = i.ToString();
            b.Width = flowLayoutPanel1.Size.Width / 3;
            //b.Width = 60;            
            b.Height = 60;
            b.Text = i.ToString();
            //b.Click += SetBtn_Click;
            return b;
        }

        Button btn1(int i)
        {
            Button b = new Button();
            b.Name = i.ToString();
            b.Width = flowLayoutPanel1.Size.Width / 3;
            b.Height = 60;
            b.Text = i.ToString();
            return b;
        }

        Button btn2(int i)
        {
            Button b = new Button();
            b.Name = i.ToString();
            //b.Width = flowLayoutPanel1.Size.Width/3;
            b.Width = flowLayoutPanel1.Size.Width / 3;
            b.Height = 60;
            b.Text = i.ToString();
            //b.Click += SetBtn_Click;
            flowLayoutPanel1.SetFlowBreak(b, true);
            return b;
        }

        void b_Click (object sender, EventArgs e)
        {
            Button b = (Button)sender;
            MessageBox.Show(b.Name.ToString());
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
