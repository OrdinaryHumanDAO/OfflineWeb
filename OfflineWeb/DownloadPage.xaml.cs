using AngleSharp;
using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OfflineWeb
{
    /// <summary>
    /// DownloadPage.xaml の相互作用ロジック
    /// </summary>
    public partial class DownloadPage : Page
    {
        public DownloadPage()
        {
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsNotUrl(urlInputForm.Text)) throw new Exception();
                saveWebPage(urlInputForm.Text);
                resultText.Text = "保存しました";
                resultText.Visibility = Visibility.Visible;
                resultText.Background = new SolidColorBrush(System.Windows.Media.Colors.LightGreen);
            }
            catch
            {
                resultText.Text = "ウェブページを取得できませんでした";
                resultText.Visibility = Visibility.Visible;
                resultText.Background = new SolidColorBrush(System.Windows.Media.Colors.OrangeRed);
            }

        }

        public void saveWebPage(string url)
        {
            Uri baseURI = new Uri(url);
            var parser = new AngleSharp.Html.Parser.HtmlParser();
            var doc = parser.ParseDocument(fetchHTML(url));

            string baseFilePath = App.OfflineInitFilePath;
            string defaultFilePath = baseFilePath + @"resouce\";
            var dirFileName = doc.QuerySelector("title").TextContent;
            string cssPath = defaultFilePath + dirFileName + @"\css\";
            string jsPath = defaultFilePath + dirFileName + @"\js\";
            string imgPath = defaultFilePath + dirFileName + @"\img\";

            //ディレクトリ追加
            SafeCreateDirectory(cssPath);
            SafeCreateDirectory(jsPath);
            SafeCreateDirectory(imgPath);


            //CSSファイルを取得する
            var i = 0;
            foreach (var stylesheetsNode in doc.QuerySelectorAll("link[rel='stylesheet']"))
            {
                String path = cssPath + "css" + i + ".css";
                var relativeURI = stylesheetsNode.GetAttribute("href");
                Uri cssURI = new Uri(baseURI, relativeURI);
                string absoluteCssURI = cssURI.AbsoluteUri;
                downloadCSS(absoluteCssURI, path);
                stylesheetsNode.SetAttribute("href", path);
                i++;
            }

            //JSファイルを取得する
            var j = 0;
            foreach (var scriptNode in doc.QuerySelectorAll("script"))
            {
                if (scriptNode.HasAttribute("src"))
                {
                    String path = jsPath + "js" + j + ".js";
                    var relativeURI = scriptNode.GetAttribute("src");
                    Uri jsURI = new Uri(baseURI, relativeURI);
                    string absoluteCssURI = jsURI.AbsoluteUri;
                    downloadJS(absoluteCssURI, path);
                    scriptNode.SetAttribute("src", path);
                }
                j++;
            }

            //画像ファイルを取得する
            var l = 0;
            foreach (var imgNode in doc.QuerySelectorAll("img"))
            {
                String path = imgPath + "img" + l + ".jpg";
                var relativeURI = imgNode.GetAttribute("src");
                Uri imgURI = new Uri(baseURI, relativeURI);
                string absoluteImgURI = imgURI.AbsoluteUri;
                downloadImg(absoluteImgURI, path);
                imgNode.SetAttribute("src", path);
                l++;
            }


            //htmlファイル書き込み
            File.WriteAllText(defaultFilePath + dirFileName + @"\index.html", doc.ToHtml());
            //htmlファイル読み出し
            //var text = File.ReadAllText(path);
        }

        public static DirectoryInfo SafeCreateDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                return null;
            }
            return Directory.CreateDirectory(path);
        }

        public String fetchHTML(string url)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            WebClient wc = new WebClient();
            Stream response = wc.OpenRead(url);
            StreamReader sr = new StreamReader(response);
            String result = sr.ReadToEnd();


            return result;
        }
        public void downloadCSS(string url, string path)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            WebClient wc = new WebClient();
            try
            {
                Stream response = wc.OpenRead(url);
                StreamReader sr = new StreamReader(response);
                String css = sr.ReadToEnd();
                File.WriteAllText(path, css);
                return;
            }
            catch (WebException exc)
            {
                return;
            }
        }
        public void downloadJS(string url, string path)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            WebClient wc = new WebClient();
            try
            {
                Stream response = wc.OpenRead(url);
                StreamReader sr = new StreamReader(response);
                String js = sr.ReadToEnd();
                File.WriteAllText(path, js);
            }
            catch (WebException exc) { return; }
        }

        public void downloadImg(string url, String path)
        {
            WebClient wc = new WebClient();
            try
            {
                wc.DownloadFile(url, path);
            }
            catch (WebException exc)
            {
                throw exc;
            }
        }

        public static bool IsNotUrl(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            return !Regex.IsMatch(
               input,
               @"^s?https?://[-_.!~*'()a-zA-Z0-9;/?:@&=+$,%#]+$"
            );
        }
    }

    
}
