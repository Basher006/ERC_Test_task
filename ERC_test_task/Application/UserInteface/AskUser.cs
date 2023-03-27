

namespace ERC_test_task.Application.UserInteface
{
    internal static class AskUser
    {
        private delegate bool ValidateFunc(string userInput);

        private static readonly string yesAnswer = "1";


        internal static string DateTime()
        {
            ValidateFunc validateDateTime = ValidateUserInputs.DateTime;
            string userInput = _askUser(TextForUserInputs.DateTime, validateDateTime);

            return userInput;
        }

        internal static int HowManyLodgers()
        {
            ValidateFunc validateHowManyLodger = ValidateUserInputs.HowManyLodger;
            string userInput = _askUser(TextForUserInputs.HowManyLodgers, validateHowManyLodger);

            return int.Parse(userInput); // На этом этапе здесь могут быть только цифры.
        }

        #region ===CWS===
        internal static bool IsHaveCWSCounter()
        {
            ValidateFunc validateUserIsHaveCWScounter = ValidateUserInputs.TwoOptions;
            string userInput = _askUser(TextForUserInputs.IsHaveCWSCounter, validateUserIsHaveCWScounter);

            return userInput == yesAnswer;
        }

        internal static double CWSCounterData()
        {
            ValidateFunc validateUserCWSCounterData = ValidateUserInputs.DoubleInput;
            string userInput = _askUser(TextForUserInputs.CWSCounterData, validateUserCWSCounterData);

            return double.Parse(userInput); // На этом этапе здесь может быть только double.
        }
        #endregion

        #region ===HWS===
        internal static bool IsHaveHWSCounter()
        {
            ValidateFunc validateUserIsHaveHWScounter = ValidateUserInputs.TwoOptions;
            string userInput = _askUser(TextForUserInputs.IsHaveHWSCounter, validateUserIsHaveHWScounter);

            return userInput == yesAnswer;
        }

        internal static double HWSCounterData()
        {
            ValidateFunc validateUserHWSCounterData = ValidateUserInputs.DoubleInput;
            string userInput = _askUser(TextForUserInputs.HWSCounterData, validateUserHWSCounterData);

            return double.Parse(userInput); // На этом этапе здесь может быть только double.
        }
        #endregion
        #region ===ES===

        internal static bool IsHaveESCounter()
        {
            ValidateFunc validateUserIsHaveEScounter = ValidateUserInputs.TwoOptions;
            string userInput = _askUser(TextForUserInputs.IsHaveElectoSupplyCounter, validateUserIsHaveEScounter);

            return userInput == yesAnswer;
        }

        internal static double NightESCounterData()
        {
            ValidateFunc validateUserESCounterData = ValidateUserInputs.DoubleInput;
            string userInput = _askUser(TextForUserInputs.ElectoSupplyNightCounterData, validateUserESCounterData);

            return double.Parse(userInput); // На этом этапе здесь может быть только double.
        }

        internal static double DaytESCounterData()
        {
            ValidateFunc validateUserESCounterData = ValidateUserInputs.DoubleInput;
            string userInput = _askUser(TextForUserInputs.ElectoSupplyDayCounterData, validateUserESCounterData);

            return double.Parse(userInput); // На этом этапе здесь может быть только double.
        }
        #endregion


        private static string _askUser(string askText, ValidateFunc validateFunc, string inputErrorText)
        {
            string userInput;
            bool isValidInput = true;
            do
            {
                if (!isValidInput)
                    Console.WriteLine(inputErrorText);


                Console.WriteLine(TextForUserInputs.AskPrefix + askText);
                userInput = Console.ReadLine();

                isValidInput = validateFunc(userInput);

            } while (!isValidInput); // Цикл будет продолжаться пока юзер не введет валидные данные.


            return userInput;
        }

        private static string _askUser(string askText, ValidateFunc validateFunc)
        {
            string inputErrorText = TextForUserInputs.DefaultInputErrorText;
            return _askUser(askText, validateFunc, inputErrorText);
        }
    }
}