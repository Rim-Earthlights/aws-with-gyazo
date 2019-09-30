using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// AWSとファイルの同期を図る
/// テストできたら現行のSyncコマンドに実装
/// </summary>


namespace awsSyncCheck {
    class syncCheck {

        static string source;
        static string dest;

        static void Main(string[] args) {
            if (args.Count() == 2) {
                source = args[0];
                dest = args[1];



            }
            else {

            }
        }
    }
}
