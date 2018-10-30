using Mock.Data.Migrations.SeedData;

namespace Mock.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public sealed class Configuration : DbMigrationsConfiguration<Mock.Data.Models.MockDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Mock.Data.Models.MockDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //You can use the DbSet<T>.AddOrUpdate() helper extension method
            //to avoid creating duplicate seed data.E.g.

            new CreateRoleAndUser(context).Excute();

            new CreateModule(context).Excute();


            context.SaveChanges();

        }
    }
}
