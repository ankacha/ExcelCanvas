using CanvasTest.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace CanvasTest.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        // --- Properties for the Search/Filter ListBox ---
        public ICollectionView FilteredFunctions { get; }
        private readonly ObservableCollection<ExcelFunction> _allFunctions;
        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsClearButtonVisible));
                    FilteredFunctions.Refresh();
                }
            }
        }
        public bool IsClearButtonVisible => !string.IsNullOrEmpty(SearchText);
        public ICommand ClearSearchCommand { get; }


        // --- Properties for the Node Canvas ---

        // This MUST be public for the UI to bind to it.
        public ObservableCollection<NodeViewModel> Nodes { get; } = new ObservableCollection<NodeViewModel>();

        private NodeViewModel? _selectedNode;
        public NodeViewModel? SelectedNode
        {
            get => _selectedNode;
            set
            {
                if (_selectedNode != value)
                {
                    if (_selectedNode != null) _selectedNode.IsSelected = false;
                    _selectedNode = value;
                    if (_selectedNode != null) _selectedNode.IsSelected = true;
                    OnPropertyChanged();
                }
            }
        }
        public ICommand DeleteSelectedNodeCommand { get; }


        // --- Constructor ---
        public MainViewModel()
        {
            _allFunctions = new ObservableCollection<ExcelFunction>(ExcelFunction.GetAvailableFunctions());
            FilteredFunctions = CollectionViewSource.GetDefaultView(_allFunctions);
            FilteredFunctions.Filter = FilterFunctions;

            ClearSearchCommand = new RelayCommand(p => SearchText = string.Empty);
            DeleteSelectedNodeCommand = new RelayCommand(p => DeleteSelectedNode(), p => SelectedNode != null);
        }

        // --- Methods to Manipulate the Canvas ---
        public void AddNode(ExcelFunction function, Point position)
        {
            var newNode = new NodeViewModel(function, position.X, position.Y);
            Nodes.Add(newNode);
            SelectedNode = newNode;
        }

        public void DeleteSelectedNode()
        {
            if (SelectedNode != null)
            {
                Nodes.Remove(SelectedNode);
                SelectedNode = null;
            }
        }

        private bool FilterFunctions(object item)
        {
            if (string.IsNullOrEmpty(SearchText)) return true;
            if (item is ExcelFunction function)
            {
                return function.Name.IndexOf(SearchText, System.StringComparison.OrdinalIgnoreCase) >= 0 ||
                       function.Description.IndexOf(SearchText, System.StringComparison.OrdinalIgnoreCase) >= 0;
            }
            return false;
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
}