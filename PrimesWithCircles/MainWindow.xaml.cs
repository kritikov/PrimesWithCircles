using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace PrimesWithCircles
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

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
                    PrimesScroll?.ScrollToEnd();
                }
            }
        }


        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

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
            // COMMENT: AdjustZoom method already called in RotationCanvas when size changes. Potentional problem with double calls?

            RotationCanvas.CenterAllCircles();
            RotationCanvas.AdjustZoom();
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
        }

        #endregion



        #region METHODS

        /// <summary>
        /// Initializes the screen and sets up the initial circles and lap counter for the rotation canvas.
        /// </summary>
        private void ResetData()
        {
            RotationCanvas.Reset();

            LapCounter = 2;
            Primes = "primes: 2";
            IsReseted = true;
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
            var (FirstCompleted, SomeOtherCompleted) = RotationCanvas.RotateCircles(elapsedSec);


            // if the first circle completed a lap
            if (FirstCompleted)
            {
                if (AutoRotation != true)
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

            RotationCanvas.AddPrime(LapCounter);

            if (Primes.Length > 0)
                Primes += ", ";

            Primes += number.ToString();
        }


        #endregion

        
    }
}
