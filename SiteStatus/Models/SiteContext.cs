using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteStatus.Models
{
    public class SiteContext : DbContext
    {
        public DbSet<Site> Sites { get; set; }
    }
}
