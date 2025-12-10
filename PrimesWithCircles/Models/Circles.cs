using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PrimesWithCircles.Models
{
    public class Circles : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

        public List<Circle> List = [];
        public Canvas Canvas { get; }

        private double baseRadious = 30.0;
        public double BaseRadious
        {
            get => baseRadious;
            set
            {
                if (baseRadious != value)
                {
                    baseRadious = value;
                    UpdateBaseRadious();
                    OnPropertyChanged(nameof(BaseRadious));
                }
            }
        }

        private bool displayShapes = true;
        public bool DisplayShapes
        {
            get => displayShapes;
            set
            {
                if (displayShapes != value)
                {
                    displayShapes = value;
                    UpdateShapesVisibility(displayShapes);
                    OnPropertyChanged(nameof(DisplayShapes));
                }
            }
        }

        private double rotationSpeed = 1;
        public double RotationSpeed
        {
            get => rotationSpeed;
            set
            {
                if (rotationSpeed != value)
                {
                    rotationSpeed = value;
                    OnPropertyChanged(nameof(RotationSpeed));

                    // Και αν πρέπει να πολλαπλασιαστεί με Math.PI:
                    baseAngularSpeed = Math.PI * value;
                }
            }
        }

        private double pointerSize = 8.0;
        public double PointerSize
        {
            get => pointerSize;
            set
            {
                if (pointerSize != value)
                {
                    pointerSize = value;
                    UpdatePointerSizes(pointerSize);
                    OnPropertyChanged(nameof(PointerSize));

                }
            }
        }

        private double baseAngularSpeed = Math.PI;

        public Circles(Canvas canvas)
        {
            Canvas = canvas;
        }


        /// <summary>
        /// Add a new circle representing the given number
        /// </summary>
        public Circle Add(int number)
        {
            var visibility = DisplayShapes == true ? Visibility.Visible : Visibility.Hidden;
            var circle = new Circle(this.Canvas, number, baseRadious, visibility, PointerSize);

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

        public void UpdateBaseRadious()
        {
            foreach (var circle in List)
            {
                circle.UpdateRadious(baseRadious);
            }
        }

        public void UpdatePointerSizes(double size)
        {
            foreach (var circle in List)
            {
                circle.UpdatePointerSize(size);
            }
        }

        public void UpdateShapesVisibility(bool isVisible)
        {
            foreach (var circle in List)
            {
                circle.UpdateVisibility(isVisible);
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
                bool lapCompleted = circle.RotateCircle(rotationStep, firstCompleted, baseRadious, baseAngularSpeed);
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
