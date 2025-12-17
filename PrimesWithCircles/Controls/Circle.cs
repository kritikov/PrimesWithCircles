using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PrimesWithCircles.Controls
{
    public class Circle
    {
        
        public double Radious { get; set; }
        public int Number { get; set; }
        public readonly Ellipse Shape;
        public readonly Ellipse Pointer;
        public readonly Polyline Trail;

        private readonly RotationCanvas canvas;
        private Point center;               // center of the circle in the canvas
        private int lapCounter = 0;         // helper to synchronize laps with the first one avoiding drift from rendering loop and floating-point precision limits
        private double angle;               // Angle in radians, canonicalized to [0, 2π)
        private double accumulatedAngle;    // accumulated angle in radians (not canonicalized)


        public Circle(RotationCanvas canvas, int number)
        {
            this.canvas = canvas;
            Radious = this.canvas.BaseRadious * number;
            Number = number;
            center = new Point(canvas.ActualWidth / 2, canvas.ActualHeight / 2);

            Shape = new Ellipse
            {
                Width = Radious * 2,
                Height = Radious * 2,
                Stroke = Brushes.DimGray,
                StrokeThickness = this.canvas.CircleThickness,
                Visibility = this.canvas.CirclesVisibility
            };

            Pointer = new Ellipse
            {
                Width = this.canvas.PointerSize,
                Height = this.canvas.PointerSize,
                Fill = Brushes.Yellow,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            Trail = new Polyline
            {
                Stroke = Brushes.Red,
                StrokeThickness = this.canvas.TrailThickness,
                Opacity = 0.5,
                Visibility = this.canvas.TrailsVisibility
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
            Radious = canvas.BaseRadious * Number;
            Shape.Width = Radious * 2;
            Shape.Height = Radious * 2;
            Center();
        }
 
        /// <summary>
        /// Set the thickness of the shape according to the parent canvas setting.
        /// </summary>
        public void UpdateCircleThickness()
        {
            Shape.StrokeThickness = canvas.CircleThickness;
            //Rescale();
            PositionPointer();
            //PositionTrail();
        }

        /// <summary>
        /// Set the size of the pointer according to the parent canvas setting.
        /// </summary>
        public void UpdatePointerSize()
        {
            Pointer.Width = canvas.PointerSize;
            Pointer.Height = canvas.PointerSize;
            RescalePointer();
            PositionPointer();
        }

        /// <summary>
        /// Set the thickness of the trail according to the parent canvas setting.
        /// </summary>
        public void UpdateTrailThickness()
        {
            Trail.StrokeThickness = canvas.TrailThickness;
            RescaleTrail();
        }

        /// <summary>
        /// Set the visibility of the circle shape according to the parent canvas setting.
        /// </summary>
        public void UpdateShapeVisibility()
        {
            Shape.Visibility = canvas.CirclesVisibility;
        }

        /// <summary>
        /// Set the visibility of the circle trail according to the parent canvas setting.
        /// </summary>
        public void UpdateTrailVisibility()
        {
            Trail.Visibility = canvas.TrailsVisibility;
        }

        /// <summary>
        /// Center the circle and all of its associated elements in the canvas.
        /// </summary>
        public void Center()
        {
            var oldCenter = new Point(center.X, center.Y);

            center = new Point(canvas.ActualWidth / 2, canvas.ActualHeight / 2);

            Canvas.SetLeft(Shape, center.X - Radious);
            Canvas.SetTop(Shape, center.Y - Radious);

            PositionPointer();
            RepositionTrail(oldCenter);
        }

        /// <summary>
        /// Position the pointer of the circle according to its angle in the canvas.
        /// </summary>
        public void PositionPointer()
        {
            double effectiveRadius = Radious - Shape.StrokeThickness / 2;

            double x = center.X + effectiveRadius * Math.Cos(angle);
            double y = center.Y + effectiveRadius * Math.Sin(angle);

            Canvas.SetLeft(Pointer, x - Pointer.Width / 2);
            Canvas.SetTop(Pointer, y - Pointer.Height / 2);
        }

        /// <summary>
        /// Reposition the trail of the circle according to the new center of the circle.
        /// </summary>
        public void RepositionTrail(Point oldCenter)
        {
            var dx = oldCenter.X - center.X;
            var dy = oldCenter.Y - center.Y;

            PointCollection newPoints = new();
            foreach (var pt in Trail.Points)
            {
                var newPt = new Point(pt.X - dx, pt.Y - dy);
                newPoints.Add(newPt);
            }
            Trail.Points = newPoints;
        }

        /// <summary>
        /// Add a new point in the trail of the circle.
        /// </summary>
        public void ExtendTrail()
        {
            double effectiveRadius = Radious - Shape.StrokeThickness / 2;

            double x = center.X + effectiveRadius * Math.Cos(angle);
            double y = center.Y + effectiveRadius * Math.Sin(angle);

            var pts = Trail.Points;
            pts.Add(new Point(x, y));
            if (pts.Count > 10) 
                pts.RemoveAt(0);
        }
       
        /// <summary>
        /// Rescale the circle and all of its elements by the given scale factor of the canvas to be visible properly.
        /// </summary>
        /// <param name="scale"></param>
        public void Rescale()
        {
            RescaleCircle();
            RescalePointer();
            RescaleTrail();
        }

        /// <summary>
        /// Rescales the circle to match the current scale and shape thickness of the parent element.
        /// </summary>
        public void RescaleCircle()
        {
            Shape.StrokeThickness = canvas.CircleThickness / canvas.CurrentScale;
        }

        /// <summary>
        /// Rescales the pointer to match the current scale and pointer size of the parent element.
        /// Must run before PositionPointer to ensure correct placement.
        /// </summary>
        public void RescalePointer()
        {
            Pointer.Width = canvas.PointerSize / canvas.CurrentScale;
            Pointer.Height = canvas.PointerSize / canvas.CurrentScale;
        }
        
        /// <summary>
        /// Rescales the trail to match the current scale and trail thickness of the parent element.
        /// </summary>
        public void RescaleTrail()
        {
            Trail.StrokeThickness = canvas.TrailThickness / canvas.CurrentScale;
        }

        /// <summary>
        /// Rotate the circle for elapsedSec seconds. Returns true if the circle completed a lap
        /// </summary>
        public bool RotateCircle(double rotationStep, bool firstLapCompleted)
        {
            double angularSpeed = canvas.BaseAngularSpeed * (canvas.BaseRadious / Radious);
            double delta = angularSpeed * rotationStep;

            angle += delta;
            accumulatedAngle += delta;

            if (angle >= 2 * Math.PI)
                angle -= 2 * Math.PI;

            PositionPointer();
            ExtendTrail();

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

    }
}
