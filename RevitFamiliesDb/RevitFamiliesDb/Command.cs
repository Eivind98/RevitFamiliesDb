#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#endregion

namespace RevitFamiliesDb
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
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

            // Access current selection

            var sel = uidoc.Selection;


            var elId = sel.GetElementIds().FirstOrDefault();


            if (elId == null) return Result.Succeeded;


            // Retrieve elements from database
            var element = new FilteredElementCollector(doc)
                .WhereElementIsElementType()
                .FirstOrDefault(x => x.Id == elId) as FloorType;

            //var dialog = new TaskDialog($"IsValidObject: {sel.IsValidObject}, ElementIdCount: {sel.GetElementIds().Count}");
            var dialog = new TaskDialog("Debug");
            //dialog.MainContent = json;


            var newElementId = Guid.NewGuid().ToString();
            using (var tx = new Transaction(doc))
            {
                tx.Start("Douche bag");
                var newElement = element.Duplicate(newElementId) as FloorType;
                var structure = CompoundStructure.CreateSimpleCompoundStructure(new List<CompoundStructureLayer>()
                {
                    new CompoundStructureLayer(100/304.8, MaterialFunctionAssignment.Structure, new ElementId(BuiltInCategory.INVALID))
                });
                structure.EndCap = EndCapCondition.NoEndCap;

                IDictionary<int, CompoundStructureError> errors = new Dictionary<int, CompoundStructureError>();
                IDictionary<int, int> twoErrors = new Dictionary<int, int>();
                structure.IsValid(doc, out errors, out twoErrors);

                //string testN = "";
                //string testV = "";

                //var testing = new FamilyTypeObject("");

                var yo = new FamilyTypeObject(elId, doc);


                foreach (Parameter param in newElement.Parameters)
                {
                    
                    if(param.HasValue && !param.IsReadOnly)
                    {
                        switch (param.StorageType)
                        {
                            case StorageType.String:
                                param.Set("");
                                break;
                            case StorageType.Integer:
                                param.Set(0);
                                break;
                            case StorageType.Double:
                                param.Set(0.0);
                                break;
                            case StorageType.ElementId:
                                param.Set(ElementId.InvalidElementId); 
                                break;

                        }

                        //testN = param.Definition.ToString();
                        //testV = param.AsDouble().ToString();
                        

                    }
                    
                }

                //dialog.MainContent = newElement.Location?.ToString();
                //dialog.MainContent = JsonConvert.SerializeObject(errors, Formatting.Indented);
                dialog.MainContent = JsonConvert.SerializeObject(yo.layerStructure, Formatting.Indented);
                //dialog.MainContent = yo.typeName + element.GetType().ToString();
                dialog.Show();
                newElement.SetCompoundStructure(structure);
                tx.Commit();
            }


            //var test = JsonConvert.DeserializeObject<CompoundStructure>(json);

            // Filtered element collector is iterable

            //foreach (Element e in col)
            //{
            //    Debug.Print(e.Name);
            //}

            // Modify document within a transaction

            //using (var tx = new Transaction(doc))
            //{
            //    tx.Start("Transaction Name");
            //    tx.Commit();
            //}

            return Result.Succeeded;
        }
    }
}
