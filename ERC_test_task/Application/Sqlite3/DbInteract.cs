using ERC_test_task.Application.UserInteface;
using ERC_test_task.Sqlite3;
using ERC_test_task.Sqlite3.DbTables;
using ERC_test_task.Sqlite3.DbTables.Interface;
using Microsoft.Data.Sqlite;

namespace ERC_test_task.Application.Sqlite3
{
    internal static class DbInteract
    {
        private static readonly string dbName;

        private static readonly string counterReadingsTableName;
        private static readonly string tariffsTableTableName;
        private static readonly string dateTimeTableTableName;
        private static readonly string paymentsTableTableName;


        static DbInteract() 
        {
            dbName = DbSettings.DbName;

            counterReadingsTableName = DbSettings.counterReadingsTable.TableName;
            tariffsTableTableName = DbSettings.tariffsTable.TableName;
            dateTimeTableTableName = DbSettings.dateTimeTable.TableName;
            paymentsTableTableName = DbSettings.paymentsTable.TableName;

            foreach (var DbTab in DbSettings.DbTabs)
            {
                CreateTableIfNotExist(DbTab);
            }
            if (!TariffsTableIsHaveRows())
            {
                SetDefaultRowsToTariffTable();
            }
        }


        private static void CreateTableIfNotExist(IDbTable dbTab)
        {
            using (var connection = new SqliteConnection($"Data Source={dbName}"))
            {
                string fieldsText = string.Join("", dbTab.FieldsCreateText);
                string commandText = $"CREATE TABLE IF NOT EXISTS {dbTab.TableName}({fieldsText})";

                ExecuteCommand(connection, commandText);
            }
        }

