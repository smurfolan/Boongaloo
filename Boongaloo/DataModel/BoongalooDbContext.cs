using Microsoft.Data.Entity;
using Microsoft.Data.Sqlite;

namespace DataModel
{
    public class BoongalooDbContext : DbContext
    {
        // This property defines the table
        public DbSet<MyEntity> MyEntities { get; set; }

        // This method connects the context with the database
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder
            {
                DataSource = @"C:\Projects\Boongaloo\Boongaloo\Boongaloo.API\App_Data\WaaaW.sqlite"
            };

            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);

            optionsBuilder.UseSqlite(connection);
        }
    }
}
