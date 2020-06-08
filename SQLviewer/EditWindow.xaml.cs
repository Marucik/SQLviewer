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
    /// Logika interakcji dla klasy EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        public int id_db;
        public EditWindow(int id)
        {
            InitializeComponent();
            id_db = id;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new DatabasesContext())
            {
                var update = context.Db.SingleOrDefault(q => q.DatabaseID == id_db);
                update.Server_address = server_address.Text;
                update.Login = login.Text;
                string encrypt_pass = PasswordHash.Encrypt(password.Text);
                update.Password = encrypt_pass;
                update.Port = port.Text;
                context.SaveChanges();
                this.Close();
            }
        }
    }
}
