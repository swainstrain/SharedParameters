using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Windows.Forms;
using Form = System.Windows.Forms.Form;
using ComboBox = System.Windows.Forms.ComboBox;
using TextBox = System.Windows.Forms.TextBox;
using Control = System.Windows.Forms.Control;
using Point = System.Drawing.Point;

namespace LP_Parameters.Commands
{
    public partial class CatParameters_Form : Form
    {
        Document _doc;


        public CatParameters_Form(Document doc) 
        {
            InitializeComponent();

            Screen screen = Screen.FromPoint(Cursor.Position);
            this.Location = screen.Bounds.Location;


            _doc = doc;

            //Categories

            Categories cat = _doc.Settings.Categories;
            List<String> myCategories = new List<String>();
            foreach (Category c in cat)
            {
                if (c.AllowsBoundParameters)
                    myCategories.Add(c.Name);
            }

            myCategories.Sort();
            checkedListBox1.DataSource = myCategories;
            checkedListBox1.DisplayMember = "Name";

            //All Shared Parameters

            IEnumerable<ParameterElement> allParameters = new FilteredElementCollector(_doc)
                 .OfClass(typeof(ParameterElement))
                 .Cast<ParameterElement>()
                 .Where(x => x is SharedParameterElement);

            //All Parameter Types & Groups

            FilteredElementCollector collectorUsed = new FilteredElementCollector(_doc);
            collectorUsed.OfClass(typeof(Family));

            //ComboBox

            TextBox tbx1(int i) //Parameter Name
            {
                TextBox b = new TextBox();
                b.Name = "tbx1"+i.ToString();
                //b.Width = groupBox1.Size.Width / 3;
                ////b.Width = 60;            
                //b.Height = 60;
                b.Text = "Parameter Name";
                b.Size = new Size(200, 30);
                b.Anchor = (AnchorStyles.Left | AnchorStyles.Right);
                //b.Click += SetBtn_Click;
                return b;
            } //Parameter Name

            TextBox tbx2(int i) //Parameter Group
            {
                TextBox b = new TextBox();
                b.Name = "tbx2" + i.ToString();
                //b.Width = groupBox1.Size.Width / 3;
                ////b.Width = 60;            
                //b.Height = 60;
                b.Text = "Parameter Group";
                b.Size = new Size(200, 30);
                b.Anchor = (AnchorStyles.Left | AnchorStyles.Right);
                //b.Click += SetBtn_Click;
                return b;
            } //Parameter Group (Create)

            ComboBox cmbox1(int i) //Parameter Name
            {
                ComboBox b = new ComboBox();
                b.Name = "cmbox1";
                //b.Text = i.ToString();
                b.Size = new Size(180, 20);
                b.Anchor = (AnchorStyles.Left | AnchorStyles.Right);

                List<String> parameters = new List<String>();

                foreach (var p in allParameters)
                {
                    //parameters.AppendLine(string.Format("<{0}> {1}", p.Id, p.GetDefinition().Name));
                    parameters.Add(p.GetDefinition().Name);
                }
                parameters.Sort();
                b.DataSource = parameters;
                b.DisplayMember = "Name";
                return b;
            } //NIU

            ComboBox cmbox2(int i) //Parameter Type
            {
                ComboBox b = new ComboBox();
                b.Name = "cmbox2";
                //b.Text = i.ToString();
                b.Size = new Size(180, 20);
                b.Anchor = (AnchorStyles.Left | AnchorStyles.Right);

                List<String> parametertypes = new List<String>();

                foreach (Family family in collectorUsed)
                {
                    if (family.IsEditable)
                    {
                        Document familyDocument = _doc.EditFamily(family);
                        FamilyManager familyManager = familyDocument.FamilyManager;

                        foreach (ParameterType item in (ParameterType[])Enum.GetValues(typeof(ParameterType)))
                        {
                            string type = item.ToString();
                            if (parametertypes.Contains(type))
                            {
                            }
                            else
                            {
                                parametertypes.Add(type);
                            }
                        }
                    }
                }
                parametertypes.Sort();
                b.DataSource = parametertypes;
                b.DisplayMember = "Name";
                return b;
            } //Parameter Type

            ComboBox cmbox3(int i) //Parameter Group
            {
                ComboBox b = new ComboBox();
                b.Name = "cmbox3";
                //b.Text = i.ToString();
                b.Size = new Size(180, 20);
                b.Anchor = (AnchorStyles.Left | AnchorStyles.Right);

                List<String> parametergroups = new List<String>();


                foreach (Family family in collectorUsed)
                {
                    if (family.IsEditable)
                    {
                        Document familyDocument = _doc.EditFamily(family);
                        FamilyManager familyManager = familyDocument.FamilyManager;

                        foreach (BuiltInParameterGroup item in (BuiltInParameterGroup[])Enum.GetValues(typeof(BuiltInParameterGroup)))
                        {
                            if (familyManager.IsUserAssignableParameterGroup(item))
                            {

                                string group = LabelUtils.GetLabelFor(item);
                                if (parametergroups.Contains(group))
                                {
                                }
                                else
                                {
                                    parametergroups.Add(group);
                                }
                            }
                        }
                    }
                }
                parametergroups.Sort();
                b.DataSource = parametergroups;
                b.DisplayMember = "Name";
                flowLayoutPanel1.SetFlowBreak(b, true);
                return b;
            } //Parameter Group (Insert)

            CheckBox ccbox1(int i) //Parameter Group
            {
                CheckBox b = new CheckBox();
                b.Name = "ccbox1" + i.ToString();
                b.Text = "Instance parameter if checked";
                b.Size = new Size(200, 30);
                b.Anchor = (AnchorStyles.Left | AnchorStyles.Right);

                flowLayoutPanel1.SetFlowBreak(b, true);
                return b;
            } //CheckBox Instance/Type


            //int n = int.Parse(textBox1.Text.ToString());
            int n = 1;

            for (int i = 0; i < n; i++)
            {
                flowLayoutPanel1.Controls.Add(tbx1(i));
                flowLayoutPanel1.Controls.Add(tbx2(i));
                flowLayoutPanel1.Controls.Add(ccbox1(i));
            }
        }

