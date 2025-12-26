using PrimesWithCircles.Controls;
using PrimesWithCircles.Enums;
using System.ComponentModel;
using System.Runtime;
using System.Windows;
using System.Windows.Media;

namespace PrimesWithCircles
{
    public partial class MainWindow : Window
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly RotationSettings settings;
        private readonly SettingsWindow settingsWindow;
        
        private bool isRotating = false;

        public MainWindow()
        {
            InitializeComponent();
            
            Loaded += OnLoaded;
            RotationCanvas.SizeChanged += OnCanvasSizeChanged;
            RotationCanvas.PrimesChanged += () =>
            {
                PrimesScroll.ScrollToEnd();
            };

            settings = new();

            DataContext = settings;
            RotationCanvas.Settings = settings;

            settingsWindow = new(settings);
        }

        #region EVENTS

        private void OnLoaded(object sender, RoutedEventArgs e)
        {

            ResetData();

            settingsWindow.Show();

        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            settingsWindow.Close();

        }

        private void OnCanvasSizeChanged(object sender, SizeChangedEventArgs e)
        {
            // COMMENT: AdjustZoom method already called in RotationCanvas when size changes. Potentional problem with double calls?

            RotationCanvas.CenterAll();
            RotationCanvas.AdjustZoom();
        }
        
        private void OnRendering(object? sender, EventArgs e)
        {
            if (isRotating)
                RotateCircles();
        }
        
        private void RotateButton_Click(object sender, RoutedEventArgs e)
        {
            // if not allready rotating, start rotating
            if (!isRotating)
            {
                RotationCanvas.IsReseted = false;
                StartRotation();
            }
        }

        private void PauseBtn_Click(object sender, RoutedEventArgs e)
        {
            StopRotation();
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            StopRotation();
            ResetData();
        }

        #endregion


        #region METHODS

       

        /// <summary>
        /// Initializes the screen and sets up the initial circles and lap counter for the rotation canvas.
        /// </summary>
        private void ResetData()
        {
            RotationCanvas.Reset();
        }

        /// <summary>
        /// Start the rotation by subscribing to the rendering event.
        /// </summary>
        private void StartRotation()
        {
            if (isRotating) return;
            CompositionTarget.Rendering += OnRendering;
            isRotating = true;
        }

        /// <summary>
        /// Stop the rotation by unsubscribing from the rendering event.
        /// </summary>
        private void StopRotation()
        {
            if (!isRotating) return;
            CompositionTarget.Rendering -= OnRendering;
            isRotating = false;
        }

        /// <summary>
        /// Rotate all circles for a rendering frame.
        /// </summary>
        private void RotateCircles()
        {
            var lapCompleted = RotationCanvas.RotateCirclesFrame();

            // if auto-rotation is disabled and a lap is completed, stop the rotation
            if (RotationCanvas.AutoRotation != true && lapCompleted)
                StopRotation();
        }

        #endregion

    }
}
