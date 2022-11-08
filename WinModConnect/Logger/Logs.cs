using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace WinModConnect.Logger
{
    public static class Logs
    {
        static StreamWriter sw0 = null;

        public static void OpenFile()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Log" + $"\\{DateTime.Now.ToString("yyyyMMdd")}" + "-" + $"{ DateTime.Now.ToString("HH-mm-ss")}.log";

            sw0 = new StreamWriter(path, true);
        }

        public static void WrtieLogs(string LogType, string title, string details)
        {
            sw0.WriteLine(DateTime.Now.ToString("dd/MM/yyyy") + " " + "\t" + DateTime.Now.ToString("HH:mm:ss") + " " + "\t" + LogType + " " + "\t" + title + " " + "\t" + details);
            sw0.Flush();
        }

    }
}
