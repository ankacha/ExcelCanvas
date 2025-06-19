using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CanvasTest.ViewModels
{
    public class GridViewModel : INotifyPropertyChanged
    {
        // --- Private Fields ---
        private int _gridSizeX = 4000;
        private int _gridSizeY = 4000;
        private Color _lineColor = Color.FromArgb(0xff, 0xa8, 0xa8, 0xa8);
        private Color _backgroundColor = Color.FromArgb(0xff, 0xff, 0xff, 0xff);
        private Visibility _gridVisibility = Visibility.Visible;

        // --- Public Properties ---
        public ObservableCollection<Shape> GridLines { get; } = new ObservableCollection<Shape>();

        public Color LineColor
        {
            get => _lineColor;
            set
            {
                if (_lineColor != value)
                {
                    _lineColor = value;
                    foreach (var line in GridLines.OfType<Line>())
                    {
                        line.Stroke = new SolidColorBrush(_lineColor);
                    }
                    OnPropertyChanged();
                }
            }
        }

        public Brush BackgroundBrush => new SolidColorBrush(_backgroundColor);

        public Visibility GridVisibility
        {
            get => _gridVisibility;
            set
            {
                if (_gridVisibility != value)
                {
                    _gridVisibility = value;
                    foreach (var line in GridLines)
                    {
                        line.Visibility = _gridVisibility;
                    }
                    OnPropertyChanged();
                }
            }
        }


        // --- Constructor ---
        public GridViewModel()
        {
            DrawGridLines(_gridSizeX, _gridSizeY);
        }

        // --- Methods ---
        public void DrawGridLines(int gridSizeX, int gridSizeY)
        {
            GridLines.Clear(); // Start fresh

            // Vertical Lines
            for (int x = -gridSizeX; x <= gridSizeX; x += 100)
            {
                Line verticalLine = new Line
                {
                    Stroke = new SolidColorBrush(_lineColor),
                    X1 = x,
                    Y1 = -gridSizeY,
                    X2 = x,
                    Y2 = gridSizeY,
                    Visibility = _gridVisibility
                };
                verticalLine.StrokeThickness = (x % 1000 == 0) ? 8 : 2;
                GridLines.Add(verticalLine);
            }

            // Horizontal Lines
            for (int y = -gridSizeY; y <= gridSizeY; y += 100)
            {
                Line horizontalLine = new Line
                {
                    Stroke = new SolidColorBrush(_lineColor),
                    X1 = -gridSizeX,
                    Y1 = y,
                    X2 = gridSizeX,
                    Y2 = y,
                    Visibility = _gridVisibility
                };
                horizontalLine.StrokeThickness = (y % 1000 == 0) ? 8 : 2;
                GridLines.Add(horizontalLine);
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}