using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PrimesWithCircles.Models
{
    public class Circle
    {
        public static double baseRadious = 30.0;
        public static double baseAngularSpeed = Math.PI;

        public double Radious { get; }
        public Ellipse Shape { get; }
        public Ellipse Pointer { get; }
        public Polyline Trail { get; }
        public int LapCounter { get; set; } = 0;  // μετρά πόσες φορές πέρασε ο πρώτος

        // Angle in radians, canonicalized to [0, 2π)
        public double Angle { get; set; }

        // accumulated angle in radians (not canonicalized)
        public double AccumulatedAngle { get; set; }

        // angular speed in radians per second
        public double AngularSpeed => baseAngularSpeed * (Circle.baseRadious / Radious);

        // number this circle represents (1-based)
        public int Number { get; set; }


        public Circle(int number = 1)
        {
            Radious = Circle.baseRadious * number;
            Number = number;

            Shape = new Ellipse
            {
                Width = Radious * 2,
                Height = Radious * 2,
                Stroke = Brushes.DimGray,
                StrokeThickness = 1.5
            };

            Pointer = new Ellipse
            {
                Width = 8,
                Height = 8,
                Fill = Brushes.OrangeRed,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            Trail = new Polyline
            {
                Stroke = Brushes.Red,
                StrokeThickness = 2,
                Opacity = 0.5
            };

            // start at top: -π/2
            Angle = -Math.PI / 2;

            AccumulatedAngle = 0.0;
        }
    }
}
