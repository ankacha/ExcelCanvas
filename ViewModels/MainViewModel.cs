// ViewModels/MainViewModel.cs
using CanvasTest.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Input;

namespace CanvasTest.ViewModels  // Add .ViewModels here
{
    public class MainViewModel : INotifyPropertyChanged // define the MainViewModel class + implement Interface for telling ui when ViewModel has changed.
    {
        public ICommand ClearSearchCommand { get; }
        public ObservableCollection<ExcelFunction> AvailableFunctions { get; set; } /* define the AvailableFunctions property to hold available 
                                                                                      Excel functions,  ObservableCollection Notifies the UI that the list has 
                                                                                        been updated. allow get and set methods*/
        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


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


        public MainViewModel() // constructor for MainViewModel class
        {
            //AvailableFunctions = new ObservableCollection<ExcelFunction>( // initialize the AvailableFunctions property with a ->//
            //    ExcelFunction.GetAvailableFunctions()                     //->  collection of ExcelFunction objects
            //);
            _allFunctions = new ObservableCollection<ExcelFunction>(ExcelFunction.GetAvailableFunctions()); //instantiate a new list from the Model
            FilteredFunctions = CollectionViewSource.GetDefaultView(_allFunctions); //Initial condition of the list
            FilteredFunctions.Filter = FilterFunctions; //Setting the .Filter method rulebook

            ClearSearchCommand = new RelayCommand(ClearSearch);
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

        //Drag state for nodes
        private bool _isDragging;
        public bool IsDragging
        {
            get => _isDragging;
            set
            {
                if (_isDragging != value)
                {
                    _isDragging = value;
                    OnPropertyChanged(); // This notifies the UI to update
                }
            }
        }

        //Drag State for Connections
        // put some code here
        //

        //Starting position on Drag


    }
}