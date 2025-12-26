using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace PrimesWithCircles.Controls
{
    public class LapLine
    {
        public Line line;

        public double Thickness { get; set; } = 2;

        private bool display = true;
        public bool Display 
        { 
            get => display;
            set
            {
                if (display != value)
                {
                    display = value;
                    line.Visibility = display ? Visibility.Visible : Visibility.Hidden;
                }
            }
        }
        
        private readonly RotationCanvas canvas;

        public LapLine(RotationCanvas canvas)
        {
            this.canvas = canvas;

            var centerOfCanvas = new Point(this.canvas.ActualWidth / 2, this.canvas.ActualHeight / 2);

            line = new Line
            {
                X1 = centerOfCanvas.X,
                Y1 = centerOfCanvas.Y,
                X2 = centerOfCanvas.X,
                Y2 = 20,
                Stroke = canvas.Settings.Theme.LapLineColor,
                StrokeThickness = Thickness,
                //StrokeDashArray = [8, 2],
                Visibility = canvas.DisplayLapLine ? Visibility.Visible : Visibility.Collapsed,
            };
        }

        /// <summary>
        /// Update the colors of the lap line according to the given theme.
        /// </summary>
        public void UpdateFromTheme()
        {
            line.Stroke = canvas.Settings.Theme.LapLineColor;
        }

        public void UpdateThickness(double thickness)
        {
            this.Thickness = thickness;
            line.StrokeThickness = this.Thickness;
            Rescale();
        }
        /// <summary>
        /// Set the start point of the lap line to the center of the canvas.
        /// </summary>
        public void Center()
        {
            var centerOfCanvas = new Point(canvas.ActualWidth / 2, canvas.ActualHeight / 2);

            line.X2 = centerOfCanvas.X;
            line.X1 = centerOfCanvas.X;
            line.Y1 = centerOfCanvas.Y;
        }

        /// <summary>
        /// Rescale the lap line thickness according to the current scale of the canvas.
        /// </summary>
        public void Rescale()
        {
            line.StrokeThickness = this.Thickness / canvas.CurrentScale;
        }

        /// <summary>
        /// Flash the lap line
        /// </summary>
        public void Flash()
        {
            if (line.Stroke is not SolidColorBrush brush) return;

            Color originalColor = brush.Color;
            Color flashColor = GetFlashColor();

            var colorAnim = new ColorAnimation
            {
                From = originalColor,
                To = flashColor,
                Duration = TimeSpan.FromMilliseconds(120),
                AutoReverse = true,
                EasingFunction = new QuadraticEase
                {
                    EasingMode = EasingMode.EaseOut
                }
            };

            brush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnim);
        }

        /// <summary>
        /// Get a contrasting color for the flash effect based on the canvas background color.
        /// </summary>
        /// <returns></returns>
        private Color GetFlashColor()
        {
            var bg = canvas.Settings.Theme.BackgroundColor.Color;

            // luminance check
            double luma = 0.2126 * bg.R + 0.7152 * bg.G + 0.0722 * bg.B;

            return luma < 128
                ? Colors.White
                : Colors.Black;
        }
    }
}
