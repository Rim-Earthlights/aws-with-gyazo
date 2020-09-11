using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace awsconfigure {
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
