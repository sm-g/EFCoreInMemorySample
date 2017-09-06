using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual DbSet<DAL.Models.Import> Imports { get; set; }
        public virtual DbSet<DAL.Models.ImportUser> Users { get; set; }
        public virtual DbSet<DAL.Models.ImportProject> Projects { get; set; }
        public virtual DbSet<DAL.Models.ImportCustomer> Customers { get; set; }
        public virtual DbSet<DAL.Models.ImportGroup> Groups { get; set; }
        public virtual DbSet<DAL.Models.ImportZone> Zones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.SetSimpleUnderscoreTableNameConvention(true);
        }
    }
}