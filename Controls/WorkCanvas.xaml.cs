using CanvasTest.Models;
using CanvasTest.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CanvasTest.Controls
{
    /// <summary>
    /// Interaktionslogik für PanAndZoomCanvas.xaml
    /// https://stackoverflow.com/questions/35165349/how-to-drag-rendertransform-with-mouse-in-wpf
    /// </summary>
    /// 

    public partial class WorkCanvas : Canvas
    {

        private readonly MainViewModel _mainViewModel;

        #region Dependency properties

        //expose "Transform" as a dependency property for access outside the class
        public static readonly DependencyProperty TransformProperty =
            DependencyProperty.Register("Transform", typeof(MatrixTransform), typeof(WorkCanvas),
                new PropertyMetadata(new MatrixTransform()));

        public MatrixTransform Transform
        {
            get { return (MatrixTransform)GetValue(TransformProperty); }
            set { SetValue(TransformProperty, value); }
        }


        #endregion

        #region class variables
        private Point _initialMousePosition;
        public float Zoomfactor { get; set; } = 1.1f;
        #endregion

        public WorkCanvas()
        {
            InitializeComponent();

            MouseDown += WorkCanvas_MouseDown;
            MouseUp += WorkCanvas_MouseUp;
            MouseMove += WorkCanvas_MouseMove;
            MouseWheel += WorkCanvas_MouseWheel;
            //Drop += WorkCanvas_Drop;

        }

        //Event handlers
        private void WorkCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle)
            {
                _initialMousePosition = Transform.Inverse.Transform(e.GetPosition(this));
            }

        }

        private void WorkCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void WorkCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                Point mousePosition = Transform.Inverse.Transform(e.GetPosition(this));
                Vector delta = Point.Subtract(mousePosition, _initialMousePosition);
                var translate = new TranslateTransform(delta.X, delta.Y);
                Matrix newMatrix = translate.Value * Transform.Matrix;
                this.Transform = new MatrixTransform(newMatrix);

                //_mainViewModel.CanvasWorldPositionText = $"X: {mousePosition.X:F0}, Y: {mousePosition.Y:F0}";

                string outputMatrix = Transform.Matrix.ToString();
                Debug.WriteLine(outputMatrix);

                foreach (UIElement child in this.Children)
                {
                    child.RenderTransform = Transform;
                }
            }

        }

        private void WorkCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            float scaleFactor = Zoomfactor;
            if (e.Delta < 0)
            {
                scaleFactor = 1f / scaleFactor;
            }

            Point mousePostion = e.GetPosition(this);

            Matrix matrix = this.Transform.Matrix;
            matrix.ScaleAt(scaleFactor, scaleFactor, mousePostion.X, mousePostion.Y);
            this.Transform = new MatrixTransform(matrix);
            string outputMatrix = Transform.Matrix.ToString();
            Debug.WriteLine(outputMatrix);
            foreach (UIElement child in this.Children)
            {
                double x = Canvas.GetLeft(child);
                double y = Canvas.GetTop(child);

                double sx = x * scaleFactor;
                double sy = y * scaleFactor;

                Canvas.SetLeft(child, sx);
                Canvas.SetTop(child, sy);

                child.RenderTransform = Transform;
            }
        }

        //private void WorkCanvas_Drop(object sender, DragEventArgs e)
        //{
        //    if (sender is not Controls.WorkCanvas canvas)
        //    {
        //        return; // Safety check
        //    }
        //    Point dropPosition = e.GetPosition(canvas);
        //    if (e.Data.GetData("ExcelFunction") is ExcelFunction function)
        //    {
        //        _mainViewModel.AddNode(function, new Point(dropPosition.X - 70, dropPosition.Y - 40));
        //    }
        //    e.Handled = true;
        //}

    }
}