using PrimesWithCircles.Enums;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Security.Policy;
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

        //public Theme Theme { get;set; }

        

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

        public Theme Theme { get; set; }
        public Brush PrimesColor => Theme.PrimesColor;
        public Brush CounterColor => Theme.CounterColor;
        public Brush BackgroundColor => Theme.BackgroundColor;

        public double CurrentScale { get; set; } = 1.0;

        #endregion


        #region Dependency Properties

        public double RotationSpeed
        {
            get => (double)GetValue(RotationSpeedProperty);
            set => SetValue(RotationSpeedProperty, value);
        }
        public static readonly DependencyProperty RotationSpeedProperty =
            DependencyProperty.Register(
                nameof(RotationSpeed),
                typeof(double),
                typeof(RotationCanvas),
                new FrameworkPropertyMetadata(
                    10.0, // default value
                    FrameworkPropertyMetadataOptions.AffectsRender,
                    OnRotationSpeedChanged
                    ));
        private static void OnRotationSpeedChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var canvas = (RotationCanvas)d;

            double newSpeed = (double)e.NewValue;
            if (newSpeed < 0)
                canvas.RotationSpeed = 0;
        }
        public double RotationSpeedMin => 5;
        public double RotationSpeedMax => 50.0;

        public bool IsReseted
        {
            get => (bool)GetValue(IsResetedProperty);
            set => SetValue(IsResetedProperty, value);
        }
        public static readonly DependencyProperty IsResetedProperty =
            DependencyProperty.Register(
                nameof(IsReseted),
                typeof(bool),
                typeof(RotationCanvas),
                new PropertyMetadata(true));


        public double BaseRadious
        {
            get => (double)GetValue(BaseRadiousProperty);
            set => SetValue(BaseRadiousProperty, value);
        }
        public static readonly DependencyProperty BaseRadiousProperty =
            DependencyProperty.Register(
                nameof(BaseRadious),
                typeof(double),
                typeof(RotationCanvas),
                new FrameworkPropertyMetadata(
                    Circle.BaseRadious, // default value
                    FrameworkPropertyMetadataOptions.AffectsRender,
                    OnBaseRadiousChanged
                    ));
        private static void OnBaseRadiousChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var canvas = (RotationCanvas)d;
            double value = (double)e.NewValue;

            double clamped = Math.Clamp(value, canvas.BaseRadiousMin, canvas.BaseRadiousMax);
            if (!value.Equals(clamped))
            {
                canvas.BaseRadious = clamped;
                return;
            }

            Circle.UpdateBaseRadious(value, canvas.circles);
            canvas.AdjustZoom();
        }
        public double BaseRadiousMin => 20.0;
        public double BaseRadiousMax => 200.0;


        public double PointerSize
        {
            get => (double)GetValue(PointerSizeProperty);
            set => SetValue(PointerSizeProperty, value);
        }
        public static readonly DependencyProperty PointerSizeProperty =
            DependencyProperty.Register(
                nameof(PointerSize),
                typeof(double),
                typeof(RotationCanvas),
                new FrameworkPropertyMetadata(
                    Circle.PointerSize, // default value
                    FrameworkPropertyMetadataOptions.AffectsRender,
                    OnPointerSizeChanged
                    ));
        private static void OnPointerSizeChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var canvas = (RotationCanvas)d;
            double value = (double)e.NewValue;

            double clamped = Math.Clamp(value, canvas.PointerSizeMin, canvas.PointerSizeMax);
            if (!value.Equals(clamped))
            {
                canvas.PointerSize = clamped;
                return;
            }

            Circle.UpdatePointerSizes(value, canvas.circles);

        }
        public double PointerSizeMin => 4.0;
        public double PointerSizeMax => 40.0;

        public double CircleThickness
        {
            get => (double)GetValue(CircleThicknessProperty);
            set => SetValue(CircleThicknessProperty, value);
        }
        public static readonly DependencyProperty CircleThicknessProperty =
            DependencyProperty.Register(
                nameof(CircleThickness),
                typeof(double),
                typeof(RotationCanvas),
                new FrameworkPropertyMetadata(
                    Circle.CircleThickness, // default value
                    FrameworkPropertyMetadataOptions.AffectsRender,
                    OnCircleThicknessChanged
                    ));
        private static void OnCircleThicknessChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var canvas = (RotationCanvas)d;
            double value = (double)e.NewValue;

            double clamped = Math.Clamp(value, canvas.CircleThicknessMin, canvas.CircleThicknessMax);
            if (!value.Equals(clamped))
            {
                canvas.CircleThickness = clamped;
                return;
            }
            Circle.UpdateCirclesThicknesses(value, canvas.circles);
        }
        public double CircleThicknessMin => 1.0;
        public double CircleThicknessMax => 10.0;

        public double TrailThickness
        {
            get => (double)GetValue(TrailThicknessProperty);
            set => SetValue(TrailThicknessProperty, value);
        }
        public static readonly DependencyProperty TrailThicknessProperty =
            DependencyProperty.Register(
                nameof(TrailThickness),
                typeof(double),
                typeof(RotationCanvas),
                new FrameworkPropertyMetadata(
                    Circle.TrailThickness, // default value
                    FrameworkPropertyMetadataOptions.AffectsRender,
                    OnTrailThicknessChanged
                    ));
        private static void OnTrailThicknessChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var canvas = (RotationCanvas)d;
            double value = (double)e.NewValue;

            double clamped = Math.Clamp(value, canvas.TrailThicknessMin, canvas.TrailThicknessMax);
            if (!value.Equals(clamped))
            {
                canvas.TrailThickness = clamped;
                return;
            }
            Circle.UpdateTrailThicknesses(value, canvas.circles);
        }
        public double TrailThicknessMin => 3.0;
        public double TrailThicknessMax => 10.0;

        public double LapLineThickness
        {
            get => (double)GetValue(LapLineThicknessProperty);
            set => SetValue(LapLineThicknessProperty, value);
        }
        public static readonly DependencyProperty LapLineThicknessProperty =
            DependencyProperty.Register(
                nameof(LapLineThickness),
                typeof(double),
                typeof(RotationCanvas),
                new FrameworkPropertyMetadata(
                    1.5, // default value
                    FrameworkPropertyMetadataOptions.AffectsRender,
                    OnLapLineThicknessChanged
                    ));
        private static void OnLapLineThicknessChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var canvas = (RotationCanvas)d;
            double value = (double)e.NewValue;

            double clamped = Math.Clamp(value, canvas.LapLineThicknessMin, canvas.LapLineThicknessMax);
            if (!value.Equals(clamped))
            {
                canvas.LapLineThickness = clamped;
                return;
            }
            canvas.lapLine.UpdateThickness(value);
        }
        public double LapLineThicknessMin => 1.0;
        public double LapLineThicknessMax => 10.0;


        public bool DisplayLapLine
        {
            get => (bool)GetValue(DisplayLapLineProperty);
            set => SetValue(DisplayLapLineProperty, value);
        }
        public static readonly DependencyProperty DisplayLapLineProperty =
            DependencyProperty.Register(
                nameof(DisplayLapLine),
                typeof(bool),
                typeof(RotationCanvas),
                new FrameworkPropertyMetadata(
                    true, // default value
                    FrameworkPropertyMetadataOptions.AffectsRender,
                    OnDisplayLapLineChanged
                    ));
        private static void OnDisplayLapLineChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var canvas = (RotationCanvas)d;
            bool value = (bool)e.NewValue;

            canvas.lapLine.Display = value;
        }

        public bool DisplayCircles
        {
            get => (bool)GetValue(DisplayCirclesProperty);
            set => SetValue(DisplayCirclesProperty, value);
        }
        public static readonly DependencyProperty DisplayCirclesProperty =
            DependencyProperty.Register(
                nameof(DisplayCircles),
                typeof(bool),
                typeof(RotationCanvas),
                new FrameworkPropertyMetadata(
                    true, 
                    FrameworkPropertyMetadataOptions.AffectsRender,
                    OnDisplayCirclesChanged
                    ));
        private static void OnDisplayCirclesChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var canvas = (RotationCanvas)d;
            bool value = (bool)e.NewValue;

            Circle.UpdateCirclesVisibility(value, canvas.circles);
        }

        public bool DisplayTrails
        {
            get => (bool)GetValue(DisplayTrailsProperty);
            set => SetValue(DisplayTrailsProperty, value);
        }
        public static readonly DependencyProperty DisplayTrailsProperty =
            DependencyProperty.Register(
                nameof(DisplayTrails),
                typeof(bool),
                typeof(RotationCanvas),
                new FrameworkPropertyMetadata(
                    true,
                    FrameworkPropertyMetadataOptions.AffectsRender,
                    OnDisplayTrailsChanged
                    ));
        private static void OnDisplayTrailsChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var canvas = (RotationCanvas)d;
            bool value = (bool)e.NewValue;

            Circle.UpdateTrailsVisibility(value, canvas.circles);
        }

        public bool DisplayPrimes
        {
            get => (bool)GetValue(DisplayPrimesProperty);
            set => SetValue(DisplayPrimesProperty, value);
        }
        public static readonly DependencyProperty DisplayPrimesProperty =
            DependencyProperty.Register(
                nameof(DisplayPrimes),
                typeof(bool),
                typeof(RotationCanvas),
               new PropertyMetadata(true));


        public PresentationMode PresentationMode
        {
            get => (PresentationMode)GetValue(PresentationModeProperty);
            set => SetValue(PresentationModeProperty, value);
        }

        public static readonly DependencyProperty PresentationModeProperty =
            DependencyProperty.Register(
                nameof(PresentationMode),
                typeof(PresentationMode),
                typeof(RotationCanvas),
                new FrameworkPropertyMetadata(
                    PresentationMode.SeekPrimes,   // default
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnPresentationModeChanged
                ));
        private static void OnPresentationModeChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var canvas = (RotationCanvas)d;

            if (Equals(e.OldValue, e.NewValue))
                return;

            canvas.Reset();
        }

        public ThemeType ThemeType
        {
            get => (ThemeType)GetValue(ThemeTypeProperty);
            set => SetValue(ThemeTypeProperty, value);
        }
        public static readonly DependencyProperty ThemeTypeProperty =
            DependencyProperty.Register(
                nameof(ThemeType),
                typeof(ThemeType),
                typeof(RotationCanvas),
                new PropertyMetadata(ThemeType.ClassicNeon, OnThemeTypeChanged)
            );
        private static void OnThemeTypeChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var canvas = (RotationCanvas)d;
            canvas.Theme = Themes.GetTheme((ThemeType)e.NewValue);
            canvas.ApplyTheme();
        }


        #endregion


        #region CONSTRUCTORS

        public RotationCanvas()
        {
            ZoomTransform  = new ScaleTransform(CurrentScale, CurrentScale);
            LayoutTransform = ZoomTransform;

            Theme = Themes.GetTheme(ThemeType.ClassicNeon);
            lapLine = new LapLine(this);
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
        public void ApplyTheme()
        {
            OnPropertyChanged(nameof(PrimesColor));
            OnPropertyChanged(nameof(CounterColor));
            OnPropertyChanged(nameof(BackgroundColor));

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
