using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteStatus.Models
{
    public class Site
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int Period { get; set; } //В секундаx перід запуску 
        public int TimeHours { get; set; } //Година запуску
        public int TimeMinutes { get; set; } //Хвилина запуску

    }
}
