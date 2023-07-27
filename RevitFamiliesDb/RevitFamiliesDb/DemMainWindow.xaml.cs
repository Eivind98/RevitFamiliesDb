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
        public Window AWindow { get; set; }

        public DemMainWindow()
        {
            InitializeComponent();

            Global.AllDemFamilyTypeObject = LoadElementsToList();

            LstDemItems.ItemsSource = Global.AllDemFamilyTypeObject;
            LstDemItems.SelectedValuePath = "DemGuid";


            AWindow = new Window() { 
            Name = "MFStuff",
            Content = Content
            };

            IntPtr revitWindowHandle = Process.GetCurrentProcess().MainWindowHandle;

            WindowInteropHelper helper = new WindowInteropHelper(AWindow);
            helper.Owner = revitWindowHandle;

            AWindow.Show();

            AWindow.Closing += MainWindow_Closing;

        }

        

        private List<FamilyTypeObject> LoadElementsToList()
        {
            string path = Global.TheJsonPath;

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

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                
                if (Directory.Exists(Global.TheDirPath))
                {
                    try
                    {
                        // Get all file names in the specified folder
                        List<string> files = Directory.GetFiles(Global.TheDirPath).ToList();
                        foreach (var file in files)
                        {
                            foreach (FamilyTypeObject famObj in Global.AllDemFamilyTypeObject)
                            {
                                if (System.IO.Path.GetFileNameWithoutExtension(file) == famObj.DemGuid)
                                {
                                    File.Delete(file);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var dialog2 = new TaskDialog("Debug")
                        {
                            MainContent = ex.Message
                        };
                        dialog2.Show();
                    }
                }



                File.WriteAllText(Global.TheJsonPath, FamilyTypeObject.PrintTypeObject(Global.AllDemFamilyTypeObject));



            }
            catch(Exception ex)
            {
                var dialog2 = new TaskDialog("Debug")
                {
                    MainContent = ex.Message
                };
                dialog2.Show();
            }


        }


        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var sel = Global.UIDoc.Selection;

            if (sel.IsValidObject)
            {
                
                var elIds = sel.GetElementIds();
                
                try
                {
                    Global.AllDemFamilyTypeObject.AddRange(HelpingMan(elIds));
                    
                    LstDemItems.Items.Refresh();
                }
                catch(Exception ex)
                {
                    var dialog = new TaskDialog("Debug")
                    {
                        MainContent = ex.Message
                    };
                    dialog.Show();
                    return;
                }

            }

        }

        public List<FamilyTypeObject> HelpingMan(ICollection<ElementId> eleIds)
        {
            List<FamilyTypeObject> lsObj = new List<FamilyTypeObject>();

            foreach(ElementId eleId in eleIds)
            {
                try
                {
                    lsObj.Add(new FamilyTypeObject(eleId, Global.Doc));
                }
                catch
                {

                }
            }

            return lsObj;
        }


        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                string selectedGuid = LstDemItems.SelectedValue?.ToString();
                var selected = LstDemItems.SelectedItems;

                if (selected != null)
                {
                    while (selected.Count > 0)
                    {
                        
                        Global.AllDemFamilyTypeObject.RemoveAll(obj => obj.DemGuid == LstDemItems.SelectedValue?.ToString());
                        
                        LstDemItems.Items.Refresh();
                        selected = LstDemItems.SelectedItems;
                    }
                }
            }
            catch (Exception ex)
            {
                var dialog = new TaskDialog("Debug")
                {
                    MainContent = ex.Message
                };
                dialog.Show();
            }
        }

        private void LstDemItems_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //var test = LstDemItems.Width;

            //WrapPanel wrpPanel = (WrapPanel)LstDemItems.ItemsPanel.FindName("WrpPanel", LstDemItems);

            //LstDemItems.Items.Refresh();


        }

        private void BtnThatShouldCloseTheFuckingWindow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AWindow.Close();

            }
            catch (Exception ex)
            {
                var dialog = new TaskDialog("Debug")
                {
                    MainContent = ex.Message
                };
                dialog.Show();
            }
            

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
