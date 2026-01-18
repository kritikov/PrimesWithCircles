using PrimesWithCircles.Infrastucture;
using PrimesWithCircles.Infrastucture.Enums;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace PrimesWithCircles.UI.Controls
{
    public class RotationCanvas : Canvas 
    {
        public event Action? PrimesChanged;


        #region PROPERTIES AND FIELDS

        public double StartAngle { get; set; } = -Math.PI / 2;  // start at top: -π/2

        public double CurrentScale { get; set; } = 1.0;

        private readonly List<Circle> circles = [];

        private LapLine? lapLine;
        private ScaleTransform ZoomTransform { get; set; }
        private bool isRotating = false;

        #endregion


        #region DEPENDENCY PROPERTIES

        public RotationSettings Settings
        {
            get => (RotationSettings)GetValue(SettingsProperty);
            set => SetValue(SettingsProperty, value);
        }

        public static readonly DependencyProperty SettingsProperty =
            DependencyProperty.Register(
                nameof(Settings),
                typeof(RotationSettings),
                typeof(RotationCanvas),
                new PropertyMetadata(null, OnSettingsChanged));

        private static void OnSettingsChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
                {
                    var canvas = (RotationCanvas)d;

                    if (e.OldValue is RotationSettings oldSettings)
                        oldSettings.PropertyChanged -= canvas.OnSettingsPropertyChanged;

                    if (e.NewValue is RotationSettings newSettings)
                    {
                        newSettings.PropertyChanged += canvas.OnSettingsPropertyChanged;
                        canvas.RotationSpeed = newSettings.RotationSpeed;
                    }
                }

        /// <summary>
        /// When a property in the settings changes then update the corresponding property in the canvas
        /// </summary>
        private void OnSettingsPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is not RotationSettings s) return;

            switch (e.PropertyName)
            {
                case nameof(RotationSettings.RotationSpeed):
                    RotationSpeed = s.RotationSpeed;
                    break;
                case nameof(RotationSettings.AutoRotation):
                    AutoRotation = s.AutoRotation;
                    break;
                case nameof(RotationSettings.IsReseted):
                    IsReseted = s.IsReseted;
                    break;
                case nameof(RotationSettings.PresentationMode):
                    PresentationMode = s.PresentationMode;
                    break;
                case nameof(RotationSettings.BaseRadious):
                    BaseRadious = s.BaseRadious;
                    break;
                case nameof(RotationSettings.LapLineThickness):
                    LapLineThickness = s.LapLineThickness;
                    break;
                case nameof(RotationSettings.CircleThickness):
                    CircleThickness = s.CircleThickness;
                    break;
                case nameof(RotationSettings.PointerSize):
                    PointerSize = s.PointerSize;
                    break;
                case nameof(RotationSettings.TrailThickness):
                    TrailThickness = s.TrailThickness;
                    break;
                case nameof(RotationSettings.ThemeType):
                    ThemeType = s.ThemeType;
                    break;
                case nameof(RotationSettings.DisplayLapLine):
                    DisplayLapLine = s.DisplayLapLine;
                    break;
                case nameof(RotationSettings.FlashLapLine):
                    FlashLapLine = s.FlashLapLine;
                    break;
                case nameof(RotationSettings.DisplayCircles):
                    DisplayCircles = s.DisplayCircles;
                    break;
                case nameof(RotationSettings.DisplayTrails):
                    DisplayTrails = s.DisplayTrails;
                    break;
            }
        }


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


        public bool AutoRotation
        {
            get => (bool)GetValue(AutoRotationProperty);
            set => SetValue(AutoRotationProperty, value);
        }
        public static readonly DependencyProperty AutoRotationProperty =
            DependencyProperty.Register(
                nameof(AutoRotation),
                typeof(bool),
                typeof(RotationCanvas),
                new FrameworkPropertyMetadata(true));


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
                    FrameworkPropertyMetadataOptions.AffectsRender
                    ));


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
                    PresentationMode.SeekPrimes,  
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnPresentationModeChanged
                ));
        private static void OnPresentationModeChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var canvas = (RotationCanvas)d;

            canvas.Reset();
        }

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

            Circle.UpdateBaseRadious(canvas.BaseRadious, canvas.circles);
            canvas.AdjustZoom();
        }


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
                    2.0, // default value
                    FrameworkPropertyMetadataOptions.AffectsRender,
                    OnLapLineThicknessChanged
                    ));
        private static void OnLapLineThicknessChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var canvas = (RotationCanvas)d;
            canvas.lapLine?.UpdateThickness(canvas.LapLineThickness);
        }


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
            Circle.UpdateCirclesThicknesses(canvas.CircleThickness, canvas.circles);
        }


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
            Circle.UpdatePointerSizes(canvas.PointerSize, canvas.circles);
        }
        

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
            Circle.UpdateTrailThicknesses(canvas.TrailThickness, canvas.circles);
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
            canvas.ApplyTheme();
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
                    true, 
                    FrameworkPropertyMetadataOptions.AffectsRender,
                    OnDisplayLapLineChanged
                    ));
        private static void OnDisplayLapLineChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var canvas = (RotationCanvas)d;
            if (canvas.lapLine != null)
            {
                canvas.lapLine.Display = canvas.DisplayLapLine;
            }
        }


        public bool FlashLapLine
        {
            get => (bool)GetValue(FlashLapLineProperty);
            set => SetValue(FlashLapLineProperty, value);
        }
        public static readonly DependencyProperty FlashLapLineProperty =
            DependencyProperty.Register(
                nameof(FlashLapLine),
                typeof(bool),
                typeof(RotationCanvas),
                new FrameworkPropertyMetadata(true));


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
            Circle.UpdateCirclesVisibility(canvas.DisplayCircles, canvas.circles);
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
            Circle.UpdateTrailsVisibility(canvas.DisplayTrails, canvas.circles);
        }


        public int LapCounter
        {
            get => (int)GetValue(LapCounterProperty);
            set => SetValue(LapCounterProperty, value);
        }
        public static readonly DependencyProperty LapCounterProperty =
            DependencyProperty.Register(
                nameof(LapCounter),
                typeof(int),
                typeof(RotationCanvas),
                new FrameworkPropertyMetadata(0));


        public string Primes
        {
            get => (string)GetValue(PrimesProperty);
            set => SetValue(PrimesProperty, value);
        }
        public static readonly DependencyProperty PrimesProperty =
            DependencyProperty.Register(
                nameof(Primes),
                typeof(string),
                typeof(RotationCanvas),
                new FrameworkPropertyMetadata(
                    "",
                    FrameworkPropertyMetadataOptions.AffectsRender,
                    OnPrimesChanged
                    ));
        private static void OnPrimesChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var canvas = (RotationCanvas)d;
            string value = (string)e.NewValue;

            canvas.PrimesChanged?.Invoke();
        }

        
        #endregion


        #region CONSTRUCTORS

        public RotationCanvas()
        {
            ZoomTransform  = new ScaleTransform(CurrentScale, CurrentScale);
            LayoutTransform = ZoomTransform;

            SizeChanged += OnCanvasSizeChanged;
        }

        #endregion


        #region EVENTS

        private void OnCanvasSizeChanged(object sender, SizeChangedEventArgs e)
        {
            // COMMENT: AdjustZoom method already called in RotationCanvas when size changes. Potentional problem with double calls?

            CenterAll();
            AdjustZoom();
        }

        private void OnRendering(object? sender, EventArgs e)
        {
            if (isRotating)
            {
                var lapCompleted = AdvanceFrame();

                if (!AutoRotation && lapCompleted)
                    StopRotating();
            }
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
            Settings.IsReseted = true;
            CurrentScale = 1.0;
            ZoomTransform.ScaleX = CurrentScale;
            ZoomTransform.ScaleY = CurrentScale;
            Zoom(CurrentScale);

            AddLine();

            if (PresentationMode == PresentationMode.OneCircle)
            {
                AddCircle(1);
                LapCounter = 0;
                Settings.DisplayPrimes = false;
            }
            else if (PresentationMode == PresentationMode.TwoCircles)
            {
                AddCircle(1);
                AddCircle(2);
                LapCounter = 0;
                Settings.DisplayPrimes = false;
            }
            else if (PresentationMode == PresentationMode.SeekPrimes)
            {
                AddCircle(1);
                AddCircle(2);
                LapCounter = 2;
                Settings.DisplayPrimes = true;
            }

            Primes = $"primes: {LapCounter}";

            AdjustZoom();
        }

        /// <summary>
        /// Center all elements in the canvas
        /// </summary>
        public void CenterAll()
        {
            lapLine?.Center();

            foreach (var circle in circles)
            {
                circle.Center();
            }
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

        /// <summary>
        /// Start rotating all circles
        /// </summary>
        public void StartRotating()
        {
            if (isRotating) return;
            Settings.IsReseted = false;
            CompositionTarget.Rendering += OnRendering;
            isRotating = true;
        }

        /// <summary>
        /// Stop rotating all circles
        /// </summary>
        public void StopRotating()
        {
            if (!isRotating) return;
            CompositionTarget.Rendering -= OnRendering;
            isRotating = false;
        }

        /// <summary>
        /// Rotate all circles for a rendering frame.
        /// </summary>
        public bool AdvanceFrame()
        {
            var (firstCircleCompletedLap, someOtherCircleCompletedLap) = RotateCircles();

            // if the first circle completed a lap
            if (firstCircleCompletedLap)
            {
                // increase lap counter
                LapCounter++;

                // if no other circle completed a lap, then the lap counter is a prime
                if (!someOtherCircleCompletedLap)
                {
                    if (PresentationMode == PresentationMode.SeekPrimes)
                    {
                        AddPrime(LapCounter);
                    }

                    if (FlashLapLine)
                        lapLine?.Flash();
                }
            }

            return firstCircleCompletedLap;
        }

        /// <summary>
        /// Rotate all circles. Stops and handles lap logic for first circle.
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

        #endregion
    }

}
