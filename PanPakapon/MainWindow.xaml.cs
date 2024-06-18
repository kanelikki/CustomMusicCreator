using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;

namespace PanPakapon
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Style = (Style)FindResource(typeof(Window));
        }
        private void Convert(object sender, RoutedEventArgs e)
        {
            if (!BaseTheme.HasValidFile || !Level1Theme.HasValidFile
                || !Level2Theme.HasValidFile || !Level3Theme.HasValidFile)
            {
                MessageBox.Show(
                    "Some of WAV files are not valid. Make sure all WAVs are in correct format" +
                    "\n(With certain ACCURATE time, +- 1ms)",
                    "Invalid data found", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (!Voices.HasValidVoice)
            {
                MessageBox.Show("Select voice data for music.",
                    "No voice data selected", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (!SavePathGetter.HasValidPath)
            {
                MessageBox.Show("Select valid desination to save the result.",
                    "Invalid result path", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                TabIndex.CompareTo(0);
                var convertWindow = new ConvertWindow();
                convertWindow.Owner = this;
                convertWindow.DataContext = this;
                convertWindow.Show();
                convertWindow.Convert(
                    new CustomMusicCreator.PataMusicModel(
                        BaseTheme.FileName, Level1Theme.FileName, Level2Theme.FileName, Level3Theme.FileName,
                        Voices.VoiceName, SavePathGetter.ResultPath, VolumeHandle.Volume)
                    );
            }
        }

        private void OpenHelp(object sender, RoutedEventArgs e) => new HelpWindow().ShowDialog();

        private void OpenCredits(object sender, RoutedEventArgs e) => new CreditWindow().ShowDialog();

        private void LinkClick(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
             Process.Start(new ProcessStartInfo(e.Uri.ToString()) { UseShellExecute = true });
        }
    }
}
