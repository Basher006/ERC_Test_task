

namespace ERC_test_task.Application.UserInteface
{
    internal struct UserInputs
    {
        public string DateTime;
        public int NumberOfLodgers;

        public bool UserIsHaveCWSCounter;
        public bool UserIsHaveHWSCounter;
        public bool UserIsHaveESCounter;

        public double ColdWaterSupplyData;
        public double HotWaterSupplyData;
        public double NightElectricitySupplyData;
        public double DayElectricitySupplyData;
        public void Clear()
        {
            DateTime = "";
            NumberOfLodgers = 0;
            UserIsHaveCWSCounter = false;
            UserIsHaveHWSCounter = false;
            UserIsHaveESCounter = false;
            ColdWaterSupplyData = 0;
            HotWaterSupplyData = 0;
            NightElectricitySupplyData = 0;
            DayElectricitySupplyData = 0;
        }
    }
}
