using System;
using System.Collections.Generic;
using System.Text;

namespace SQLviewer
{
    /// <summary>
    /// klasa ktora przechowuje informacje o tabeli
    /// </summary>
    class TableInfo
    {
        public string ColumnName;
        public string DataType;
        public int MaxLength;
        public bool PrimaryKey;

        /// <summary>
        /// konstruktor ktory przypisuje argumenty do pol publicznych klasy
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="dataType"></param>
        /// <param name="maxLength"></param>
        /// <param name="primaryKey"></param>
        public TableInfo(string columnName, string dataType, int maxLength, bool primaryKey)
        {
            ColumnName = columnName;
            DataType = dataType;
            MaxLength = maxLength;
            PrimaryKey = primaryKey;
        }

        /// <summary>
        /// metoda przeslaniajaca ktora zwraca wartosci z pol publicznych
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string pk = PrimaryKey ? "; PK]" : "]";

            return $"{ColumnName} [{DataType} ; Len:{MaxLength} {pk}";
        }
    }
}
