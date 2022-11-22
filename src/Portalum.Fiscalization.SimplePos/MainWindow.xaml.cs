using System.Windows;
using System.Windows.Media;

namespace Portalum.Fiscalization.SimplePos
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly PosPage posPage1 = new PosPage();
        private readonly PosPage posPage2 = new PosPage();

        public MainWindow()
        {
            this.InitializeComponent();

            this.ButtonPos1_Click(null, null);
        }

        private void ButtonPos1_Click(object sender, RoutedEventArgs e)
        {
            this.ButtonPos2.Background = new SolidColorBrush(Color.FromRgb(96, 118, 134));
            this.ButtonPos1.Background = new SolidColorBrush(Color.FromRgb(152, 192, 17));
            

            this.NavigationFrame.Navigate(posPage1);
        }

        private void ButtonPos2_Click(object sender, RoutedEventArgs e)
        {
            this.ButtonPos1.Background = new SolidColorBrush(Color.FromRgb(96, 118, 134));
            this.ButtonPos2.Background = new SolidColorBrush(Color.FromRgb(152, 192, 17));

            this.NavigationFrame.Navigate(posPage2);
        }
    }
}
