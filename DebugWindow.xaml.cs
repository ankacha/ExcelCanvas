using CanvasTest.ViewModels;
using System.Windows;

namespace CanvasTest
{
    public partial class DebugWindow : Window
    {
        public DebugWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            // Set the DataContext of this window to the existing MainViewModel.
            // This is how all the bindings in the XAML will find their data.
            DataContext = viewModel;
        }
    }
}