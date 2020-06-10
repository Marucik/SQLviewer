using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// klasa odpowiadająca za interakcje uzytkownika w oknie wyboru bazy do edycji
    /// </summary>
    public partial class EditDatabaseWindow : Window
    {
        public ObservableCollection<dynamic> databaseEntries = new ObservableCollection<dynamic>();

        /// <summary>
        /// kontruktor klasy ktory inicjalizuje komponenty oraz wyswietla dane z pliku z zapisanymi bazami po czym zapisuje je do tablicy
        /// </summary>
        public EditDatabaseWindow()
        {
            InitializeComponent();
            using (var context = new DatabasesContext())
            {
                var connections = context.Db
                                        .Select(q => new { ID = q.DatabaseID, Address = q.Server_address, Login = q.Login, Password = q.Password, Port = q.Port })
                                        .ToList();

                foreach(var item in connections)
                {
                    databaseEntries.Add(item);
                }

            }

            ConnectionList.ItemsSource = databaseEntries;
        }

        /// <summary>
        /// metoda ktora wyswietla okno do edycji danego rekordu po czym odswieza swoja zawartosc
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(ConnectionList.SelectedItem!=null)
            {
                dynamic data = ConnectionList.SelectedItem;
                var editwindow = new EditWindow(data.ID, data.Address, data.Login, data.Password, data.Port);

                if (editwindow.ShowDialog() == true)
                {

                }

                using (var context = new DatabasesContext())
                {
                    var connections = context.Db
                                            .Select(q => new { ID = q.DatabaseID, Address = q.Server_address, Login = q.Login, Password = q.Password, Port = q.Port })
                                            .ToList();

                    databaseEntries.Clear();

                    foreach (var item in connections)
                    {
                        databaseEntries.Add(item);
                    }
                }
            }            
        }
    }
}
