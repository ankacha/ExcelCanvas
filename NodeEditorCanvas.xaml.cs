using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace YourNamespace
{
    public partial class NodeEditorCanvas : UserControl
    {
        private double _zoomLevel = 1.0;
        private const double GridSize = 20;

        public NodeEditorCanvas()
        {
            InitializeComponent();
            DrawGrid();
        }

        private void DrawGrid()
        {
            GridCanvas.Children.Clear();

            double effectiveGridSize = GridSize;

            // Draw grid lines...
            for (double x = 0; x <= GridCanvas.Width; x += effectiveGridSize)
            {
                Line line = new Line
                {
                    X1 = x,
                    Y1 = 0,
                    X2 = x,
                    Y2 = GridCanvas.Height,
                    Stroke = Brushes.LightGray,
                    StrokeThickness = 0.5
                };
                GridCanvas.Children.Add(line);
            }

            // Similar for horizontal lines...
        }

        private void CanvasScroller_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                // Zoom logic here
                e.Handled = true;
            }
        }

        private void MainCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Handle canvas clicks
        }

        // Public method to add nodes
        public void AddNode(string nodeType, Point position)
        {
            // Add node logic
        }
    }
}