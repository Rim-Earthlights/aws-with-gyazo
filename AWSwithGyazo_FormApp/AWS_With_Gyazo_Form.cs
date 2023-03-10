using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AWSwithGyazo_FormApp {
    public partial class AWS_With_Gyazo_Form : Form {
        //[DllImport("kernel32.dll")]
        //static extern IntPtr GetConsoleWindow();

        //[DllImport("user32.dll")]
        //static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        //const int SW_HIDE = 0;
        //const int SW_SHOW = 5;

        // Watcher用 ファイル作成時の暴走抑止

        /// <summary>
        /// 作成時の更新時間管理
        /// </summary>
        DateTime lastWriteTime = DateTime.MinValue;
        /// <summary>
        /// 作成時ファイル名管理
        /// </summary>
        string createFileName = "";

        #region 定数

        /// <summary>
        /// s3サーバへコピー
        /// </summary>
        const char FILE_MOD_COPY = 'c';
        /// <summary>
        /// s3サーバから削除
        /// </summary>
        const char FILE_MOD_DELETE = 'r';
        /// <summary>
        /// s3サーバと同期
        /// </summary>
        const char FILE_MOD_SYNC = 's';

        #endregion

        /// <summary>
        /// Init
        /// </summary>
        public AWS_With_Gyazo_Form() => InitializeComponent();


        /// <summary>
        /// フォームが読み込まれる前に呼び出されるイベントです。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e) {
            if (System.IO.File.Exists(global.settingFile)) {

                Configure conf = new Configure();

                global.watchPath = conf.GetSettingData("WATCH");

                if (int.TryParse(conf.GetSettingData("SYNCTIME"), out int result)) {
                    global.SyncTime = result;
                }
                else {
                    global.SyncTime = -1;
                }

                global.awsId = conf.GetSettingData("AWSID");
                global.awsKey = conf.GetSettingData("AWSKEY");

                Watcher_create();
                Watcher_state(true, false);
                this.WindowState = FormWindowState.Minimized;
                Show_notify(true);
            }
            else {
                File.Create("setting.ini");
                using (StreamWriter sw = new StreamWriter(@"setting.ini")) {
                    sw.Write("WATCH=");
                }

                MessageBox.Show("setting.iniを作成しました。" + Environment.NewLine + "監視するパスを指定してください。"); ;
            }
        }

        /// <summary>
        /// タスクアイコンの表示、非表示を切り替えます。
        /// </summary>
        /// <param name="shown">表示するかどうか</param>
        internal void Show_notify(bool shown) {
            switch (shown) {
                case true:
                    this.ShowInTaskbar = false;
                    notify_AWS.Visible = true;
                    break;
                case false:
                    this.ShowInTaskbar = true;
                    notify_AWS.Visible = false;
                    break;
            }
        }

        /// <summary>
        /// FileSystemWatcher の初期化をします。
        /// </summary>
        internal void Watcher_create() {
            global.watcher = new System.IO.FileSystemWatcher();
            //監視するディレクトリを指定
            global.watcher.Path = global.watchPath;
            //最終更新日時、ファイル名の変更を監視する
            global.watcher.NotifyFilter =
                (System.IO.NotifyFilters.FileName
                 | System.IO.NotifyFilters.LastWrite);
            //すべてのファイルを監視
            global.watcher.Filter = "*.*";
            // サブディレクトリも監視対象
            global.watcher.IncludeSubdirectories = true;
            //UIのスレッドにマーシャリングする
            global.watcher.SynchronizingObject = this;

            //イベントハンドラの追加
            global.watcher.Changed += new System.IO.FileSystemEventHandler(Watcher_Changed);
            global.watcher.Created += new System.IO.FileSystemEventHandler(Watcher_Changed);
            global.watcher.Deleted += new System.IO.FileSystemEventHandler(Watcher_Changed);
            global.watcher.Renamed += new System.IO.RenamedEventHandler(Watcher_Renamed);
        }

        /// <summary>
        /// ファイル監視の再開、中止を指定します。
        /// </summary>
        /// <param name="state">true -> 再開 | false -> 中止</param>
        /// <param name="_isSync">同期作業時trueに設定する</param>
        internal void Watcher_state(bool state, bool _isSync) {
            switch (state) {
                case true:
                    try {
                        global.enable_watch = true;
                        global.watcher.EnableRaisingEvents = true;
                        if (_isSync) {
                            notify_AWS.BalloonTipText = "同期が終了しました。";
                            notify_AWS.ShowBalloonTip(800);

                        }
                        else {
                            notify_AWS.BalloonTipText = "監視を有効にしました。";
                            notify_AWS.ShowBalloonTip(800);

                        }
                    }
                    catch (Exception ex) {
                        MessageBox.Show(ex.Message);
                        Application.Exit();
                    }

                    break;
                case false:
                    try {

                        global.enable_watch = false;
                        global.watcher.EnableRaisingEvents = false;
                        if (_isSync) {
                            notify_AWS.BalloonTipText = "同期中...";
                            notify_AWS.ShowBalloonTip(800);

                        }
                        else {
                            notify_AWS.BalloonTipText = "監視を無効にしました。";
                            notify_AWS.ShowBalloonTip(800);
                        }

                        notify_AWS.ShowBalloonTip(800);
                    }
                    catch (Exception ex) {
                        MessageBox.Show(ex.Message);
                        Application.Exit();
                    }

                    break;
            }
        }

        /// <summary>
        /// 監視対象のファイルが(作成/削除/変更)された場合に呼び出されるイベントです。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        internal void Watcher_Changed(System.Object source, System.IO.FileSystemEventArgs e) {
            if (Regex.IsMatch(e.FullPath, @"\.\w+$")) {
                switch (e.ChangeType) {
                    case System.IO.WatcherChangeTypes.Created:
                        File_manage(FILE_MOD_COPY, e.FullPath);
                        createFileName = e.FullPath;
                        lastWriteTime = File.GetLastWriteTime(e.FullPath);
                        break;
                    case System.IO.WatcherChangeTypes.Changed:
                        if (createFileName == e.FullPath) {
                            if (lastWriteTime != File.GetLastWriteTime(e.FullPath)) {
                                File_manage(FILE_MOD_COPY, e.FullPath);
                            }
                        }
                        else {
                            File_manage(FILE_MOD_COPY, e.FullPath);
                        }
                        break;
                    case System.IO.WatcherChangeTypes.Deleted:
                        File_manage(FILE_MOD_DELETE, e.FullPath);
                        break;
                }
            }
        }

        /// <summary>
        /// 監視対象のファイル名が変更された場合に呼び出されるイベントです。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        internal void Watcher_Renamed(System.Object source, System.IO.RenamedEventArgs e) {
            if (Regex.IsMatch(e.FullPath, @"\.\w+$")) {
                File_manage(FILE_MOD_DELETE, e.OldFullPath);
                File_manage(FILE_MOD_COPY, e.FullPath);
            }
        }

        /// <summary>
        /// サーバーファイルの操作（コピー、削除等）を行います。
        /// </summary>
        /// <param name="type">c = copy | r = erase | s = sync </param>
        /// <param name="source_path">フルパス</param>
        public void File_manage(char type, string source_path) {
            string uploadPath = null;
            if (!string.IsNullOrEmpty(source_path)) {
                uploadPath = System.IO.Path.GetDirectoryName(source_path).Replace(global.watchPath, "");
            }

            // MessageBox.Show(aws_rm_command);
            // MessageBox.Show(aws_cp_command);

            switch (type) {
                case 'c':
                    string aws_cp_command = global.AWS_cp.Replace("%SOURCE%", "\"" + source_path + "\"");
                    aws_cp_command = aws_cp_command.Replace("%UPPATH%", uploadPath.Replace(@"\", @"/") + "/");

                    ExcuteCmd(aws_cp_command, true);

                    notify_AWS.BalloonTipText = System.IO.Path.GetFileName(source_path) + "をアップロードしました";
                    System.Media.SystemSounds.Asterisk.Play();
                    notify_AWS.ShowBalloonTip(800);
                    // System.Windows.Forms.Clipboard.SetText(AWS_URL + System.IO.Path.GetFileName(source_path));
                    break;
                case 'r':
                    string aws_rm_command = global.AWS_rm.Replace("%SOURCE%",
                        "\"" + System.IO.Path.GetFileName(source_path) + "\"");
                    aws_rm_command = aws_rm_command.Replace("%UPPATH%", uploadPath.Replace(@"\", @"/"));

                    ExcuteCmd(aws_rm_command, true);

                    notify_AWS.BalloonTipText = System.IO.Path.GetFileName(source_path) + "を削除しました";
                    System.Media.SystemSounds.Asterisk.Play();
                    notify_AWS.ShowBalloonTip(800);
                    break;
                case 's':
                    Watcher_state(false, true);

                    System.IO.Directory.GetFiles("", "*.*", SearchOption.AllDirectories);

                    Console.ReadKey();

                    Watcher_state(true, true);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// s3サーバのファイル一覧を取得して返す, はず(覚えてないし多分今参照されてない)
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public Dictionary<string, List<string>> lsCommand(string dir) {
            var psInfo = new ProcessStartInfo {
                FileName = "cmd.exe",
                Arguments = "/c " + global.AWS_ls.Replace("%UPPATH%", dir),
                // Arguments = "/c aws s3 ls s3://gyazo-upload",
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                // RedirectStandardError = true,
                // StandardOutputEncoding = Encoding.UTF8
            };
            var p = Process.Start(psInfo);

            var s3data = new Dictionary<string, List<string>>();
            s3data.Add("file", new List<string>());
            s3data.Add("dir", new List<string>());
            string o_data = null;
            while (!String.IsNullOrEmpty(o_data = p.StandardOutput.ReadLine())) {
                ListObject s3 = new ListObject(o_data);
                if (s3.getf_Size > 0) {
                    s3data["file"].Add(s3.getfileName);
                }
                else if (s3.getf_Size == -1) {
                    // add dir list
                    s3data["dir"].Add(s3.getfileName);
                }
            }

            return s3data;
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            try {
                Watcher_state(false, false);
                global.watcher.Dispose();
                Show_notify(false);
                notify_AWS.Dispose();
            }
            catch {

            }
        }

        private void StartWatch_toolstripClick(object sender, EventArgs e) {
            Watcher_state(!global.enable_watch, false);
        }

        private void ExitProgram_toolstripClick(object sender, EventArgs e) {
            Application.Exit();
        }

        private void サーバーとデータを同期するToolStripMenuItem_Click(object sender, EventArgs e) {
            File_manage(FILE_MOD_SYNC, global.watchPath);
        }

        public void ExcuteCmd(string Arguments, bool wait = false) {

            ProcessStartInfo excInfo = new ProcessStartInfo {
                FileName = "cmd.exe",
                Arguments = "/c " + Arguments,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
            };
            Process exCmd = new Process { StartInfo = excInfo, };
            exCmd.Start();

            if (wait) {
                exCmd.WaitForExit();
            }

            return;
        }

    }

    /// <summary>
    /// ファイル情報格納クラス 整理用です
    /// </summary>
    class ListObject {
        public DateTime getdateTime { get; private set; }
        public long getf_Size { get; private set; }
        public string getfileName { get; private set; }


        public ListObject(string line) {
            // split string data (date, time, f_size, fineName);
            List<string> list = new List<string>();
            string[] temp = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in temp) {
                list.Add(item);
            }

            if (list.Count() == 2) {
                this.getdateTime = DateTime.Now;
                this.getf_Size = -1;
                this.getfileName = list[1];
            }
            else if (list.Count() == 4) {
                string date = String.Format("{0} {1}", list[0], list[1]);

                if (DateTime.TryParse(date, out DateTime dt)) {
                    // set data
                    this.getdateTime = dt;
                }
                else {
                    this.getdateTime = DateTime.Now;
                }

                this.getf_Size = long.Parse(list[2]);
                this.getfileName = list[3];
            }
        }
    }

    /// <summary>
    /// 設定クラス
    /// </summary>
    public class Configure {

        private Dictionary<string, string> Settings = new Dictionary<string, string>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Configure() {
            var lines = File.ReadAllLines(global.settingFile);
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
            }
            else {
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
                }
                else {
                    Settings[key] = value;
                }
            }
            else {
                if (value.Contains("\r\n")) {
                    Settings.Add(key, value.Replace("\r\n", "\\r\\n"));
                }
                else {
                    Settings.Add(key, value);
                }
            }
        }

        /// <summary>
        /// 設定の保存
        /// </summary>
        public void SaveSetting() {
            using (var stream = new FileStream(global.settingFile, FileMode.Create, FileAccess.Write)) {
                using (var sw = new StreamWriter(stream, Encoding.UTF8)) {
                    foreach (var item in Settings) {
                        sw.WriteLine("{0}={1}", item.Key, item.Value);
                    }
                }
            }
        }
    }

}
