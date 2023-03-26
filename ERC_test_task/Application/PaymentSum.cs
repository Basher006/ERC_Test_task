

namespace ERC_test_task.Application.UserInteface
{
    internal struct PaymentSum
    {
        public double ColdWaterSupply;
        public double HotWaterSupply;
        public double HotWaterSupply_Water;
        public double HotWaterSupply_Heat;
        public double Electricity_Night;
        public double Electricity_Day;
        public double Electricity_Sum;

        public double GetAllSum()
        {
            double _electricity_Sum = Electricity_Night != 0 && Electricity_Day != 0 ? Electricity_Night + Electricity_Day : Electricity_Sum;
            return ColdWaterSupply + HotWaterSupply + HotWaterSupply_Water + HotWaterSupply_Heat + _electricity_Sum;
        }
        public void Clear()
        {
            ColdWaterSupply = 0;
            HotWaterSupply = 0;
            HotWaterSupply_Water = 0;
            HotWaterSupply_Heat = 0;
            Electricity_Night = 0;
            Electricity_Day = 0;
            Electricity_Sum = 0;
        }
    }
}
