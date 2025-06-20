using CanvasTest.Models;
using CanvasTest.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CanvasTest.Controls
{
    public partial class Node : UserControl
    {
        // Dependency Properties for binding
        public static readonly DependencyProperty FunctionNameProperty =
            DependencyProperty.Register("FunctionName", typeof(string), typeof(Node),
                new PropertyMetadata("SUM"));

        public static readonly DependencyProperty NodeValueProperty =
            DependencyProperty.Register("NodeValue", typeof(string), typeof(Node),
                new PropertyMetadata(""));
        public static readonly DependencyProperty IconPathProperty =
                DependencyProperty.Register("IconPath", typeof(string), typeof(Node),
                    new PropertyMetadata("M12,2 L12,22 M2,12 L22,12")); // Default plus icon

        public string FunctionName
        {
            get { return (string)GetValue(FunctionNameProperty); }
            set { SetValue(FunctionNameProperty, value); }
        }

        public string NodeValue
        {
            get { return (string)GetValue(NodeValueProperty); }
            set { SetValue(NodeValueProperty, value); }
        }

        // ADD THIS NEW PROPERTY

        public string IconPath

        {
            get { return (string)GetValue(IconPathProperty); }
            set { SetValue(IconPathProperty, value); }
        }

        public Node()
        {
            InitializeComponent();
        }





        // S E L E C T I O N L O G I C


 

        // CONNECTION LOGIC
        private void LeftConnection_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void RightConnection_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void LeftConnection_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void RightConnection_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

    }
}
