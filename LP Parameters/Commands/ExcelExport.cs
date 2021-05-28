using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
    public class ExcelExport : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData,ref string message,ElementSet elements)
        {
            UIApplication app = commandData.Application;
            Document doc = app.ActiveUIDocument.Document;

            // Extract and group the data from Revit in a 
            // dictionary, where the key is the category 
            // name and the value is a list of elements.

            Stopwatch sw = Stopwatch.StartNew();

            Dictionary<string, List<Element>> sortedElements = new Dictionary<string, List<Element>>();

            // Iterate over all elements, both symbols and 
            // model elements, and them in the dictionary.


            List<BuiltInCategory> builtInCats = new List<BuiltInCategory>();
            builtInCats.Add(BuiltInCategory.OST_DuctTerminal);
            builtInCats.Add(BuiltInCategory.OST_MechanicalEquipment);
            builtInCats.Add(BuiltInCategory.OST_DuctAccessory);

            ElementMulticategoryFilter filter = new ElementMulticategoryFilter(builtInCats);

            FilteredElementCollector collector = new FilteredElementCollector(doc, doc.ActiveView.Id)
                .WherePasses(filter);

            string name;

            foreach (Element e in collector)
            {
                Category category = e.Category;

                if (null != category)
                {
                    name = category.Name;

                    // If this category was not yet encountered,
                    // add it and create a new container for its
                    // elements.

                    //if (!name.Contains("Fittings"))
                    //{

                        if (!sortedElements.ContainsKey(name))
                        {
                            sortedElements.Add(name, new List<Element>());
                        }
                        sortedElements[name].Add(e);
                    //}
                }
            }

            // Launch or access Excel via COM Interop:

            X.Application excel = new X.Application();

            if (null == excel)
            {
                XtraCs.LabUtils.ErrorMsg("Failed to get or start Excel.");

                return Result.Failed;
            }
            excel.Visible = true;

            X.Workbook workbook = excel.Workbooks.Add(Missing.Value);

            X.Worksheet worksheet;

            // We cannot delete all work sheets, 
            // Excel requires at least one.
            //
            //while( 1 < workbook.Sheets.Count ) 
            //{
            //  worksheet = workbook.Sheets.get_Item(1) as X.Worksheet;
            //  worksheet.Delete();
            //}

            // Loop through all collected categories and 
            // create a worksheet for each except the first.
            // We sort the categories and work trough them 
            // from the end, since the worksheet added last 
            // shows up first in the Excel tab.

            List<string> keys = new List<string>(sortedElements.Keys);

            keys.Sort();
            keys.Reverse();

            bool first = true;

            int nElements = 0;
            int nCategories = keys.Count;

            foreach (string categoryName in keys)
            {
                List<Element> elementSet = sortedElements[categoryName];

                // Create and name the worksheet

                if (first)
                {
                    worksheet = workbook.Sheets.get_Item(1)
                      as X.Worksheet;

                    first = false;
                }
                else
                {
                    worksheet = excel.Worksheets.Add(
                      Missing.Value, Missing.Value,
                      Missing.Value, Missing.Value)
                      as X.Worksheet;
                }

                name = (31 < categoryName.Length)
                  ? categoryName.Substring(0, 31)
                  : categoryName;

                name = name
                  .Replace(':', '_')
                  .Replace('/', '_');

                worksheet.Name = name;

                // Determine the names of all parameters 
                // defined for the elements in this set.

                List<string> paramNames = new List<string>();

                foreach (Element e in elementSet)
                {
                    ParameterSet parameters = e.Parameters;

                    foreach (Parameter parameter in parameters)
                    {
                        name = parameter.Definition.Name;

                        if (name.Contains("AMG") && !paramNames.Contains(name))
                        {
                            paramNames.Add(name);
                        }
                    }
                }
                paramNames.Sort();

                // Add the header row in bold.

                worksheet.Cells[1, 1] = "ID";
                worksheet.Cells[1, 2] = "IsType";

                int column = 3;

                foreach (string paramName in paramNames)
                {
                    worksheet.Cells[1, column] = paramName;
                    ++column;
                }
                var range = worksheet.get_Range("A1", "Z1");

                range.Font.Bold = true;
                range.EntireColumn.AutoFit();

                int row = 2;

                foreach (Element e in elementSet)
                {
                    // First column is the element id,
                    // second a flag indicating type (symbol)
                    // or not, both displayed as an integer.

                    worksheet.Cells[row, 1] = e.Id.IntegerValue;

                    worksheet.Cells[row, 2] = (e is ElementType)
                      ? 1
                      : 0;

                    column = 3;

                    string paramValue;

                    foreach (string paramName in paramNames)
                    {
                        paramValue = "*NA*";

                        IList<Parameter> plist = e.GetParameters(paramName);

                        foreach (Parameter p in plist)
                        {

                            if (null != p)
                            {
                                //try
                                //{
                                paramValue = XtraCs.LabUtils.GetParameterValue(p);
                                //}
                                //catch( Exception ex )
                                //{
                                //  Debug.Print( ex.Message );
                                //}
                            }
                            worksheet.Cells[row, column++]
                          = paramValue;
                        }

                        
                    } // column

                    ++nElements;
                    ++row;

                } // row

            } // category == worksheet


            sw.Stop();

            TaskDialog.Show("Parameter Export",
              string.Format(
                "{0} categories and a total "
                + "of {1} elements exported "
                + "in {2:F2} seconds.",
                nCategories, nElements,
                sw.Elapsed.TotalSeconds));

            return Result.Succeeded;
        }
    }

}