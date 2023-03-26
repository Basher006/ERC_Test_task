using ERC_test_task.Sqlite3.DbTables.Interface;

namespace ERC_test_task.Sqlite3.DbTables
{
    internal struct CounterReadingsTable_Fields
    {
        public int _id;
        public int DTime_id;
        public int ResidentCount;
        public double ColdWaterSupply_CounterReadings;
        public double HotWaterSupply_CounterReadings;
        public double ElectricitySupplyDay_CounterReadings;
        public double ElectricitySupplyNight_CounterReadings;
    }
    internal class CounterReadingsTable : IDbTable
    {
        string IDbTable.TableName => "CounterReadings";

        string[] IDbTable.FieldsCreateText => new string[]
        {
            "_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, ",
            "DTime_id INTEGER NOT NULL, ",
            "ResidentCount INTEGER NOT NULL, ",
            "ColdWaterSupply_CounterReadings REAL, ",
            "HotWaterSupply_CounterReadings REAL, ",
            "ElectricitySupplyDay_CounterReadings REAL, ",
            "ElectricitySupplyNight_CounterReadings REAL",
        };
    }
}
