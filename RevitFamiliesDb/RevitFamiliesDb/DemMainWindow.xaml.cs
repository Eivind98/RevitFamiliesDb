using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace RevitFamiliesDb
{
    /// <summary>
    /// Interaction logic for DemMainWindow.xaml
    /// </summary>
    public partial class DemMainWindow : UserControl
    {
        public DemMainWindow()
        {
            InitializeComponent();


            Global.AllDemFamilyTypeObject = LoadElementsToList();

            LstDemItems.ItemsSource = Global.AllDemFamilyTypeObject;
            LstDemItems.SelectedValuePath = "DemGuid";
            

            Window window = new Window() { 
            Name = "MFStuff",
            Content = Content
            };

            // Get the handle of the Revit window
            IntPtr revitWindowHandle = Process.GetCurrentProcess().MainWindowHandle;

            // Get the Revit window as a WindowInteropHelper object
            WindowInteropHelper helper = new WindowInteropHelper(window);
            helper.Owner = revitWindowHandle;

            window.Show();

        }

        public int ColumnsCount { get; set; }

        private List<FamilyTypeObject> LoadElementsToList()
        {
            string path = "C:\\Users\\eev_9\\OneDrive\\02 - Projects\\Programming stuff\\Test.json";

            try
            {
                List<FamilyTypeObject> allDemObjects = JsonConvert.DeserializeObject<List<FamilyTypeObject>>(File.ReadAllText(path));
                return allDemObjects;
            }
            catch
            {
                return null;
            }

        }

        private void BtnAddToProject_Click(object sender, RoutedEventArgs e)
        {
            if(LstDemItems.SelectedValue != null)
            {
                var dialog = new TaskDialog("Debug")
                {
                    MainContent = LstDemItems.SelectedValue.ToString()
                };
                dialog.Show();

            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var sel = Global.UIDoc.Selection;

            if (sel.IsValidObject)
            {
                var elId = sel.GetElementIds().FirstOrDefault();

                try
                {
                    var element = new FilteredElementCollector(Global.Doc)
                        .WhereElementIsElementType()
                        .FirstOrDefault(x => x.Id == elId) as FloorType;
                    Global.AllDemFamilyTypeObject.Add(new FamilyTypeObject(elId, Global.Doc));

                    //LstDemItems.ItemsSource = Global.AllDemFamilyTypeObject;
                    LstDemItems.Items.Refresh();
                }
                catch
                {
                    return;
                }

            }

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (LstDemItems.SelectedValue != null)
            {
                foreach (FamilyTypeObject obj in Global.AllDemFamilyTypeObject)
                {
                    if (LstDemItems.SelectedValue.ToString() == obj.DemGuid)
                    {
                        var dialog = new TaskDialog("Debug")
                        {
                            MainContent = obj.DemGuid
                        };
                        dialog.Show();

                        //Global.AllDemFamilyTypeObject.Remove(obj);

                    }

                }
                //LstDemItems.ItemsSource = null;
                //LstDemItems.ItemsSource = Global.AllDemFamilyTypeObject;

            }

        }

        private void DemWindow_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {
            File.WriteAllText("C:\\Users\\eev_9\\OneDrive\\02 - Projects\\Programming stuff\\Test.json", FamilyTypeObject.PrintTypeObject(Global.AllDemFamilyTypeObject));
        }

        // Implement INotifyPropertyChanged to notify the UI of property changes
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void LstDemItems_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //var test = LstDemItems.Width;

            //WrapPanel wrpPanel = (WrapPanel)LstDemItems.ItemsPanel.FindName("WrpPanel", LstDemItems);

            //LstDemItems.Items.Refresh();


        }
    }

    public class SubtractConverter : IValueConverter
    {
        public double Value { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double val = System.Convert.ToDouble(value);
            return val - Value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }


}
