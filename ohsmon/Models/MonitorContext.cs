using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ohsmon.Models
{
    public class MonitorContext : DbContext
    {
        public MonitorContext(DbContextOptions<MonitorContext> options)
            : base(options)
        {
        }

        public DbSet<MonitorItem> MonitorItems { get; set; }
    }
}
