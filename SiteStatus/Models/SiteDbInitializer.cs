using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteStatus.Models
{
    public class SiteDbInitializer : DropCreateDatabaseAlways<SiteContext>
    {
        protected override void Seed(SiteContext db)
        {
            db.Sites.Add(new Site { Url = "http://fs.to", Period = 25, TimeHours = 0, TimeMinutes = 0});
            db.Sites.Add(new Site { Url = "http://microsoft.com", Period = 120, TimeHours = 0, TimeMinutes = 0});
            db.Sites.Add(new Site { Url = "http://apple.com", Period = 0, TimeHours = 22, TimeMinutes = 15});

            base.Seed(db);
        }
    }
}
