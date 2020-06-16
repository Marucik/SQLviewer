using System;
using System.Collections.Generic;
using System.Text;

namespace SQLviewer
{
    /// <summary>
    /// Klasa ktora przechowuje informacje o tabeli.
    /// </summary>
    class TableInfo
    {
        public string ColumnName;
        public string DataType;
        public int MaxLength;
        public bool PrimaryKey;

        /// <summary>
        /// Konstruktor ktory przypisuje argumenty do pol publicznych klasy.
        /// </summary>
        /// <param name="columnName">Nazwa kolumny.</param>
        /// <param name="dataType">Typ danych</param>
        /// <param name="maxLength">Długość pola</param>
        /// <param name="primaryKey">Informacja o byciu KluczemGłównym.</param>
        public TableInfo(string columnName, string dataType, int maxLength, bool primaryKey)
        {
            ColumnName = columnName;
            DataType = dataType;
            MaxLength = maxLength;
            PrimaryKey = primaryKey;
        }

        /// <summary>
        /// Metoda przeslaniajaca ktora zwraca wartosci z pól publicznych.
        /// </summary>
        /// <returns>Zwraca dane w postaci <see cref="string"/>a.</returns>
        public override string ToString()
        {
            string pk = PrimaryKey ? "; PK]" : "]";

            return $"{ColumnName} [{DataType} ; Len:{MaxLength} {pk}";
        }
    }
}
