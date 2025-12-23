using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace PrimesWithCircles.Controls
{
    public class RotationCanvas : Canvas, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string? propName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));


        #region PROPERTIES AND FIELDS

        private readonly List<Circle> circles = [];
        private LapLine? lapLine;
        private ScaleTransform ZoomTransform { get; set; }

        private PresentationMode presentationMode = PresentationMode.SeekPrimes;
        public PresentationMode PresentationMode
        {
            get => presentationMode;
            set
            {
                if (presentationMode != value)
                {
                    presentationMode = value;
                    Reset();
                    OnPropertyChanged(nameof(PresentationMode));
                }
            }
        }

        public Theme Theme { get;set; }

        public SolidColorBrush CounterColor
        {
            get => Theme.CounterColor;
            set
            {
                if (Theme.CounterColor != value)
                {
                    Theme.CounterColor = value;
                    OnPropertyChanged(nameof(CounterColor));
                }
            }
        }

        public SolidColorBrush PrimesColor
        {
            get => Theme.PrimesColor;
            set
            {
                if (Theme.PrimesColor != value)
                {
                    Theme.PrimesColor = value;
                    OnPropertyChanged(nameof(PrimesColor));
                }
            }
        }

        private bool isReseted = true;
        public bool IsReseted
        {
            get => isReseted;
            set
            {
                if (isReseted != value)
                {
                    isReseted = value;
                    OnPropertyChanged(nameof(IsReseted));
                }
            }
        }

        public event Action? PrimesChanged;
        private string primes = "";
        public string Primes
        {
            get => primes;
            set
            {
                if (primes != value)
                {
                    primes = value;
                    OnPropertyChanged(nameof(Primes));
                    PrimesChanged?.Invoke();
                }
            }
        }

        public double StartAngle { get; set; } = -Math.PI / 2;  // start at top: -π/2

        private int lapCounter = 0;
        public int LapCounter
        {
            get => lapCounter;
            set
            {
                if (lapCounter != value)
                {
                    lapCounter = value;
                    OnPropertyChanged(nameof(LapCounter));
                }
            }
        }

        private bool autoRotation = true;
        public bool AutoRotation
        {
            get => autoRotation;
            set
            {
                if (autoRotation != value)
                {
                    autoRotation = value;
                    OnPropertyChanged(nameof(AutoRotation));
                }
            }
        }

        public SolidColorBrush BackgroundColor
        {
            get => Theme.BackgroundColor;
            set
            {
                if (Theme.BackgroundColor != value)
                {
                    Theme.BackgroundColor = value;
                    OnPropertyChanged(nameof(BackgroundColor));
                }
            }
        }

        public double BaseRadious
        {
            get => Circle.BaseRadious;
            set
            {
                if (Circle.BaseRadious != value)
                {
                    Circle.BaseRadious = value;
                    UpdateBaseRadious();
                    AdjustZoom();
                    OnPropertyChanged(nameof(BaseRadious));
                }
            }
        }
        public double BaseRadiousMin => 20.0;
        public double BaseRadiousMax => 200.0;

        private bool displayLapLine = true;
        public bool DisplayLapLine
        {
            get => displayLapLine;
            set
            {
                if (displayLapLine != value)
                {
                    displayLapLine = value;
                    lapLine.line.Visibility = displayLapLine ? Visibility.Visible : Visibility.Hidden;
                    OnPropertyChanged(nameof(DisplayLapLine));
                }
            }
        }

        public bool DisplayCircles
        {
            get => Circle.DisplayCircles;
            set
            {
                if (Circle.DisplayCircles != value)
                {
                    Circle.DisplayCircles = value;
                    UpdateCirclesVisibility();
                    OnPropertyChanged(nameof(DisplayCircles));
                }
            }
        }

        public bool DisplayTrails
        {
            get => Circle.DisplayTrails;
            set
            {
                if (Circle.DisplayTrails != value)
                {
                    Circle.DisplayTrails = value;
                    UpdateTrailsVisibility();
                    OnPropertyChanged(nameof(DisplayTrails));
                }
            }
        }

        private bool displayPrimes = true;
        public bool DisplayPrimes
        {
            get => displayPrimes;
            set
            {
                if (displayPrimes != value)
                {
                    displayPrimes = value;
                    OnPropertyChanged(nameof(DisplayPrimes));
                }
            }
        }

        private double rotationSpeed = 10;
        public double RotationSpeed
        {
            get => rotationSpeed;
            set
            {
                if (rotationSpeed != value)
                {
                    rotationSpeed = value;
                    OnPropertyChanged(nameof(RotationSpeed));
                }
            }
        }
        public double RotationSpeedMin => 0.1;
        public double RotationSpeedMax => 100.0;
        
        public double PointerSize
        {
            get => Circle.PointerSize;
            set
            {
                if (Circle.PointerSize != value)
                {
                    Circle.PointerSize = value;
                    UpdatePointerSizes();
                    OnPropertyChanged(nameof(PointerSize));

                }
            }
        }
        public double PointerSizeMin => 4.0;
        public double PointerSizeMax => 40.0;
        
        public double LapLineThickness
        {
            get => LapLine.Thickness;
            set
            {
                if (LapLine.Thickness != value)
                {
                    LapLine.Thickness = value;
                    lapLine.Rescale();
                    OnPropertyChanged(nameof(LapLineThickness));

                }
            }
        }
        public double LapLineThicknessMin => 1.0;
        public double LapLineThicknessMax => 10.0;

        public double CircleThickness
        {
            get => Circle.CircleThickness;
            set
            {
                if (Circle.CircleThickness != value)
                {
                    Circle.CircleThickness = value;
                    UpdateCircleThicknesses();
                    OnPropertyChanged(nameof(CircleThickness));

                }
            }
        }
        public double CircleThicknessMin => 1.0;
        public double CircleThicknessMax => 10.0;
        
        public double TrailThickness
        {
            get => Circle.TrailThickness;
            set
            {
                if (Circle.TrailThickness != value)
                {
                    Circle.TrailThickness = value;
                    UpdateTrailThicknesses();
                    OnPropertyChanged(nameof(TrailThickness));

                }
            }
        }
        public double TrailThicknessMin => 3.0;
        public double TrailThicknessMax => 10.0;

        public double CurrentScale { get; set; } = 1.0;


        #endregion


        #region CONSTRUCTORS

        public RotationCanvas()
        {
            ZoomTransform  = new ScaleTransform(CurrentScale, CurrentScale);
            LayoutTransform = ZoomTransform;

            Theme = Themes.GetTheme(ThemeType.ClassicNeon);
        }

        #endregion


        #region METHODS

        /// <summary>
        /// Add a new circle representing the given number
        /// </summary>
        public void AddCircle(int number)
        {
            var circle = new Circle(this, number);
            this.circles.Add(circle);

            Children.Insert(circles.Count, circle.Shape);
            Children.Insert(circles.Count * 2, circle.Trail);
            Children.Insert(circles.Count * 3, circle.Pointer);

            circle.Center();
        }

        /// <summary>
        /// Add the lap line
        /// </summary>
        public void AddLine()
        {
            lapLine = new LapLine(this);
            lapLine.Center();
            Children.Add(lapLine.line);
        }

        /// <summary>
        /// Update the colors from the theme
        /// </summary>
        public void UpdateFromTheme()
        {
            if (lapLine != null)
                lapLine.UpdateFromTheme();
            foreach (var circle in circles)
            {
                circle.UpdateFromTheme();
            }
        }

        /// <summary>
        /// Add a new circle for the given prime number
        /// </summary>
        /// <param name="number"></param>
        public void AddPrime(int number)
        {
            AddCircle(number);
            AdjustZoom();

            if (Primes.Length > 0)
                Primes += ", ";
            Primes += number.ToString();
        }

        /// <summary>
        /// Clear all circles and reset to initial state with circles 1 and 2
        /// </summary>
        public void Reset()
        {
            circles.Clear();
            Children.Clear();
            IsReseted = true;
            CurrentScale = 1.0;
            ZoomTransform.ScaleX = CurrentScale;
            ZoomTransform.ScaleY = CurrentScale;
            Zoom(CurrentScale);

            AddLine();

            if (PresentationMode == PresentationMode.OneCircle)
            {
                AddCircle(1);
                LapCounter = 0;
                DisplayPrimes = false;
            }
            else if (PresentationMode == PresentationMode.TwoCircles)
            {
                AddCircle(1);
                AddCircle(2);
                LapCounter = 0;
                DisplayPrimes = false;
            }
            else if (PresentationMode == PresentationMode.SeekPrimes)
            {
                AddCircle(1);
                AddCircle(2);
                LapCounter = 2;
                DisplayPrimes = true;
            }

            Primes = $"primes: {LapCounter}";

            AdjustZoom();
        }

        /// <summary>
        /// Center all elements in the canvas
        /// </summary>
        public void CenterAll()
        {
            if (lapLine != null)
                lapLine.Center();

            foreach (var circle in circles)
            {
                circle.Center();
            }
        }

        /// <summary>
        /// Set radius for all circles based on BaseRadious
        /// </summary>
        public void UpdateBaseRadious()
        {
            foreach (var circle in circles)
            {
                circle.UpdateRadious();
            }
        }

        /// <summary>
        /// Set pointer sizes for all circles
        /// </summary>
        public void UpdatePointerSizes()
        {
            foreach (var circle in circles)
            {
                circle.UpdatePointerSize();
            }
        }

        /// <summary>
        /// Set shape thicknesses for all circles
        /// </summary>
        public void UpdateCircleThicknesses()
        {
            foreach (var circle in circles)
            {
                circle.UpdateCircleThickness();
            }
        }

        /// <summary>
        /// Set trail thicknesses for all circles
        /// </summary>
        public void UpdateTrailThicknesses()
        {
            foreach (var circle in circles)
            {
                circle.UpdateTrailThickness();
            }
        }

        /// <summary>
        /// Set shape visibility for all circles
        /// </summary>
        public void UpdateCirclesVisibility()
        {
            foreach (var circle in circles)
            {
                circle.UpdateCircleVisibility();
            }
        }

        /// <summary>
        /// Set trail visibility for all circles
        /// </summary>
        public void UpdateTrailsVisibility()
        {
            foreach (var circle in circles)
            {
                circle.UpdateTrailVisibility();
            }
        }

        /// <summary>
        /// Rotate all circles for elapsedSec seconds. Stops and handles lap logic for first circle.
        /// </summary>
        public (bool FirstCompleted, bool SomeOtherCompleted) RotateCircles()
        {
            bool firstCompleted = false;
            bool someOtherCompleted = false;

            // rotate all circles
            foreach (var circle in circles)
            {
                bool lapCompleted = circle.RotateCircle(firstCompleted);
                if (circle.Number == 1 && lapCompleted)
                    firstCompleted = true;

                if (circle.Number > 1 && firstCompleted && lapCompleted)
                    someOtherCompleted = true;

                circle.RedrawCircle();
            }

            return (firstCompleted, someOtherCompleted);
        }

        /// <summary>
        /// Returns the largest radius value among all circles in the collection.
        /// </summary>
        public double GetMaxRadious()
        {
            if (this.circles.Count == 0)
                return 0.0;

            return this.circles.Max(c => c.Radious);
        }

        /// <summary>
        /// Rescale all elements according to the given currentScale value.
        /// </summary>
        /// <param name="currentScale"></param>
        public void RescaleAll()
        {
            lapLine?.Rescale();

            foreach (var circle in circles)
            {
                circle.Rescale();
            }
        }

        /// <summary>
        /// Zoom with animation
        /// </summary>
        /// <param name="targetScale"></param>
        public void Zoom(double targetScale)
        {
            CurrentScale = targetScale;

            DoubleAnimation anim = new()
            {
                To = CurrentScale,
                Duration = TimeSpan.FromMilliseconds(1000),
                EasingFunction = new QuinticEase { EasingMode = EasingMode.EaseOut }
            };

            ZoomTransform.BeginAnimation(ScaleTransform.ScaleXProperty, anim);
            ZoomTransform.BeginAnimation(ScaleTransform.ScaleYProperty, anim);

            RescaleAll();
        }

        /// <summary>
        /// Adjust the zoom level automatically to fit all circles in the canvas
        /// </summary>
        public void AdjustZoom()
        {
            if (circles.Count == 0) return;

            double maxRadius = GetMaxRadious();
            double neededSize = maxRadius * 2 + 100; // buffer

            double actualWidthWithoutScale = ActualWidth * ZoomTransform.ScaleX;
            double actualHeightWithoutScale = ActualHeight * ZoomTransform.ScaleX;

            double scaleX = actualWidthWithoutScale / neededSize;
            double scaleY = actualHeightWithoutScale / neededSize;

            double newScale = Math.Min(scaleX, scaleY);

            if (newScale < 1)
            {
                Zoom(newScale);
            }

        }

        #endregion
    }

}
