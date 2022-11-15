﻿using System.Windows;

namespace PanPakapon
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string ResultPath = "";
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
                    "\n(With certain ACCURATE time with 44100 Hz Frequency)",
                    "Invalid data found", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (!Voices.HasValidVoice)
            {
                MessageBox.Show("Select voice data for music.",
                    "No voice data selected", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (!SavePathGetter.HasValidPath)
            {
                MessageBox.Show("Select desination to save the result.",
                    "Invalid result path", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                TabIndex.CompareTo(0);
                var convertWindow = new ConvertWindow();
                convertWindow.Owner= this;
                convertWindow.DataContext= this;
                convertWindow.Show();
                convertWindow.Convert(
                    new CustomMusicCreator.PataMusicModel(
                        BaseTheme.FileName, Level1Theme.FileName, Level2Theme.FileName, Level3Theme.FileName,
                        Voices.VoiceName, SavePathGetter.ResultPath)
                    );
            }
        }
    }
}
