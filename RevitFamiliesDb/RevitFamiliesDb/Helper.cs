using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    using (Transaction tx = new Transaction(doc, "name"))
                    {
                        tx.Start();
                        //within this transaction and event handler execute code block 
                        //is where the interaction with Revit is done.
                        //tx.Commit();
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
        
        public static CompoundStructure GetCompound(Element element)
        {
            HostObjAttributes test = element as HostObjAttributes;

            return test.GetCompoundStructure();


            

        }


    }
}
