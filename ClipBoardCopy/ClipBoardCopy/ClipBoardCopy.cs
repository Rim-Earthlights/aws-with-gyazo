using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

/// <summary>
/// S3の表示URLをクリップボードにコピーする
/// 右クリックメニューに追加するのは自分でやる ex) ClipBoardCopy.exe "%1"
/// </summary>

namespace ClipBoardCopy
{
    class Program
    { 
        // クリップボード操作のために必要
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetOpenClipboardWindow();
        // エラー用 プロセス取得
        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [STAThread]
        static void Main(string[] args)
        {          
            if (args.Count() > 0){
                switch (args[0]) {
                    case "-f":

                        break;
                    case "-d":
                        break;
                    default:
                        break;
                }
            }


            // 何故か作業ディレクトリが別になるのでディレクトリを自ファイルの場所へ
            System.IO.Directory.SetCurrentDirectory(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
            // notify icon
            NotifyIcon notify = new NotifyIcon();
            notify.Icon = new System.Drawing.Icon(@"set_icon.ico");
            notify.Visible = true;

            // Missing Args
            if (args.Count() < 1 || args.Count() > 1)
            {
                // Console.WriteLine("ERROR: Arguments is null.");
                System.Media.SystemSounds.Asterisk.Play();
                notify.BalloonTipText = "引数に誤りがあります。";
                notify.ShowBalloonTip(1000);
                notify.Visible = false;
                notify.Dispose();
            }
            else if (args.Count() == 1)
            {
                // char mode = Convert.ToChar(args[0].ToString().Substring(1,1));
                string fullPath = args[0].ToString();
                string fileName = System.IO.Path.GetFileName(fullPath);
                string aws_url = "";
                string watch_path = "";
                // MessageBox.Show(fileName);
                // read setting file
                ArrayList Ar = new ArrayList();
                string line;
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
                    else if (HasString(Ar[i].ToString(), "URL="))
                    {
                        aws_url = Ar[i].ToString().Replace("URL=", "");
                    }
                }

                string path = fullPath.Replace(watch_path,"");
                path = path.Replace("\\", "/");
                var pathlist = path.Split('/');
                var pathEncoded = pathlist.Select(x => System.Web.HttpUtility.UrlEncode(x));
                path = "";
                foreach (var item in pathEncoded) {
                    path += item + "/";
                }
                bool flag = false;
                // MessageBox.Show(aws_url + path);
                try
                {
                    var cp = System.IO.Path.Combine(aws_url + path).TrimEnd('/');
                    Clipboard.SetText(cp, TextDataFormat.Text);
                    flag = true;
                }
                // ExternalExceptionが出てる理由がわからない。
                // (ClipBoardを使っているプロセスも出てこない為)
                // 一応SetTextする前にClear置いたら例外が出ないっぽいので(SetTextのクリア処理がちゃんとできてない？)
                // これで様子見。例外処理は残しておく。
                catch (Exception ex)
                {
                    IntPtr hWnd = GetOpenClipboardWindow();
                    if (IntPtr.Zero != hWnd) // 通らない(クリップボード使用中のプロセス無し)
                    {
                        uint pid = 0;
                        uint tid = GetWindowThreadProcessId(hWnd, out pid);
                        MessageBox.Show("クリップボードを開けませんでした。以下のプログラムが使用中です："
                            + Environment.NewLine + Process.GetProcessById((int)pid).Modules[0].FileName);
                    }
                    else
                    {
                        MessageBox.Show("Error Message: \n" + ex.Message); // Message: 要求されたクリップボード操作に成功しませんでした
                    }
                        flag = false;
                }
                if (flag)
                {
                    System.Media.SystemSounds.Asterisk.Play();
                    notify.BalloonTipText = fileName + "の共有URLをクリップボードにコピーしました。";
                    notify.ShowBalloonTip(750);
                    System.Threading.Thread.Sleep(2500);
                    notify.Visible = false;
                    notify.Dispose();
                    // MessageBox.Show("コピーされました。");
                }
            }
            else
            {
                    
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
