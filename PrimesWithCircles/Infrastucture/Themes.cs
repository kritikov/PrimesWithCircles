using PrimesWithCircles.Infrastucture.Enums;
using System.Windows.Media;

namespace PrimesWithCircles.Infrastucture
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
                type: ThemeType.DarkNeon,
                backgroundColor: Brushes.Black,
                circleColor: Brushes.LightGray,
                pointerColor: Brushes.Yellow,
                trailColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF3B3B")),
                lapLineColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00E5FF")),
                counterColor: Brushes.White,
                primesColor: Brushes.White
            ),

            new(
                type: ThemeType.ClassicNeon,
                backgroundColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0B0F14")),
                circleColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9FA8DA")),
                pointerColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD400")),
                trailColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF5252")),
                lapLineColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FFC6")),
                counterColor: Brushes.White,
                primesColor: Brushes.White
            ),

            new(
                type: ThemeType.BlackOrange,
                backgroundColor: Brushes.Black,
                circleColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E0E0E0")),
                pointerColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF9800")),
                trailColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF5722")),
                lapLineColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC107")),
                counterColor: Brushes.White,
                primesColor: Brushes.White
            ),

            new(
                type: ThemeType.Matrix,
                backgroundColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#040904")),
                circleColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#43A047")),
                pointerColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C6FF00")),
                trailColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00E676")),
                lapLineColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00C853")),
                counterColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CCFF90")),
                primesColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CCFF90"))
            ),

            new(
                type: ThemeType.ColdScience,
                backgroundColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#101820")),
                circleColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#90A4AE")),
                pointerColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#81D4FA")),
                trailColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4DD0E1")),
                lapLineColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#26C6DA")),
                counterColor: Brushes.White,
                primesColor: Brushes.White
            ),

            new(
                type: ThemeType.Synthwave,
                backgroundColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#16001E")),
                circleColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B0BEC5")),
                pointerColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFEB3B")),
                trailColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF4081")),
                lapLineColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#651FFF")),
                counterColor: Brushes.White,
                primesColor: Brushes.White
            ),

            new(
                type: ThemeType.Minimal,
                backgroundColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#121212")),
                circleColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#757575")),
                pointerColor: Brushes.White,
                trailColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFB74D")),
                lapLineColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9E9E9E")),
                counterColor: Brushes.White,
                primesColor: Brushes.White
            ),

            new(
                type: ThemeType.Light,
                backgroundColor: Brushes.White,
                circleColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E3A8A")),
                pointerColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FACC15")),
                trailColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC2626")),
                lapLineColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#16A34A")),
                counterColor: Brushes.Black,
                primesColor: Brushes.Black
            ),

            new(
                type: ThemeType.SoftPaper,
                backgroundColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FAFAF8")),
                circleColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B0BEC5")),
                pointerColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFB300")),
                trailColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C62828")),
                lapLineColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A1887F")),
                counterColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#37474F")),
                primesColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#37474F"))
            ),

            new(
                type: ThemeType.ScienceLab,
                backgroundColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E3F2FD")),
                circleColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#90CAF9")),
                pointerColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FB8C00")),
                trailColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E64A19")),
                lapLineColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00796B")),
                counterColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#102027")),
                primesColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#102027"))
            ),

            new(
                type: ThemeType.WarmMinimal,
                backgroundColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF8E1")),
                circleColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A1887F")),
                pointerColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F57C00")),
                trailColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E53935")),
                lapLineColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6D4C41")),
                counterColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3E2723")),
                primesColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3E2723"))
            ),

            new(
                type: ThemeType.ModernFlatUI,
                backgroundColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ECFDF5")),
                circleColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9CA3AF")),
                pointerColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FBBF24")),
                trailColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EF4444")),
                lapLineColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#059669")),
                counterColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#064E3B")),
                primesColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#064E3B"))
            ),

            new(
                type: ThemeType.UltraMinimal,
                backgroundColor: Brushes.White,
                circleColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#BDBDBD")),
                pointerColor: Brushes.Black,
                trailColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E53935")),
                lapLineColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#BDBDBD")),
                counterColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#212121")),
                primesColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#212121"))
            ),

            new(
                type: ThemeType.WhiteboardPro,
                backgroundColor: Brushes.White,
                circleColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#374151")),
                pointerColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F59E0B")),
                trailColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC2626")),
                lapLineColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2563EB")),
                counterColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#111827")),
                primesColor: new SolidColorBrush((Color)ColorConverter.ConvertFromString("#111827"))
            ),
        ];


        public static Theme GetTheme(ThemeType themeType)
        {
            var theme = list.First(x => x.Type == themeType);
            return theme;
        }
    }
    
}
