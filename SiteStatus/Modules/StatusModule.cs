using SiteStatus.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;

namespace SiteStatus.Modules
{
    public class StatusModule
    {
            //Статус перевірки
        public static bool StartChecker = true;

        public static void Init()
        {           
            SiteContext db = new SiteContext();
            AutoResetEvent autoEvent = new AutoResetEvent(false);                            
            List<Timer> timerList = new List<Timer>();
                //Беремо дані часу перевірки  
            foreach (Site a in db.Sites)
            {
                Checker check = new Checker(a.Url);
                if(a.Period!=0)
                    //Таймер виклику функції через період часу
                    timerList.Add(new Timer(check.StatusChecker, autoEvent, a.Period * 1000, a.Period * 1000));
                else
                    if(a.TimeHours!=0 || a.TimeMinutes!=0)
                        //Таймер виклику функції в точний час кожного дня
                        timerList.Add(new Timer(check.StatusChecker, 
                                                autoEvent, 
                                                StatusModule.TimeStart((byte)a.TimeHours, (byte)a.TimeMinutes), 
                                                TimeSpan.FromDays(1)));
            }         
                //Зупиняємо потік, а потім продовжуєм коли StartChecker = false 
            autoEvent.WaitOne();
                //Зупиняємо всі таймери
            foreach (Timer timer in timerList)
            {
                timer.Dispose();
            }
        }

        //Функція для визначення періоду черз який увімкнеться таймер 
        public static TimeSpan TimeStart(byte hours, byte minutes)
        {
            TimeSpan startBy = new TimeSpan(hours, minutes, 0) - DateTime.Now.TimeOfDay;
            if (startBy < new TimeSpan(0, 0, 0))
                startBy = + new TimeSpan(24, 0, 0);
            return startBy;
        }
    }

    public class Checker
    {
        public string Url { get; set; }

        public bool Status { get; set; }

        public Checker(string url) 
        {
            this.Url = url;
        }

        public Checker(){}
     
        public void StatusChecker(object stateInfo)
        {
            AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
            try
            {                    
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(Url);
                request.Timeout = 3000;
                request.AllowAutoRedirect = false;
                request.Method = "HEAD";

                using (var response = request.GetResponse())
                {
                    Status = true;
                }
            }
            catch
            {
                Status = false;
            }               
                //Запис у файл поточного обєкта
            Save(this);             
                //Перевіряємо чи потрібно продовжувати перевіряти
            if (StatusModule.StartChecker == false)
            {               
                    //Відновлюємо потік
                autoEvent.Set();
            }
        }

        public static void Save(Checker obj) 
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("Status.txt", true, System.Text.Encoding.Default))
                {
                    sw.WriteLine("{0}   {1}", obj.Url, obj.Status);
                }
            }
            catch { }
        }      
    }
} 

