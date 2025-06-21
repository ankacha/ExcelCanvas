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
        private int _gridSizeX = 5000;
        private int _gridSizeY = 5000;
        private Color _lineColor = Color.FromArgb(0x22, 0x00, 0x00, 0x00);
        private Color _backgroundColor = Color.FromArgb(255, 255, 255, 255);
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

                //set the major, minor and origin line styles
                if ( x % 5000 == 0)
                {
                    verticalLine.StrokeThickness = 4;
                    verticalLine.Stroke = new SolidColorBrush(Colors.Blue);
                }
                else if ( x % 1000 == 0) 
                {
                    verticalLine.StrokeThickness = 4;
                    verticalLine.Stroke = new SolidColorBrush(Colors.LightBlue);

                }
                else
                {
                    verticalLine.StrokeThickness = 2;
                }
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
                //set the major, minor and origin line styles

                if (y % 5000 == 0)
                {
                    horizontalLine.StrokeThickness = 4;
                    horizontalLine.Stroke = new SolidColorBrush(Colors.DarkRed);
                }
                else if (y % 1000 == 0)
                {
                    horizontalLine.StrokeThickness = 4;
                    horizontalLine.Stroke = new SolidColorBrush(Colors.Coral);

                }
                else
                {
                    horizontalLine.StrokeThickness = 2;
                }
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