        public List<string> getParameterName()

        {
            List<String> parametersname = new List<String>();

            foreach (Control control in flowLayoutPanel1.Controls.OfType<Control>())
            {
                if (control.Name.Contains("tbx1"))
                {
                    parametersname.Add(control.Text);
                }
            }
                return parametersname;
        }
        public List<bool> instancetype()
        {
            List<bool> instancetype = new List<bool>();

            foreach (CheckBox check in flowLayoutPanel1.Controls.OfType<CheckBox>())
            {
                //if (control.Name.Contains("ccbox1"))
                //{
                    if (check.Checked)
                    {
                        instancetype.Add(true);
                    }
                    else
                    {
                        instancetype.Add(false);
                    }
                //}
            }
            return instancetype;
        }
        public List<string> getGroupName()

        {
            List<String> groupsname = new List<String>();

            foreach (Control control in flowLayoutPanel1.Controls.OfType<Control>())
            {
                if (control.Name.Contains("tbx2"))
                {
                    groupsname.Add(control.Text);
                }
            }

            return groupsname;

        }
        public List<string> getselected()
        {
            List<String> catselected = new List<String>();
            foreach (object itemChecked in checkedListBox1.CheckedItems)
            {
                catselected.Add(itemChecked.ToString());
            }

            return catselected;
        }
        private void OKbtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }
        private void Cancelbtn_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, true);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, false);
            }
        }
        private void PlusBtn_Click(object sender, EventArgs e)
        {

            TextBox tbx1(int i) //Parameter Name
            {
                TextBox b = new TextBox();
                b.Name = "tbx1" + i.ToString();
                //b.Width = groupBox1.Size.Width / 3;
                ////b.Width = 60;            
                //b.Height = 60;
                b.Text = "Parameter Name";
                b.Size = new Size(200, 30);
                b.Anchor = (AnchorStyles.Left | AnchorStyles.Right);
                //b.Click += SetBtn_Click;
                return b;
            } //Parameter Name

            TextBox tbx2(int i) //Parameter Group
            {
                TextBox b = new TextBox();
                b.Name = "tbx2" + i.ToString();
                //b.Width = groupBox1.Size.Width / 3;
                ////b.Width = 60;            
                //b.Height = 60;
                b.Text = "Parameter Group";
                b.Size = new Size(200, 30);
                b.Anchor = (AnchorStyles.Left | AnchorStyles.Right);
                //b.Click += SetBtn_Click;
                return b;
            } //Parameter Group (Create)

            CheckBox ccbox1(int i) //Parameter Group
            {
                CheckBox b = new CheckBox();
                b.Name = "ccbox1" + i.ToString();
                b.Text = "Instance parameter if checked";
                b.Size = new Size(200, 30);
                b.Anchor = (AnchorStyles.Left | AnchorStyles.Right);

                flowLayoutPanel1.SetFlowBreak(b, true);
                return b;
            } //CheckBox Instance/Type


            int n = flowLayoutPanel1.Controls.Count / 2;

            flowLayoutPanel1.Controls.Add(tbx1(n));
            flowLayoutPanel1.Controls.Add(tbx2(n));
            flowLayoutPanel1.Controls.Add(ccbox1(n));

        }
        private void MinusBtn_Click(object sender, EventArgs e)
        {
            int n = flowLayoutPanel1.Controls.Count - 1;
            int m = flowLayoutPanel1.Controls.Count - 2;
            int o = flowLayoutPanel1.Controls.Count - 3;

            Control last = flowLayoutPanel1.Controls.OfType<Control>().ElementAt(n);
            Control secondlast = flowLayoutPanel1.Controls.OfType<Control>().ElementAt(m);
            Control thirdlast = flowLayoutPanel1.Controls.OfType<Control>().ElementAt(o);

            flowLayoutPanel1.Controls.Remove(last);
            flowLayoutPanel1.Controls.Remove(secondlast);
            flowLayoutPanel1.Controls.Remove(thirdlast);

        }
        //private void MainForm_Load(object sender, EventArgs e)
        //{
        //    Form frm = new Form();
        //    frm.MdiParent = this;
        //    frm.Show();
        //}
    }
}
