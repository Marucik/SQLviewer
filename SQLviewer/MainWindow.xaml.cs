﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SQLviewer
{
    public partial class MainWindow : Window
    {
        private string connectionString;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var AddDatabasewindow = new AddDatabaseWindow();
            AddDatabasewindow.Show();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            var ConnectDatabaseWindow = new ConnectDatabaseWindow();

            if (ConnectDatabaseWindow.ShowDialog() == true)
            {
                connectionString = ConnectDatabaseWindow.ConnectionString;
            }

            string masterConnection = connectionString.Replace("%%%", "master");

            using var connection = new SqlConnection(masterConnection);
            connection.Open();

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

            e.Handled = true;
        }

        private void DatabasesBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            string db = comboBox.SelectedItem as string;

            string specyficDB = connectionString.Replace("%%%", db);

            using var connection = new SqlConnection(specyficDB);
            connection.Open();

            var command = new SqlCommand("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE NOT TABLE_SCHEMA='sys'", connection);
            var reader = command.ExecuteReader();

            List<string> Tables = new List<string>();

            while (reader.Read())
            {
                string tableName = reader.GetString(0);

                Tables.Add(tableName);
            }

            TablesBox.IsEnabled = true;
            TablesBox.ItemsSource = Tables;

            connection.Close();

            e.Handled = true;
        }

        private void TablesBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            string table = comboBox.SelectedItem as string;
            string db = DatabasesBox.SelectedItem as string;

            string specyficDB = connectionString.Replace("%%%", db);

            using var connection = new SqlConnection(specyficDB);
            //connection.Open();

            //var command = new SqlCommand("SELECT " + 
            //                                "c.name 'Column Name', " +
            //                                "t.Name 'Data type', " +
            //                                "c.max_length 'Max Length', " +
            //                                "c.is_nullable, " +
            //                                "ISNULL(i.is_primary_key, 0) 'Primary Key' " +
            //                            "FROM " +
            //                                "sys.columns c" +
            //                            "INNER JOIN " +
            //                                "sys.types t ON c.user_type_id = t.user_type_id " +
            //                            "LEFT OUTER JOIN " +
            //                                "sys.index_columns ic ON ic.object_id = c.object_id AND ic.column_id = c.column_id " +
            //                            "LEFT OUTER JOIN " +
            //                                "sys.indexes i ON ic.object_id = i.object_id AND ic.index_id = i.index_id " +
            //                            "WHERE " +
            //                                "c.object_id = OBJECT_ID('SalesLT.Address')"
            //                                , connection);
            //var reader = command.ExecuteReader();

            //List<string> Columns = new List<string>();

            //while (reader.Read())
            //{
            //    string tableName = reader.GetString(0);

            //    Columns.Add(tableName);
            //}

            //connection.Close();

            e.Handled = true;
        }
    }
}