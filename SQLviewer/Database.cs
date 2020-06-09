using Microsoft.EntityFrameworkCore;
using System;

namespace SQLviewer
{
    public class DatabasesContext : DbContext
    {
        public string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private static bool _created = false;

        public DatabasesContext()
        {
            if (!_created)
            {
                _created = true;
                Database.EnsureCreated();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionbuilder)
        {
            optionbuilder.UseSqlite(@"Data Source="+path+"\\Databases.db");
        }

        public DbSet<Database> Db { get; set; }
    }

    public class Database
    {
        public int DatabaseID { get; set; }
        public string Server_address { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Port { get; set; }
    }
}
