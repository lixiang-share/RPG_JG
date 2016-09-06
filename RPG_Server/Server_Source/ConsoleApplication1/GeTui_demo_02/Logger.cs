using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeTui_demo_02
{
    class Logger
    {
        private const string LogPath = "C:/Users/Administrator/Desktop/GeTuiTools/logInfo.txt";
        private static int MaxLogLength = 1024;
        private static bool isFirstLogToFile = true;
        private static StringBuilder contentBuf = new StringBuilder();

        public static void Log(Object msg)
        {
            string time = DateTime.Now.ToString(System.Globalization.CultureInfo.InvariantCulture);
            string content = "======================= " + time + " =========================\n" + msg.ToString() + "\n";
            WriteLog(content);

        }

        private static void WriteLog(string content)
        {
            if (content != null)
            {
                contentBuf.Append(content);
            }
          //  if (contentBuf.Length > MaxLogLength)
           // {
                LogToFile(contentBuf.ToString());
                contentBuf.Clear();
          //  }
        }

        private static void LogToFile(string content)
        {
            if (isFirstLogToFile)
            {
                File.WriteAllText(LogPath, content);
                isFirstLogToFile = false;
            }
            else
            {
                File.AppendAllText(LogPath, content);
            }
        }
    }
}
