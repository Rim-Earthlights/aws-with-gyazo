using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// SQLテスト用
/// 使うかわからん
/// </summary>


namespace ConnectTest {
    class sqlConnectTest {
        static void Main(string[] args) {
            string conStr = String.Format("server=172.37.39.100;user id=Mia;password=August0;database=AWS_ACCESS;SslMode=none;");
            string userName = "kusozako";
            const string SQL_FindPass = "SELECT ACC_ID, ACC_SECKEY FROM AUTH WHERE LOGIN_NAME = '%USER_NAME%'";
            var kusozako = SQL_FindPass.Replace("%USER_NAME%",userName);
            MySqlConnection con = new MySqlConnection(conStr);
            con.Open();
            MySqlCommand cmd = new MySqlCommand(kusozako, con);
            MySqlDataReader reader = cmd.ExecuteReader();

            var LoginData = new Dictionary<string,List<string>>();

            //Output Column Name
            string[] names = new string[reader.FieldCount];
            for (int i = 0; i < reader.FieldCount; i++) {
                names[i] = reader.GetName(i);
            }

            //テーブル出力
            while (reader.Read()) {
                string[] row = new string[reader.FieldCount];
                for (int i = 0; i < reader.FieldCount; i++) {
                    row[i] = reader.GetString(i);
                }
                for (int j = 0; j < row.Count(); j++) {
                    if (LoginData.ContainsKey(names[j])) {
                        LoginData[names[j]].Add(row[j]);
                    } else {
                        LoginData.Add(names[j], new List<string>() { row[j] });
                    }
                }
            }
            foreach (var Key in LoginData.Keys) {
                foreach (var value in LoginData[Key]) {
                        Console.WriteLine("Key:{0}, Value:{1}", Key, value);
                }
            }


            Console.ReadKey();
        }
    }
}
