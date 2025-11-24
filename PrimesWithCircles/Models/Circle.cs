using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PrimesWithCircles.Models
{
    internal class Circle
    {
        internal Ellipse Pointer;
        internal double Radious;
        internal Ellipse Shape;
        internal double Angle = -Math.PI / 2;

        public Circle(double radious) { 
            Radious = radious;

            Pointer = new()
            {
                Width = 6,
                Height = 6,
                Fill = Brushes.Red
            };

            Shape = new Ellipse
            {
                Width = Radious * 2,
                Height = Radious * 2,
                Stroke = Brushes.Gray,
                StrokeThickness = 2,
            };
        }
    }
}
