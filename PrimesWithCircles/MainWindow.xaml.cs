using PrimesWithCircles.Models;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace PrimesWithCircles
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public Circles Circles { get; set; }
        private bool isRotating = false;

        private int lapCounter = 2;
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

        private string primes;
        public string Primes
        {
            get => primes;
            set
            {
                if (primes != value)
                {
                    primes = value;
                    OnPropertyChanged(nameof(Primes));
                    PrimesScroll?.ScrollToEnd();
                }
            }
        }


        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Circles = new Circles(RotationCanvas);

            Loaded += OnLoaded;
            RotationCanvas.SizeChanged += OnCanvasSizeChanged;
        }


        #region EVENTS

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ResetData();
        }

        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


        private void OnCanvasSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Circles.CenterAllCircles();
        }

        
        private void OnRendering(object? sender, EventArgs e)
        {
            var rotateStep = 0.01;
            RotateCircles(rotateStep);
        }
        private void RotateButton_Click(object sender, RoutedEventArgs e)
        {
            // if not allready rotating, start rotating
            if (!isRotating)
            {
                IsReseted = false;
                StartRotation();
            }
        }

        private void PauseBtn_Click(object sender, RoutedEventArgs e)
        {
            StopRotation();
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            StopRotation();
            ResetData();
            ResetZoom();
        }

        #endregion



        #region METHODS

        /// <summary>
        /// Initializes the screen and sets up the initial circles and lap counter for the rotation canvas.
        /// </summary>
        private void ResetData()
        {
            Circles.Clear();

            AddCircle(1);
            AddCircle(2);

            LapCounter = 2;
            Primes = "primes: 2";
            IsReseted = true;
        }

        /// <summary>
        ///  Add a new circle with given number.
        /// </summary>
        private void AddCircle(int number)
        {
            Circles.Add(number);

            AutoAdjustZoom();
        }

        /// <summary>
        /// Start the rotation by subscribing to the rendering event.
        /// </summary>
        private void StartRotation()
        {
            if (isRotating) return;
            CompositionTarget.Rendering += OnRendering;
            isRotating = true;
        }

        /// <summary>
        /// Stop the rotation by unsubscribing from the rendering event.
        /// </summary>
        private void StopRotation()
        {
            if (!isRotating) return;
            CompositionTarget.Rendering -= OnRendering;
            isRotating = false;
        }

        /// <summary>
        /// Rotate all circles for elapsedSec seconds. Stops and handles lap logic for first circle.
        /// </summary>
        private void RotateCircles(double elapsedSec)
        {
            var (FirstCompleted, SomeOtherCompleted) = Circles.RotateCircles(elapsedSec);


            // if the first circle completed a lap
            if (FirstCompleted)
            {
                if (AutoModeCheck.IsChecked != true)
                    StopRotation();

                // increase lap counter
                LapCounter++;

                // if no other circle completed a lap, then the lap counter is a prime
                if (!SomeOtherCompleted)
                    AddPrime(LapCounter);
            }
        }

        /// <summary>
        /// Add a new prime to the list of primes
        /// </summary>
        private void AddPrime(int number) {

            AddCircle(LapCounter);

            if (Primes.Length > 0)
                Primes += ", ";

            Primes += number.ToString();
        }


        /// <summary>
        /// Remove the zoom by setting it to 1
        /// </summary>
        private void ResetZoom()
        {
            Zoom(1);
        }

        /// <summary>
        /// Zoom with animation
        /// </summary>
        /// <param name="targetScale"></param>
        private void Zoom(double targetScale)
        {
            DoubleAnimation anim = new()
            {
                To = targetScale,
                Duration = TimeSpan.FromMilliseconds(1000),
                EasingFunction = new QuinticEase { EasingMode = EasingMode.EaseOut }
            };

            ZoomTransform.BeginAnimation(ScaleTransform.ScaleXProperty, anim);
            ZoomTransform.BeginAnimation(ScaleTransform.ScaleYProperty, anim);
        }

        /// <summary>
        /// Adjust the zoom level automatically to fit all circles in the canvas
        /// </summary>
        public void AutoAdjustZoom()
        {
            if (Circles.List.Count == 0) return;

            double maxRadius = Circles.GetMaxRadious();
            double neededSize = maxRadius * 2 + 100; // buffer

            double actualWidthWithoutScale = RotationCanvas.ActualWidth * ZoomTransform.ScaleX;
            double actualHeightWithoutScale = RotationCanvas.ActualHeight * ZoomTransform.ScaleX;

            double scaleX = actualWidthWithoutScale / neededSize;
            double scaleY = actualHeightWithoutScale / neededSize;

            double newScale = Math.Min(scaleX, scaleY);

            if (newScale < 1)
            {
                Zoom(newScale);
                Circles.RescaleCircles(newScale);
            }

        }

        #endregion

        
    }
}
