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

namespace LP_Parameters.Commands
{
    public partial class Form1 : Form
    {
        Document _doc;               

        public Form1(Document doc)
        {
            InitializeComponent();
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

            ComboBox cmbox1(int i) //Parameter Name
            {
                ComboBox b = new ComboBox();
                b.Name = i.ToString();
                //b.Text = i.ToString();
                b.Size = new Size(250, 20);
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
            }

            ComboBox cmbox2(int i) //Parameter Type
            {
                ComboBox b = new ComboBox();
                b.Name = i.ToString();
                //b.Text = i.ToString();
                b.Size = new Size(250, 20);
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
            }

            ComboBox cmbox3(int i) //Parameter Group
            {
                ComboBox b = new ComboBox();
                b.Name = i.ToString();
                //b.Text = i.ToString();
                b.Size = new Size(250, 20);
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
            }

            //int n = int.Parse(textBox1.Text.ToString());
            for (int i = 0; i < allParameters.Count(); i++)
            {
                flowLayoutPanel1.Controls.Add(cmbox1(i));
                //cmbox1(i).Click += new EventHandler(OKbtn_Click);
                //btn(i).Anchor = (AnchorStyles.Left | AnchorStyles.Right);
                flowLayoutPanel1.Controls.Add(cmbox2(i));
                flowLayoutPanel1.Controls.Add(cmbox3(i));
            }
        }

        //public string getParameterName()
        //{
        //    string s;
        //    s = comboBox1.Text;
        //    return s;
        //}

        public List<string> getselected()
        {
            List<String> catselected = new List<String>();

            //foreach (int indexChecked in checkedListBox1.CheckedIndices)
            //{
            //    // The indexChecked variable contains the index of the item.
            //    MessageBox.Show("Index#: " + indexChecked.ToString() + ", is checked. Checked state is:" +
            //                    checkedListBox1.GetItemCheckState(indexChecked).ToString() + ".");
            //}

            foreach (object itemChecked in checkedListBox1.CheckedItems)
            {
                // Use the IndexOf method to get the index of an item.
                catselected.Add(itemChecked.ToString());

                //MessageBox.Show("Item with title: \"" + itemChecked.ToString() +
                //                "\", is checked. Checked state is: " +
                //                checkedListBox1.GetItemCheckState(checkedListBox1.Items.IndexOf(itemChecked)).ToString() + ".");
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
        
    }
}
