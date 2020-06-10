using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Linq;

namespace SQLviewer
{
    /// <summary>
    /// klasa odpowiadająca za interakcje uzytkownika w oknie usuwania baz z listy
    /// </summary>
    public partial class RemoveDatabaseWindow : Window
    {
        public ObservableCollection<dynamic> databaseEntries = new ObservableCollection<dynamic>();

        /// <summary>
        /// kontruktor klasy ktory inicjalizuje komponenty oraz wyswietla dane z pliku z zapisanymi bazami po czym zapisuje je do tablicy
        /// </summary>
        public RemoveDatabaseWindow()
        {
            InitializeComponent();

            using (var context = new DatabasesContext())
            {
                var connections = context.Db
                                        .Select(q => new { ID = q.DatabaseID, Address = q.Server_address, Login = q.Login, Password = q.Password, Port = q.Port })
                                        .ToList();

                foreach (var item in connections)
                {
                    databaseEntries.Add(item);
                }

            }

            ConnectionList.ItemsSource = databaseEntries;
        }

        /// <summary>
        /// metoda ktora usuwa wybrana baze z listy po czym odswieza ta liste
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(ConnectionList.SelectedItem!=null)
            {
                dynamic data = ConnectionList.SelectedItem;
                int id = data.ID;

                using (var context = new DatabasesContext())
                {
                    var remove = context.Db.SingleOrDefault(q => q.DatabaseID == id);
                    context.Db.Remove(remove);
                    context.SaveChanges();
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
