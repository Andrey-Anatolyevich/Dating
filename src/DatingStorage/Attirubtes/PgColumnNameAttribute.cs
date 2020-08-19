using System;

namespace DatingStorage.Attirubtes
{
    internal class PgColumnNameAttribute : Attribute
    {
        public PgColumnNameAttribute(string colName)
        {
            ColumnName = colName;
        }

        public string ColumnName;
    }
}
