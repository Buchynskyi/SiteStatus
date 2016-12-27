using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using SiteStatus.Models;
using System.Data.Entity;

namespace SiteStatus.Controllers
{
    public class ValuesController : ApiController
    {
        SiteContext db = new SiteContext();

        public IEnumerable<Site> GetSites()
        {
            return db.Sites;
        }

        public Site GetSite(int id)
        {
            Site site = db.Sites.Find(id);
            return site;
        }

        [HttpPost]
        public void CreateSite([FromBody]Site site)
        {
            db.Sites.Add(site);
            db.SaveChanges();
        }

        [HttpPut]
        public void EditSite(int id, [FromBody]Site site)
        {
            if (id == site.Id)
            {
                db.Entry(site).State = EntityState.Modified;

                db.SaveChanges();
            }
        }

        public void DeleteSite(int id)
        {
            Site site = db.Sites.Find(id);
            if (site != null)
            {
                db.Sites.Remove(site);
                db.SaveChanges();
            }
        }
    }
}
