using ERC_test_task.Application.Sqlite3;
using ERC_test_task.Application.UserInteface;
using ERC_test_task.Sqlite3;
using ERC_test_task.Sqlite3.DbTables;

namespace ERC_test_task.Application
{
    internal static class Core
    {

        private static readonly double defaultCounterData = 0;

        internal static PaymentSum CalcPaymentSum(UserInputs userData) 
        {
            var paymentSum = new PaymentSum();

            CalcPaymentSum_For_CWS(userData, ref paymentSum);
            CalcPaymentSum_For_HWS(userData, ref paymentSum);
            CalcPaymentSum_For_ES(userData, ref paymentSum);

            return paymentSum;
        }

        internal static bool TryGetLastCountersReadingsRow(out CounterReadingsTable_Fields counterReadingsTable_Fields)
        {
            return DbInteract.TryGetLastCountersReadingsRow(out counterReadingsTable_Fields);
        }

        internal static void SaveData(UserInputs userData, PaymentSum paymentSum) => DbInteract.SaveData(userData, paymentSum);

        #region===DateTime===
        internal static bool TryGetLastDtimeFromDB(out string dateTime)
        {
            dateTime = DbInteract.GetLastDateTimeRow().DateTime;

            if (string.IsNullOrEmpty(dateTime))
                return false;
            else
                return true;
        }

        internal static int SaveNewDateTime_andGetId(string dateTime)
        {
            DbInteract.SetNewDateTime(dateTime);
            var lastDatetimeRow = DbInteract.GetLastDateTimeRow();
            return lastDatetimeRow._id;
        }

        internal static string DateTimeIncriment(string dateTime)
        {
            string[] splitedDtime = dateTime.Split(".");

            string year_s = splitedDtime[1];
            int year_i = int.Parse(year_s);

            string mounth_s = splitedDtime[0];
            int mounth_i = int.Parse(mounth_s);

            if (mounth_i < 12)
                mounth_i += 1;
            else
            {
                mounth_i = 1;
                year_i += 1;
            }
            return mounth_i.ToString() + "." + year_i.ToString();
        }
        #endregion

        #region===CWS===
        private static void CalcPaymentSum_For_CWS(UserInputs userData, ref PaymentSum paymentSum)
        {
            TariffsTable_Fields CWS_Tariff = DbInteract.GetRow_FromTariffsTable_byServisName(DbSettings.CWS_defloatName);

            if (userData.UserIsHaveCWSCounter)
                CalcPaymentSum_For_CWSCounter(userData.CWSCounterData, ref paymentSum, CWS_Tariff);
            else
                CalcPaymentSum_For_CWSNorm(userData.NumberOfLodgers, ref paymentSum, CWS_Tariff);
        }

        private static void CalcPaymentSum_For_CWSCounter(double CWSCounterData, ref PaymentSum paymentSum, TariffsTable_Fields CWS_Tariff)
        {
            double pastCWSCounterData = GetPastCWSCounterData();
            ValidateNewCounterData(CWSCounterData, pastCWSCounterData, "ХВС");

            double amount = GetAmountForCounterData(CWSCounterData, pastCWSCounterData);
            double tariff = CWS_Tariff.TariffRate;

            paymentSum.ColdWaterSupply = CalcPaymentSum(amount, tariff);
        }

        private static double GetPastCWSCounterData()
        {
            double pastCWSCounterData;

            if (TryGetLastCountersReadingsRow(out CounterReadingsTable_Fields counterReadingsTable_Fields))
            {
                pastCWSCounterData = counterReadingsTable_Fields.ColdWaterSupply_CounterReadings;
            }
            else pastCWSCounterData = defaultCounterData;

            return pastCWSCounterData;
        }

        private static void CalcPaymentSum_For_CWSNorm(int numberOfLodgerss, ref PaymentSum paymentSum, TariffsTable_Fields CWS_Tariff)
        {
            double amount = GetAmountForNormed(CWS_Tariff.StandartNorm, numberOfLodgerss);
            double tariff = CWS_Tariff.TariffRate;

            paymentSum.ColdWaterSupply = CalcPaymentSum(amount, tariff);
        }
        #endregion

