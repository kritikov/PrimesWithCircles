using PrimesWithCircles.Controls;
using PrimesWithCircles.Enums;
using System.ComponentModel;
using System.Windows.Media;

namespace PrimesWithCircles
{
    public class RotationSettings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public static double RotationSpeedMin => 1;
        public static double RotationSpeedMax => 100.0;
        public static double BaseRadiousMin => 20.0;
        public static double BaseRadiousMax => 200.0;
        public static double LapLineThicknessMin => 1.0;
        public static double LapLineThicknessMax => 10.0;
        public static double CircleThicknessMin => 1.0;
        public static double CircleThicknessMax => 10.0;
        public static double PointerSizeMin => 4.0;
        public static double PointerSizeMax => 40.0;
        public static double TrailThicknessMin => 3.0;
        public static double TrailThicknessMax => 10.0;

        public Brush PrimesColor => Theme.PrimesColor;
        public Brush CounterColor => Theme.CounterColor;
        public Brush BackgroundColor => Theme.BackgroundColor;

        private Theme theme = Themes.GetTheme(ThemeType.ClassicNeon);
        public Theme Theme {
            get => theme;
            set
            {
                if (theme != value)
                {
                    theme = value;
                    PropertyChanged?.Invoke(this, new(nameof(Theme)));
                    PropertyChanged?.Invoke(this, new(nameof(PrimesColor)));
                    PropertyChanged?.Invoke(this, new(nameof(CounterColor)));
                    PropertyChanged?.Invoke(this, new(nameof(BackgroundColor)));
                }
            }
        }

        private ThemeType themeType = ThemeType.ClassicNeon;
        public ThemeType ThemeType
        {
            get => themeType;
            set
            {
                if (themeType != value)
                {
                    themeType = value;
                    Theme = Themes.GetTheme(themeType);
                    PropertyChanged?.Invoke(this, new(nameof(ThemeType)));
                }
            }
        }


        private double rotationSpeed = 10;
        public double RotationSpeed
        {
            get => rotationSpeed;
            set
            {
                double clamped = Math.Clamp(value, RotationSpeedMin, RotationSpeedMax);
                if (rotationSpeed != clamped)
                {
                    rotationSpeed = clamped;
                    PropertyChanged?.Invoke(this, new(nameof(RotationSpeed)));
                }
            }
        }

        private bool autoRotation = true;
        public bool AutoRotation
        {
            get => autoRotation;
            set
            {
                if (autoRotation != value)
                {
                    autoRotation = value;
                    PropertyChanged?.Invoke(this, new(nameof(AutoRotation)));
                }
            }
        }

        private bool isReseted = true;
        public bool IsReseted
        {
            get => isReseted;
            set
            {
                if (isReseted != value)
                {
                    isReseted = value;
                    PropertyChanged?.Invoke(this, new(nameof(IsReseted)));
                }
            }
        }

        private PresentationMode presentationMode = PresentationMode.SeekPrimes;
        public PresentationMode PresentationMode
        {
            get => presentationMode;
            set
            {
                if (presentationMode != value)
                {
                    presentationMode = value;
                    PropertyChanged?.Invoke(this, new(nameof(PresentationMode)));
                }
            }
        }

        private double baseRadious = 10;
        public double BaseRadious
        {
            get => baseRadious;
            set
            {
                double clamped = Math.Clamp(value, BaseRadiousMin, BaseRadiousMax);
                if (baseRadious != clamped)
                {
                    baseRadious = clamped;
                    PropertyChanged?.Invoke(this, new(nameof(BaseRadious)));
                }
            }
        }

        private double lapLineThickness = 2.0;
        public double LapLineThickness
        {
            get => lapLineThickness;
            set
            {
                double clamped = Math.Clamp(value, LapLineThicknessMin, LapLineThicknessMax);
                if (lapLineThickness != clamped)
                {
                    lapLineThickness = clamped;
                    PropertyChanged?.Invoke(this, new(nameof(LapLineThickness)));
                }
            }
        }

        private double circleThickness = Circle.CircleThickness;
        public double CircleThickness
        {
            get => circleThickness;
            set
            {
                double clamped = Math.Clamp(value, CircleThicknessMin, CircleThicknessMax);
                if (circleThickness != clamped)
                {
                    circleThickness = clamped;
                    PropertyChanged?.Invoke(this, new(nameof(CircleThickness)));
                }
            }
        }

        private double pointerSize = Circle.PointerSize;
        public double PointerSize
        {
            get => pointerSize;
            set
            {
                double clamped = Math.Clamp(value, PointerSizeMin, PointerSizeMax);
                if (pointerSize != clamped)
                {
                    pointerSize = clamped;
                    PropertyChanged?.Invoke(this, new(nameof(PointerSize)));
                }
            }
        }

        private double trailThickness = Circle.TrailThickness;
        public double TrailThickness
        {
            get => trailThickness;
            set
            {
                double clamped = Math.Clamp(value, TrailThicknessMin, TrailThicknessMax);
                if (trailThickness != clamped)
                {
                    trailThickness = clamped;
                    PropertyChanged?.Invoke(this, new(nameof(TrailThickness)));
                }
            }
        }

       
        private bool displayPrimes = true;
        public bool DisplayPrimes
        {
            get => displayPrimes;
            set
            {
                if (displayPrimes != value)
                {
                    displayPrimes = value;
                    PropertyChanged?.Invoke(this, new(nameof(DisplayPrimes)));
                }
            }
        }

        private bool displayLapLine = true;
        public bool DisplayLapLine
        {
            get => displayLapLine;
            set
            {
                if (displayLapLine != value)
                {
                    displayLapLine = value;
                    PropertyChanged?.Invoke(this, new(nameof(DisplayLapLine)));
                }
            }
        }

        private bool flashLapLine = true;
        public bool FlashLapLine
        {
            get => flashLapLine;
            set
            {
                if (flashLapLine != value)
                {
                    flashLapLine = value;
                    PropertyChanged?.Invoke(this, new(nameof(FlashLapLine)));
                }
            }
        }

        private bool displayCircles = true;
        public bool DisplayCircles
        {
            get => displayCircles;
            set
            {
                if (displayCircles != value)
                {
                    displayCircles = value;
                    PropertyChanged?.Invoke(this, new(nameof(DisplayCircles)));
                }
            }
        }

        private bool displayTrails = true;
        public bool DisplayTrails
        {
            get => displayTrails;
            set
            {
                if (displayTrails != value)
                {
                    displayTrails = value;
                    PropertyChanged?.Invoke(this, new(nameof(DisplayTrails)));
                }
            }
        }



    }
}
