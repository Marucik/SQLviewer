using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SQLviewer
{
    /// <summary>
    /// klasa odpowiadająca za interakcje uzytkownika w oknie dodawania bazy
    /// </summary>
    public partial class AddDatabaseWindow : Window
    {
        /// <summary>
        /// kontruktor klasy ktory inicjalizuje komponenty
        /// </summary>
        public AddDatabaseWindow()
        {
            InitializeComponent();        
        }

        /// <summary>
        /// metoda ktora wywoluje metode AddDatabase po kliknieciu buttona
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddDatabase();
            e.Handled = true;
        }

        /// <summary>
        /// metoda ktora wywoluje metode AddDatabase po wcisnieciu klawisza enter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Password_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AddDatabase();
                e.Handled = true;
            }
        }

        /// <summary>
        /// metoda ktora dodaje baze
        /// </summary>
        private void AddDatabase()
        {
            string pass_hash = PasswordHash.Encrypt(Password.Password);

            var db = new Database
            {
                Server_address = Server_address.Text,
                Login = Login.Text,
                Password = pass_hash,
                Port = Port.Text
            };

            using (var context = new DatabasesContext())
            {
                context.Db.Add(db);
                context.SaveChanges();
            }

            Close();
        }
    }
}
