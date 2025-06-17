// ViewModels/MainViewModel.cs
using CanvasTest.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows;

namespace CanvasTest.ViewModels  // Add .ViewModels here
{
    public class MainViewModel : INotifyPropertyChanged // define the MainViewModel class + implement Interface for telling ui when ViewModel has changed.
    {
        public ObservableCollection<ExcelFunction> AvailableFunctions { get; set; } /* define the AvailableFunctions property to hold available 
                                                                                      Excel functions,  ObservableCollection Notifies the UI that the list has 
                                                                                        been updated. allow get and set methods*/
        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        #region Search and filtering logic
        //specific to buttons, ICommand + ICollectionView
        
        public ICommand ClearSearchCommand { get; }

        private string _searchText;
        public readonly ObservableCollection<ExcelFunction> _allFunctions;

        public ICollectionView FilteredFunctions { get; }

        public bool IsClearButtonVisible => !string.IsNullOrEmpty(SearchText);

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value) //only triggers if the value has actually changed
                {
                    _searchText = value;
                    OnPropertyChanged(); // Notify UI that SearchText has changed
                    OnPropertyChanged(nameof(IsClearButtonVisible)); //Tell the UI that the dependent property has also changed!
                    FilteredFunctions.Refresh(); // Trigger the filter to be re-applied
                }
            }
        }
        
        // Rulebook for the ICollectionView MainViewModel.FilteredFunctions.Filter method.

        private bool FilterFunctions(object item)
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                return true; // No filter, show all items
            }

            if (item is ExcelFunction function)
            {
                return function.Name.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                       function.Description.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0;
            }

            return false;
        }

        private void ClearSearch(object parameter)
        {
            SearchText = string.Empty;
        }

        #endregion

        public MainViewModel() // constructor for MainViewModel class
        {
            //AvailableFunctions = new ObservableCollection<ExcelFunction>( // initialize the AvailableFunctions property with a ->//
            //    ExcelFunction.GetAvailableFunctions()                     //->  collection of ExcelFunction objects
            //);
            _allFunctions = new ObservableCollection<ExcelFunction>(ExcelFunction.GetAvailableFunctions()); //instantiate a new list from the Model
            FilteredFunctions = CollectionViewSource.GetDefaultView(_allFunctions); //Initial condition of the list
            FilteredFunctions.Filter = FilterFunctions; //Setting the .Filter method rulebook

            ClearSearchCommand = new RelayCommand(p => SearchText = string.Empty);
            DeleteSelectedNodeCommand = new RelayCommand(p => DeleteSelectedNode(), p => SelectedNode != null);
        }

        #region Dragging properties and logic for UI
        //Drag state for nodes
        private bool _isNodeDragging;
        public bool IsNodeDragging
        {
            get => _isNodeDragging;
            set
            {
                if (_isNodeDragging != value)
                {
                    _isNodeDragging = value;
                    OnPropertyChanged(); // This notifies the UI to update
                }
            }
        }

        //Drag State for Connections
        private bool _isConnectionDragging;
        public bool IsConnectionDragging
        {
            get => _isConnectionDragging;
            set
            {
                if (_isConnectionDragging != value)
                {
                    _isConnectionDragging = value;
                    OnPropertyChanged();
                }
            }
        }

            //Continutous 'new' starting position for calculation of the mousemove
            //NOT THE ORIGINAL POSITION
        private Point _nodeDragPreviousPosition;
        public Point NodeDragPreviousPosition
        {
            get => _nodeDragPreviousPosition;
            set
            {
                if (_nodeDragPreviousPosition != value)
                {
                    _nodeDragPreviousPosition = value;
                    OnPropertyChanged();
                }
            }

        }

        //Gets the node's original position for drag canelling
        private Point _nodeOriginalPosition;
        public Point NodeOriginalPosition
        {
            get => _nodeOriginalPosition;
            set
            {
                if (_nodeOriginalPosition != value)
                {
                    _nodeOriginalPosition = value;
                    OnPropertyChanged();
                }
            }

        }

        // D E B U G  P R O P E R T I E S
        private string _mousePostitionText = "X 0, Y 0";
        public string MousePostitionText
        {
            get => _mousePostitionText;
            set
            {
                if (_mousePostitionText != value)
                {
                    _mousePostitionText = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _hoveredElementText = "Nothing under mouse";
        public string HoveredElementText
        {
            get => _hoveredElementText;
            set
            {
                if (_hoveredElementText != value)
                {
                    _hoveredElementText = value;
                    OnPropertyChanged();
                }
            }


        }
    }
    #endregion
}