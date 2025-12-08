using PrimesWithCircles.Models;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PrimesWithCircles
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;


        private readonly List<Circle> circles = [];
        private Point centerOfCanvas;
        private bool isRotating = false;
        private DateTime lastRenderTime;

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

        private string primes = "2";
        public string Primes
        {
            get => primes;
            set
            {
                if (primes != value)
                {
                    primes = value;
                    OnPropertyChanged(nameof(Primes));
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
            centerOfCanvas = new Point(RotationCanvas.ActualWidth / 2, RotationCanvas.ActualHeight / 2);

            CenterAllCircles();
            //AutoAdjustZoom();
        }

        
        private void OnRendering(object? sender, EventArgs e)
        {
            DateTime now = DateTime.UtcNow;
            double elapsedSec = (now - lastRenderTime).TotalSeconds;
            lastRenderTime = now;
            RotateCircles(elapsedSec);
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            StartRotation();
        }

        private void PauseBtn_Click(object sender, RoutedEventArgs e)
        {
            StopRotation();
        }

        private void SpeedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Circle.baseAngularSpeed = Math.PI * e.NewValue;
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            StopRotation();
            ResetData();
            ResetZoom();
        }

        private void RotateButton_Click(object sender, RoutedEventArgs e)
        {
            // if not allready rotating, start rotating
            if (!isRotating)
                StartRotation();
        }


        #endregion



        #region METHODS

        /// <summary>
        /// Initializes the screen and sets up the initial circles and lap counter for the rotation canvas.
        /// </summary>
        private void ResetData()
        {
            circles.Clear();
            RotationCanvas.Children.Clear();

            // initial two circles
            AddCircle(1);
            AddCircle(2);

            LapCounter = 2;
            Primes = "2";
        }

        /// <summary>
        ///  Add a new circle with given number.
        /// </summary>
        private void AddCircle(int number)
        {
            var circle = new Circle(number);

            // add the circle to the list for looping purposes
            circles.Add(circle);

            // add the shapes that will be rotated in the canvas
            RotationCanvas.Children.Add(circle.Trail);
            RotationCanvas.Children.Add(circle.Shape);
            RotationCanvas.Children.Add(circle.Pointer);

            CenterCircle(circle);
            PositionPointer(circle);
            AutoAdjustZoom();
        }

        /// <summary>
        /// Center the circle in the canvas.
        /// </summary>
        private void CenterCircle(Circle circle)
        {
            Canvas.SetLeft(circle.Shape, centerOfCanvas.X - circle.Radious);
            Canvas.SetTop(circle.Shape, centerOfCanvas.Y - circle.Radious);
        }

        /// <summary>
        /// ανακεντράρουμε ΟΛΟΥΣ τους κύκλους
        /// </summary>
        private void CenterAllCircles()
        {
            foreach (var circle in circles)
            {
                circle.Trail.Points.Clear();
                CenterCircle(circle);
                PositionPointer(circle);
            }
        }

        /// <summary>
        /// Position the pointer of the circle according to its angle in the canvas.
        /// </summary>
        private void PositionPointer(Circle circle)
        {
            double x = centerOfCanvas.X + circle.Radious * Math.Cos(circle.Angle);
            double y = centerOfCanvas.Y + circle.Radious * Math.Sin(circle.Angle);

            Canvas.SetLeft(circle.Pointer, x - circle.Pointer.Width / 2);
            Canvas.SetTop(circle.Pointer, y - circle.Pointer.Height / 2);

            // trail: append a point
            if (circle.Trail != null)
            {
                var pts = circle.Trail.Points;
                pts.Add(new Point(x, y));
                if (pts.Count > 10) // cap trail length
                    pts.RemoveAt(0);
            }
        }

        /// <summary>
        /// Start the rotation by subscribing to the rendering event.
        /// </summary>
        private void StartRotation()
        {
            if (isRotating) return;
            lastRenderTime = DateTime.UtcNow;
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
            bool firstCompleted = false;
            bool someOtherCompleted = false;

            // Περιστρέφουμε ΟΛΟΥΣ όπως πάντα
            foreach (var circle in circles) {
                bool lapCompleted = RotateCircle(circle, elapsedSec, firstCompleted);
                if (circle.Number == 1 && lapCompleted)
                    firstCompleted = true;
                
                if (circle.Number > 1 && firstCompleted && lapCompleted)
                    someOtherCompleted = true;
            }


            // Αν ο πρώτος έκανε κύκλο → προχωράμε την λογική
            if (firstCompleted)
            {
                if (AutoModeCheck.IsChecked != true)
                    StopRotation();

                LapCounter++;

                // Prime detection
                if (!someOtherCompleted)
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

            Primes += number.ToString() + " ";
        }

        /// <summary>
        /// Rotate individual circle for elapsedSec seconds. Returns true if that circle completed a lap (crossed finishLine).
        /// </summary>
        private bool RotateCircle(Circle circle, double elapsedSec, bool firstLapCompleted)
        {
            double delta = circle.AngularSpeed * elapsedSec;

            circle.Angle += delta;
            circle.AccumulatedAngle += delta;

            if (circle.Angle >= 2 * Math.PI)
                circle.Angle -= 2 * Math.PI;

            PositionPointer(circle);

            if (circle.AccumulatedAngle >= 2 * Math.PI)
            {
                circle.AccumulatedAngle -= 2 * Math.PI;

                if (circle.Number == 1)
                    return true;
            }

            if (firstLapCompleted)
            {
                circle.LapCounter++;

                if (circle.LapCounter == circle.Number)
                {
                    circle.LapCounter = 0;
                    circle.Angle = -Math.PI / 2;
                    circle.AccumulatedAngle = 0.0;
                    return true;
                }
            }

            return false;
        }



        private void ResetZoom()
        {
            SetZoom(1);
        }


        public void SetZoom(double scale)
        {
            //if (scale < 0.05) scale = 0.05; // μη γίνει και pixel dust
            ZoomTransform.ScaleX = scale;
            ZoomTransform.ScaleY = scale;
        }

        public void AutoAdjustZoom()
        {
            if (circles.Count == 0) return;

            double maxRadius = circles[circles.Count - 1].Radious;
            double neededSize = maxRadius * 2 + 200; // buffer

            double actualWidthWithoutScale = RotationCanvas.ActualWidth * ZoomTransform.ScaleX;
            double actualHeightWithoutScale = RotationCanvas.ActualHeight * ZoomTransform.ScaleX;

            double scaleX = actualWidthWithoutScale / neededSize;
            double scaleY = actualHeightWithoutScale / neededSize;

            double newScale = Math.Min(scaleX, scaleY);

            if (newScale < 1)
                SetZoom(newScale); 

        }

        #endregion

    }
}
