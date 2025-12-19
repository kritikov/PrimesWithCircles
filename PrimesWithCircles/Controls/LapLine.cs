using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PrimesWithCircles.Controls
{
    public class LapLine
    {
        public Line line;
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
                Stroke = Brushes.Blue,
                StrokeThickness = canvas.LapLineThickness,
                StrokeDashArray = [2, 2],
                Visibility = canvas.DisplayLapLine ? Visibility.Visible : Visibility.Collapsed
            };
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

        public void Rescale()
        {
            line.StrokeThickness = canvas.LapLineThickness / canvas.CurrentScale;
        }
    }
}
