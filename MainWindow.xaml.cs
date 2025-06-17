using CanvasTest.Controls;
using CanvasTest.Models;
using CanvasTest.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CanvasTest
{
    public partial class MainWindow : Window
    {
        // The ViewModel is the single source of truth for our application's state.
        private readonly MainViewModel _mainViewModel;

        // Fields to manage the UI aspect of dragging a node on the canvas.
        private NodeViewModel? _draggedNode;
        private Point _dragStartOffset;

        //Fields that manage the panning functionality
        private bool _isPanning;
        private Point _panStartPoint;

        public MainWindow()
        {
            InitializeComponent();

            // Create and set the ViewModel as the DataContext for the whole window.
            _mainViewModel = new MainViewModel();
            DataContext = _mainViewModel;

        }

        // --- Drag-and-Drop from Function List ---

        // This is the ONLY event handler needed for the function list.
        // It replaces the old MouseMove and MouseLeave handlers.
        private void FunctionListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Find the data for the item that was clicked.
            if (e.OriginalSource is DependencyObject source &&
                ((FrameworkElement)source).DataContext is ExcelFunction function)
            {
                // Package the data and start the built-in WPF drag-and-drop operation.
                var dragData = new DataObject("ExcelFunction", function);
                DragDrop.DoDragDrop((DependencyObject)e.OriginalSource, dragData, DragDropEffects.Copy);
            }
        }

        private void NodeCanvas_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData("ExcelFunction") is ExcelFunction function)
            {
                Point dropPosition = e.GetPosition(NodeCanvas);
                _mainViewModel.AddNode(function, new Point(dropPosition.X - 70, dropPosition.Y - 40));
            }
            e.Handled = true;
        }


        // --- Node Interaction on the Canvas ---

        private void Node_LeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is FrameworkElement clickedElement &&
       (clickedElement.Name == "LeftConnection" || clickedElement.Name == "RightConnection"))
            {
                // This is a connection port, not the node body.
                // For now, we just prevent dragging.
                // Later, you would start logic here to draw a connection line.
                e.Handled = true; // Mark the event as handled
                return;           // and exit the method to prevent dragging.
            }

            // Get the ViewModel for the node that was clicked.
            if (sender is FrameworkElement element && element.DataContext is NodeViewModel nodeVM)
            {
                // 1. Tell the MainViewModel that this is now the selected node.
                _mainViewModel.SelectedNode = nodeVM;

                // 2. Prepare for a drag operation.
                _draggedNode = nodeVM;
                _dragStartOffset = e.GetPosition(element);
                element.CaptureMouse();
                e.Handled = true; // Stop the event from bubbling further.
            }
        }

        private void Node_MouseMove(object sender, MouseEventArgs e)
        {
            // If we are dragging a node...
            if (_draggedNode != null && e.LeftButton == MouseButtonState.Pressed)
            {
                // ...update the X and Y properties on its ViewModel.
                Point currentPosition = e.GetPosition(NodeCanvas);
                _draggedNode.X = currentPosition.X - _dragStartOffset.X;
                _draggedNode.Y = currentPosition.Y - _dragStartOffset.Y;

            }
        }

        private void Node_LeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                element.ReleaseMouseCapture();
                _draggedNode = null;
                e.Handled = true;
            }
        }

        // --- Global Canvas and Window Events ---

        private void NodeCanvas_LeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // If the user clicks on the empty canvas, deselect any selected node.
            if (e.Source == NodeCanvas)
            {
                _mainViewModel.SelectedNode = null;
            }
        }
        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            // Existing logic for coordinates...
            var canvasPosition = e.GetPosition(NodeCanvas);
            _mainViewModel.MousePositionText = $"X: {canvasPosition.X:F0}, Y: {canvasPosition.Y:F0}";

            // Existing logic for hovered element...
            if (Mouse.DirectlyOver is DependencyObject element)
            {
                var ancestry = TreeHelper.GetAncestryPath(element);
                var path = string.Join(" ➝ ", ancestry.Select(a => a.GetType().Name));
                _mainViewModel.HoveredElementText = path;
            }
            else
            {
                _mainViewModel.HoveredElementText = "Nothing under mouse";
            }
        }
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                _mainViewModel.DeleteSelectedNode();
            }
        }

        // This handler is just for visual feedback (e.g., changing the cursor).
        private void NodeCanvas_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = e.Data.GetDataPresent("ExcelFunction") ? DragDropEffects.Copy : DragDropEffects.None;
            e.Handled = true;
        }

        private DebugWindow? _debugWindow; // Field to hold a reference to the window

        private void DebugWindow_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            // If the window is not already open, create and show it.
            if (_debugWindow == null || !_debugWindow.IsLoaded)
            {
                _debugWindow = new DebugWindow(_mainViewModel);
                // When the debug window is closed, clear our reference to it.
                _debugWindow.Closed += (s, args) => _debugWindow = null;
                _debugWindow.Show();
            }
            else
            {
                // If it's already open, just bring it to the front.
                _debugWindow.Activate();
            }
        }
        private void NodeCanvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Middle mouse button is used for panning
            if (e.ChangedButton == MouseButton.Middle && e.ButtonState == MouseButtonState.Pressed)
            {
                _isPanning = true;
                _panStartPoint = e.GetPosition(this); // Get position relative to the window
                NodeCanvas.CaptureMouse();
                Mouse.OverrideCursor = Cursors.ScrollAll;
                e.Handled = true;
            }
        }


        //NodeCanvas Event Handlers
        private void NodeCanvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_isPanning)
            {
                var transformGroup = (TransformGroup)NodeCanvas.RenderTransform;
                var translateTransform = (TranslateTransform)transformGroup.Children[1];

                Point currentPoint = e.GetPosition(this);
                Vector delta = currentPoint - _panStartPoint;

                // Calculate the potential new pan position
                Point newPanPoint = new Point(translateTransform.X + delta.X, translateTransform.Y + delta.Y);

                // Constrain the new position using the helper method
                Point clampedPoint = ClampPanPoint(newPanPoint);

                translateTransform.X = clampedPoint.X;
                translateTransform.Y = clampedPoint.Y;

                _panStartPoint = currentPoint;
            }
            // Handle node dragging (this logic is separate and remains the same)
            else if (_draggedNode != null && e.LeftButton == MouseButtonState.Pressed)
            {
                Point currentPosition = e.GetPosition(NodeCanvas);
                _draggedNode.X = currentPosition.X - _dragStartOffset.X;
                _draggedNode.Y = currentPosition.Y - _dragStartOffset.Y;
            }
        }


        private void NodeCanvas_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle && _isPanning)
            {
                _isPanning = false;
                NodeCanvas.ReleaseMouseCapture();
                Mouse.OverrideCursor = null;

                // Get the final transform state and sync it back to the ViewModel
                var transformGroup = (TransformGroup)NodeCanvas.RenderTransform;
                var translateTransform = (TranslateTransform)transformGroup.Children[1];
                _mainViewModel.PanX = translateTransform.X;
                _mainViewModel.PanY = translateTransform.Y;

                e.Handled = true;
            }
        }

        private void NodeCanvas_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var transformGroup = (TransformGroup)NodeCanvas.RenderTransform;
            var scaleTransform = (ScaleTransform)transformGroup.Children[0];
            var translateTransform = (TranslateTransform)transformGroup.Children[1];

            const double ZoomSpeed = 1.05;
            const double MaxZoom = 5.0;
            const double MinZoom = 0.5;

            double currentScale = scaleTransform.ScaleX;
            double newScale = e.Delta > 0 ? currentScale * ZoomSpeed : currentScale / ZoomSpeed;
            newScale = Math.Clamp(newScale, MinZoom, MaxZoom);

            Point mousePosition = e.GetPosition(NodeCanvas);

            // Calculate the new pan offset
            double newPanX = mousePosition.X - (mousePosition.X - translateTransform.X) * (newScale / currentScale);
            double newPanY = mousePosition.Y - (mousePosition.Y - translateTransform.Y) * (newScale / currentScale);

            // Constrain the new position using the helper method
            Point clampedPoint = ClampPanPoint(new Point(newPanX, newPanY));

            // Directly apply the new scale and the clamped pan
            scaleTransform.ScaleX = newScale;
            scaleTransform.ScaleY = newScale;
            translateTransform.X = clampedPoint.X;
            translateTransform.Y = clampedPoint.Y;

            // Update the ViewModel with the final values to keep it in sync
            _mainViewModel.Scale = newScale;
            _mainViewModel.PanX = clampedPoint.X;
            _mainViewModel.PanY = clampedPoint.Y;

            e.Handled = true;
        }

        private void ScrollViewer_Drop(object sender, DragEventArgs e)
        {
            // Forward the drop event to the canvas's logic
            NodeCanvas_Drop(sender, e);
            e.Handled = true;
        }

        // Add this new method to your MainWindow class
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Get the current scale (usually 1.0 on startup)
            double currentScale = _mainViewModel.Scale;

            // Calculate the center point of the canvas
            double canvasCenterX = NodeCanvas.Width / 2;

            // Calculate the center point of the viewport (the visible area)
            double viewportCenterX = CanvasScrollViewer.ActualWidth / 2;

            // Calculate the required pan offset to align the centers
            double initialPanX = viewportCenterX - (canvasCenterX * currentScale);

            // Repeat for the Y axis
            double canvasCenterY = NodeCanvas.Height / 2;
            double viewportCenterY = CanvasScrollViewer.ActualHeight / 2;
            double initialPanY = viewportCenterY - (canvasCenterY * currentScale);

            // Get the transform objects directly
            var transformGroup = (TransformGroup)NodeCanvas.RenderTransform;
            var translateTransform = (TranslateTransform)transformGroup.Children[1];

            // Apply the initial pan
            translateTransform.X = initialPanX;
            translateTransform.Y = initialPanY;

            // Sync the values back to the ViewModel to ensure consistency
            _mainViewModel.PanX = initialPanX;
            _mainViewModel.PanY = initialPanY;
        }
        private Point ClampPanPoint(Point panPoint)
        {
            // Get the current scale
            var transformGroup = (TransformGroup)NodeCanvas.RenderTransform;
            var scaleTransform = (ScaleTransform)transformGroup.Children[0];
            double currentScale = scaleTransform.ScaleX;

            // Define a margin to ensure a part of the canvas is always visible
            double margin = 200;

            // Calculate the scaled dimensions of the canvas
            double scaledWidth = NodeCanvas.Width * currentScale;
            double scaledHeight = NodeCanvas.Height * currentScale;

            // Calculate the max pan left/up
            double maxX = CanvasScrollViewer.ActualWidth - margin;
            double maxY = CanvasScrollViewer.ActualHeight - margin;

            // Calculate the max pan right/down
            double minX = -scaledWidth + margin;
            double minY = -scaledHeight + margin;

            // Clamp and return the constrained point
            double clampedX = Math.Clamp(panPoint.X, minX, maxX);
            double clampedY = Math.Clamp(panPoint.Y, minY, maxY);

            return new Point(clampedX, clampedY);
        }
    }
}