        #region===HWS===
        private static void CalcPaymentSum_For_HWS(UserInputs userData, ref PaymentSum paymentSum)
        {
            TariffsTable_Fields HWS_Tariff = DbInteract.GetRow_FromTariffsTable_byServisName(DbSettings.HWS_defloatName);
            TariffsTable_Fields HWSWater_Tariff = DbInteract.GetRow_FromTariffsTable_byServisName(DbSettings.HWSWater_defloatName);
            TariffsTable_Fields HWSHeat_Tariff = DbInteract.GetRow_FromTariffsTable_byServisName(DbSettings.HWSHeat_defloatName);

            if (userData.UserIsHaveHWSCounter)
                CalcPaymentSum_For_HWSCounter(userData.HWSCounterData, ref paymentSum, HWS_Tariff, HWSWater_Tariff, HWSHeat_Tariff);
            else
                CalcPaymentSum_For_HWSNorm(userData.NumberOfLodgers, ref paymentSum, HWS_Tariff, HWSWater_Tariff, HWSHeat_Tariff);
        }

        private static void CalcPaymentSum_For_HWSCounter(double HWSCounterData, ref PaymentSum paymentSum, TariffsTable_Fields HWS_Tariff, TariffsTable_Fields HWSWater_Tariff, TariffsTable_Fields HWSHeat_Tariff)
        {

            double pastHWSCounterData = GetPastHWSCounterData();
            ValidateNewCounterData(HWSCounterData, pastHWSCounterData, "ГВС");

            double amount = GetAmountForCounterData(HWSCounterData, pastHWSCounterData);
            //supply
            double tariff = HWS_Tariff.TariffRate;
            paymentSum.HotWaterSupply = CalcPaymentSum(amount, tariff);
            //water 
            double tariff_water = HWSWater_Tariff.TariffRate;
            paymentSum.HotWaterSupply_Water = CalcPaymentSum(amount, tariff_water);
            //heat
            double amount_heat = amount * HWSHeat_Tariff.StandartNorm;
            double tariff_heat = HWSHeat_Tariff.StandartNorm;
            paymentSum.HotWaterSupply_Heat = CalcPaymentSum(amount_heat, tariff_heat);
        }

        private static double GetPastHWSCounterData()
        {
            double pastHWSCounterData;

            if (TryGetLastCountersReadingsRow(out CounterReadingsTable_Fields counterReadingsTable_Fields))
            {
                pastHWSCounterData = counterReadingsTable_Fields.HotWaterSupply_CounterReadings;
            }
            else pastHWSCounterData = defaultCounterData;

            return pastHWSCounterData;
        }

        private static void CalcPaymentSum_For_HWSNorm(int numberOfLodgers, ref PaymentSum paymentSum, TariffsTable_Fields HWS_Tariff, TariffsTable_Fields HWSWater_Tariff, TariffsTable_Fields HWSHeat_Tariff)
        {
            //supply
            double amount = GetAmountForNormed(HWS_Tariff.StandartNorm, numberOfLodgers);
            double tariff = HWS_Tariff.TariffRate;
            paymentSum.HotWaterSupply = CalcPaymentSum(amount, tariff);
            //water
            double amount_water = GetAmountForNormed(HWSWater_Tariff.StandartNorm, numberOfLodgers);
            double tariff_water = HWSWater_Tariff.TariffRate;
            paymentSum.HotWaterSupply_Water = CalcPaymentSum(amount_water, tariff_water);
            //heat
            double amount_heat = amount * HWSHeat_Tariff.StandartNorm;
            double tariff_heat = HWSHeat_Tariff.StandartNorm;
            paymentSum.HotWaterSupply_Heat = CalcPaymentSum(amount_heat, tariff_heat);
        }
        #endregion

