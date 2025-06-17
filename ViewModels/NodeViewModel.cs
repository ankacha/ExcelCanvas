using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CanvasTest.Models;

namespace CanvasTest.ViewModels
{
    public class NodeViewModel : INotifyPropertyChanged
    {

        public Guid Id { get; }
        public ExcelFunction FunctionModel { get; }

        private double _x;
        public double X
        {
            get => _x;
            set { _x = value; OnPropertyChanged(); }
        }

        private double _y;
        public double Y
        {
            get => _y;
            set { _y = value; OnPropertyChanged(); }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged();
                }
            }
        }
        //Properties to be displayed by the UI
        public string FunctionName => FunctionModel.Name;
        public string IconPath => FunctionModel.IconPath;
        public string NodeValue { get; set; } = "0"; //need to change this later

        public NodeViewModel(ExcelFunction functionModel, double x, double y)
        {
            Id = Guid.NewGuid();
            FunctionModel = functionModel;
            _x = x;
            _y = y;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
















    }
}
