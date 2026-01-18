using PrimesWithCircles.Infrastucture;
using System.Windows;
using System.Windows.Media;

namespace PrimesWithCircles.UI.Windows
{
    public partial class MainWindow : Window
    {
        private readonly RotationSettings settings;
        private readonly SettingsWindow settingsWindow;
        
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
            settingsWindow.Owner = this;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
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
