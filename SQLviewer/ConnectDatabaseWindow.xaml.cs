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
    /// <summary>
    /// klasa odpowiadająca za interakcje uzytkownika w oknie polaczenia z dana baza
    /// </summary>
    public partial class ConnectDatabaseWindow : Window
    {
        private int selectedDatabaseID;
        public string ConnectionString;

        /// <summary>
        /// kontruktor klasy ktory inicjalizuje komponenty i wyswietlajacy liste baz
        /// </summary>
        public ConnectDatabaseWindow()
        {
            InitializeComponent();
            DataContext = this;

            using var context = new DatabasesContext();
            var connections = context.Db
                                .Select(q => new { ID = q.DatabaseID, q.Server_address, q.Login })
                                .ToList();

            ConnectionList.ItemsSource = connections;
        }

        /// <summary>
        /// metoda ktora laczy z dana baza po wpisaniu jej danych lub wybraniu z listy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            if (button.Name == "ConnectOnce")
            {
                ConnectionString = $"Server=tcp:{ServerAddress.Text},{Port.Text};Initial Catalog=%%%;User ID={Login.Text};Password={Password.Password};Connection Timeout=15";
            } 
            else
            {
                using var context = new DatabasesContext();
                var connectionParams = context.Db
                                            .Where(q => q.DatabaseID == selectedDatabaseID)
                                            .Select(q => new { q.Server_address, q.Login, q.Password, q.Port })
                                            .FirstOrDefault();

                string decrypt_pass = PasswordHash.Decrypt(connectionParams.Password);
                ConnectionString = $"Server=tcp:{connectionParams.Server_address},{connectionParams.Port};Initial Catalog=%%%;User ID={connectionParams.Login};Password={decrypt_pass};Connection Timeout=15";
            }

            DialogResult = true;
        }

        /// <summary>
        /// metoda ktora przypisuje id wybranego rekordu do pola selectedDatabaseID
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var grid = sender as DataGrid;
            dynamic data = grid.SelectedItem;

            selectedDatabaseID = data.ID;
        }

        /// <summary>
        /// metoda ktora pozwola nawiazac polaczenie poprzez wcisniecie klawisza Enter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Password_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                ConnectionString = $"Server=tcp:{ServerAddress.Text},{Port.Text};Initial Catalog=%%%;User ID={Login.Text};Password={Password.Password};Connection Timeout=15";
                DialogResult = true;
            }

        }
    }
}
