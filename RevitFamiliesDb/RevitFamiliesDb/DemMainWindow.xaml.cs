using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Newtonsoft.Json;
using RevitFamiliesDb.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public List<string> SortingStuff { get; set; } = new List<string>();
        public ListSortDirection AsOrDes { get; set; } = ListSortDirection.Ascending;
        public List<FamilyTypeObject> RelevantItems { get; set; } = new List<FamilyTypeObject>();
        Document Doc { get; set; }
        ExternalEvent M_exEvent { get; set; }
        MyEventHandler MyEvent { get; set; }

        public DemMainWindow(UIDocument UiDoc, ExternalEvent exEvent, MyEventHandler handler)
        {

            Doc = UiDoc.Document;
            InitializeComponent();
            M_exEvent = exEvent;
            MyEvent = handler;

            
            Global.AllDemElements = LoadDemElementsToList();
            RefreshThisMF();

            LstDemItems.SelectedValuePath = "DemGuid";

            AWindow = new Window()
            {
                Name = "MFStuff",
                Content = Content
            };

            IntPtr revitWindowHandle = Process.GetCurrentProcess().MainWindowHandle;

            WindowInteropHelper helper = new WindowInteropHelper(AWindow);
            helper.Owner = revitWindowHandle;

            AWindow.Show();

            AWindow.Closing += MainWindow_Closing;
        }

        private List<DemElement> LoadDemElementsToList()
        {
            List<DemElement> yo = new List<DemElement>();

            try
            {
                List<DemCeilingType> allDemCeilings = JsonConvert.DeserializeObject<List<DemCeilingType>>(File.ReadAllText(Global.TheCeilingPath));
                yo.AddRange(allDemCeilings);

            }
            catch
            {
                var dialog = new TaskDialog("Debug")
                {
                    MainContent = "Ceiling"
                };
                dialog.Show();
            }

            try
            {
                List<DemFloorType> allDemFloors = JsonConvert.DeserializeObject<List<DemFloorType>>(File.ReadAllText(Global.TheFloorPath));
                yo.AddRange(allDemFloors);
            }
            catch
            {
                var dialog = new TaskDialog("Debug")
                {
                    MainContent = "Floor"
                };
                dialog.Show();
            }

            return yo;
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
                return new List<FamilyTypeObject>();
            }
        }

        private void RefreshThisMF()
        {
            try
            {
                string searchString = TxBoxSearch.Text.ToLower();
                LstDemItems.ItemsSource = null;

                List<string> filteringTypes = new List<string>
                {
                    (bool)BtnToggleCeiling.IsChecked ? "Ceilings" : null,
                    (bool)BtnToggleFloor.IsChecked ? "Floors" : null,
                    (bool)BtnToggleRoof.IsChecked ? "Roofs" : null,
                    (bool)BtnToggleWall.IsChecked ? "Walls" : null
                };

                bool noValuesToFilter = filteringTypes.All(type => type == null);
                bool hasSearchString = !string.IsNullOrWhiteSpace(searchString);

                List<DemElement> filteredList;

                if (hasSearchString && noValuesToFilter)
                {
                    filteredList = Global.AllDemElements
                        .Where(item => item.Name.ToLower().Contains(searchString))
                        .ToList();
                }
                else if (hasSearchString && !noValuesToFilter)
                {
                    filteredList = Global.AllDemElements
                        .Where(item => filteringTypes.Contains(item.Category))
                        .Where(item => item.Name.ToLower().Contains(searchString))
                        .ToList();
                }
                else if (!noValuesToFilter)
                {
                    filteredList = Global.AllDemElements
                        .Where(item => filteringTypes.Contains(item.Category))
                        .ToList();
                }
                else
                {
                    filteredList = Global.AllDemElements;
                }

                LstDemItems.ItemsSource = filteredList;



                LstDemItems.Items.SortDescriptions.Clear();

                foreach (string s in SortingStuff)
                {
                    LstDemItems.Items.SortDescriptions.Add(new SortDescription(s, AsOrDes));
                }
            }
            catch
            {

            }
        }


        private void BtnAddToProject_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Global.ElementsToProjectList = LstDemItems.SelectedItems;

                M_exEvent.Raise();


            }
            catch (Exception ex)
            {
                var dialog = new TaskDialog("Debug")
                {
                    MainContent = ex.Message,
                };
                dialog.Show();
            }




            //if (LstDemItems.SelectedValue != null)
            //{
            //    var dialog = new TaskDialog("Debug")
            //    {
            //        MainContent = LstDemItems.SelectedValue.ToString()
            //    };
            //    dialog.Show();
            //}
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {


                //if (Directory.Exists(Global.TheDirPath) && Directory.Exists(System.IO.Path.GetDirectoryName(Global.TheJsonPath)))
                //{
                //    try
                //    {
                //        string[] filesInFolder = Directory.GetFiles(Global.TheDirPath);
                //        List<string> objPaths = new List<string>();
                //        foreach (FamilyTypeObject famObj in Global.AllDemFamilyTypeObject)
                //        {
                //            objPaths.Add(famObj.ThePath);
                //        }

                //        foreach (string str in filesInFolder)
                //        {
                //            if (!objPaths.Contains(str))
                //            {
                //                File.Delete(str);
                //            }
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        var dialog = new TaskDialog("Debug")
                //        {
                //            MainContent = ex.Message
                //        };
                //        dialog.Show();
                //    }

                //    File.WriteAllText(Global.TheJsonPath, FamilyTypeObject.PrintTypeObject(Global.AllDemFamilyTypeObject));
                //}
                //else
                //{
                //    var dialog = new TaskDialog("Debug")
                //    {
                //        MainContent = "one of these paths do not exist:" + Global.TheJsonPath + " - - - - - - - " + Global.TheDirPath
                //    };
                //    dialog.Show();
                //}
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

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var sel = Global.UIDoc.Selection;

            if (sel.IsValidObject)
            {
                var elIds = sel.GetElementIds();

                try
                {
                    Global.AllDemFamilyTypeObject.AddRange(HelpingMan(elIds));

                    RefreshThisMF();
                }
                catch (Exception ex)
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

            foreach (ElementId eleId in eleIds)
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

                var selectedItems = LstDemItems.SelectedItems.Cast<FamilyTypeObject>().ToList();

                foreach (var selectedItem in selectedItems)
                {
                    Global.AllDemFamilyTypeObject.RemoveAll(obj => obj.DemGuid == selectedItem.DemGuid);

                }

                RefreshThisMF();

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

        private void SizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                if (ItemRefernce.Count != 0)
                {
                    foreach (var item in ItemRefernce)
                    {
                        item.Width = e.NewValue;

                        GridLengthConverter gridLengthConverter = new GridLengthConverter();

                        item.RowDefinitions[0].Height = (GridLength)gridLengthConverter.ConvertFrom(e.NewValue.ToString());
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

        public List<System.Windows.Controls.Grid> ItemRefernce { get; private set; } = new List<System.Windows.Controls.Grid>();

        private void IconTemplateGrid_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ItemRefernce.Add(sender as System.Windows.Controls.Grid);
            }
            catch (Exception ex)
            {
                var dialog = new TaskDialog("Debug")
                {
                    MainContent = "Loader...." + ex.Message
                };
                dialog.Show();
            }
        }

        private void IconTemplateGrid_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ItemRefernce.Remove(sender as System.Windows.Controls.Grid);
            }
            catch (Exception ex)
            {
                var dialog = new TaskDialog("Debug")
                {
                    MainContent = "Unloader...." + ex.Message
                };
                dialog.Show();
            }
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (e != null)
                {
                    string yo = ((ComboBoxItem)e.AddedItems[0]).Tag.ToString();

                    switch (yo)
                    {
                        case "Name":
                            SortingStuff.Clear();
                            SortingStuff.Add("Name");
                            SortingStuff.Add("ComStructureLayers.GetWidth");
                            break;
                        case "Thickness":
                            SortingStuff.Clear();
                            SortingStuff.Add("ComStructureLayers.GetWidth");
                            SortingStuff.Add("Name");
                            break;
                        case "Type":
                            SortingStuff.Clear();
                            SortingStuff.Add("Type");
                            SortingStuff.Add("Name");
                            SortingStuff.Add("ComStructureLayers.GetWidth");
                            break;
                        default:
                            var dialog = new TaskDialog("Debug")
                            {
                                MainContent = yo
                            };
                            dialog.Show();
                            break;

                    }

                    RefreshThisMF();
                }
            }
            catch (Exception ex)
            {
                var dialog = new TaskDialog("Debug")
                {
                    MainContent = "Selection Changed stuff ..... " + ex.Message + " - - - - " + ((ComboBoxItem)e.AddedItems[0]).Tag.ToString()
                };
                dialog.Show();
            }
        }

        private void BtnToggleAsOrDes_Checked(object sender, RoutedEventArgs e)
        {
            AsOrDes = ListSortDirection.Descending;
            RefreshThisMF();
        }

        private void BtnToggleAsOrDes_Unchecked(object sender, RoutedEventArgs e)
        {
            AsOrDes = ListSortDirection.Ascending;
            RefreshThisMF();
        }

        private void TxBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {


                RefreshThisMF();




                //var dialog = new TaskDialog("Debug")
                //{
                //    MainContent = yo
                //};
                //dialog.Show();

            }
            catch (Exception ex)
            {
                var dialog = new TaskDialog("Debug")
                {
                    MainContent = "Something -   " + ex.Message
                };
                dialog.Show();
            }



        }

        private void BtnToggleCeiling_Checked(object sender, RoutedEventArgs e)
        {
            RefreshThisMF();
        }

        private void BtnToggleCeiling_Unchecked(object sender, RoutedEventArgs e)
        {
            RefreshThisMF();
        }

        private void BtnToggleFloor_Checked(object sender, RoutedEventArgs e)
        {
            RefreshThisMF();
        }

        private void BtnToggleFloor_Unchecked(object sender, RoutedEventArgs e)
        {
            RefreshThisMF();
        }

        private void BtnToggleRoof_Checked(object sender, RoutedEventArgs e)
        {
            RefreshThisMF();
        }

        private void BtnToggleRoof_Unchecked(object sender, RoutedEventArgs e)
        {
            RefreshThisMF();
        }

        private void BtnToggleWall_Checked(object sender, RoutedEventArgs e)
        {
            RefreshThisMF();
        }

        private void BtnToggleWall_Unchecked(object sender, RoutedEventArgs e)
        {
            RefreshThisMF();
        }
    }

    public class ItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Ceiling { get; set; }
        public DataTemplate Floor { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is DemCeilingType)
                return Ceiling;
            else if (item is DemFloorType)
                return Floor;

            return base.SelectTemplate(item, container);
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

    public class MetersToMM : IValueConverter
    {
        public double Value = 1000;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double val = System.Convert.ToDouble(value);
            return val * Value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double val = System.Convert.ToDouble(value);
            return val / Value;
        }
    }

    public class SliderValueToWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            double sliderValue = (double)value;

            return sliderValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


}
