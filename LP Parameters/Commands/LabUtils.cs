#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Application = Autodesk.Revit.ApplicationServices.Application;
#endregion // Namespaces

namespace XtraCs
{
  static class LabUtils
  {
        public static string RealString(double a)
        {
            return a.ToString("0.##");
        }

        public static string GetParameterValue(Parameter param)
        {
            string s;
            switch (param.StorageType)
            {
                case StorageType.Double:
                    //
                    // the internal database unit for all lengths is feet.
                    // for instance, if a given room perimeter is returned as
                    // 102.36 as a double and the display unit is millimeters,
                    // then the length will be displayed as
                    // peri = 102.36220472440
                    // peri * 12 * 25.4
                    // 31200 mm
                    //
                    //s = param.AsValueString(); // value seen by user, in display units
                    //s = param.AsDouble().ToString(); // if not using not using LabUtils.RealString()
                    s = RealString(param.AsDouble()); // raw database value in internal units, e.g. feet
                    break;

                case StorageType.Integer:
                    s = param.AsInteger().ToString();
                    break;

                case StorageType.String:
                    s = param.AsString();
                    break;

                case StorageType.ElementId:
                    s = param.AsElementId().IntegerValue.ToString();
                    break;

                case StorageType.None:
                    s = "?NONE?";
                    break;

                default:
                    s = "?ELSE?";
                    break;
            }
            return s;
        }

    }
}
