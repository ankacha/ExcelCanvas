// In a new file, e.g., CanvasItemTemplateSelector.cs
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using CanvasTest.ViewModels; // Your using statements may vary

namespace CanvasTest.Converters
{


    public class CanvasItemTemplateSelector : DataTemplateSelector
    {
        // Define properties to hold the templates from your XAML resources
        public DataTemplate NodeTemplate { get; set; }
        public DataTemplate ShapeTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            // 'item' is the object from your AllCanvasItems collection
            if (item is NodeViewModel)
            {
                return NodeTemplate;
            }

            if (item is Shape)
            {
                return ShapeTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}