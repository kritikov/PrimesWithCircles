using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PrimesWithCircles.Models
{
    public class Circle
    {
        public double Radious { get; }
        public Ellipse Shape { get; }
        public Ellipse Pointer { get; }
        public Polyline Trail { get; }

        // Angle in radians, canonicalized to [0, 2π)
        public double Angle { get; set; }

        // angular speed in radians per second
        public double AngularSpeed { get; set; }

        // logical index / number this circle represents (1-based)
        public int Index { get; set; }

        // whether this number has been marked prime visually
        public bool IsPrimeVisual { get; set; } = false;

        public Circle(double radious, int index = 1)
        {
            Radious = radious;
            Index = index;

            Shape = new Ellipse
            {
                Width = radious * 2,
                Height = radious * 2,
                Stroke = Brushes.Gray,
                StrokeThickness = 1.5
            };

            Pointer = new Ellipse
            {
                Width = 8,
                Height = 8,
                Fill = Brushes.White,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            Trail = new Polyline
            {
                Stroke = Brushes.LightGray,
                StrokeThickness = 1,
                Opacity = 0.6
            };

            // start at top: -π/2
            Angle = -System.Math.PI / 2;
        }
    }
}
