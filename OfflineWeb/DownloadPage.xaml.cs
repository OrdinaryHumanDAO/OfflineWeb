using AngleSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            var yahooURL = "https://www.yahoo.co.jp/";
            var wikipediaURL = "https://ja.wikipedia.org/wiki/%E3%83%A1%E3%82%A4%E3%83%B3%E3%83%9A%E3%83%BC%E3%82%B8";
            var appleURL = "https://www.apple.com/jp/store?afid=p238%7CsKSMckWTz-dc_mtid_18707vxu38484_pcrid_673906064385_pgrid_13140806301_pntwk_g_pchan__pexid__&cid=aos-jp-kwgo-brand--slid---product-";
            var aaaURL = "https://hurugiblog.com/talon-zipper";

            try
            {
                saveWebPage(urlInputForm.Text);
                text1.Text = "保存しました";
            }
            catch (Exception ex)
            {
                text1.Text = "エラーが発生しました。";
            }

        }

        public async void saveWebPage(string url)
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
    }

    
}
