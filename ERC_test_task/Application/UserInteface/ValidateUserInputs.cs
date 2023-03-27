

namespace ERC_test_task.Application.UserInteface
{
    internal static class ValidateUserInputs
    {
        private static readonly char[] SplitcharsForDtime = { '.', ',' };

        private static readonly int[] monthValidRange = new int[] { 1, 12 };
        private static readonly int[] yearValidRange = new int[] { 1970, System.DateTime.Now.Year };


        internal static bool DateTime(string userInput)
        {
            // Проверка на пустую строку или Null.
            if (string.IsNullOrEmpty(userInput))
                return false;

            // Проверка на наличеие одной точки или запятой.
            string[] UserInput_DotSplit = userInput.Split(SplitcharsForDtime);
            if (UserInput_DotSplit.Length != 2)
                return false;

            // Проверка на то что месяц и год является числом
            // и проверка на диапазон значений (1..12 - для месяца, ~1970..текущий год - для года)
            bool mountIsValid = false;
            bool yearIsValid = false;

            string month = UserInput_DotSplit[0];
            string year = UserInput_DotSplit[1];

            if (int.TryParse(month, out int month_i))
            {
                if (month_i >= monthValidRange[0] && month_i <= monthValidRange[1])
                    mountIsValid = true;
            }

            if (int.TryParse(year, out int year_i))
            {
                if (year_i >= yearValidRange[0] && year_i <= yearValidRange[1])
                    yearIsValid = true;
            }

            return mountIsValid && yearIsValid;
        }

        internal static bool HowManyLodger(string userInput)
        {
            // Проверка на то что инпут число и что оно больше нуля.
            int IntegerResult;

            bool success = int.TryParse(userInput, out int parseResult);
            if (success)
            {
                IntegerResult = parseResult;
                if (IntegerResult > 0)
                    return true;
            }
            return false;
        }

        internal static bool TwoOptions(string userInput)
        {
            bool success = int.TryParse(userInput, out int parseResult);
            if (success)
            {
                if (parseResult == 1 || parseResult == 2)
                    return true;
            }
            return false;
        }

        internal static bool DoubleInput(string userInput)
        {
            return double.TryParse(userInput, out double _);
        }

        private static bool IsInteger(string s)
        {
            return int.TryParse(s, out int _);
        }
    }
}