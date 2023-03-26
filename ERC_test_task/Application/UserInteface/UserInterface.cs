

namespace ERC_test_task.Application.UserInteface
{
    public static class UserInterface
    {
        private delegate bool ValidateFunc(string userInput);


        public static void MainLoop()
        {
            var userInputs = new UserInputs();
            var paymentResult = new PaymentSum();

            while (true)
            {
                userInputs.Clear();
                paymentResult.Clear();

                
                userInputs.DateTime = GetDateTime();
                PrintDateTime(userInputs.DateTime);

                userInputs.NumberOfLodgers = AskUser.HowManyLodgers();

                userInputs.UserIsHaveCWSCounter = AskUser.IsHaveColdWaterSupplyCounter();
                if (userInputs.UserIsHaveCWSCounter)
                    userInputs.ColdWaterSupplyData = AskUser.ColdWaterSupplyCounterData();

                userInputs.UserIsHaveHWSCounter = AskUser.IsHaveHotWaterSupplyCounter();
                if (userInputs.UserIsHaveHWSCounter)
                    userInputs.HotWaterSupplyData = AskUser.HotWaterSupplyCounterData();

                userInputs.UserIsHaveESCounter = AskUser.IsHaveElectricitySupplyCounter();
                if (userInputs.UserIsHaveESCounter)
                {
                    userInputs.NightElectricitySupplyData = AskUser.NightElectricitySupplyCounterData();
                    userInputs.DayElectricitySupplyData = AskUser.DaytElectricitySupplyCounterData();
                }

                paymentResult = Core.CalcPaymentSum(userInputs);

                Core.SaveData(userInputs, paymentResult);

                PrintPaymentResult(paymentResult, userInputs.DateTime);
            }
        }

        private static string GetDateTime()
        {
            if (Core.TryGetLastDtimeFromDB(out string dateTime))
                return dateTime;
            else
                return AskUser.DateTime();
        }

        private static void PrintPaymentResult(PaymentSum paymentResult, string dateTime)
        {
            string printTxt = "" +
                $"\nРасчет начислений за {dateTime}" +
                $"\n\tХВС.....................{Math.Round(paymentResult.ColdWaterSupply, 2)} руб." +
                $"\n\tГВС.....................{Math.Round(paymentResult.HotWaterSupply, 2)} руб." +
                $"\n\tГВС Теплоноситель.......{Math.Round(paymentResult.HotWaterSupply_Water, 2)} руб." +
                $"\n\tГВС Тепловая энергия....{Math.Round(paymentResult.HotWaterSupply_Heat, 2)} руб." +
                $"\n\tЭЭ день.................{Math.Round(paymentResult.Electricity_Day, 2)} руб." +
                $"\n\tЭЭ ночь.................{Math.Round(paymentResult.Electricity_Night, 2)} руб." +
                $"\n\tЭЭ сумма................{Math.Round(paymentResult.Electricity_Sum, 2)} руб." +
                $"\n" +
                $"\n\tСумма...................{Math.Round(paymentResult.GetAllSum(), 2)} руб.";

            Console.WriteLine(printTxt);
        }
        

        private static void PrintDateTime(string dateTime)
        {
            Console.WriteLine($"\n\n=== Новый расчет за месяц: {dateTime} ===");
        }
    }
}