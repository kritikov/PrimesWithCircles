using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PrimesWithCircles.Models
{
    public class Circle
    {
        
        public double Radious { get; }
        public int Number { get; set; }

        private readonly double shapeStrokeThickness = 1.5;
        private readonly double trailStrokeThickness = 2;
        private readonly double pointerWidth = 8;
        private readonly double pointerHeight = 8;
        private readonly Canvas canvas;
        private readonly Ellipse shape;
        private readonly Ellipse pointer;
        private readonly Polyline trail;
        private int lapCounter = 0;
        private double angle;               // Angle in radians, canonicalized to [0, 2π)
        private double accumulatedAngle;    // accumulated angle in radians (not canonicalized)


        public Circle(Canvas canvas, int number, double baseRadious)
        {
            this.canvas = canvas;
            Radious = baseRadious * number;
            Number = number;

            shape = new Ellipse
            {
                Width = Radious * 2,
                Height = Radious * 2,
                Stroke = Brushes.DimGray,
                StrokeThickness = shapeStrokeThickness
            };

            pointer = new Ellipse
            {
                Width = pointerWidth,
                Height = pointerHeight,
                Fill = Brushes.Yellow,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            trail = new Polyline
            {
                Stroke = Brushes.Red,
                StrokeThickness = trailStrokeThickness,
                Opacity = 0.5
            };

            // start at top: -π/2
            angle = -Math.PI / 2;

            accumulatedAngle = 0.0;

            canvas.Children.Add(shape);
            canvas.Children.Add(trail);
            canvas.Children.Add(pointer);

            Center();
        }

        /// <summary>
        /// Center the circle in the canvas.
        /// </summary>
        public void Center()
        {
            trail.Points.Clear();

            var centerOfCanvas = new Point(canvas.ActualWidth / 2, canvas.ActualHeight / 2);

            Canvas.SetLeft(shape, centerOfCanvas.X - Radious);
            Canvas.SetTop(shape, centerOfCanvas.Y - Radious);

            PositionPointer();
        }

        /// <summary>
        /// Position the pointer of the circle according to its angle in the canvas.
        /// </summary>
        public void PositionPointer()
        {
            var centerOfCanvas = new Point(canvas.ActualWidth / 2, canvas.ActualHeight / 2);

            double x = centerOfCanvas.X + Radious * Math.Cos(angle);
            double y = centerOfCanvas.Y + Radious * Math.Sin(angle);

            Canvas.SetLeft(pointer, x - pointer.Width / 2);
            Canvas.SetTop(pointer, y - pointer.Height / 2);

            // trail: append a point
            //if (trail != null)
            //{
            //    var pts = trail.Points;
            //    pts.Add(new Point(x, y));
            //    if (pts.Count > 10) // cap trail length
            //        pts.RemoveAt(0);
            //}
        }

        /// <summary>
        /// Rotate the circle for elapsedSec seconds. Returns true if the circle completed a lap
        /// </summary>
        public bool RotateCircle(double rotationStep, bool firstLapCompleted, double baseRadious, double baseAngularSpeed)
        {
            double angularSpeed = baseAngularSpeed * (baseRadious / Radious);
            double delta = angularSpeed * rotationStep;

            angle += delta;
            accumulatedAngle += delta;

            if (angle >= 2 * Math.PI)
                angle -= 2 * Math.PI;

            PositionPointer();

            if (accumulatedAngle >= 2 * Math.PI)
            {
                accumulatedAngle -= 2 * Math.PI;

                if (Number == 1)
                    return true;
            }

            if (firstLapCompleted)
            {
                lapCounter++;

                if (lapCounter == Number)
                {
                    lapCounter = 0;
                    angle = -Math.PI / 2;
                    accumulatedAngle = 0.0;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Rescale the circle by the given scale factor of the canvas to be visible properly.
        /// </summary>
        /// <param name="scale"></param>
        public void Rescale(double scale)
        {
            shape.StrokeThickness = shapeStrokeThickness / scale;
            trail.StrokeThickness = trailStrokeThickness / scale;
            pointer.Width = pointerWidth / scale;
            pointer.Height = pointerHeight / scale;
        }
    }
}
