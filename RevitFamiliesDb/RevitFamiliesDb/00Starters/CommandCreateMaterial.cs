#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Newtonsoft.Json;
using RevitFamiliesDb.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

#endregion

namespace RevitFamiliesDb
{
    [Transaction(TransactionMode.Manual)]
    public class CommandCreateMaterial : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            var uiapp = commandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var app = uiapp.Application;
            var doc = uidoc.Document;
            Trace.Write("1");

            List<DemMaterial> demSelectedElements = new List<DemMaterial>();
            try
            {
                demSelectedElements = JsonConvert.DeserializeObject<List<DemMaterial>>(File.ReadAllText(Global.TheMaterialPath));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }

            

            Trace.Write("2");

            using (var tx = new Transaction(doc))
            {
                tx.Start("Douche bag");

                demSelectedElements[0].CreateThisMF(doc);

                tx.Commit();
            }



            //List<DemElement> demExistingElements = Helper.LoadDemElementsFromFile();
            //Trace.Write("3");
            //demExistingElements.AddRange(demSelectedElements);
            Trace.Write("4");
            
            Trace.Write("3");
            File.WriteAllText(Global.TheMaterialPath, JsonConvert.SerializeObject(demSelectedElements));

            Trace.Write("5");

            return Result.Succeeded;
        }
    }

    

}
