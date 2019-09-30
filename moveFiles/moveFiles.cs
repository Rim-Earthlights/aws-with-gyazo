using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

/// <summary>
/// ファイル移動用
/// 自環境専用のやつなので使うときは編集必須
/// </summary>

namespace moveFiles {
    class moveFiles {

        #region 定数

        /// <summary>
        /// s3 mv 日時ディレクトリ追加
        /// </summary>
        const string aws_cp_add_date = "aws s3 mv s3://gyazo-upload/{fileName} s3://gyazo-upload/{year}/{month}/{fileName} --grants read=uri=http://acs.amazonaws.com/groups/global/AllUsers";
        /// <summary>
        /// s3 mv 日時ディレクトリ削除
        /// </summary>
        const string aws_cp_remove_date = "aws s3 mv s3://gyazo-upload/{year}/{month}/{fileName} s3://gyazo-upload/{fileName} --grants read=uri=http://acs.amazonaws.com/groups/global/AllUsers";

        #endregion

        #region クラス変数

        /// <summary>
        /// 呼出元 ローカルに保存してあるやつ
        /// </summary>
        static string sourceDir;
        static string mv_mode;
        #endregion

        static void Main(string[] args) {

            if (args.Count() == 2) {
                sourceDir = args[0];
                mv_mode = args[1];
            }
            $ //TODO mv_modeに合わせて実装変えて;

            // const string sourceDir = @"D:\Share\gyazo-upload\";

            List<string> fileList =
                System.IO.Directory.GetFiles(sourceDir, "*.png", SearchOption.AllDirectories).ToList();
            foreach (var file in fileList) {
                string removeSrcDir = file.Remove(0, sourceDir.Length);
                int year = Convert.ToInt16(removeSrcDir.Substring(0, 4));
                int month = Convert.ToInt16(removeSrcDir.Substring(5, 2));
                string fileName = removeSrcDir.Remove(0, 8);

                Console.Write("{0}年{1:D2}月 | {2} ...", year, month, fileName);

                string mvDir = Path.Combine(sourceDir, fileName);

                string cmd = aws_cp_remove_date.Replace("{fileName}", fileName);
                cmd = cmd.Replace("{year}", String.Format("{0:D4}", year));
                cmd = cmd.Replace("{month}", String.Format("{0:D2}", month));

                // Console.WriteLine("source: {0}\nmv: {1}\ncmd: {2}", file, mvDir, cmd);
                if (File.Exists(mvDir)) {
                    File.Delete(mvDir);
                }
                System.IO.File.Move(file, mvDir);
                ExcuteCmd(cmd, false);
                Console.WriteLine("OK");
            }

            Console.ReadKey();
        }

        public static void ExcuteCmd(string Arguments, bool wait = false) {

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
        }
    }
}