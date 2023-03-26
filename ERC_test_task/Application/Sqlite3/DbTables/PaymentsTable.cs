using ERC_test_task.Sqlite3.DbTables.Interface;

namespace ERC_test_task.Sqlite3.DbTables
{
    internal struct PaymentsTable_Fields
    {
        public int _id;
        public int DTime_id;
        public int CounterReadings_id;
        public decimal ColdWaterSupply_Payment;
        public decimal HotWaterSupply_Payment;
        public decimal HotWaterSupplyWater_Payment;
        public decimal HotWaterSupplyHeat_Payment;
        public decimal ElectricitySupplyNight_payment;
        public decimal ElectricitySupplyDay_payment;
        public decimal ElectricitySupplySum_payment;
        public decimal Sum_payment;
    }

    internal class PaymentsTable : IDbTable
    {
        string IDbTable.TableName => "PaumentData";
        string[] IDbTable.FieldsCreateText => new string[]
        {
            "_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, ",
            "DTime_id INTEGER NOT NULL, ",
            "CounterReadings_id INTEGER NOT NULL, ",
            "ColdWaterSupply_Payment REAL NOT NULL, ",
            "HotWaterSupply_Payment REAL NOT NULL, ",
            "HotWaterSupplyWater_Payment REAL NOT NULL, ",
            "HotWaterSupplyHeat_Payment REAL NOT NULL, ",
            "ElectricitySupplyNight_payment REAL, ",
            "ElectricitySupplyDay_payment REAL, ",
            "ElectricitySupplySum_payment REAL NOT NULL, ",
            "Sum_payment REAL NOT NULL"
        };
    }
}
