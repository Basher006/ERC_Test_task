using ERC_test_task.Sqlite3.DbTables.Interface;

namespace ERC_test_task.Sqlite3.DbTables
{
    internal struct DateTimeTable_Fields
    {
        public int _id;
        public string DateTime;
    }
    internal class DateTimeTable : IDbTable
    {
        string IDbTable.TableName => "Datetime";
        string[] IDbTable.FieldsCreateText => new string[]
        {
           "_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, ",
            "DateTime TEXT NOT NULL"
        };
    }
}
