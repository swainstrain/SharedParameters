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
    public class CatParameters : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get Document
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;


            Categories cats = doc.Settings.Categories;
            List<String> catsel = new List<String>();
            List<String> parametersname = new List<String>();
            List<String> groupssname = new List<String>();
            List<bool> instancetype = new List<bool>();


            using (CatParameters_Form form = new CatParameters_Form(doc))
            {
                form.ShowDialog();
                if (form.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                {
                    return Result.Cancelled;
                }

                catsel = form.getselected();
                parametersname = form.getParameterName();
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
                    
                    //String modulePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    //String paramFile = modulePath + "\\Prova.txt";
                    //if (File.Exists(paramFile))
                    //{
                    //    File.Delete(paramFile);
                    //}
                    //FileStream fs = File.Create(paramFile);
                    //fs.Close();

                    DefinitionFile parafile = revitApp.OpenSharedParameterFile();

                    
                    //

                    List<String> definitions = new List<String>();


                    trans.Start();
                    for (int i = 0; i < parametersname.Count; i++)
                    {
                        

                        DefinitionGroup group = parafile.Groups.get_Item(groupssname[i]);

                        if (group is null)
                        {
                            group = parafile.Groups.Create(groupssname[i]);
                        }
                        
                        

                        if (group.Definitions.Count() ==0 )
                        {
                            ExternalDefinitionCreationOptions opts = new ExternalDefinitionCreationOptions(parametersname[i], ParameterType.Text);

                            Definition sharedParamDef = group.Definitions.Create(opts);

                            if (instancetype[i] is true)
                            {
                                InstanceBinding binding = revitApp.Create.NewInstanceBinding(categoryset);
                                commandData.Application.ActiveUIDocument.Document.ParameterBindings.Insert(sharedParamDef, binding);
                            }
                            else
                            {
                                TypeBinding binding = revitApp.Create.NewTypeBinding(categoryset);
                                commandData.Application.ActiveUIDocument.Document.ParameterBindings.Insert(sharedParamDef, binding);
                            }

                            
                        }
                        else
                        {

                            foreach (Definition def in group.Definitions)
                            {
                                definitions.Add(def.Name);
                            }

                            if (definitions.Contains(parametersname[i]))
                            {
                                Definition sharedParamDef = group.Definitions.get_Item(parametersname[i]);

                                if (instancetype[i] is true)
                                {
                                    InstanceBinding binding = revitApp.Create.NewInstanceBinding(categoryset);
                                    commandData.Application.ActiveUIDocument.Document.ParameterBindings.Insert(sharedParamDef, binding);
                                }
                                else
                                {
                                    TypeBinding binding = revitApp.Create.NewTypeBinding(categoryset);
                                    commandData.Application.ActiveUIDocument.Document.ParameterBindings.Insert(sharedParamDef, binding);
                                }
                                
                            }
                            else
                            {
                                ExternalDefinitionCreationOptions opts = new ExternalDefinitionCreationOptions(parametersname[i], ParameterType.Text);
                                Definition sharedParamDef = group.Definitions.Create(opts);

                                if (instancetype[i] is true)
                                {
                                    InstanceBinding binding = revitApp.Create.NewInstanceBinding(categoryset);
                                    commandData.Application.ActiveUIDocument.Document.ParameterBindings.Insert(sharedParamDef, binding);
                                }
                                else
                                {
                                    TypeBinding binding = revitApp.Create.NewTypeBinding(categoryset);
                                    commandData.Application.ActiveUIDocument.Document.ParameterBindings.Insert(sharedParamDef, binding);
                                }

                            }
                            definitions.Clear();
                        }
                        
                    }
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


