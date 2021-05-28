using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Architecture;
using ComharDesign.Command;
using X = Microsoft.Office.Interop.Excel;

namespace ComharDesign
{

    /// <summary>
    /// Export all parameters for each model 
    /// element to Excel, one sheet per category.
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class ExcelImport : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication app = commandData.Application;
            Document doc = app.ActiveUIDocument.Document;

            
            string path = "";

            using (ExcelImport_Form form = new ExcelImport_Form())
            {
                form.ShowDialog();
                if (form.DialogResult == DialogResult.Cancel)
                {
                    return Result.Cancelled;
                }
                path += form.getExcel();   //Excel File Path

            }

            //var directory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //string mySheet = Path.Combine(directory, "Maximo Revit Asset Information.xlsx");

            //string mySheet = @"C:\Users\Letizia\Google Drive\Upwork\Darran Blackbyrne\Maximo Revit Asset Information_prova.xlsx";
            X.Application xlApp = new X.Application();
            xlApp.Visible = true;

            if (xlApp == null)
            {
                MessageBox.Show("Excel is not properly installed");
            }

            X.Workbook xlWorkbook = xlApp.Workbooks.Open(path,
             0, false, 5, "", "", false, X.XlPlatform.xlWindows, "",
             true, false, 0, true, false, false);

            X._Worksheet sheet = (X._Worksheet)xlWorkbook.Sheets[1];
            X.Range range = sheet.UsedRange;

            int rowCount = range.Rows.Count;
            int colCount = range.Columns.Count;

            List<string> paramNames = new List<string>();

            //for (int i = 1; i <= rowCount; i++)
            //{
            for (int j = 1; j <= colCount; j++)
            {
                string value = ((X.Range)sheet.Cells[1, j]).Value;
                paramNames.Add(value);
            }
            //}


            string r = "AMG-Location";
            string rvalue = "*NA*";
            int xloc = 0;
            int yloc = 0;
            int x = 0;
            int y = 0;

            List<BuiltInCategory> builtInCats = new List<BuiltInCategory>();
            builtInCats.Add(BuiltInCategory.OST_DuctTerminal);
            builtInCats.Add(BuiltInCategory.OST_MechanicalEquipment);
            builtInCats.Add(BuiltInCategory.OST_DuctAccessory);

            ElementMulticategoryFilter filter = new ElementMulticategoryFilter(builtInCats);

            FilteredElementCollector collector = new FilteredElementCollector(doc, doc.ActiveView.Id)
                .WherePasses(filter);

            Stopwatch sw = Stopwatch.StartNew();

            foreach (Element e in collector)
            {
                ParameterSet parameters = e.Parameters;

                //find the value of AMG_Location parameter in the element
                foreach (Parameter p in parameters) if (p.Definition.Name.Equals(r))
                {                    
                    rvalue = XtraCs.LabUtils.GetParameterValue(p);
                    //MessageBox.Show(rvalue);
                }

                //if the AMG_Location parameter is populated search for the row and column

                if (rvalue != null)
                { 

                    X.Range header = sheet.UsedRange.Find(rvalue);
                    if (header != null)
                    {
                        // Header is found
                        xloc = header.Row;
                        yloc = header.Column; //it should be always 3
                        //MessageBox.Show(xloc.ToString() + yloc.ToString());
                    }

                    //for each parameter different from AMG_Location search the correspondent value an the Excel file and write it into the element

                    foreach (Parameter p in parameters) if (p.Definition.Name.Contains("AMG") && p.Definition.Name != r)
                    {

                        
                        X.Range param = sheet.UsedRange.Find(p.Definition.Name);

                            if (param != null)
                            {
                                y = param.Column;

                                string value = ((X.Range)sheet.Cells[xloc, y]).Text;
                                //if (value != double)
                                //{
                                    //MessageBox.Show(value);

                                    using (Transaction trans = new Transaction(doc, "Parameters"))
                                    {
                                        trans.Start();
                                        p.Set(value);
                                        trans.Commit();
                                    }
                                //}
                            }                     
                        

                    }

                    
                }

                
            }

            xlApp.Quit();

            sw.Stop();

            //TaskDialog.Show("Parameter Export",
            //  string.Format(
            //    "{0} categories and a total "
            //    + "of {1} elements exported "
            //    + "in {2:F2} seconds.",
            //    nCategories, nElements,
            //    sw.Elapsed.TotalSeconds));

            TaskDialog.Show("Parameter Import",
              string.Format(
                //"{0} categories and a total "
                //+ "of {1} elements exported "
                 "{0:F2} seconds.",
                //nCategories, nElements,
                sw.Elapsed.TotalSeconds));

            return Result.Succeeded;
        }
    }

}