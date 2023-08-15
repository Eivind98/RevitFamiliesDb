using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitFamiliesDb.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interop;


namespace RevitFamiliesDb
{
    /// <summary>
    /// Interaction logic for DemMainWindow.xaml
    /// </summary>
    public partial class DemMainWindow : UserControl
    {
        public Window AWindow { get; set; }
        public List<string> SortingStuff { get; set; } = new List<string>();
        public bool AsOrDes { get; set; } = false;
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

            
            Global.AllDemElements = Helper.LoadDemElementsFromFile();
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

        private void RefreshThisMF()
        {
            try
            {
                string searchString = TxBoxSearch.Text.ToLower();
                LstDemItems.ItemsSource = null;

                List<Type> filteringTypes = new List<Type>
                {
                    (bool)BtnToggleCeiling.IsChecked ? typeof(DemCeilingType) : null,
                    (bool)BtnToggleFloor.IsChecked ? typeof(DemFloorType) : null,
                    (bool)BtnToggleRoof.IsChecked ? typeof(DemRoofType) : null,
                    (bool)BtnToggleWall.IsChecked ? typeof(DemWallType) : null
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
                        .Where(item => filteringTypes.Contains(GetElementType(item)))
                        .Where(item => item.Name.ToLower().Contains(searchString))
                        .ToList();
                }
                else if (!noValuesToFilter)
                {
                    filteredList = Global.AllDemElements
                        .Where(item => filteringTypes.Contains(GetElementType(item)))
                        .ToList();
                }
                else
                {
                    filteredList = Global.AllDemElements;
                }

                List<DemElement> sortedList = filteredList.OrderBy(element => element.Name).ToList();
                LstDemItems.ItemsSource = sortedList;

            }
            catch
            {

            }
        }

        public Type GetElementType(DemElement ele)
        {
            if (ele is DemCeilingType)
            {
                return typeof(DemCeilingType);
            }
            else if (ele is DemFloorType)
            {
                return typeof(DemFloorType);
            }
            else if (ele is DemRoofType)
            {
                return typeof(DemRoofType);
            }
            else if (ele is DemWallType)
            {
                return typeof(DemWallType);
            }
            else
            {
                return null;
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
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                Helper.SaveDemElementsToFile(Global.AllDemElements);

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
            Global.AllDemElements.AddRange(Helper.CreateFromSelection(Global.UIDoc));
            RefreshThisMF();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                var selectedItems = LstDemItems.SelectedItems.Cast<DemElement>().ToList();

                foreach (var selectedItem in selectedItems)
                {
                    Global.AllDemElements.RemoveAll(obj => obj.UniqueId == selectedItem.UniqueId);

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

                        TextBlock label = item.FindName("LblName") as TextBlock;
                        if (label != null)
                        {
                            double newFontSize = e.NewValue/6.4;
                            label.FontSize = newFontSize;
                        }
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
                            SortingStuff.Add("Width");
                            break;
                        case "Thickness":
                            SortingStuff.Clear();
                            SortingStuff.Add("Width");
                            SortingStuff.Add("Name");
                            break;
                        case "Type":
                            SortingStuff.Clear();
                            SortingStuff.Add("Category");
                            SortingStuff.Add("Name");
                            SortingStuff.Add("Width");
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
            AsOrDes = true;
            RefreshThisMF();
        }

        private void BtnToggleAsOrDes_Unchecked(object sender, RoutedEventArgs e)
        {
            AsOrDes = false;
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

        private void LstDemItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            LstProperties.ItemsSource = LstDemItems.SelectedItems;


        }

    }

    public class ItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Ceiling { get; set; }
        public DataTemplate Floor { get; set; }
        public DataTemplate Roof { get; set; }
        public DataTemplate Wall { get; set; }
        public DataTemplate Material { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is DemCeilingType)
                return Ceiling;
            else if (item is DemFloorType)
                return Floor;
            else if (item is DemRoofType)
                return Roof;
            else if (item is DemWallType)
                return Wall;
            else if (item is DemMaterial)
                return Material;

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
