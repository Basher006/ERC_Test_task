using ERC_test_task.Sqlite3.DbTables.Interface;

namespace ERC_test_task.Sqlite3.DbTables
{
    internal struct TariffsTable_Fields
    {
        public int _id;
        public string ServiceName;
        public double TariffRate;
        public double StandartNorm;
        public string UnitName;
    }

    internal class TariffsTable : IDbTable
    {
        string IDbTable.TableName => "Tariffs";
        string[] IDbTable.FieldsCreateText => new string[]
        {
            "_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, ",
            "ServiceName TEXT NOT NULL, ",
            "TariffRate REAL NOT NULL, ",
            "StandartNorm REAL, ",
            "UnitName TEXT NOT NULL"
        };
    }
}
