using Microsoft.EntityFrameworkCore;
using System;

namespace SQLviewer
{
    /// <summary>
    /// klasa dziedziczaca z DbContext ktora przypisuje wlasciwosci z klasy Database do tablicy jako rekord i zapisuje w pliku Databases.db
    /// </summary>
    public class DatabasesContext : DbContext
    {
        public string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private static bool _created = false;

        /// <summary>
        /// metoda ktora sprawdza czy dana baz juz istnieje, w przeciwnym razie tworzy ja
        /// </summary>
        public DatabasesContext()
        {
            if (!_created)
            {
                _created = true;
                Database.EnsureCreated();
            }
        }

        /// <summary>
        /// metoda ktora zapisuje plik Databases.db pod dana sciezka
        /// </summary>
        /// <param name="optionbuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionbuilder)
        {
            optionbuilder.UseSqlite(@"Data Source="+path+"\\Databases.db");
        }

        public DbSet<Database> Db { get; set; }
    }

    /// <summary>
    /// klasa posiadajaca wlasciwosci ktore sa pozniej wstawiane do tablicy
    /// </summary>
    public class Database
    {
        public int DatabaseID { get; set; }
        public string Server_address { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Port { get; set; }
    }
}
