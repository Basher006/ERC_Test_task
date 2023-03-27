

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

                userInputs.UserIsHaveCWSCounter = AskUser.IsHaveCWSCounter();
                if (userInputs.UserIsHaveCWSCounter)
                    userInputs.CWSCounterData = AskUser.CWSCounterData();

                userInputs.UserIsHaveHWSCounter = AskUser.IsHaveHWSCounter();
                if (userInputs.UserIsHaveHWSCounter)
                    userInputs.HWSCounterData = AskUser.HWSCounterData();

                userInputs.UserIsHaveESCounter = AskUser.IsHaveESCounter();
                if (userInputs.UserIsHaveESCounter)
                {
                    userInputs.NightESCounterData = AskUser.NightESCounterData();
                    userInputs.DayESCounterData = AskUser.DaytESCounterData();
                }

                paymentResult = Core.CalcPaymentSum(userInputs);

                Core.SaveData(userInputs, paymentResult);

                PrintPaymentResult(paymentResult, userInputs.DateTime);
            }
        }

        private static string GetDateTime()
        {
            // Пробуем получить последную запись с датой из базы данных, если получилось увеличиваем ее на 1 месяц.
            // Иначе спрашиваем пользователя.
            if (Core.TryGetLastDtimeFromDB(out string dateTime))
                return Core.DateTimeIncriment(dateTime);
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
                $"\n\tСумма...................{Math.Round(paymentResult.GetTotalSum(), 2)} руб.";

            Console.WriteLine(printTxt);
        }
        
        private static void PrintDateTime(string dateTime)
        {
            Console.WriteLine($"\n\n=== Новый расчет за месяц: {dateTime} ===");
        }
    }
}