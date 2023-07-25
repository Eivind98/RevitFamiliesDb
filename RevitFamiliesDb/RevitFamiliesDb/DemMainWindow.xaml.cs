using Autodesk.Revit.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
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
            //LstDemItems.DisplayMemberPath = "Name";
            //LstDemItems.SelectedValuePath = "Id";
            

            Window window = new Window() { 
            Name = "MFStuff",
            Content = Content
            
            };

            window.Show();

            
            
        }

        private List<FamilyTypeObject> LoadElementsToList()
        {
            string path = "C:\\Users\\eev_9\\OneDrive\\02 - Projects\\Programming stuff\\Test.json";
            _ = new List<FamilyTypeObject>();

            List<FamilyTypeObject> allDemObjects = JsonConvert.DeserializeObject<List<FamilyTypeObject>>(File.ReadAllText(path));

            

            return allDemObjects;
        }


        private void BtnAddToProject_Click(object sender, RoutedEventArgs e)
        {
            if(LstDemItems.SelectedValue != null)
            {
                var dialog = new TaskDialog("Debug");
                dialog.MainContent = LstDemItems.SelectedValue.ToString();
                dialog.Show();

            }


        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
