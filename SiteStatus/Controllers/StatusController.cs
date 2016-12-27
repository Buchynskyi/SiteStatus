using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using SiteStatus.Modules;


namespace SiteStatus.Controllers
{
    public class StatusController : ApiController
    {
        public bool GetCheckStatus()
        {
                //Повертаємо статус перевірки
            return StatusModule.StartChecker;
        }

        [HttpPut]
        public void EditCheckStatus(bool cs)
        {
                //Задаємо статус перевірки
            StatusModule.StartChecker = cs;
        }
    }
}
