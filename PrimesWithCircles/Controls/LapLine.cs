using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PrimesWithCircles.Controls
{
    public class LapLine
    {
        public static double Thickness = 1.5;
        public Line line;

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
                Stroke = canvas.Theme.LapLineColor,
                StrokeThickness = LapLine.Thickness,
                StrokeDashArray = [2, 2],
                Visibility = canvas.DisplayLapLine ? Visibility.Visible : Visibility.Collapsed
            };
        }

        /// <summary>
        /// Update the colors of the lap line according to the given theme.
        /// </summary>
        public void UpdateFromTheme()
        {
            line.Stroke = canvas.Theme.LapLineColor;
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
            line.StrokeThickness = LapLine.Thickness / canvas.CurrentScale;
        }
    }
}
