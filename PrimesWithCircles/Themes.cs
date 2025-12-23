using PrimesWithCircles.Enums;
using System.Windows.Media;

namespace PrimesWithCircles
{
    public class Theme(
        ThemeType type,
        SolidColorBrush backgroundColor,
        SolidColorBrush counterColor,
        SolidColorBrush primesColor,
        SolidColorBrush circleColor,
        SolidColorBrush pointerColor,
        SolidColorBrush trailColor,
        SolidColorBrush lapLineColor
        )
    {
        public ThemeType Type = type;
        public SolidColorBrush BackgroundColor = backgroundColor;
        public SolidColorBrush CircleColor = circleColor;
        public SolidColorBrush PointerColor = pointerColor;
        public SolidColorBrush TrailColor = trailColor;
        public SolidColorBrush LapLineColor = lapLineColor;
        public SolidColorBrush CounterColor = counterColor;
        public SolidColorBrush PrimesColor = primesColor;
    };

    public class Themes
    {
        private static List<Theme> list =
        [
            new(
                type: ThemeType.Dark,
                backgroundColor: Brushes.Black,
                circleColor: Brushes.LightGray,
                pointerColor: Brushes.Yellow,
                trailColor: Brushes.Red,
                lapLineColor: Brushes.Blue,
                counterColor: Brushes.White,
                primesColor: Brushes.White
                ),
            new(
                type: ThemeType.Light,
                backgroundColor: Brushes.White,
                circleColor: Brushes.DarkBlue,
                pointerColor: Brushes.Yellow,
                trailColor: Brushes.Red,
                lapLineColor:Brushes.Green,
                counterColor: Brushes.Black,
                primesColor: Brushes.Black
                ),
            new(
                type: ThemeType.ClassicNeon,
                backgroundColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0B0F14")),
                circleColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A0A0A0")),
                pointerColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD400")),
                trailColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF4D4D")),
                lapLineColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4DA6FF")),
                counterColor: Brushes.White,
                primesColor: Brushes.White
                ),
             new(
                type: ThemeType.Matrix,
                backgroundColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#050A05")),
                circleColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4CAF50")),
                pointerColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B6FF00")),
                trailColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FF88")),
                lapLineColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00B3FF")),
                counterColor: Brushes.White,
                primesColor: Brushes.White
                ),
             new(
                type: ThemeType.ColdScience,
                backgroundColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0E1117")),
                circleColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8A8F98")),
                pointerColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4FC3F7")),
                trailColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00BCD4")),
                lapLineColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E88E5")),
                counterColor: Brushes.White,
                primesColor: Brushes.White
                ),
             new(
                type: ThemeType.Synthwave,
                backgroundColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#12001A")),
                circleColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9E9E9E")),
                pointerColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFEA00")),
                trailColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2E88")),
                lapLineColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7C4DFF")),
                counterColor: Brushes.White,
                primesColor: Brushes.White
                ),
             new(
                type: ThemeType.Minimal,
                backgroundColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#111111")),
                circleColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#666666")),
                pointerColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")),
                trailColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF9800")),
                lapLineColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2196F3")),
                counterColor: Brushes.White,
                primesColor: Brushes.White
                ),
             new(
                type: ThemeType.LightEngineering,
                backgroundColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F5F7FA")),
                circleColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9AA0A6")),
                pointerColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F9A825")),
                trailColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E53935")),
                lapLineColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E88E5")),
                counterColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#263238")),
                primesColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#263238"))
                ),
             new(
                type: ThemeType.SoftPaper,
                backgroundColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FAFAFA")),
                circleColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B0BEC5")),
                pointerColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFB300")),
                trailColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D32F2F")),
                lapLineColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1976D2")),
                counterColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#37474F")),
                primesColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#37474F"))
                ),
             new(
                type: ThemeType.ScienceLab,
                backgroundColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E3F2FD")),
                circleColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#90A4AE")),
                pointerColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6F00")),
                trailColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D84315")),
                lapLineColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0D47A1")),
                counterColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#102027")),
                primesColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#102027"))
                ),
             new(
                type: ThemeType.WarmMinimal,
                backgroundColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF8E1")),
                circleColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A1887F")),
                pointerColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F57C00")),
                trailColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D32F2F")),
                lapLineColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5D4037")),
                counterColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3E2723")),
                primesColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3E2723"))
                ),
             new(
                type: ThemeType.ModernFlatUI,
                backgroundColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F1F8E9")),
                circleColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9E9E9E")),
                pointerColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC107")),
                trailColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E53935")),
                lapLineColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2E7D32")),
                counterColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1B5E20")),
                primesColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1B5E20"))
                ),
             new(
                type: ThemeType.UltraMinimal,
                backgroundColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")),
                circleColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#BDBDBD")),
                pointerColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000")),
                trailColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F44336")),
                lapLineColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2196F3")),
                counterColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1B5E20")),
                primesColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#212121"))
                ),
             new(
                type: ThemeType.DarkNeon,
                backgroundColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0B0E11")),
                circleColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C7C7C7")),
                pointerColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD600")),
                trailColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF3D00")),
                lapLineColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2979FF")),
                counterColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")),
                primesColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"))
                ),
             new(
                type: ThemeType.DarkCyan,
                backgroundColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#111827")),
                circleColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9CA3AF")),
                pointerColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#22D3EE")),
                trailColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EF4444")),
                lapLineColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#38BDF8")),
                counterColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E5E7EB")),
                primesColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E5E7EB"))
                ),
             new(
                type: ThemeType.WhiteboardPro,
                backgroundColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")),
                circleColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4B5563")),
                pointerColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F59E0B")),
                trailColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC2626")),
                lapLineColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2563EB")),
                counterColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#111827")),
                primesColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#111827"))
                ),
             new(
                type: ThemeType.BlackOrange,
                backgroundColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000")),
                circleColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D1D5DB")),
                pointerColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FB8C00")),
                trailColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E53935")),
                lapLineColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E88E5")),
                counterColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")),
                primesColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"))
                ),
        ];

        public static Theme GetTheme(ThemeType themeType)
        {
            var theme = list.First(x => x.Type == themeType);
            return theme;
        }
    }
    
}
