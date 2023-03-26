using ERC_test_task.Sqlite3.DbTables;
using ERC_test_task.Sqlite3.DbTables.Interface;

namespace ERC_test_task.Sqlite3
{
    internal static class DbSettings
    {
        // Название файла с базой данных (SQLite3).
        internal static readonly string DbName = "ERC test task.db";

        // Таблицы базы данных.
        internal static readonly IDbTable counterReadingsTable = new CounterReadingsTable();
        internal static readonly IDbTable tariffsTable = new TariffsTable();
        internal static readonly IDbTable dateTimeTable = new DateTimeTable();
        internal static readonly IDbTable paymentsTable = new PaymentsTable();

        internal static readonly List<IDbTable> DbTabs = new()
        {
            counterReadingsTable,
            tariffsTable,
            dateTimeTable,
            paymentsTable
        };


        // Значения по умолчанию для таблицы TariffsTable()
        // Дбавляются автоматически если при запуске приложения таблица TariffsTable() не имеет в себе данных. 
        internal static readonly string CWS_defloatName = "ХВС";
        internal static readonly string HWS_defloatName = "ГВС";
        internal static readonly string ES_defloatName = "ЭЭ";
        internal static readonly string ESDay_defloatName = "ЭЭ день";
        internal static readonly string ESNight_defloatName = "ЭЭ ночь";
        internal static readonly string HWSWater_defloatName = "ГВС Теплоноситель";
        internal static readonly string HWSHeat_defloatName = "ГВС Тепловая энергия";
        internal static TariffsTable_Fields[] GetDefaultRowsForTariffsTable()
        {
            TariffsTable_Fields CWS = new TariffsTable_Fields()
            {
                ServiceName = CWS_defloatName,
                TariffRate = 35.78D,
                StandartNorm = 4.85D,
                UnitName = "м3"
            };
            TariffsTable_Fields HWS = new TariffsTable_Fields()
            {
                ServiceName = HWS_defloatName,
                TariffRate = 158.98D,
                StandartNorm = 4.01D,
                UnitName = "м3"
            };
            TariffsTable_Fields ES = new TariffsTable_Fields()
            {
                ServiceName = ES_defloatName,
                TariffRate = 4.28D,
                StandartNorm = 164.0D,
                UnitName = "кВт.ч"
            };
            TariffsTable_Fields ESDay = new TariffsTable_Fields()
            {
                ServiceName = ESDay_defloatName,
                TariffRate = 4.9D,
                UnitName = "кВт.ч"
            };
            TariffsTable_Fields ESNight = new TariffsTable_Fields()
            {
                ServiceName = ESNight_defloatName,
                TariffRate = 2.31D,
                UnitName = "кВт.ч"
            };
            TariffsTable_Fields HWSWater = new TariffsTable_Fields()
            {
                ServiceName = HWSWater_defloatName,
                TariffRate = 35.78D,
                StandartNorm = 4.01D,
                UnitName = "м3"
            };
            TariffsTable_Fields HWSHeat = new TariffsTable_Fields()
            {
                ServiceName = HWSHeat_defloatName,
                TariffRate = 998.69D,
                StandartNorm = 0.05349D,
                UnitName = "Гкал"
            };

            return new TariffsTable_Fields[]
            {
                CWS,
                HWS,
                ES,
                ESDay,
                ESNight,
                HWSWater,
                HWSHeat
            };
        }
    }
}
