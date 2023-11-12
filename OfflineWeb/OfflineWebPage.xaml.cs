using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;


namespace OfflineWeb
{
    /// <summary>
    /// OfflineWebPage.xaml の相互作用ロジック
    /// </summary>
    public partial class OfflineWebPage : Page
    {
        ObservableCollection<string> WebPageDirPaths = new ObservableCollection<string>();
        ObservableCollection<string> WebPageDirNames = new ObservableCollection<string>();

        public OfflineWebPage()
        {
            InitializeComponent();

            string resoucePath = App.OfflineInitFilePath + "resouce";
            if (Directory.Exists(resoucePath))
            {
                string[] dirs = Directory.GetDirectories(App.OfflineInitFilePath + "resouce");


                foreach (var dir in dirs)
                {
                    string dirName = Path.GetFileName(dir);
                    if (dirName == null) return;
                    WebPageDirPaths.Add(dir);
                    WebPageDirNames.Add(dirName);
                }
            }

            WebPageList.ItemsSource = WebPageDirNames;
        }

        private void WebPageList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            string url = WebPageDirPaths[WebPageList.SelectedIndex] + @"\index.html";
            ProcessStartInfo pi = new ProcessStartInfo()
            {
                FileName = url,
                UseShellExecute = true,
            };

            Process.Start(pi);
        }

    }
}
