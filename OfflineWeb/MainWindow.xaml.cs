using System;
using System.Windows;

namespace OfflineWeb
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void NavigateToDownloadPage(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("/downloadPage.xaml", UriKind.Relative);
            frame.Source = uri;
        }

        private void NavigateToOfflineWebPage(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("/OfflineWebPage.xaml", UriKind.Relative);
            frame.Source = uri;
        }
    }
}
