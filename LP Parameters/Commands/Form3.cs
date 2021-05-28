using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using System.Windows.Forms;
using Form = System.Windows.Forms.Form;

namespace LP_Parameters.Commands
{
    
    public partial class Form3 : Form
    {
        Document _doc;

        public Form3(Document doc)
        {
            InitializeComponent();
            _doc = doc;
        }  
        
        TextBox btn(int i)
        {
            TextBox b = new TextBox();
            b.Name = i.ToString();
            //b.Width = groupBox1.Size.Width / 3;
            ////b.Width = 60;            
            //b.Height = 60;
            b.Text = i.ToString();
            b.Size = new Size(250, 20);
            b.Anchor = (AnchorStyles.Left | AnchorStyles.Right);
            //b.Click += SetBtn_Click;
            return b;
        }

        ComboBox btn1(int i)
        {
            ComboBox b = new ComboBox();
            b.Name = i.ToString();
            b.Text = i.ToString();
            b.Size = new Size(250, 20);
            b.Anchor = (AnchorStyles.Left | AnchorStyles.Right);

            List<ViewPlan> views = new List<ViewPlan>(new FilteredElementCollector(_doc)
                .OfClass(typeof(ViewPlan))
                .Cast<ViewPlan>()
                .Where<ViewPlan>(v => v.CanBePrinted
                 && ViewType.FloorPlan == v.ViewType));


            Categories cat = _doc.Settings.Categories;
            List<String> myCategories = new List<String>();
            //SortedList<string, Category> myCategories = new SortedList<string, Category>();
            foreach (Category c in cat)
            {
                if (c.AllowsBoundParameters)
                    myCategories.Add(c.Name);
                myCategories.Sort();
            }

            b.DataSource = myCategories;
            b.DisplayMember = "Name";


            return b;
        }
                

        TextBox btn2(int i)
        {
            TextBox b = new TextBox();
            b.Name = i.ToString();
            //b.Width = flowLayoutPanel1.Size.Width/3;
            //b.Width = groupBox1.Size.Width / 3;
            //b.Height = 60;
            b.Text = i.ToString();
            //b.Click += SetBtn_Click;
            b.Size = new Size(250, 20);
            b.Anchor = (AnchorStyles.Left | AnchorStyles.Right);
            flowLayoutPanel1.SetFlowBreak(b, true);
            return b;
        }

        private void SetBtn_Click_1(object sender, EventArgs e)
        {
            int n = int.Parse(textBox1.Text.ToString());
            for (int i = 0; i < n; i++)
            {
                flowLayoutPanel1.Controls.Add(btn(i));
                btn(i).Anchor = (AnchorStyles.Left | AnchorStyles.Right);
                flowLayoutPanel1.Controls.Add(btn1(i));
                flowLayoutPanel1.Controls.Add(btn2(i));
            }
        }
    }
}
