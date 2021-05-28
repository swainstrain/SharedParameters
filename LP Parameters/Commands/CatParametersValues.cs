using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.ApplicationServices;
using LP_Parameters.Commands;

namespace LP_Parameters
{
    [TransactionAttribute(TransactionMode.Manual)]
    public class CatParametersValues : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get Document
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;


            Categories cats = doc.Settings.Categories;
            List<String> catsel = new List<String>();
            List<String> parametersname = new List<String>();
            List<String> parametersvalue = new List<String>();
            List<String> groupssname = new List<String>();
            List<bool> instancetype = new List<bool>();


            using (CatParametersValues_Form form = new CatParametersValues_Form(doc))
            {
                form.ShowDialog();
                if (form.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                {
                    return Result.Cancelled;
                }

                catsel = form.getselected();
                parametersname = form.getParameterName();
                parametersvalue = form.getParameterValue();
                groupssname = form.getGroupName();
                instancetype = form.instancetype();
            }

            //string pn ="";

            //foreach (bool s in instancetype)
            //{
            //    pn += s.ToString() + Environment.NewLine;
            //}

            //TaskDialog.Show("Title", pn);



            Application revitApp = commandData.Application.Application;
            CategorySet categoryset = revitApp.Create.NewCategorySet();

            foreach (string c in catsel)
            {
                foreach (Category cat in cats)
                {
                    if (cat.Name == c)
                    {
                        categoryset.Insert(cat);
                    }
                }
            }


            try
            {
                using (Transaction trans = new Transaction(doc, "Add Parameter"))

                {
                    DefinitionFile parafile = revitApp.OpenSharedParameterFile();

                    List<String> definitions = new List<String>();

                    Definition sharedParamDef = null;

                    //DefinitionGroup group = null;


                    trans.Start();

                    for (int i = 0; i < parametersname.Count; i++)
                    {                                              

                        foreach (DefinitionGroup group in parafile.Groups ) 
                        {
                                foreach (Definition def in group.Definitions) if (def.Name == parametersname[i])
                                {
                                    sharedParamDef = group.Definitions.get_Item(parametersname[i]);
                                }
                        }

                        //string s = sharedParamDef.Name.ToString();

                        //TaskDialog.Show("Title", s);

                        foreach (Category cat in categoryset)
                        {

                            ElementId catid = cat.Id;

                            FilteredElementCollector collector = new FilteredElementCollector(doc);

                            IList<Element> eles = collector
                                .OfCategoryId(catid)
                                .WhereElementIsNotElementType()
                                .ToElements();

                            foreach (Element ele in eles)

                            {
                                Parameter param = ele.get_Parameter(sharedParamDef);
                                param.Set(parametersvalue[i]);
                            }
                        }

                    }

                    //string pn = "";

                    //foreach (string s in definitions)
                    //{
                    //    pn += s.ToString() + Environment.NewLine;
                    //}


                    trans.Commit();
                }
            }

            catch (Exception ex)
            {
                throw new Exception("Failed to create shared parameter: " + ex.Message);
            }

            return Result.Succeeded;

        }


    }    
    
}


