using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

// AWS_with_Gyazo_console
// これはテスト用のコンソールプログラムです。
// もう覚えてないので起動しません

namespace AWS_with_Gyazo
{
    class AWS_with_Gyazo_console
    {

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        

        internal static System.IO.FileSystemWatcher watcher = null;
        internal static string AWS_URL = "";
        internal static string watch_path = "";
        internal static readonly string AWS_cp = "aws s3 cp %SOURCE% s3://%UPPATH% --grants read=uri=http://acs.amazonaws.com/groups/global/AllUsers";
        internal static readonly string AWS_rm = "aws s3 rm s3://%UPPATH%/%SOURCE%";
        internal static string multiplecheck = "";
        internal static System.Windows.Forms.NotifyIcon notify;

        [STAThread]
        static void Main(string[] args)
        {
            string line;
            ArrayList Ar = new ArrayList();

            using (System.IO.StreamReader Sr = new System.IO.StreamReader(@"setting.ini"))
            {
                while ((line = Sr.ReadLine()) != null)
                {
                    Ar.Add(line);
                }
            }

            for (int i = 0; i < Ar.Count; i++)
            {
                if (HasString(Ar[i].ToString(), "WATCH="))
                {
                    watch_path = Ar[i].ToString().Replace("WATCH=", "");
                }
                if (HasString(Ar[i].ToString(), "URL="))
                {
                    AWS_URL = Ar[i].ToString().Replace("URL=", "");
                }
            }
            watcher = new System.IO.FileSystemWatcher();
            //監視するディレクトリを指定
            watcher.Path = watch_path;
            //最終アクセス日時、最終更新日時、ファイル、フォルダ名の変更を監視する
            watcher.NotifyFilter =
                (System.IO.NotifyFilters.FileName
                | System.IO.NotifyFilters.LastWrite);
            //すべてのファイルを監視
            watcher.Filter = "*.*";
            watcher.IncludeSubdirectories = true;
            // UIのスレッドにマーシャリングする
            // コンソールアプリケーションでの使用では必要ないのでコメントアウト
            // watcher.SynchronizingObject = this;

            //イベントハンドラの追加
            watcher.Changed += new System.IO.FileSystemEventHandler(watcher_Changed);
            watcher.Created += new System.IO.FileSystemEventHandler(watcher_Changed);
            watcher.Deleted += new System.IO.FileSystemEventHandler(watcher_Changed);
            watcher.Renamed += new System.IO.RenamedEventHandler(watcher_Renamed);

            //監視を開始する
            watcher.EnableRaisingEvents = true;

            notify = new System.Windows.Forms.NotifyIcon();
            notify.Icon = new System.Drawing.Icon("cloud.ico");
            notify.Visible = true;
            notify.DoubleClick += new EventHandler(notify_dClick);

            var handle = GetConsoleWindow();


            ShowWindow(handle, SW_HIDE);

            Console.ReadKey();

        }
        // Console.ReadKey();
        // Console.WriteLine("監視を開始しました。");
        internal static void notify_dClick(object sender, EventArgs e)
        {
            watcher.EnableRaisingEvents = false;
            watcher.Dispose();
            notify.Dispose();

            Application.Exit();
        }


        internal static void watcher_Changed(System.Object source, System.IO.FileSystemEventArgs e)
        {
            // e.FullPath
            switch (e.ChangeType)
            {
                case System.IO.WatcherChangeTypes.Changed:
                    if (multiplecheck != e.FullPath)
                    {
                        if (Regex.IsMatch(e.FullPath, @"\.\w+$"))
                        {
                            file_manage('c', e.FullPath);
                        }
                    }
                    break;
                case System.IO.WatcherChangeTypes.Created:
                    if (multiplecheck != e.FullPath)
                    {
                        if (Regex.IsMatch(e.FullPath, @"\.\w+$"))
                        {
                            file_manage('c', e.FullPath);
                        }
                    }
                    break;
                case System.IO.WatcherChangeTypes.Deleted:
                    file_manage('r', e.FullPath);
                    break;
            }
        }

        internal static void watcher_Renamed(System.Object source, System.IO.RenamedEventArgs e)
        {
            file_manage('r', e.FullPath);
            // file_manage('c', e.FullPath);
        }

        internal static void file_manage(char type, string source_path)
        {
            // get full path
            multiplecheck = source_path;
            string aws_cp_command = AWS_cp.Replace("%SOURCE%", "\"" + source_path + "\"");
            string upPath = System.IO.Path.GetDirectoryName(source_path).Replace(watch_path, "");
            aws_cp_command = aws_cp_command.Replace("%UPPATH%", upPath.Replace(@"\", @"/") + "/");
            // MessageBox.Show(aws_cp_command);
            string aws_rm_command = AWS_rm.Replace("%SOURCE%", System.IO.Path.GetFileName(source_path));
            aws_rm_command = aws_rm_command.Replace("%UPPATH%", upPath.Replace(@"\", @"/"));
            // MessageBox.Show(aws_rm_command);
            switch (type)
            {
                case 'c':
                        Process aws_cp = new Process();
                        aws_cp.StartInfo.FileName = "cmd.exe";
                        aws_cp.StartInfo.Arguments = "/c " + aws_cp_command;
                        aws_cp.StartInfo.CreateNoWindow = true;
                        aws_cp.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        aws_cp.Start();
                        System.Media.SystemSounds.Asterisk.Play();
                        notify.BalloonTipText = System.IO.Path.GetFileName(source_path) + "をアップロードしました";
                        notify.ShowBalloonTip(1000);
                        // System.Windows.Forms.Clipboard.SetText(AWS_URL + System.IO.Path.GetFileName(source_path));
                    break;
                case 'r':
                    Process aws_rm = new Process();
                    aws_rm.StartInfo.FileName = "cmd.exe";
                    aws_rm.StartInfo.Arguments = "/c " + aws_rm_command;
                    aws_rm.StartInfo.CreateNoWindow = true;
                    aws_rm.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    aws_rm.Start();
                    System.Media.SystemSounds.Asterisk.Play();
                    notify.BalloonTipText = System.IO.Path.GetFileName(source_path) + "を削除しました";
                    notify.ShowBalloonTip(1000);
                    break;
                default:
                    break;
            }
        }

        internal static bool HasString(string target, string word)
        {
            if (word == null || word == "")
                return false;

            if (target.IndexOf(word) >= 0)
                return true;
            else
                return false;
        }


    }
}
