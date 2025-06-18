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

        private void WorkCanvas_Drop(object sender, DragEventArgs e)
        {
            Point dropPosition = e.GetPosition(WorkCanvas);

            if (e.Data.GetData("ExcelFunction") is ExcelFunction function)
            {
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
                Point currentPosition = e.GetPosition(WorkCanvas);
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

        private void WorkCanvas_LeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // If the user clicks on the empty canvas, deselect any selected node.
            if (e.Source == WorkCanvas)
            {
                _mainViewModel.SelectedNode = null;
            }
        }
        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            // Existing logic for coordinates...
            var canvasPosition = e.GetPosition(WorkCanvas);
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
        private void WorkCanvas_DragEnter(object sender, DragEventArgs e)
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



        //WorkCanvas Event Handlers
        private void WorkCanvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {

            // Handle node dragging (this logic is separate and remains the same)
            if (_draggedNode != null && e.LeftButton == MouseButtonState.Pressed)
            {
                Point currentPosition = e.GetPosition(WorkCanvas);
                _draggedNode.X = currentPosition.X - _dragStartOffset.X;
                _draggedNode.Y = currentPosition.Y - _dragStartOffset.Y;
            }
        }




    }
}