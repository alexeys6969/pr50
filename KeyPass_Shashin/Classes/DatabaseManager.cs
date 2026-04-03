using Microsoft.EntityFrameworkCore;
using KeyPass_Shashin.Models;

namespace KeyPass_Shashin.Classes
{
    public class DatabaseManager : DbContext
    {
        public DbSet<Storage> Storages { get; set; }
        public DbSet<User> Users { get; set; }
        public DatabaseManager() => 
            Database.EnsureCreated();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseMySql(
                "server=127.0.0.1;port=3307;uid=root;pwd=;database=Storage;",
                new MySqlServerVersion(new Version(8, 0, 11)));
        }
    }
}
