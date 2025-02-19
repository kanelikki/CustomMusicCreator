using System.Windows.Controls;

namespace PanPakapon
{
    /// <summary>
    /// Interaction logic for VolumeChanger.xaml
    /// </summary>
    public partial class VolumeChanger : UserControl
    {
        public double Volume { get; private set; } = 1;
        public VolumeChanger()
        {
            InitializeComponent();
        }
        private void ChangeVolume(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            Volume = e.NewValue;
            if (VolumeText != null) //make sure the value is initialised
            {
                VolumeText.Text = Volume.ToString("P0");
            }
        }
    }
}
