

namespace ERC_test_task.Application.UserInteface
{
    internal struct UserInputs
    {
        public string DateTime;
        public int NumberOfLodgers;

        public bool UserIsHaveCWSCounter;
        public bool UserIsHaveHWSCounter;
        public bool UserIsHaveESCounter;

        public double CWSCounterData;
        public double HWSCounterData;
        public double NightESCounterData;
        public double DayESCounterData;

        public void Clear()
        {
            DateTime = "";
            NumberOfLodgers = 0;
            UserIsHaveCWSCounter = false;
            UserIsHaveHWSCounter = false;
            UserIsHaveESCounter = false;
            CWSCounterData = 0;
            HWSCounterData = 0;
            NightESCounterData = 0;
            DayESCounterData = 0;
        }
    }
}