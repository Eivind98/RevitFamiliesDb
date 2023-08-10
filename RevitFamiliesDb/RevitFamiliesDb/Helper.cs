using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using RevitFamiliesDb.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Markup;

namespace RevitFamiliesDb
{
    public class MyEventHandler : IExternalEventHandler
    {
        public static ExternalEvent HandlerEvent = null;
        public static MyEventHandler Handler = null;
        public void Execute(UIApplication app)
        {
            Document doc = app.ActiveUIDocument.Document;
            try
            {
                //notifying me of raised event
                Trace.WriteLine("Raised");
                try
                {

                    IList listOfIList = Global.ElementsToProjectList;

                    if (listOfIList.Count > 0)
                    {
                        using (var tx = new Transaction(doc))
                        {
                            tx.Start("Creating");


                            foreach (DemElement yo in listOfIList)
                            {
                                switch (yo.Category)
                                {
                                    case "Ceilings":
                                        ((DemCeilingType)yo).CreateThisMF(doc);
                                        break;
                                    case "Floors":
                                        ((DemFloorType)yo).CreateThisMF(doc);
                                        break;
                                    case "Roofs":
                                        ((DemRoofType)yo).CreateThisMF(doc);
                                        break;
                                    case "Walls":
                                        ((DemWallType)yo).CreateThisMF(doc);
                                        break;

                                }

                            };
                            tx.Commit();
                        }

                    }

                }
                catch (Exception e)
                {
                    //catch whatever exception
                    throw e;
                }
            }
            catch (InvalidOperationException)
            {

                throw;
            }
        }

        public string GetName()
        {
            return "My Event Handler";
        }
        public void GetData(MechanicalEquipment mechEq)
        {

        }
    }


    public class Helper
    {
        public static DemElement GetDemElement(ElementId eleId, Document doc)
        {
            Element Ele = new FilteredElementCollector(doc)
                .WhereElementIsElementType()
                .FirstOrDefault(x => x.Id == eleId);

            DemElement outPut = new DemElement();

            switch (Ele.Category.Name)
            {
                case "Ceilings":
                    outPut = new DemCeilingType(Ele as CeilingType);
                    break;
                case "Floors":
                    outPut = new DemFloorType(Ele as FloorType);
                    break;
                case "Roofs":
                    outPut = new DemRoofType(Ele as RoofType);
                    break;
                case "Walls":
                    outPut = new DemWallType(Ele as WallType);
                    break;
            }

            return outPut;
        }



        public static CompoundStructure GetCompound(Element element)
        {
            HostObjAttributes test = element as HostObjAttributes;

            return test.GetCompoundStructure();




        }


    }
}
