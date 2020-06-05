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
    public partial class AddDatabaseWindow : Window
    {
        private string hash;

        public AddDatabaseWindow()
        {
            InitializeComponent();        
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string pass_hash = Password_hash.Encrypt(password.Password);
            var db = new Database
            {
                Server_address = server_name.Text,
                Login = login.Text,
                Password = pass_hash
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
