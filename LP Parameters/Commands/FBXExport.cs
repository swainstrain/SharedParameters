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
    public class FBXExport : IExternalCommand
    {
        public Document RevitDoc { get; private set; }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get UIDocument
            UIDocument uidoc = commandData.Application.ActiveUIDocument;

            //Get Document
            Document doc = uidoc.Document;
            

            FBXExportOptions options = new FBXExportOptions();


            

            //IEnumerable<ViewSet> viewSet = coll.Cast<ViewSet>().ToList();
            //ICollection<ElementId> viewIds = new List<ElementId>();

            ViewSet set = new ViewSet();

            var classFilter = new ElementClassFilter(typeof(View3D));
            FilteredElementCollector views = new FilteredElementCollector(doc);
            views = views.WherePasses(classFilter);
            foreach (View view in views)
            {
                if (view.CanBePrinted)
                {
                    if (view.Name.Contains("FBX"))
                    {
                        set.Insert(view);
                    }
                }
            }

    
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            try
            {
                using (Transaction trans = new Transaction(doc, "Export FBX"))
                {
                    trans.Start();


                    foreach (View view in set)
                    {

                        doc.Export(path, view.Name, set, options);

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
