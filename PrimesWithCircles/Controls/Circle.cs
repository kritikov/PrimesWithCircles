using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using static System.Formats.Asn1.AsnWriter;

namespace PrimesWithCircles.Controls
{
    public class Circle
    {
        
        public double Radious { get; set; }
        public int Number { get; set; }

        private readonly double shapeStrokeThickness = 1.5;
        private readonly double trailStrokeThickness = 2;
        private readonly RotationCanvas parent;
        private readonly Ellipse shape;
        private readonly Ellipse pointer;
        private readonly Polyline trail;
        private int lapCounter = 0;         // helper to synchronize laps with the first one avoiding drift from rendering loop and floating-point precision limits
        private double angle;               // Angle in radians, canonicalized to [0, 2π)
        private double accumulatedAngle;    // accumulated angle in radians (not canonicalized)


        public Circle(RotationCanvas canvas, int number)
        {
            this.parent = canvas;
            Radious = parent.BaseRadious * number;
            Number = number;

            shape = new Ellipse
            {
                Width = Radious * 2,
                Height = Radious * 2,
                Stroke = Brushes.DimGray,
                StrokeThickness = shapeStrokeThickness,
                Visibility = parent.ShapesVisibility
            };

            pointer = new Ellipse
            {
                Width = parent.PointerSize,
                Height = parent.PointerSize,
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
        /// Set the radious of the circle according to its number and the base radious of the parent canvas.
        /// </summary>
        public void UpdateRadious()
        {
            Radious = parent.BaseRadious * Number;
            shape.Width = Radious * 2;
            shape.Height = Radious * 2;
            Center();
        }

        /// <summary>
        /// Set the size of the pointer according to the parent canvas setting.
        /// </summary>
        public void UpdatePointerSize()
        {
            pointer.Width = parent.PointerSize;
            pointer.Height = parent.PointerSize;
            PositionPointer();
            RescalePointer();
        }

        /// <summary>
        /// Set the visibility of the circle shape according to the parent canvas setting.
        /// </summary>
        public void UpdateShapeVisibility()
        {
            shape.Visibility = parent.ShapesVisibility;
        }

        /// <summary>
        /// Center the circle in the canvas.
        /// </summary>
        public void Center()
        {
            trail.Points.Clear();

            var centerOfCanvas = new Point(parent.ActualWidth / 2, parent.ActualHeight / 2);

            Canvas.SetLeft(shape, centerOfCanvas.X - Radious);
            Canvas.SetTop(shape, centerOfCanvas.Y - Radious);

            PositionPointer();
        }

        /// <summary>
        /// Position the pointer of the circle according to its angle in the canvas.
        /// </summary>
        public void PositionPointer()
        {
            var centerOfCanvas = new Point(parent.ActualWidth / 2, parent.ActualHeight / 2);

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
        public bool RotateCircle(double rotationStep, bool firstLapCompleted)
        {
            double angularSpeed = parent.BaseAngularSpeed * (parent.BaseRadious / Radious);
            double delta = angularSpeed * rotationStep;

            angle += delta;
            accumulatedAngle += delta;

            if (angle >= 2 * Math.PI)
                angle -= 2 * Math.PI;

            PositionPointer();

            // we use accumulatedAngle to count laps only for the first lap that is the base of the whole system
            if (accumulatedAngle >= 2 * Math.PI)
            {
                accumulatedAngle -= 2 * Math.PI;

                if (Number == 1)
                    return true;
            }

            // We use lapCounter to track full laps for synchronization with the first circle,
            // because accumulatedAngle would eventually drift due to floating-point precision
            // limits and the continuous updates in the rendering loop.
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
        public void Rescale()
        {
            shape.StrokeThickness = shapeStrokeThickness / parent.CurrentScale;
            trail.StrokeThickness = trailStrokeThickness / parent.CurrentScale;
            RescalePointer();
        }

        /// <summary>
        /// Rescales the pointer to match the current scale and pointer size of the parent element.
        /// </summary>
        public void RescalePointer()
        {
            pointer.Width = parent.PointerSize / parent.CurrentScale;
            pointer.Height = parent.PointerSize / parent.CurrentScale;
        }
    }
}
