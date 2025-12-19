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
        private LapLine lapLine;
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

        private double baseRadious = 80.0;
        public double BaseRadious
        {
            get => baseRadious;
            set
            {
                if (baseRadious != value)
                {
                    baseRadious = value;
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

        private bool displayCircles = true;
        public bool DisplayCircles
        {
            get => displayCircles;
            set
            {
                if (displayCircles != value)
                {
                    displayCircles = value;
                    UpdateCirclesVisibility();
                    OnPropertyChanged(nameof(DisplayCircles));
                }
            }
        }

        private bool displayTrails = true;
        public bool DisplayTrails
        {
            get => displayTrails;
            set
            {
                if (displayTrails != value)
                {
                    displayTrails = value;
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

        private double pointerSize = 10.0;
        public double PointerSize
        {
            get => pointerSize;
            set
            {
                if (pointerSize != value)
                {
                    pointerSize = value;
                    UpdatePointerSizes();
                    OnPropertyChanged(nameof(PointerSize));

                }
            }
        }
        public double PointerSizeMin => 4.0;
        public double PointerSizeMax => 40.0;

        private double lapLineThickness = 1.5;
        public double LapLineThickness
        {
            get => lapLineThickness;
            set
            {
                if (lapLineThickness != value)
                {
                    lapLineThickness = value;
                    lapLine.line.StrokeThickness = lapLineThickness;
                    OnPropertyChanged(nameof(LapLineThickness));

                }
            }
        }
        public double LapLineThicknessMin => 1.0;
        public double LapLineThicknessMax => 10.0;

        private double circleThickness = 1.5;
        public double CircleThickness
        {
            get => circleThickness;
            set
            {
                if (circleThickness != value)
                {
                    circleThickness = value;
                    UpdateCircleThicknesses();
                    OnPropertyChanged(nameof(CircleThickness));

                }
            }
        }
        public double CircleThicknessMin => 3.0;
        public double CircleThicknessMax => 10.0;

        private double trailThickness = 2;
        public double TrailThickness
        {
            get => trailThickness;
            set
            {
                if (trailThickness != value)
                {
                    trailThickness = value;
                    UpdateTrailThicknesses();
                    OnPropertyChanged(nameof(TrailThickness));

                }
            }
        }
        public double TrailThicknessMin => 3.0;
        public double TrailThicknessMax => 10.0;

        public double CurrentScale { get; set; } = 1.0;

        public Visibility CirclesVisibility
        {
            get => DisplayCircles == true ? Visibility.Visible : Visibility.Hidden;
        }

        public Visibility TrailsVisibility
        {
            get => DisplayTrails == true ? Visibility.Visible : Visibility.Hidden;
        }

        #endregion


        #region CONSTRUCTORS

        public RotationCanvas()
        {
            ZoomTransform  = new ScaleTransform(CurrentScale, CurrentScale);
            LayoutTransform = ZoomTransform;

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

        public void AddLine()
        {
            lapLine = new LapLine(this);
            lapLine.Center();

            Children.Add(lapLine.line);
        }

        public void AddPrime(int number)
        {
            AddCircle(number);
            AdjustZoom();
        }

        /// <summary>
        /// Clear all circles and reset to initial state with circles 1 and 2
        /// </summary>
        public void Reset()
        {
            circles.Clear();
            Children.Clear();
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
                circle.UpdateShapeVisibility();
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
            if (lapLine != null)
                lapLine.Rescale();

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