        #region===ES===
        private static void CalcPaymentSum_For_ES(UserInputs userData, ref PaymentSum paymentSum)
        {
            TariffsTable_Fields ES_Tariff = DbInteract.GetRow_FromTariffsTable_byServisName(DbSettings.ES_defloatName);
            TariffsTable_Fields ESDay_Tariff = DbInteract.GetRow_FromTariffsTable_byServisName(DbSettings.ESDay_defloatName);
            TariffsTable_Fields ESNight_Tariff = DbInteract.GetRow_FromTariffsTable_byServisName(DbSettings.ESNight_defloatName);

            if (userData.UserIsHaveESCounter)
                CalcPaymentSum_For_ESCounter(userData.NightESCounterData, userData.DayESCounterData, ref paymentSum, ESDay_Tariff, ESNight_Tariff);
            else
                CalcPaymentSum_For_ESNorm(userData.NumberOfLodgers, ref paymentSum, ES_Tariff);
        }

        private static void CalcPaymentSum_For_ESCounter(double HWSCounterData_Night, double HWSCounterData_Day, ref PaymentSum paymentSum, TariffsTable_Fields ESDay_Tariff, TariffsTable_Fields ESNight_Tariff)
        {
            //Night
            double pastESCounterData_night = GetPastESCounterData_night();
            ValidateNewCounterData(HWSCounterData_Night, pastESCounterData_night, "ЭЭ Ночь");

            double amount_night = GetAmountForCounterData(HWSCounterData_Night, pastESCounterData_night);
            double tariff_night = ESNight_Tariff.TariffRate;
            paymentSum.Electricity_Night = CalcPaymentSum(amount_night, tariff_night);
            //Day
            double pastESCounterData_day = GetPastESCounterData_day();
            ValidateNewCounterData(HWSCounterData_Day, pastESCounterData_day, "ЭЭ День");

            double amount_day = GetAmountForCounterData(HWSCounterData_Day, pastESCounterData_day);
            double tariff_day = ESDay_Tariff.TariffRate;
            paymentSum.Electricity_Day = CalcPaymentSum(amount_day, tariff_day);
            //sum
            paymentSum.Electricity_Sum = paymentSum.Electricity_Night + paymentSum.Electricity_Day;
        }

        private static double GetPastESCounterData_night()
        {
            double pastESCounterData_night;

            if (TryGetLastCountersReadingsRow(out CounterReadingsTable_Fields counterReadingsTable_Fields))
            {
                pastESCounterData_night = counterReadingsTable_Fields.ElectricitySupplyNight_CounterReadings;
            }
            else pastESCounterData_night = defaultCounterData;

            return pastESCounterData_night;
        }

        private static double GetPastESCounterData_day()
        {
            double pastESCounterData_day;

            if (TryGetLastCountersReadingsRow(out CounterReadingsTable_Fields counterReadingsTable_Fields))
            {
                pastESCounterData_day = counterReadingsTable_Fields.ElectricitySupplyDay_CounterReadings;
            }
            else pastESCounterData_day = defaultCounterData;

            return pastESCounterData_day;
        }

        private static void CalcPaymentSum_For_ESNorm(int numberOfLodgers, ref PaymentSum paymentSum, TariffsTable_Fields ES_Tariff)
        {
            double amount = GetAmountForNormed(ES_Tariff.StandartNorm, numberOfLodgers);
            double tariff = ES_Tariff.TariffRate;

            paymentSum.Electricity_Night = 0;
            paymentSum.Electricity_Day = 0;
            paymentSum.Electricity_Sum = CalcPaymentSum(amount, tariff);
        }
        #endregion

        #region===Payment formula===
        private static double GetAmountForCounterData(double nowData, double pastData)
        {
            return nowData - pastData;
        }

        private static double GetAmountForNormed(double norm, int numberOfLodgers)
        {
            return norm * numberOfLodgers;
        }

        private static double CalcPaymentSum(double amount, double tariff)
        {
            return amount * tariff;
        }
        #endregion

        private static void ValidateNewCounterData(double nowData, double pastData) 
        {
            if (nowData < pastData)
                throw new Exception($"Ошибка! Неверные данные! Новые показания счетчика меньше предыдущих! Предыдущие: {pastData}, Новые: {nowData}");
        }

        private static void ValidateNewCounterData(double nowData, double pastData, string counterName)
        {
            if (nowData < pastData)
                throw new Exception($"Ошибка! Неверные данные! Новые показания счетчика {counterName} меньше предыдущих! Новые: {nowData}, Предыдущие: {pastData}");
        }
    }
}
