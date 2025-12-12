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
        public readonly Ellipse Shape;
        public readonly Ellipse Pointer;
        public readonly Polyline Trail;

        private readonly double trailStrokeThickness = 2;
        private readonly RotationCanvas parent;
        private int lapCounter = 0;         // helper to synchronize laps with the first one avoiding drift from rendering loop and floating-point precision limits
        private double angle;               // Angle in radians, canonicalized to [0, 2π)
        private double accumulatedAngle;    // accumulated angle in radians (not canonicalized)


        public Circle(RotationCanvas canvas, int number)
        {
            this.parent = canvas;
            Radious = parent.BaseRadious * number;
            Number = number;

            Shape = new Ellipse
            {
                Width = Radious * 2,
                Height = Radious * 2,
                Stroke = Brushes.DimGray,
                StrokeThickness = parent.ShapeThickness,
                Visibility = parent.ShapesVisibility
            };

            Pointer = new Ellipse
            {
                Width = parent.PointerSize,
                Height = parent.PointerSize,
                Fill = Brushes.Yellow,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            Trail = new Polyline
            {
                Stroke = Brushes.Red,
                StrokeThickness = trailStrokeThickness,
                Opacity = 0.5
            };

            // start at top: -π/2
            angle = -Math.PI / 2;

            accumulatedAngle = 0.0;
        }

        /// <summary>
        /// Set the radious of the circle according to its number and the base radious of the parent canvas.
        /// </summary>
        public void UpdateRadious()
        {
            Radious = parent.BaseRadious * Number;
            Shape.Width = Radious * 2;
            Shape.Height = Radious * 2;
            Center();
        }

        /// <summary>
        /// Set the size of the pointer according to the parent canvas setting.
        /// </summary>
        public void UpdatePointerSize()
        {
            Pointer.Width = parent.PointerSize;
            Pointer.Height = parent.PointerSize;
            RescalePointer();
            PositionPointer();
        }

        public void UpdateShapeThickness()
        {
            Shape.StrokeThickness = parent.ShapeThickness;
            Rescale();
            PositionPointer();
        }

        /// <summary>
        /// Set the visibility of the circle shape according to the parent canvas setting.
        /// </summary>
        public void UpdateShapeVisibility()
        {
            Shape.Visibility = parent.ShapesVisibility;
        }

        /// <summary>
        /// Center the circle in the canvas.
        /// </summary>
        public void Center()
        {
            Trail.Points.Clear();

            var centerOfCanvas = new Point(parent.ActualWidth / 2, parent.ActualHeight / 2);

            Canvas.SetLeft(Shape, centerOfCanvas.X - Radious);
            Canvas.SetTop(Shape, centerOfCanvas.Y - Radious);

            PositionPointer();
        }

        /// <summary>
        /// Position the pointer of the circle according to its angle in the canvas.
        /// </summary>
        public void PositionPointer()
        {
            var centerOfCanvas = new Point(parent.ActualWidth / 2, parent.ActualHeight / 2);

            double effectiveRadius = Radious - parent.ShapeThickness / 2;

            double x = centerOfCanvas.X + effectiveRadius * Math.Cos(angle);
            double y = centerOfCanvas.Y + effectiveRadius * Math.Sin(angle);

            Canvas.SetLeft(Pointer, x - Pointer.Width / 2);
            Canvas.SetTop(Pointer, y - Pointer.Height / 2);

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
            Shape.StrokeThickness = parent.ShapeThickness / parent.CurrentScale;
            Trail.StrokeThickness = parent.ShapeThickness / parent.CurrentScale;
            RescalePointer();
        }

        /// <summary>
        /// Rescales the pointer to match the current scale and pointer size of the parent element.
        /// Must run before PositionPointer to ensure correct placement.
        /// </summary>
        public void RescalePointer()
        {
            Pointer.Width = parent.PointerSize / parent.CurrentScale;
            Pointer.Height = parent.PointerSize / parent.CurrentScale;
        }
    }
}
