using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace task
{
    public class Logger
    {
        private string logFile = HttpContext.Current.Server.MapPath("~/logs.txt");

        public void addLog(string log) {
            using (StreamWriter sw = File.AppendText(logFile))  //creating log file
            {
                sw.WriteLine($"{DateTime.Now.ToString()} - {log}");
            }
        }

        public List<string> getLogs() {
            return File.ReadAllLines(logFile).ToList();  //fetching logs
        }
    }
}