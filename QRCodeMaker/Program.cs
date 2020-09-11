using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;
using ZXing.QrCode;

namespace QRCodeMaker {
    static class Program {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            if (args.Length > 0) {
                string appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                Configure conf = new Configure(Path.Combine(appPath, "setting.ini"));
                string watchPath = conf.GetSettingData("WATCH");
                string base_url = conf.GetSettingData("URL");
                var qr_path = Path.Combine(Path.GetTempPath(), "qr.png");

                var renewPath = args[0].Replace(watchPath, "").Replace("\\", "/");
                var renewPathSplited = renewPath.Split('/');
                var encordedPath = renewPathSplited.Select(x => System.Web.HttpUtility.UrlEncode(x));
                var path = "";
                foreach (var item in encordedPath) {
                    path += item + '/';
                }

                var url = base_url + path.TrimEnd('/');
                var qr = MakeQRcode(url);

                qr.Save(qr_path, ImageFormat.Png);
                Process.Start(qr_path);

            }
        }

        /// <summary>
        /// StringからQRコードをBitMapで生成します。
        /// </summary>
        /// <param name="content">QRコードに埋め込む文字列</param>
        /// <returns>QRコードのBitMap</returns>
        static Bitmap MakeQRcode(string content) {
            BarcodeWriter writer = new BarcodeWriter();
            BarcodeFormat format = BarcodeFormat.QR_CODE;

            int width = 256;
            int height = 256;

            QrCodeEncodingOptions options = new QrCodeEncodingOptions() {
                CharacterSet = "UTF-8",
                ErrorCorrection = ZXing.QrCode.Internal.ErrorCorrectionLevel.M,
                Height = height,
                Width = width,
                Margin = 2,
            };

            writer.Options = options;
            writer.Format = format;

            return writer.Write(content);
            // writer.Write(contents).Save(Path.Combine(Path.GetTempPath(), "qr.png"), ImageFormat.Png);
        }
    }


    /// <summary>
    /// 設定クラス
    /// </summary>
    public class Configure {

        private string _setFilePath;
        private Dictionary<string, string> Settings = new Dictionary<string, string>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Configure(string setFilePath) {
            _setFilePath = setFilePath;
            var lines = File.ReadAllLines(setFilePath);
            var data = lines.Select(x => x.Split(new char[] { '=' }, 2));
            Settings = data.ToDictionary(x => x[0], x => x[1]);
        }

        /// <summary>
        /// 設定の読込
        /// </summary>
        /// <param name="key">読み込みたい項目のキー</param>
        /// <returns>value</returns>
        public string GetSettingData(string key) {
            if (Settings.ContainsKey(key)) {
                return Settings
                    .Where(x => x.Key == key)
                    .Select(x => x.Value)
                    .FirstOrDefault();
            } else {
                return "";
            }
        }

        /// <summary>
        /// 設定の編集
        /// </summary>
        /// <param name="key">設定したい項目のキー</param>
        /// <param name="value">変更後の値</param>
        public void SetSettingData(string key, string value) {
            if (Settings.ContainsKey(key)) {
                if (value.Contains("\r\n")) {
                    Settings[key] = value.Replace("\r\n", "\\r\\n");
                } else {
                    Settings[key] = value;
                }
            } else {
                if (value.Contains("\r\n")) {
                    Settings.Add(key, value.Replace("\r\n", "\\r\\n"));
                } else {
                    Settings.Add(key, value);
                }
            }
        }

        /// <summary>
        /// 設定の保存
        /// </summary>
        public void SaveSetting() {
            using (var stream = new FileStream(_setFilePath, FileMode.Create, FileAccess.Write)) {
                using (var sw = new StreamWriter(stream, Encoding.UTF8)) {
                    foreach (var item in Settings) {
                        sw.WriteLine("{0}={1}", item.Key, item.Value);
                    }
                }
            }
        }
    }
}
