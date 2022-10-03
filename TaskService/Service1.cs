using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Net.Http;
using TaskService.Model;
using System.Net;
using Newtonsoft.Json;

namespace TaskService
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer();

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);  //creating cron for every 1 min
            timer.Interval = 60000; //number in milisecinds  
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    //calling api
                    Request request = new Request();
                    request.filePath = @"C:\Users\Shazad Shiraz\Desktop\Test123.txt";
                    webClient.BaseAddress = "http://localhost:60149";
                    string url = "/api/File";
                    webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                    string data = JsonConvert.SerializeObject(request);
                    var response = webClient.UploadString(url, data);
                    var result = JsonConvert.DeserializeObject<object>(response);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
