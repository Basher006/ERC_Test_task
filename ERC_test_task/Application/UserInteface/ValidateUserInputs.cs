

namespace ERC_test_task.Application.UserInteface
{
    internal static class ValidateUserInputs
    {
        private static readonly char[] SplitcharsForDtime = { '.', ',' };

        private static readonly int AcceptCharsCountInMounth = 2;
        private static readonly int AcceptCharsCountInYear = 4;


        internal static bool ValidateDateTime(string userInput)
        {
            // Проверка на пустую строку или Null.
            if (string.IsNullOrEmpty(userInput))
                return false;

            // Проверка на наличеие одной точки или запятой.
            string[] UserInput_DotSplit = userInput.Split(SplitcharsForDtime);
            if (UserInput_DotSplit.Length != 2)
                return false;

            // Проверка на колличество символов и являются ли они числом.
            // TODO: добавить проверку на диапазон значений (1..12 - для месяца, ~1970..текущий год - для года)
            string mount = UserInput_DotSplit[0];
            string year = UserInput_DotSplit[1];

            bool mountIsValid = (mount.Length == AcceptCharsCountInMounth) && IsInteger(mount);
            bool yearIsValid = (year.Length == AcceptCharsCountInYear) && IsInteger(year);


            return mountIsValid && yearIsValid;
        }

        internal static bool ValidateHowManyLodger(string userInput)
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

        internal static bool ValidateUserTwoOptions(string userInput)
        {
            bool success = int.TryParse(userInput, out int parseResult);
            if (success)
            {
                if (parseResult == 1 || parseResult == 2)
                    return true;
            }
            return false;
        }

        internal static bool ValidateDoubleInput(string userInput)
        {
            return double.TryParse(userInput, out double _);
        }

        private static bool IsInteger(string s)
        {
            return int.TryParse(s, out int _);
        }
    }
}
