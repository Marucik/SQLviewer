using Microsoft.Win32;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SQLviewer
{
    /// <summary>
    /// Klasa odpowiadająca za interakcje uzytkownika w oknie glownym.
    /// </summary>
    public partial class MainWindow : Window 
    {
        private string connectionString;
        private string currentDatabase;

        /// <summary>
        /// Kontruktor klasy ktory inicjalizuje komponenty.
        /// </summary>
        public MainWindow() 
        {
            InitializeComponent();
        }

        /// <summary>
        /// Metoda ktora otwiera okno dodawania bazy.
        /// </summary>
        private void MenuItem_Click(object sender, RoutedEventArgs e)  
        {
            var AddDatabasewindow = new AddDatabaseWindow();
            AddDatabasewindow.Show();
        }

        /// <summary>
        /// Metoda ktora otwiera okno polaczenia z baza za pomoca polaczenia jednorazowego lub wybrania bazy z listy.
        /// </summary>
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)  
        {                                                                
            var ConnectDatabaseWindow = new ConnectDatabaseWindow();     

            if (ConnectDatabaseWindow.ShowDialog() == true)
            {
                connectionString = ConnectDatabaseWindow.ConnectionString;
            }

            if (connectionString == null)
            {
                MessageBox.Show("You didn't selected database");
            }
            else
            {
                string masterConnection = connectionString.Replace("%%%", "master");

                using var connection = new SqlConnection(masterConnection);

                try
                {
                connection.Open();
                }
                catch (SqlException)
                {
                    MessageBox.Show("Cannot connect to database");
                    return;
                }

                var command = new SqlCommand("SELECT name FROM sys.databases WHERE database_id != 1;", connection);
                var reader = command.ExecuteReader();

                List<string> DBnames = new List<string>();

                while (reader.Read())
                {
                    string databaseName = reader.GetString(0);

                    DBnames.Add(databaseName);
                }

                DatabasesBox.IsEnabled = true;
                DatabasesBox.ItemsSource = DBnames;
                DatabasesBox.SelectedIndex = 0;

                connection.Close();
            }

            e.Handled = true;
        }

        /// <summary>
        /// Metoda ktora wyswietla liste baz w liscie rozwijanej po polaczeniu z baza danych.
        /// </summary>
        private void DatabasesBox_SelectionChanged(object sender, SelectionChangedEventArgs e)  
        {                                                                                       
            var comboBox = sender as ComboBox;                                                  
            string db = comboBox.SelectedItem as string;

            currentDatabase = db;

            string specyficDB = connectionString.Replace("%%%", db);

            using var connection = new SqlConnection(specyficDB);
            connection.Open();

            var command = new SqlCommand("SELECT TABLE_SCHEMA, TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE NOT TABLE_SCHEMA='sys'", connection);
            var reader = command.ExecuteReader();

            List<string> Tables = new List<string>();

            while (reader.Read())
            {
                string tableSchema = reader.GetString(0);
                string tableName = reader.GetString(1);

                Tables.Add($"{tableSchema}.{tableName}");
            }

            TablesBox.IsEnabled = true;
            TablesBox.ItemsSource = Tables;

            connection.Close();

            e.Handled = true;
        }

        /// <summary>
        /// Metoda ktora wyswietla liste tabel z wybranej bazy.
        /// </summary>
        private void TablesBox_SelectionChanged(object sender, SelectionChangedEventArgs e) 
        {                                                                                    
            var comboBox = sender as ComboBox;                                              
            string table = comboBox.SelectedItem as string;

            string specyficDB = connectionString.Replace("%%%", currentDatabase);

            using var connection = new SqlConnection(specyficDB);
            connection.Open();

            var command = new SqlCommand("SELECT " +
                                            "c.name 'ColumnName', " +
                                            "t.Name 'DataType', " +
                                            "c.max_length 'MaxLength', " +
                                            "ISNULL(i.is_primary_key, 0) 'PrimaryKey' " +
                                        "FROM " +
                                            "sys.columns c " +
                                        "INNER JOIN " +
                                            "sys.types t ON c.user_type_id = t.user_type_id " +
                                        "LEFT OUTER JOIN " +
                                            "sys.index_columns ic ON ic.object_id = c.object_id AND ic.column_id = c.column_id " +
                                        "LEFT OUTER JOIN " +
                                            "sys.indexes i ON ic.object_id = i.object_id AND ic.index_id = i.index_id " +
                                        "WHERE " +
                                            $"c.object_id = OBJECT_ID('{table}')"
                                            , connection);

            var reader = command.ExecuteReader();

            List<TableInfo> Columns = new List<TableInfo>();

            while (reader.Read())
            {
                string columnName = reader.GetString(0);
                string dataType = reader.GetString(1);
                int maxLength = reader.GetInt16(2);
                bool primaryKey = reader.GetBoolean(3);

                TableInfo tableInfo = new TableInfo(columnName, dataType, maxLength, primaryKey);

                Columns.Add(tableInfo);
            }

            ColumnsList.ItemsSource = Columns;

            connection.Close();

            if (Query.Text == "")
            {
                Query.Text = $"SELECT * FROM {table}";
            }

            e.Handled = true;
        }

        /// <summary>
        /// Metoda ktora wysyla zapytanie SQL z textboxa do DataGrida ktory wyswietla jego wynik.
        /// </summary>
        private void CommitQuery_Click(object sender, RoutedEventArgs e) 
        {                                                                  
            if (currentDatabase == null)
            {
                return;
            }

            string specyficDB = connectionString.Replace("%%%", currentDatabase);

            using var connection = new SqlConnection(specyficDB);
            connection.Open();

            string query = Query.Text.Replace("\r\n", " ");

            var command = new SqlCommand(query, connection);

            DataTable dataTable = new DataTable();

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
            try
            {
                sqlDataAdapter.Fill(dataTable);
            }
            catch
            {
                Results.DataContext = null;
                
            }

            Results.DataContext = dataTable.DefaultView;

            connection.Close();
            sqlDataAdapter.Dispose();
        }

        /// <summary>
        /// Metoda ktora otwiera okno edytowania bazy z listy.
        /// </summary>
        private void EditDatabase_Click(object sender, RoutedEventArgs e)  
        {
            var EditDatabaseWindow = new EditDatabaseWindow();
            EditDatabaseWindow.Show();
        }

        /// <summary>
        /// Metoda ktora otwiera okno usuwania bazy z listy. 
        /// </summary>
        private void RemoveDatabase_Click(object sender, RoutedEventArgs e) 
        {
            var RemoveDatabaseWindow = new RemoveDatabaseWindow();
            RemoveDatabaseWindow.Show();
        }

        /// <summary>
        /// Metoda ktora zapisuje tabele z bazy do pliku CSV.
        /// </summary>
        private void SaveToCSV_Click(object sender, RoutedEventArgs e)  
        {
            Results.SelectAllCells();
            Results.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            ApplicationCommands.Copy.Execute(null, Results);
            Results.UnselectAllCells();
            string scrapedResults = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "comma-separated values (*.csv)| *.csv"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, scrapedResults);
            }
        }
    }
}
