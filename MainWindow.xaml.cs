
using CanvasTest.Controls;
using CanvasTest.Models;
using CanvasTest.ViewModels;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Printing;
using System.Text;
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

namespace CanvasTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _mainViewModel;
        
        private readonly List<Node> _managedNodes = new List<Node>();

        private Point _StartPosition; // Last known position of the mouse, used to calculate the drag distance
        private Point NodeOffset; // Offset of the mouse position relative to the node being dragged, used to keep the node under the cursor during drag
        private Node _selectedNode = null; // Initialize selected node to null, var stores the currently selected node 

        private Point? _dragStartPoint;
        private ExcelFunction _draggedItem;



        // Constructor for MainWindow
        public MainWindow()
        {
            InitializeComponent();// Initialize the components of the MainWindow
            DataContext = new MainViewModel(); // Set the DataContext to the MainViewModel to include available functions
            _mainViewModel = (MainViewModel)DataContext;
            this.Closing += MainWindow_Closing;

            MessageBox.Show($"Loaded {_mainViewModel._allFunctions.Count} functions"); // Show a message box with the count of available functions as debug information
            this.Loaded += MainWindow_Loaded; // Attach Loaded event handler
        }



        // This method is called when the MainWindow is loaded
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }


        //Method for resetting dragging values
        private void ClearDragState()
        {
            _dragStartPoint = null;
            _draggedItem = null;
            _mainViewModel.IsDragging = false;
        }

        //Find out which item the mouse is currently hovering over
        private bool IsHoveringOverElementNamed(string targetName)
        {
            DependencyObject? element = Mouse.DirectlyOver as DependencyObject;

            while (element != null)
            {
                if (element is FrameworkElement fe && fe.Name == targetName)
                {
                    return true;
                }

                // Prefer VisualTreeHelper if possible
                if (element is Visual || element is Visual3D)
                {
                    element = VisualTreeHelper.GetParent(element);
                }
                else
                {
                    element = LogicalTreeHelper.GetParent(element);
                }
            }

            return false;
        } //Finds out what is currently directly under the mouse

        private void Window_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            // Global safety net - clear drag state when mouse is released anywhere
            if (e.LeftButton == MouseButtonState.Released)
            {
                ClearDragState();
            }
        }

        // Helper method to get the visual parent of a specific type - used for drag and drop to target the correct element
        private T GetVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            var parent = child;
            while (parent != null)
            {
                if (parent is T typedParent)
                    return typedParent;
                parent = VisualTreeHelper.GetParent(parent);
            }
            return null;
        }

        // This method is called when the mouse moves over the NodeCanvas (debug to statusbar with mouse position)
        private void NodeCanvas_MousePosition(object sender, MouseEventArgs e) //Eventhandler for mousemove on nodecanvas, sender is the NodeCanvas
        {
           
        }

        // This method is called when the left mouse button is pressed on the NodeCanvas (debug to statusbar with entity name and event)
        private void NodeCanvas_LeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var elementName = ((FrameworkElement)sender).Name;// Get the name of the element clicked
            if (e.Source == NodeCanvas) // Check if the source of the event is the NodeCanvas itself
            {
                ClearSelection(); // Clear any previous selection when clicking on the canvas
            }
            if (e.LeftButton == MouseButtonState.Pressed) // Check if the left button is still pressed
            {
                MouseStatus.Text = $"Left Button Pressed {elementName}";    // Write in the status bar what has been clicked
            }

            //var position = e.GetPosition(NodeCanvas);
            //MessageBox.Show($"Canvas Clicked at \nX:{position.X:F0}, \nY:{position.Y:F0}");
        }

        private async void NodeCanvas_LeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var elementName = ((FrameworkElement)sender).Name; // Get the name of the element 'unclicked'
        

            if (e.LeftButton == MouseButtonState.Released) // Check if the left button is released
            {
                MouseStatus.Text = $"Left Button Released {elementName}"; // write in the status bar what has been unclicked
            }
            ClearDragState(); // Clear any drag state
            Mouse.Capture(null); // Release mouse capture to stop dragging
            //_connectionDrag = false;
            //NodeCanvas.Children.Remove(_previewPath);
            await Task.Delay(500); // Delay to allow the status text to be visible
            MouseStatus.Text = null; // Clear the status text
        }        // This method is called when the left mouse button is released on the NodeCanvas (debug to statusbar with entity name and event)


        private void Node_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mainViewModel.IsDragging && e.LeftButton == MouseButtonState.Pressed) // Check if dragging is active and left button is pressed
            {
                var currentposition = e.GetPosition(NodeCanvas); // Get the current mouse position relative to the Canvas
                var Node = (UIElement)sender; // Get the UIElement that is being dragged (the Node)

                //Update the element's position
                Canvas.SetLeft(Node, currentposition.X - NodeOffset.X); // Set the left position of the Node
                Canvas.SetTop(Node, currentposition.Y - NodeOffset.Y); // Set the top position of the Node

                //Update the last known mouse position
                _StartPosition = currentposition; // Store the current mouse position for the next move event
            }
        }        // This method is called when the mouse moves whilst buttons are held down and isDragging is true (to allow dragging of the Node)
        private void Node_LeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true; // Prevents the event from bubbling up to the Canvas
            
            var clickedNode = (Node)sender; // Get the Node that was clicked
            var elementName = ((FrameworkElement)sender).Name; // Get the name of the element clicked
            var hoveredElement = Mouse.DirectlyOver as DependencyObject; // Variable to store the element currently under the mouse cursor

            //ADD SELECTION LOGIC HERE
            ClearSelection(); // Clear any previous selection
            SelectNode(clickedNode); // Select the clicked node
            if (IsHoveringOverElementNamed("LeftConnection") || IsHoveringOverElementNamed("RightConnection"))
            {
                // Don't start drag from the left connection
                e.Handled = true;
                _mainViewModel.IsDragging = false; //lets make sure the node isn't being dragged
                ((UIElement)sender).CaptureMouse(); // ties the mousemovements to the node at the application level 
                return;
            }
            if (e.LeftButton == MouseButtonState.Pressed) // Check if the left button is pressed
                {
                _mainViewModel.IsDragging = true; // Set dragging state to true
                    MouseStatus.Text = $"Left Button Pressed {elementName}"; // Update the statusbar text
                    _StartPosition = e.GetPosition(NodeCanvas); // Store the initial mouse position
                    NodeOffset = e.GetPosition((UIElement)sender); // Get the offset of the mouse position relative to the node clicked
                    ((UIElement)sender).CaptureMouse(); // Capture mouse to receive mouse move events
                }
            

        }        // This method is called when the left mouse button is pressed on a Node (to allow selection and dragging of the Node)
        private async void Node_LeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true; // Prevents the event from bubbling up to the Canvas and triggering it
            var elementName = ((FrameworkElement)sender).Name; // Get the name of the element by taking the sender arg, casting it to FWE type which can provide the Name of the node
            if (e.LeftButton == MouseButtonState.Released) // Check if the left button is released, if it IS
            {
                MouseStatus.Text = $"Left Button Released {elementName}"; // Update the status text
                _mainViewModel.IsDragging = false; // Set dragging state to false ie. 'nothing is being dragged mate
                ((UIElement)sender).ReleaseMouseCapture(); // Release mouse capture
            }
            //othewise do nothing special
            //but do this stuff regardless of whether the button was released or not
            await Task.Delay(500); // Delay to allow the status text to be visible but not too long to be annoying
            MouseStatus.Text = null; // Clear the status text in the status bar
            _StartPosition = new Point(0, 0); // Reset the start position which was set when the mouse was pressed down


        } //This method is called when the left mouse button is released

        // Start drag operation
        private void FunctionListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // IMPORTANT: Clear any previous state first
            ClearDragState();
            // Check what was actually clicked
            var originalSource = e.OriginalSource as DependencyObject; // Get the original source of the mouse event as a DependencyObject

            // Find if we clicked on a ListBoxItem (not scrollbar)
            ListBoxItem clickedItem = null; // Initialize clickedItem to null, will be set if we find a ListBoxItem
            DependencyObject current = originalSource; //Set the dependancy object from the original source of the mouse event as the current object

            while (current != null && clickedItem == null) // Loop until we find a ListBoxItem, and nothing else has been clicked
            {
                // If we hit a scrollbar component, bail out
                if (current is System.Windows.Controls.Primitives.ScrollBar ||
                    current.GetType().Name.Contains("Thumb") ||
                    current.GetType().Name.Contains("Track") ||
                    current.GetType().Name.Contains("RepeatButton"))
                    {
                        return; // Don't start drag
                    }
                clickedItem = current as ListBoxItem; // Try to cast the current object to a ListBoxItem, if it is not null then we clicked on a ListBoxItem
                current = VisualTreeHelper.GetParent(current); // Move up the visual tree to find the parent ListBoxItem, if it exists
            }
            // Only prepare for drag if we found a ListBoxItem
            if (clickedItem != null && clickedItem.DataContext is ExcelFunction function) // Check if the clicked item has a DataContext of type ExcelFunction
            {
                _dragStartPoint = e.GetPosition(null);// Store the initial mouse position relative to the screen
                _draggedItem = function; // set the dragged item to the clicked function as passed in from the DataContext of the clicked ListBoxItem
            }
        } //This method is called when the left mouse button is depressed on the FunctionListBox
        private void FunctionListBox_MouseMove(object sender, MouseEventArgs e)
        {
            // Check all conditions before starting drag
            if (!_mainViewModel.IsDragging &&
                e.LeftButton == MouseButtonState.Pressed &&
                _dragStartPoint.HasValue &&
                _draggedItem != null)
            {
                Point currentPosition = e.GetPosition(null);
                Vector diff = _dragStartPoint.Value - currentPosition;

                if (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                     Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    // Capture the item to drag BEFORE starting the operation
                    var itemToDrag = _draggedItem;

                    // Set dragging flag
                    _mainViewModel.IsDragging = true;

                    // Create data object with the captured item
                    var dragData = new DataObject("ExcelFunction", itemToDrag);

                    try
                    {
                        // Start the drag operation
                        DragDrop.DoDragDrop(FunctionListBox, dragData, DragDropEffects.Copy);
                    }

                    finally
                    {
                        // ALWAYS clear state after drag completes
                        ClearDragState();
                    }
                }
            }
        } // This method is called when the mouse moves a minimum distance and IS dragging tarted from the FunctionListBox
        private void FunctionListBox_MouseLeave(object sender, MouseEventArgs e)
        {
            // Only clear if we're not in the middle of a drag operation
            if (!_mainViewModel.IsDragging)
            {
                ClearDragState();
            }
        }
        // Handle drag enter (visual feedback)
        private void NodeCanvas_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("ExcelFunction"))
            {
                e.Effects = DragDropEffects.Move;
            }
            else
            {
                e.Effects = DragDropEffects.None;
                NodeCanvas.Cursor = Cursors.No;
            }
            e.Handled = true; // Mark the event as handled to prevent further processing
        }
        private void NodeCanvas_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("ExcelFunction"))
            {
                var function = e.Data.GetData("ExcelFunction") as ExcelFunction;
                var dropPosition = e.GetPosition(NodeCanvas);

                try
                {
                    // Create new node
                    var newNode = new Node
                    {
                        FunctionName = function.Name,
                        NodeValue = "0",
                        IconPath = function.IconPath
                    };

                    // Add event handlers
                    newNode.MouseLeftButtonDown += Node_LeftButtonDown;
                    newNode.MouseLeftButtonUp += Node_LeftButtonUp;
                    newNode.MouseMove += Node_MouseMove;

                    // IMPORTANT: Track the node for cleanup
                    _managedNodes.Add(newNode);

                    // Position the node
                    Canvas.SetLeft(newNode, dropPosition.X - 70);
                    Canvas.SetTop(newNode, dropPosition.Y - 40);

                    // Add to canvas
                    NodeCanvas.Children.Add(newNode);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error creating node: {ex.Message}");
                }
            }

            ClearDragState();
            e.Handled = true;
        }
        private void NodeCanvas_DragLeave(object sender, DragEventArgs e) //
        {
            NodeCanvas.Cursor = Cursors.Arrow; // Reset cursor
            e.Handled = true; // Mark the event as handled to prevent further processing
        }// Method called when the mouse leaves the NodeCanvas during a drag operation i.e. if you drag the mouse out of the canvas

        //Selection Helper Logic
        private void SelectNode(Node node)
        {
            if (node != null)
            {
                _selectedNode = node; // Set the selected node
                node.IsSelected = true; // Mark the node as selected
            }
        }
        private void ClearSelection()
        {
            if (_selectedNode != null)
            {
                _selectedNode.IsSelected = false; // Deselect the previously selected node
                _selectedNode = null; // Clear the selected node reference
            }
        }

        #region Delete Node Logic
        private void MainWindow_KeyDown(object sender, KeyEventArgs e) // Event handler for key down events in the MainWindow
        {
            if (e.Key == Key.Delete && _selectedNode != null) // Check if Delete key is pressed and a node is selected
            {
                DeleteSelectedNode();
            }


        }
        private void DeleteSelectedNode()
        {
            if (_selectedNode != null)
            {
                // Use the cleanup method instead of just removing from canvas
                CleanupNode(_selectedNode);
                _selectedNode = null;
            }
        }
        #endregion


        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            var element = Mouse.DirectlyOver as DependencyObject;

            if (element == null)
            {
                HoveredElement.Text = "Nothing under mouse";
                return;
            }

            var ancestry = TreeHelper.GetAncestryPath(element);
            var path = string.Join(" ➝ ", ancestry.Select(a =>
                $"({a.GetType().Name}{(a is FrameworkElement fe && !string.IsNullOrEmpty(fe.Name) ? $"#{fe.Name}" : "")})"));

            HoveredElement.Text = path;
            // Existing coordinate tracking
            var canvasPosition = e.GetPosition(NodeCanvas); // Get the mouse position relative to the Canvas + store in a variable
            CoordinatesText.Text = $"X: {canvasPosition.X:F0}, Y: {canvasPosition.Y:F0}"; // Display the mouse position in the CoordinatesText TextBlock on the StatusBar
            debugPanelXCoord.Text = $"X: {canvasPosition.X:F0}";
            debugPanelYCoord.Text = $"X: {canvasPosition.Y:F0}";


        }



        private void PropertiesPanel_Collapsed(object sender, RoutedEventArgs e)
        {
            NodeCanvas.Background = null; // Reset background when properties panel is collapsed
            NodeCanvas.UpdateLayout(); // Force layout update to apply changes immediately
            NodeCanvas.Background = this.Resources["CanvasGridBrush"] as Brush; // Restore the background from resources

        }





        private void NodeCanvas_RightButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void CleanupNode(Node node)
        {
            if (node == null) return;

            try
            {
                // Remove all event handlers we added
                node.MouseLeftButtonDown -= Node_LeftButtonDown;
                node.MouseLeftButtonUp -= Node_LeftButtonUp;
                node.MouseMove -= Node_MouseMove;

                // Remove from our tracking list
                _managedNodes.Remove(node);

                // Remove from canvas
                NodeCanvas.Children.Remove(node);

                // Optional: Explicitly dispose if node implements IDisposable
                if (node is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
            catch (Exception ex)
            {
                // Log error but don't crash
                System.Diagnostics.Debug.WriteLine($"Error cleaning up node: {ex.Message}");
            }
        }
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Clean up all managed nodes
            var nodesToCleanup = _managedNodes.ToList(); // Create copy to avoid modification during iteration
            foreach (var node in nodesToCleanup)
            {
                CleanupNode(node);
            }
            _managedNodes.Clear();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            debugMouseLeft.Fill = Brushes.LightGreen;
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            debugMouseLeft.Fill = null;
        }

        private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            debugMouseRight.Fill = Brushes.LightGreen;
        }

        private void Window_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            debugMouseRight.Fill = null;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                debugMouseMiddle.Fill = Brushes.LightGreen;
            }
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Released)
            {
                debugMouseMiddle.Fill = null;
            }
        }
    }
}

