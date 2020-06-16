using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Klasa odpowiadająca za interakcje uzytkownika w oknie edytowania baz z listy.
    /// </summary>
    public partial class EditWindow : Window
    {
        public int id_db;

        /// <summary>
        /// Kontruktor klasy ktory inicjalizuje komponenty oraz przypisuje argumenty do textBoxow.
        /// </summary>
        /// <param name="id">Id połączenia.</param>
        /// <param name="svr_address">Adres serwera.</param>
        /// <param name="lgn">Login połączenia.</param>
        /// <param name="pswd">Zaszyfrowane hasło połączenia.</param>
        /// <param name="prt">Port serwera.</param>
        public EditWindow(int id, string svr_address, string lgn, string pswd, string prt)
        {
            InitializeComponent();

            id_db = id;
            server_address.Text = svr_address;
            login.Text = lgn;
            password.Text = PasswordHash.Decrypt(pswd);
            port.Text = prt;
        }

        /// <summary>
        /// Metoda ktora edytuje dany rekord i zamyka okno edytowania.
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using var context = new DatabasesContext();
            var update = context.Db.SingleOrDefault(q => q.DatabaseID == id_db);
            string encrypt_pass = PasswordHash.Encrypt(password.Text);

            update.Server_address = server_address.Text;
            update.Login = login.Text;
            update.Password = encrypt_pass;
            update.Port = port.Text;

            context.SaveChanges();
            this.Close();
        }
    }
}
