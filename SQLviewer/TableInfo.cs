using System;
using System.Collections.Generic;
using System.Text;

namespace SQLviewer
{
    class TableInfo
    {
        public string ColumnName;
        public string DataType;
        public int MaxLength;
        public bool PrimaryKey;

        public TableInfo(string columnName, string dataType, int maxLength, bool primaryKey)
        {
            ColumnName = columnName;
            DataType = dataType;
            MaxLength = maxLength;
            PrimaryKey = primaryKey;
        }

        public override string ToString()
        {
            string pk = PrimaryKey ? "; PK]" : "]";

            return $"{ColumnName} [{DataType} ; Len:{MaxLength} {pk}";
        }
    }
}
