using Microsoft.EntityFrameworkCore;
using SecurityService_Core_Stores.Stores.Configurations;

namespace SecurityService_Core_Stores
{
    public class CustomerContext : DbContext
    {
        public CustomerContext(DbContextOptions options) : base(options)
        {

        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DocscanConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserHashConfiguration());

            // исправление проблем с двойными кавычками
            //foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
            //{
            //    // Замена имён таблиц
            //    entity.SetTableName(entity.GetTableName()?.ToLower());
            //    entity.SetSchema("public");

            //    // Замена имён колонок            
            //    foreach (var property in entity.GetProperties())
            //    {
            //        property.SetColumnName(property.GetColumnName()?.ToLower());
            //    }

            //    foreach (var key in entity.GetKeys())
            //    {
            //        key.SetName(key.GetName()?.ToLower());
            //    }

            //    foreach (var key in entity.GetForeignKeys())
            //    {
            //        key.SetConstraintName(key.GetConstraintName()?.ToLower());
            //    }

            //    foreach (var index in entity.GetIndexes())
            //    {
            //        index.SetDatabaseName(index.GetDatabaseName()?.ToLower());
            //    }
            //}
        }
    }
}
