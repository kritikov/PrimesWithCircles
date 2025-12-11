using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace PrimesWithCircles.Controls
{
    public class RotationCanvas : Canvas, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string? propName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));


        #region PROPERTIES AND FIELDS

        private readonly List<Circle> circles = [];
        private ScaleTransform ZoomTransform { get; set; }

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
        public double BaseRadiousMin => 10.0;
        public double BaseRadiousMax => 200.0;


        private bool displayShapes = true;
        public bool DisplayShapes
        {
            get => displayShapes;
            set
            {
                if (displayShapes != value)
                {
                    displayShapes = value;
                    UpdateShapesVisibility();
                    OnPropertyChanged(nameof(DisplayShapes));
                }
            }
        }

        public double RotationSpeedMin => 0.1;
        public double RotationSpeedMax => 20.0;

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
                    BaseAngularSpeed = Math.PI * value;
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
                    UpdatePointerSizes();
                    OnPropertyChanged(nameof(PointerSize));

                }
            }
        }
        public double PointerSizeMin => 4.0;
        public double PointerSizeMax => 40.0;

        public double BaseAngularSpeed { get; set; } = Math.PI;
        public double CurrentScale { get; set; } = 1.0;

        public Visibility ShapesVisibility
        {
            get => DisplayShapes == true ? Visibility.Visible : Visibility.Hidden;
        }

        #endregion


        #region CONSTRUCTORS

        public RotationCanvas()
        {
            ZoomTransform  = new ScaleTransform(CurrentScale, CurrentScale);
            LayoutTransform = ZoomTransform;
        }

        #endregion


        #region METHODS

        /// <summary>
        /// Add a new circle representing the given number
        /// </summary>
        public void AddCircle(int number)
        {
            var circle = new Circle(this, number);
            this.circles.Add(circle);

            AdjustZoom();
        }

        /// <summary>
        /// Clear all circles
        /// </summary>
        public void Clear()
        {
            this.circles.Clear();
            this.Children.Clear();
        }

        /// <summary>
        /// Center all circles in the canvas
        /// </summary>
        public void CenterAllCircles()
        {
            foreach (var circle in circles)
            {
                circle.Center();
            }
        }

        public void UpdateBaseRadious()
        {
            foreach (var circle in circles)
            {
                circle.UpdateRadious();
            }
        }

        public void UpdatePointerSizes()
        {
            foreach (var circle in circles)
            {
                circle.UpdatePointerSize();
            }
        }

        public void UpdateShapesVisibility()
        {
            foreach (var circle in circles)
            {
                circle.UpdateShapeVisibility();
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
            foreach (var circle in circles)
            {
                bool lapCompleted = circle.RotateCircle(rotationStep, firstCompleted);
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
            if (this.circles.Count == 0)
                return 0.0;

            return this.circles.Max(c => c.Radious);
        }

        /// <summary>
        /// Rescale all circles according to the given currentScale value.
        /// </summary>
        /// <param name="currentScale"></param>
        public void RescaleCircles()
        {
            foreach (var circle in circles)
            {
                circle.Rescale();
            }

        }

        /// <summary>
        /// Zoom with animation
        /// </summary>
        /// <param name="targetScale"></param>
        public void Zoom(double targetScale)
        {
            CurrentScale = targetScale;

            DoubleAnimation anim = new()
            {
                To = CurrentScale,
                Duration = TimeSpan.FromMilliseconds(1000),
                EasingFunction = new QuinticEase { EasingMode = EasingMode.EaseOut }
            };

            ZoomTransform.BeginAnimation(ScaleTransform.ScaleXProperty, anim);
            ZoomTransform.BeginAnimation(ScaleTransform.ScaleYProperty, anim);

            RescaleCircles();
        }

        /// <summary>
        /// Adjust the zoom level automatically to fit all circles in the canvas
        /// </summary>
        public void AdjustZoom()
        {
            if (circles.Count == 0) return;

            double maxRadius = GetMaxRadious();
            double neededSize = maxRadius * 2 + 100; // buffer

            double actualWidthWithoutScale = ActualWidth * ZoomTransform.ScaleX;
            double actualHeightWithoutScale = ActualHeight * ZoomTransform.ScaleX;

            double scaleX = actualWidthWithoutScale / neededSize;
            double scaleY = actualHeightWithoutScale / neededSize;

            double newScale = Math.Min(scaleX, scaleY);

            if (newScale < 1)
            {
                Zoom(newScale);
            }

        }

        #endregion
    }

}
