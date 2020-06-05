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
        public AddDatabaseWindow()
        {
            InitializeComponent();        
        }


        private void Button_Click(object sender, RoutedEventArgs e)
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
