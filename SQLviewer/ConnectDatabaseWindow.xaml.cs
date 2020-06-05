using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SQLviewer
{
    public partial class ConnectDatabaseWindow : Window
    {
        private int selectedDatabaseID;
        public string ConnectionString;

        public ConnectDatabaseWindow()
        {
            InitializeComponent();
            DataContext = this;

            using (var context = new DatabasesContext())
            {
                var connections = context.Db
                                        .Select(q => new { ID = q.DatabaseID, q.Server_address, q.Login })
                                        .ToList();

                ConnectionList.ItemsSource = connections;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            if (button.Name == "ConnectOnce")
            {
                ConnectionString = $"Server=tcp:{ServerAddress.Text},{Port.Text};Initial Catalog=%%%;User ID={Login.Text};Password={Password.Password};Connection Timeout=60";
            } 
            else
            {
                using (var context = new DatabasesContext())
                {
                    var connectionParams = context.Db
                                            .Where(q => q.DatabaseID == selectedDatabaseID)
                                            .Select(q => new { q.Server_address, q.Login, q.Password, q.Port })
                                            .FirstOrDefault();
                    string decrypt_pass = PasswordHash.Decrypt(connectionParams.Password);
                    ConnectionString = $"Server=tcp:{connectionParams.Server_address},{connectionParams.Port};Initial Catalog=%%%;User ID={connectionParams.Login};Password={decrypt_pass};Connection Timeout=60";
                }
            }

            DialogResult = true;
        }

        private void ConnectionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var grid = sender as DataGrid;
            dynamic data = grid.SelectedItem;

            selectedDatabaseID = data.ID;
        }
    }
}
