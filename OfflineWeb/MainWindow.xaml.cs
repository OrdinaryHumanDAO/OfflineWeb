using AngleSharp;
using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Navigation;

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
