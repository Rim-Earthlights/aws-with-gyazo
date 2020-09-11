using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace AWSwithGyazo_FormApp {
    class LogAccessor {

        private FileStream fileStream;

        public LogAccessor(string fileName) {
            this.fileStream = new FileStream(fileName, FileMode.Append, FileAccess.Write);
        }

        public void Write(string text) {
            using (var sw = new StreamWriter(this.fileStream)) {
                sw.Write(text);
            }
        }
    }
}
