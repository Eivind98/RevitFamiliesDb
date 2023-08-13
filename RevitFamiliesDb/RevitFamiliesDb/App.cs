#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using System;
using System.Collections.Generic;
using System.IO;

#endregion

namespace RevitFamiliesDb
{
    public class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            // Create a custom ribbon tab
            var tabName = "Eivind";
            application.CreateRibbonTab(tabName);

            // Add a new ribbon panel
            RibbonPanel panel = application.CreateRibbonPanel(tabName, "IFC Tools");

            new ButtonBuilder("Export IFC", typeof(CommandCreateMaterial))
                .Text("Hello Eivind")
                .Tooltip("Go to 3D View you want to export")
                .ImagePath("C:\\Users\\eev_9\\source\\repos\\Eivind98\\EivindsAddins\\ExampleAddIn\\Logos\\Door Orientation.png")
                .Build(panel);

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

    }
}