        internal static TariffsTable_Fields GetRow_FromTariffsTable_byServisName(string serviceName)
        {
            TariffsTable_Fields row = new TariffsTable_Fields();
            using (var connection = new SqliteConnection($"Data Source={dbName}"))
            {
                connection.Open();
                var command = connection.CreateCommand();

                command.CommandText = $"SELECT * FROM {tariffsTableTableName} WHERE ServiceName = '{serviceName}'";
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows && reader.Read())
                    {
                        var _id = reader["_id"];
                        var _serviceName = reader["ServiceName"];
                        var tariffRate = reader["TariffRate"];
                        var standartNorm = reader["StandartNorm"];
                        var unitName = reader["UnitName"];

                        row._id = Convert.ToInt32(_id);
                        row.ServiceName = (string)_serviceName;
                        row.TariffRate = Convert.ToDouble(tariffRate);
                        row.StandartNorm = (double)standartNorm;
                        row.UnitName = (string)unitName;
                    }
                }
            }
            return row;
        }

        private static bool TariffsTableIsHaveRows()
        {
            using (var connection = new SqliteConnection($"Data Source={dbName}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM {tariffsTableTableName}";
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }

        private static void SetDefaultRowsToTariffTable()
        {
            var defaultRows = DbSettings.GetDefaultRowsForTariffsTable();

            using (var connection = new SqliteConnection($"Data Source={dbName}"))
            {
                foreach (var row in defaultRows)
                {
                    string serviceName = row.ServiceName;
                    double tariffRate = row.TariffRate;
                    double standartNorm = row.StandartNorm;
                    string unitName = row.UnitName;

                    string tariffRate_s = tariffRate.ToString().Replace(",", ".");
                    string standartNorm_s = standartNorm.ToString().Replace(",", ".");

                    string commandText = $"INSERT INTO {tariffsTableTableName} (ServiceName, TariffRate, StandartNorm, UnitName) VALUES ('{serviceName}', {tariffRate_s}, {standartNorm_s}, '{unitName}')";
                    ExecuteCommand(connection, commandText);
                }
            }
        }

        internal static void SaveData(UserInputs userData, PaymentSum paymentSum)
        {
            SetNewDateTime(userData.DateTime);
            var lastDTimeRow = GetLastDateTimeRow();

            SaveUserData(userData, lastDTimeRow._id);
            CounterReadingsTable_Fields lastUserDataRow;

            if (TryGetLastCountersReadingsRow(out CounterReadingsTable_Fields counterReadingsTable_Fields ))
            {
                lastUserDataRow = counterReadingsTable_Fields;
            }
            else
                lastUserDataRow = new CounterReadingsTable_Fields();

            SavePaymentData(paymentSum, lastDTimeRow._id, lastUserDataRow._id);
        }
        internal static void SetNewDateTime(string dateTime)
        {
            using (var connection = new SqliteConnection($"Data Source={dbName}"))
            {
                string commandText = $"INSERT INTO {dateTimeTableTableName} (DateTime) VALUES ('{dateTime}')";
                ExecuteCommand(connection, commandText);
            }
        }
        
        internal static void SavePaymentData(PaymentSum paymentSum, int dateTime_id, int counterReadings_id)
        {
            using (var connection = new SqliteConnection($"Data Source={dbName}"))
            {
                string commandText = $"INSERT INTO {paymentsTableTableName} " +
                    $"(DTime_id, " +
                    $"CounterReadings_id, " +
                    $"ColdWaterSupply_Payment, " +
                    $"HotWaterSupply_Payment, " +
                    $"HotWaterSupplyWater_Payment, " +
                    $"HotWaterSupplyHeat_Payment, " +
                    $"ElectricitySupplyNight_payment, " +
                    $"ElectricitySupplyDay_payment, " +
                    $"ElectricitySupplySum_payment," +
                    $"Sum_payment" +
                    $") VALUES " +
                    $"({dateTime_id}, " +
                    $"{counterReadings_id}, " +
                    $"{paymentSum.ColdWaterSupply.ToString().Replace(',', '.')}, " +
                    $"{paymentSum.HotWaterSupply.ToString().Replace(',', '.')}, " +
                    $"{paymentSum.HotWaterSupply_Water.ToString().Replace(',', '.')}, " +
                    $"{paymentSum.HotWaterSupply_Heat.ToString().Replace(',', '.')}, " +
                    $"{paymentSum.Electricity_Night.ToString().Replace(',', '.')}, " +
                    $"{paymentSum.Electricity_Day.ToString().Replace(',', '.')}, " +
                    $"{paymentSum.Electricity_Sum.ToString().Replace(',', '.')}, " +
                    $"{paymentSum.GetAllSum().ToString().Replace(',', '.')})";
                ExecuteCommand(connection, commandText);
            }
        }
        
        internal static void SaveUserData(UserInputs userData, int dateTime_id)
        {
            using (var connection = new SqliteConnection($"Data Source={dbName}"))
            {
                string commandText = $"INSERT INTO CounterReadings " +
                    $"(DTime_id, " +
                    $"ResidentCount, " +
                    $"ColdWaterSupply_CounterReadings, " +
                    $"HotWaterSupply_CounterReadings, " +
                    $"ElectricitySupplyDay_CounterReadings, " +
                    $"ElectricitySupplyNight_CounterReadings" +
                    $") VALUES " +
                    $"({dateTime_id}, " +
                    $"{userData.NumberOfLodgers}, " +
                    $"{userData.ColdWaterSupplyData.ToString().Replace(',', '.')}, " +
                    $"{userData.HotWaterSupplyData.ToString().Replace(',', '.')}, " +
                    $"{userData.DayElectricitySupplyData.ToString().Replace(',', '.')}, " +
                    $"{userData.NightElectricitySupplyData.ToString().Replace(',', '.')})";
                ExecuteCommand(connection, commandText);
            }
        }

        internal static bool TryGetLastCountersReadingsRow(out CounterReadingsTable_Fields counterReadingsTable_Fields)
        {
            counterReadingsTable_Fields = new CounterReadingsTable_Fields();
            using (var connection = new SqliteConnection($"Data Source={dbName}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                string commandText = $"SELECT * FROM {counterReadingsTableName} ORDER BY _id DESC LIMIT 1";
                command.CommandText = commandText;

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows && reader.Read())
                    {
                        var _id = reader["_id"];
                        var dTime_id = reader["DTime_id"];
                        var residentCount = reader["ResidentCount"];
                        var coldWaterSupply_CounterReadings = reader["ColdWaterSupply_CounterReadings"];
                        var hotWaterSupply_CounterReadings = reader["HotWaterSupply_CounterReadings"];
                        var electricitySupplyDay_CounterReadings = reader["ElectricitySupplyDay_CounterReadings"];
                        var electricitySupplyNight_CounterReadings = reader["ElectricitySupplyNight_CounterReadings"];

                        counterReadingsTable_Fields._id = Convert.ToInt32(_id);
                        counterReadingsTable_Fields.DTime_id = Convert.ToInt32(dTime_id);
                        counterReadingsTable_Fields.ResidentCount = Convert.ToInt32(residentCount);
                        counterReadingsTable_Fields.ColdWaterSupply_CounterReadings = Convert.ToDouble(coldWaterSupply_CounterReadings);
                        counterReadingsTable_Fields.HotWaterSupply_CounterReadings = Convert.ToDouble(hotWaterSupply_CounterReadings);
                        counterReadingsTable_Fields.ElectricitySupplyDay_CounterReadings = Convert.ToDouble(electricitySupplyDay_CounterReadings);
                        counterReadingsTable_Fields.ElectricitySupplyNight_CounterReadings = Convert.ToDouble(electricitySupplyNight_CounterReadings);

                        return true;
                    }
                }
            }
            return false;
        }

        internal static DateTimeTable_Fields GetLastDateTimeRow()
        {
            DateTimeTable_Fields row = new DateTimeTable_Fields();
            using (var connection = new SqliteConnection($"Data Source={dbName}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                string commandText = $"SELECT * FROM {dateTimeTableTableName} ORDER BY _id DESC LIMIT 1";
                command.CommandText = commandText;

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows && reader.Read())
                    {
                        var _id = reader["_id"];
                        var dateTime = reader["DateTime"];

                        row._id = Convert.ToInt32(_id);
                        row.DateTime = dateTime.ToString();
                    }
                }
            }
            return row;
        }

        private static void ExecuteCommand(SqliteConnection connection, string commandText)
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = commandText;

            command.ExecuteNonQuery();
        }
    }
}
