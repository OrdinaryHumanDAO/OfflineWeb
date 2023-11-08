using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace OfflineWeb
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        /// <summary>
        /// 時刻初期データファイルパス
        /// </summary>
        public static string OfflineInitFilePath
        {
            get
            {
                // アプリケーションのパス取得
                string? appPath = AppContext.BaseDirectory;
                if (appPath is null)
                {
                    throw new DirectoryNotFoundException("実行ファイルのパス取得失敗");
                }

                // パスとファイル名を結合して返す
                return appPath;
            }
        }

    }


}
