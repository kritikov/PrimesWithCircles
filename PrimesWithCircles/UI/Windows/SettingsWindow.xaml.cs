using PrimesWithCircles.Infrastucture;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

namespace PrimesWithCircles.UI.Windows
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {

        public event Action? RotateRequested;
        public event Action? PauseRequested;
        public event Action? ResetRequested;

        public SettingsWindow(RotationSettings rotationSettings)
        {
            InitializeComponent();

            DataContext = rotationSettings;
        }

        private void RotateButton_Click(object sender, RoutedEventArgs e)
        {
            RotateRequested?.Invoke();
        }

        private void PauseBtn_Click(object sender, RoutedEventArgs e)
        {
            PauseRequested?.Invoke();
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            ResetRequested?.Invoke();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = e.Uri.AbsoluteUri,
                UseShellExecute = true
            });
        }

    }
}
