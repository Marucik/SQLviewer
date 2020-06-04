using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var db = new Database
            {
                server_name = server_name.Text,
                login = login.Text,
                password = password.Text
            };
            using (var context = new Databases())
            {
                context.Db.Add(db); 
                context.SaveChanges();
            }
            this.Close();
        }
    }
}
