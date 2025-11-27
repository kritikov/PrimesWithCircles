using PrimesWithCircles.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PrimesWithCircles
{
    public partial class MainWindow : Window
    {
        private List<Circle> circles = new List<Circle>();
        private double baseAngularSpeed = Math.PI; // radians per second for first circle (π rad/s ≈ 180°/s)
        private double firstRadius = 30;
        private Point screenCenter;

        private int lapCounter = 2;
        private const double finishLine = 3 * Math.PI / 2;

        private bool isStepping = false;
        private double startAngle;
        private double targetAngle;

        // rendering
        private bool isRunning = false;
        private DateTime lastRenderTime;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // compute center after layout
            screenCenter = new Point(MainCanvas.ActualWidth / 2, MainCanvas.ActualHeight / 2);

            // initial two circles
            AddCircle(firstRadius, index: 1);
            AddCircle(firstRadius * 2, index: 2);

            // set initial speeds
            UpdateAngularSpeeds();

            LapCounterText.Text = lapCounter.ToString();
        }

        private void StartRendering()
        {
            if (isRunning) return;
            lastRenderTime = DateTime.UtcNow;
            CompositionTarget.Rendering += OnRendering;
            isRunning = true;
        }

        private void StopRendering()
        {
            if (!isRunning) return;
            CompositionTarget.Rendering -= OnRendering;
            isRunning = false;
        }

        private void OnRendering(object? sender, EventArgs e)
        {
            DateTime now = DateTime.UtcNow;
            double elapsedSec = (now - lastRenderTime).TotalSeconds;
            lastRenderTime = now;

            if (AutoModeCheck.IsChecked == true)
            {
                // continuous rotate
                RotateCircles(elapsedSec);
            }
            else
            {
                // manual: only rotate if stepping active
                if (isStepping)
                    RotateCircles(elapsedSec);
            }
        }

        private void AddCircle(double radious, int? index = null)
        {
            int idx = index ?? (circles.Count + 1);
            var circle = new Circle(radious, idx);

            // style
            circle.Shape.Stroke = Brushes.DimGray;
            circle.Pointer.Fill = Brushes.OrangeRed;

            // add to canvas: add shape under pointer/trail so trail sits in front
            MainCanvas.Children.Add(circle.Shape);
            MainCanvas.Children.Add(circle.Pointer);

            circles.Add(circle);

            CenterCircle(circle);
            PositionPointer(circle);
        }

        // helper to center shape
        private void CenterCircle(Circle circle)
        {
            Canvas.SetLeft(circle.Shape, screenCenter.X - circle.Radious);
            Canvas.SetTop(circle.Shape, screenCenter.Y - circle.Radious);
        }

        private void PositionPointer(Circle circle)
        {
            double x = screenCenter.X + circle.Radious * Math.Cos(circle.Angle);
            double y = screenCenter.Y + circle.Radious * Math.Sin(circle.Angle);

            Canvas.SetLeft(circle.Pointer, x - circle.Pointer.Width / 2);
            Canvas.SetTop(circle.Pointer, y - circle.Pointer.Height / 2);

            // trail: append a point
            if (circle.Trail != null)
            {
                var pts = circle.Trail.Points;
                pts.Add(new System.Windows.Point(x, y));
                if (pts.Count > 60) // cap trail length
                    pts.RemoveAt(0);
            }
        }

        private void UpdateAngularSpeeds()
        {
            // first circle gets baseAngularSpeed (radians/sec)
            for (int i = 0; i < circles.Count; i++)
            {
                var c = circles[i];
                // speed scaled inversely by radius ratio to firstRadius (as your original)
                c.AngularSpeed = baseAngularSpeed * (firstRadius / c.Radious);
            }
        }

        /// <summary>
        /// Rotate all circles for elapsedSec seconds. Stops and handles lap logic for first circle.
        /// </summary>
        private void RotateCircles(double elapsedSec)
        {
            bool firstCompleted = false;
            bool someOtherCompleted = false;

            for (int i = 0; i < circles.Count; i++)
            {
                var c = circles[i];
                bool lap = RotateCircle(c, elapsedSec);

                if (i == 0 && lap) firstCompleted = true;
                else if (lap) someOtherCompleted = true;
            }

            if (firstCompleted)
            {
                StopRendering();
                lapCounter++;
                LapCounterText.Text = lapCounter.ToString();

                if (!someOtherCompleted)
                {
                    // new circle for prime
                    AddCircle(firstRadius * (lapCounter), index: circles.Count + 1);
                    UpdateAngularSpeeds();
                }

                isStepping = false;
            }
        }

        /// <summary>
        /// Rotate individual circle for elapsedSec seconds. Returns true if that circle completed a lap (crossed finishLine).
        /// </summary>
        private bool RotateCircle(Circle circle, double elapsedSec)
        {
            double prev = circle.Angle;
            // angle += ω * dt
            circle.Angle += circle.AngularSpeed * elapsedSec;

            // detect crossing 3π/2 relative to starting at -π/2
            bool completed = prev < finishLine && circle.Angle >= finishLine;

            // wrap to [0, 2π)
            if (circle.Angle >= 2 * Math.PI)
                circle.Angle -= 2 * Math.PI;

            PositionPointer(circle);
            return completed;
        }

        // --- UI handlers ---
        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            StartRendering();
        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            StopRendering();
        }

        private void StepButton_Click(object sender, RoutedEventArgs e)
        {
            if (AutoModeCheck.IsChecked == true) return;

            // if not running, start stepping
            if (!isRunning)
                StartRendering();

            // Start a one-lap step for first circle
            isStepping = true;
            startAngle = circles[0].Angle;
            targetAngle = startAngle + 2 * Math.PI;
        }

        private void SpeedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // scale baseAngularSpeed by slider (slider 0.02..1 default 0.5)
            baseAngularSpeed = Math.PI * e.NewValue; // π * factor
            UpdateAngularSpeeds();
        }

        private void AddCircleBtn_Click(object sender, RoutedEventArgs e)
        {
            AddCircle(firstRadius * (circles.Count + 1), index: circles.Count + 1);
            UpdateAngularSpeeds();
        }

        /// <summary>
        /// Fast sieve: compute primes up to N and color circles accordingly.
        /// For demonstration: compute first M primes and create enough circles to show them.
        /// </summary>
        private void ComputePrimesBtn_Click(object sender, RoutedEventArgs e)
        {
            // compute primes using sieve up to some limit (e.g. 2000)
            int limit = 2000;
            var primes = Sieve(limit);

            // For demo, mark circles whose index (starting from 1) are primes
            for (int i = 0; i < circles.Count; i++)
            {
                int number = i + 1; // mapping: circle index -> number
                if (number <= limit && primes[number])
                {
                    MarkCirclePrime(circles[i]);
                }
                else
                {
                    UnmarkCircle(circles[i]);
                }
            }
        }

        private void MarkCirclePrime(Circle c)
        {
            c.IsPrimeVisual = true;
            c.Pointer.Fill = Brushes.LimeGreen;
            c.Shape.Stroke = Brushes.Green;
        }

        private void UnmarkCircle(Circle c)
        {
            c.IsPrimeVisual = false;
            c.Pointer.Fill = Brushes.OrangeRed;
            c.Shape.Stroke = Brushes.DimGray;
        }

        private bool[] Sieve(int n)
        {
            var isPrime = Enumerable.Repeat(true, n + 1).ToArray();
            isPrime[0] = isPrime[1] = false;
            for (int p = 2; p * p <= n; p++)
            {
                if (!isPrime[p]) continue;
                for (int multiple = p * p; multiple <= n; multiple += p)
                    isPrime[multiple] = false;
            }
            return isPrime;
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            StopRendering();
            MainCanvas.Children.Clear();
            circles.Clear();
            lapCounter = 0;
            AddCircle(firstRadius, index: 1);
            AddCircle(firstRadius * 2, index: 2);
            UpdateAngularSpeeds();
            LapCounterText.Text = lapCounter.ToString();
        }
    }
}
