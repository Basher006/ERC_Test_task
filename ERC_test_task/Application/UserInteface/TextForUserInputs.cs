

namespace ERC_test_task.Application.UserInteface
{
    internal class TextForUserInputs
    {
        internal static readonly string DefaultInputErrorText = "Вы ввели не корректные данные! Попробуйте ещё раз.";

        internal static readonly string DateTime = "Это первый запуск программы. Введите Месяц и год для периуда расчета в формате \"02.2023\":";

        internal static readonly string HowManyLodgers = "Введите колличество проживающих в квартире:";

        internal static readonly string IsHaveCWSCounter = "У вас есть счетчик ХВС? (Введите \"1\" или \"2\")\n\t1: Да\n\t2: Нет";
        internal static readonly string CWSCounterData = "Введите показания счетчика ХВС";

        internal static readonly string IsHaveHWSCounter = "У вас есть счетчик ГВС? (Введите \"1\" или \"2\")\n\t1: Да\n\t2: Нет";
        internal static readonly string HWSCounterData = "Введите показания счетчика ГВС";

        internal static readonly string IsHaveElectoSupplyCounter = "У вас есть счетчик ЭЭ? (Введите \"1\" или \"2\")\n\t1: Да\n\t2: Нет";
        internal static readonly string ElectoSupplyNightCounterData = "Введите показания счетчика ЭЭ Ночь";
        internal static readonly string ElectoSupplyDayCounterData = "Введите показания счетчика ЭЭ День";

        internal static readonly string AskPrefix = "\n=> ";
    }
}