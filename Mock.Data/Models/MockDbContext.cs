using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Data.Models
{
    public class MockDbContext : DbContext
    {

        public MockDbContext() : base("MockDbConnection")
        {

        }

        public DbSet<AppUser> AppUser { get; set; }

        public DbSet<AppRole> AppRole { get; set; }

        public DbSet<UserRole> UserRole { get; set; }

        public DbSet<AppMenu> AppMenu { get; set; }
        public DbSet<Upload> Upload { get; set; }
        public DbSet<UploadEntry> UploadEntry { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();//移除复数表名的契约
        }
    }
}
