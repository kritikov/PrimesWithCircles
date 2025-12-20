using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PrimesWithCircles
{
    public class Theme(
        ThemeType type,
        SolidColorBrush circleColor,
        SolidColorBrush pointerColor,
        SolidColorBrush trailColor,
        SolidColorBrush lapLineColor
        )
    {
        public ThemeType Type = type;
        public SolidColorBrush CircleColor = circleColor;
        public SolidColorBrush PointerColor = pointerColor;
        public SolidColorBrush TrailColor = trailColor;
        public SolidColorBrush LapLineColor = lapLineColor;
    };


    public class Themes
    {
        private static List<Theme> list =
        [
            new(ThemeType.Dark,  Brushes.LightGray, Brushes.Yellow, Brushes.Red, Brushes.Blue),
            new(ThemeType.Light,  Brushes.LightGray, Brushes.Yellow, Brushes.Red, Brushes.Orange)
        ];

        public static Theme GetTheme(ThemeType themeType)
        {
            var theme = list.First(x => x.Type == themeType);
            return theme;
        }
    }


    public enum ThemeType
    {
        Dark,
        Light
    }
}
