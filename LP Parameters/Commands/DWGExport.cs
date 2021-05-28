using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Architecture;

namespace ComharDesign
{
    [TransactionAttribute(TransactionMode.Manual)]
    public class DWGExport : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get UIDocument
            UIDocument uidoc = commandData.Application.ActiveUIDocument;

            //Get Document
            Document doc = uidoc.Document;
            

            DWGExportOptions options = new DWGExportOptions();
            options.Colors = ExportColorMode.TrueColor;
            //options.ExportOfSolids = SolidGeometry.Polymesh;
            //options.FileVersion = ACADVersion.R2013;
            //options.HatchPatternsFileName = @"C:\Program Files\Autodesk\Revit 2016\ACADInterop\acdb.pat";
            //options.HideScopeBox = true;
            //options.HideUnreferenceViewTags = true;
            //options.HideReferencePlane = true;
            //options.LayerMapping = "AIA";
            //options.LineScaling = LineScaling.PaperSpace;
            //options.LinetypesFileName = @"C:\Program Files\Autodesk\Revit 2016\ACADInterop\acdb.lin";
            options.MergedViews = true;
            //options.PropOverrides = PropOverrideMode.ByEntity;
            //options.TextTreatment = TextTreatment.Exact;


            IEnumerable<ViewSheet> AllSheets = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Sheets)
                .Cast<ViewSheet>();

            List<ElementId> viewIds = new List<ElementId>();

            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            

            try
            {
                using (Transaction trans = new Transaction(doc, "Export DWG"))
                {
                    trans.Start();

                    foreach (ViewSheet viewSheet in AllSheets)
                    {
                        Parameter param_checked = viewSheet.get_Parameter(BuiltInParameter.SHEET_SCHEDULED);


                        if (param_checked.AsValueString().Equals("Yes"))
                            

                        {
                            viewIds.Add(viewSheet.Id);
                            var name = viewSheet.Name;
                            doc.Export(path, name, viewIds, options);
                        }                       
                        
                        

                    }                    
                    
                    trans.Commit();
                }

                return Result.Succeeded;
            }
            catch (Exception e)
            {
                message = e.Message;
                return Result.Failed;
            }

        }        
    }

}
