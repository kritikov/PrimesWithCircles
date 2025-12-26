using System.Windows;

namespace PrimesWithCircles
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {

        private bool isRotating = false;

        public SettingsWindow(RotationSettings rotationSettings)
        {
            InitializeComponent();

            DataContext = rotationSettings;
        }

        private void RotateButton_Click(object sender, RoutedEventArgs e)
        {
            //((MainWindow)Owner).RotateButtonClicked();
        }

        private void PauseBtn_Click(object sender, RoutedEventArgs e)
        {
            //((MainWindow)Owner).PauseButtonClicked();
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            //((MainWindow)Owner).ResetButtonClicked();
        }

    }
}
