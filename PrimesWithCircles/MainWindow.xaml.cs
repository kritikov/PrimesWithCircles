using PrimesWithCircles.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PrimesWithCircles
{
    public partial class MainWindow : Window
    {
        private List<Circle> circles = new List<Circle>();
        private double rotationSpeed = 0.1;
        private double firstRadius = 10;
        private Point screenCenter;


        private int lapCounter = 2;
        const double finishLine = 3 * Math.PI / 2;
        private readonly DispatcherTimer timer = new();

        private bool isStepping = false;
        private double targetAngle;
        private double startAngle;


        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            screenCenter = new Point(MainCanvas.ActualWidth / 2, MainCanvas.ActualHeight / 2);

            AddCircle(firstRadius);
            AddCircle(firstRadius * 2);

            timer.Interval = TimeSpan.FromMilliseconds(16);
            timer.Tick += OnTick;
        }


        /// <summary>
        /// Add a circle to the list
        /// </summary>
        private void AddCircle(double radious)
        {
            var circle = new Circle(radious);
            circles.Add(circle);

            MainCanvas.Children.Add(circle.Shape);
            MainCanvas.Children.Add(circle.Pointer);

            CenterCircle(circle);
            PositionPointer(circle);
        }

        /// <summary>
        ///  Position a circle in the center of the screen
        /// </summary>
        private void CenterCircle(Circle circle)
        {
            // position the circle in the center of the screen
            Canvas.SetLeft(circle.Shape, screenCenter.X - circle.Radious);
            Canvas.SetTop(circle.Shape, screenCenter.Y - circle.Radious);

        }

        /// <summary>
        ///  Position the pointer of a circle to a specific angle
        /// </summary>
        private void PositionPointer(Circle circle)
        {
            // position the pointer on top of the circle
            double x1 = screenCenter.X + circle.Radious * Math.Cos(circle.Angle);
            double y1 = screenCenter.Y + circle.Radious * Math.Sin(circle.Angle);
            Canvas.SetLeft(circle.Pointer, x1 - circle.Pointer.Width / 2);
            Canvas.SetTop(circle.Pointer, y1 - circle.Pointer.Height / 2);
        }

        /// <summary>
        ///  Rotate all circles on each timer tick
        /// </summary>
        private void OnTick(object sender, EventArgs e)
        {
            RotateCircles();
        }

        /// <summary>
        /// Rotate all the circles
        /// </summary>
        private void RotateCircles()
        {
            bool firstCircleCompletedLap = false;
            bool someOtherCircleCompletedLap = false;

            // foreach: περιστροφή όλων
            for (int i = 0; i < circles.Count; i++)
            {
                bool lapCompleted = RotateCircle(circles[i]);

                // ΜΟΝΟ ο πρώτος μάς ενδιαφέρει
                if (i == 0 && lapCompleted)
                    firstCircleCompletedLap = true;
                else if (lapCompleted)
                    someOtherCircleCompletedLap = true;
            }

            // if the first circle completed a lap, stop the timer
            if (firstCircleCompletedLap)
            {
                timer.Stop();
                lapCounter++;
                LapCounterText.Text = lapCounter.ToString();

                // if no other circle completed a lap, add a new circle
                if (!someOtherCircleCompletedLap)
                {
                    AddCircle(firstRadius * (lapCounter));
                }
            }
        }

        /// <summary>
        /// Rotate a circle
        /// </summary>
        private bool RotateCircle(Circle circle)
        {
            double prev = circle.Angle;

            // increment
            circle.Angle += rotationSpeed / (circle.Radious / firstRadius);

            // detect lap BEFORE wrap
            bool completedLap = circle.Angle >= (3 * Math.PI / 2);

            if (completedLap)
                circle.Angle -= Math.PI * 2;

            PositionPointer(circle);

            return completedLap;
        }


        // Called when user clicks "Step"
        private void StepButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }
    }
}
