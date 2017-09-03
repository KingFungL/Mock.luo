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
        public DbSet<AppModule> AppModule { get; set; }
        public DbSet<RoleModule> RoleMenu { get; set; }
        public DbSet<Article> Article { get; set; }
        public DbSet<Upload> Upload { get; set; }
        public DbSet<Items> Items { get; set; }
        public DbSet<ItemsDetail> ItemsDetail { get; set; }
        public DbSet<PointArtice> PointArtice { get; set; }
        public DbSet<ThirdUser> ThirdUser { get; set; }
        public DbSet<Review> Review { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();//移除复数表名的契约
        }
    }
}
