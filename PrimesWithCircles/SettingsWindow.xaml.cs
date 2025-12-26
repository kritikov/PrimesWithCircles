using System.Windows;

namespace PrimesWithCircles
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

    }
}
