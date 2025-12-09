using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PrimesWithCircles.Models
{
    public class Circles
    {
        public List<Circle> List = [];
        public Canvas Canvas { get; }
        public double BaseRadious = 30.0;
        public double BaseAngularSpeed = Math.PI;

        public Circles(Canvas canvas)
        {
            Canvas = canvas;
        }


        /// <summary>
        /// Add a new circle representing the given number
        /// </summary>
        public Circle Add(int number)
        {
            var circle = new Circle(this.Canvas, number, BaseRadious);

            this.List.Add(circle);

            return circle;
        }

        /// <summary>
        /// Clear all circles
        /// </summary>
        public void Clear()
        {
            this.List.Clear();
            this.Canvas.Children.Clear();
            

        }

        /// <summary>
        /// Center all circles in the canvas
        /// </summary>
        public void CenterAllCircles()
        {
            foreach (var circle in List)
            {
                circle.Center();
            }
        }

        /// <summary>
        /// Rotate all circles for elapsedSec seconds. Stops and handles lap logic for first circle.
        /// </summary>
        public (bool FirstCompleted, bool SomeOtherCompleted) RotateCircles(double rotationStep)
        {
            bool firstCompleted = false;
            bool someOtherCompleted = false;

            // rotate all circles
            foreach (var circle in List)
            {
                bool lapCompleted = circle.RotateCircle(rotationStep, firstCompleted, BaseRadious, BaseAngularSpeed);
                if (circle.Number == 1 && lapCompleted)
                    firstCompleted = true;

                if (circle.Number > 1 && firstCompleted && lapCompleted)
                    someOtherCompleted = true;
            }

            return (firstCompleted, someOtherCompleted);
        }

        /// <summary>
        /// Returns the largest radius value among all circles in the collection.
        /// </summary>
        public double GetMaxRadious()
        {
            if (this.List.Count == 0)
                return 0.0;

            return this.List.Max(c => c.Radious);
        }

        /// <summary>
        /// Rescale all circles according to the given currentScale value.
        /// </summary>
        /// <param name="currentScale"></param>
        public void RescaleCircles(double currentScale)
        {
            foreach (var circle in List)
            {
                circle.Rescale(currentScale);
            }
        }
    }

}
