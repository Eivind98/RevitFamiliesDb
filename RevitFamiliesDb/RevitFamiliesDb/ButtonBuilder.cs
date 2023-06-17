using Autodesk.Revit.UI;
using System;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace RevitFamiliesDb
{
    internal class ButtonBuilder
    {
        private string _name;
        private string _text = string.Empty;
        private string _assemblyName = Assembly.GetExecutingAssembly().Location;
        private string _imagePath = string.Empty;
        private string _className;
        private string _tooltip = string.Empty;
        public ButtonBuilder(string name, Type cmdType)
        {
            _name = name;
            _className = cmdType.FullName;
        }

        public ButtonBuilder Text(string text)
        {
            _text = text;
            return this;
        }

        public ButtonBuilder ImagePath(string imagePath)
        {
            _imagePath = imagePath;
            return this;
        }

        public ButtonBuilder Tooltip(string tooltip)
        {
            _tooltip = tooltip;
            return this;
        }

        public void Build(RibbonPanel panel)
        {
            var buttonData = new PushButtonData(
                _name,
                _text,
                _assemblyName,
                _className);

            var button = panel.AddItem(buttonData) as PushButton;
            if (!string.IsNullOrEmpty(_tooltip))
            {
                button.ToolTip = _tooltip;
            }

            if (!string.IsNullOrEmpty(_imagePath))
            {
                var pb1Image = new BitmapImage(new Uri(_imagePath));
                button.LargeImage = pb1Image;
            }
        }
    }
}
