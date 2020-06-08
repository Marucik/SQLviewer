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
    /// Logika interakcji dla klasy EditDatabaseWindow.xaml
    /// </summary>
    public partial class EditDatabaseWindow : Window
    {
        public string connectionString;
        private int selectedDatabaseID;
        public ObservableCollection<dynamic> databaseEntries = new ObservableCollection<dynamic>();
        public EditDatabaseWindow()
        {
            InitializeComponent();
            using (var context = new DatabasesContext())
            {
                var connections = context.Db
                                        .Select(q => new { ID = q.DatabaseID, q.Server_address, q.Login })
                                        .ToList();

                foreach(var item in connections)
                {
                    databaseEntries.Add(item);
                }

            }

            ConnectionList.ItemsSource = databaseEntries;
        }

        private void ConnectionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var grid = sender as DataGrid;
            dynamic data = grid.SelectedItem;

            selectedDatabaseID = data.ID;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var editwindow = new EditWindow(selectedDatabaseID);
            if (editwindow.ShowDialog() == true)
            {
  
            }
            using (var context = new DatabasesContext())
            {
                var connections = context.Db
                                        .Select(q => new { ID = q.DatabaseID, q.Server_address, q.Login })
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
