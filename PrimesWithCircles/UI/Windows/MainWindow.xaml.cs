using PrimesWithCircles.Infrastucture;
using System.Windows;
using System.Windows.Media;

namespace PrimesWithCircles.UI.Windows
{
    public partial class MainWindow : Window
    {
        private readonly RotationSettings settings;
        private readonly SettingsWindow settingsWindow;
        
        private bool isRotating = false;

        public MainWindow()
        {
            InitializeComponent();
            
            RotationCanvas.PrimesChanged += () =>
            {
                PrimesScroll.ScrollToEnd();
            };

            settings = new();

            DataContext = settings;
            RotationCanvas.Settings = settings;

            settingsWindow = new(settings);
            settingsWindow.RotateRequested += RotateRequested;
            settingsWindow.PauseRequested += PauseRequested;
            settingsWindow.ResetRequested += ResetRequested;
        }

        #region EVENTS

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RotationCanvas.Reset();
            settingsWindow.Show();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            settingsWindow.Close();
        }


        #endregion


        #region METHODS

        private void RotateRequested()
        {
            RotationCanvas.StartRotating();
        }

        private void PauseRequested()
        {
            RotationCanvas.StopRotating();
        }

        private void ResetRequested()
        {
            RotationCanvas.StopRotating();
            RotationCanvas.Reset();
        }

        #endregion

        
    }
}
