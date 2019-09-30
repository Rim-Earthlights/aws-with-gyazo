using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// AWSとファイルの同期を図る
/// テストできたら現行のSyncコマンドに実装
/// </summary>


namespace awsSyncCheck {
    class syncCheck {

        internal static readonly string AWS_ls = "aws s3 ls s3://%UPPATH% --recursive";

        static string source;
        static string dest;

        static void Main(string[] args) {
            if (args.Count() == 2) {
                source = args[0];
                dest = args[1];

                

            }
            else {
                // 仮置
                source = @"D:\Share";
                dest = "rim.public-upload";
                // split string data (date, time, f_size, fineName);

                // ローカル情報
                var files = Directory.EnumerateFiles(Path.Combine(source, dest), "*.*", SearchOption.AllDirectories).Select(x => new FileInfo(x));
                var local = new List<ListObject>();
                foreach (var file in files) {
                    DateTime date = file.CreationTime;
                    long length = file.Length;
                    string fileName = file.FullName.Replace(Path.Combine(source, dest) + "\\", "").Replace("\\", "/");
                    ListObject lo = new ListObject(date, length, fileName);
                    local.Add(lo);
                }

                // S3側
                var s3data = lsCommand(dest);

                 //TODO;

                Console.ReadKey();
            }
        }

        static void ExcuteCmd(string Arguments, bool wait = false) {

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

        static List<ListObject> lsCommand(string dir) {
            var psInfo = new ProcessStartInfo {
                FileName = "cmd.exe",
                Arguments = "/c " + AWS_ls.Replace("%UPPATH%", dir),
                // Arguments = "/c aws s3 ls s3://gyazo-upload",
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                // RedirectStandardError = true,
                // StandardOutputEncoding = Encoding.UTF8
            };
            var p = Process.Start(psInfo);

            var s3data = new List<ListObject>();
            string o_data = null;
            while (!String.IsNullOrEmpty(o_data = p.StandardOutput.ReadLine())) {
                ListObject s3 = new ListObject(o_data);
                if (s3.getf_Size > -1) {
                    s3data.Add(s3);
                }
            }

            return s3data;
        }

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
                else if (list.Count() > 3) {
                    string date = String.Format("{0} {1}", list[0], list[1]);

                    if (DateTime.TryParse(date, out DateTime dt)) {
                        // set data
                        this.getdateTime = dt;
                    }
                    else {
                        this.getdateTime = DateTime.Now;
                    }

                    this.getf_Size = long.Parse(list[2]);
                    this.getfileName = "";
                    for (int i = 3; i < list.Count(); i++) {
                        this.getfileName += list[i];
                    }
                }
            }
            public ListObject(DateTime date, long fileSize, string fileName) {
                this.getdateTime = date;
                this.getf_Size = fileSize;
                this.getfileName = fileName;
            }
        }

    }
}
