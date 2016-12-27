using Microsoft.Owin.Hosting;
using SiteStatus.Models;
using SiteStatus.Modules;
using System;
using System.Data.Entity;
using System.IO;
using System.Threading;
using Topshelf;

namespace SiteStatus
{   

    public class Program
    {
        public static void Main()
        {            
            HostFactory.Run(x =>
            {
                x.Service<OwinService>(s =>
                {
                    s.ConstructUsing(() => new OwinService());
                    s.WhenStarted(service => service.Start());
                    s.WhenStopped(service => service.Stop());
                });
                x.RunAsLocalSystem();
                x.SetServiceName("SiteStatus");               
            });                    
        }

        public class OwinService
        {

            private IDisposable _webApp;

            public void Start()
            {   
                    //Запускаємо хост 
                _webApp = WebApp.Start<Startup>("http://localhost:9000/");
                    // Ініціалізатор БД
                AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
                Database.SetInitializer(new SiteDbInitializer());               
                    //Запускаэмо опитування сайтів в новому потоці
                Thread loggerThread = new Thread(new ThreadStart(StatusModule.Init));
                loggerThread.Start();                            
            }

            public void Stop()
            {
                StatusModule.StartChecker = false;                
                _webApp.Dispose();
                Thread.Sleep(1000);
            }          
        }         
    }
}
