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
        private double pointersSpeed = 0.1;
        private double firstRadius = 10;
        private Point screenCenter;


        private int lapCounter = 0;
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
            timer.Start();
        }


        /// <summary>
        /// Add a circle to the list
        /// </summary>
        /// <param name="radious"></param>
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
        /// <param name="circle"></param>
        private void CenterCircle(Circle circle)
        {
            // position the circle in the center of the screen
            Canvas.SetLeft(circle.Shape, screenCenter.X - circle.Radious);
            Canvas.SetTop(circle.Shape, screenCenter.Y - circle.Radious);

        }

        /// <summary>
        ///  Position the pointer of a circle to a specific angle
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="angle"></param>
        private void PositionPointer(Circle circle)
        {
            // position the pointer on top of the circle
            double x1 = screenCenter.X + circle.Radious * Math.Cos(circle.Angle);
            double y1 = screenCenter.Y + circle.Radious * Math.Sin(circle.Angle);
            Canvas.SetLeft(circle.Pointer, x1 - circle.Pointer.Width / 2);
            Canvas.SetTop(circle.Pointer, y1 - circle.Pointer.Height / 2);
        }

        /// <summary>
        /// Rotate all the circles
        /// </summary>
        private void RotateCircles()
        {
            foreach (var circle in circles)
            {
                RotateCircle(circle);
            }
        }

        /// <summary>
        /// Rotate a circle
        /// </summary>
        /// <param name="circle"></param>
        private void RotateCircle(Circle circle)
        {
            circle.Angle += pointersSpeed / (circle.Radious / firstRadius);
            if (circle.Angle >= Math.PI * 2)
                circle.Angle -= Math.PI * 2;

            PositionPointer(circle);
        }

        

        
        private void OnTick(object sender, EventArgs e)
        {
            RotateCircles();
            //if (AutoModeCheck.IsChecked == true)
            //{
            //    RotateNormally();
            //}
            //else
            //{
            //    // Manual mode: only rotate if a step cycle is active
            //    if (isStepping)
            //    {
            //        StepRotate();
            //    }
            //}
        }

        private void StepRotate()
        {
            //double prev = angle;

            //angle += 0.1;
            //if (angle >= Math.PI * 2)
            //    angle -= Math.PI * 2;

            //UpdateDotPositions();

            //// Αν έχουμε ξεπεράσει την targetAngle -> ένας πλήρης κύκλος ολοκληρώθηκε
            //double normalizedAngle = angle < startAngle ? angle + 2 * Math.PI : angle;

            //if (normalizedAngle >= targetAngle)
            //{
            //    // "Κουμπώνουμε" την τελεία ακριβώς στη θέση τερματισμού
            //    angle = startAngle;
            //    isStepping = false;
            //}
        }


        private void DetectLap(double prev)
        {
            //double finishLine = 3 * Math.PI / 2;

            //if (prev < finishLine && angle >= finishLine)
            //{
            //    lapCounter++;
            //    LapCounterText.Text = lapCounter.ToString();
            //}
        }

        private void UpdateDotPositions()
        {
            //double x1 = screenCenter.X + firstRadius * Math.Cos(angle);
            //double y1 = screenCenter.Y + firstRadius * Math.Sin(angle);

            //Canvas.SetLeft(dot, x1 - dot.Width / 2);
            //Canvas.SetTop(dot, y1 - dot.Height / 2);

            //double x2 = screenCenter.X + radius2 * Math.Cos(angle / 2);
            //double y2 = screenCenter.Y + radius2 * Math.Sin(angle / 2);

            //Canvas.SetLeft(dot2, x2 - dot2.Width / 2);
            //Canvas.SetTop(dot2, y2 - dot2.Height / 2);
        }

        // Called when user clicks "Step"
        private void StepButton_Click(object sender, RoutedEventArgs e)
        {
            //if (AutoModeCheck.IsChecked == true)
            //    return;

            //if (!isStepping)
            //{
            //    isStepping = true;
            //    startAngle = angle;
            //    targetAngle = angle + 2 * Math.PI;
            //}
        }
    }
}
