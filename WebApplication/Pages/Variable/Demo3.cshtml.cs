using Microsoft.AspNetCore.Mvc.RazorPages;
using Asix;


namespace WebApplication.Pages.Variable
{
    /// <summary>
    /// Przykład odczytu wartości wielu zmiennych i wypracowania dodatkowych informacji o zmiennej - informacji o przekroczeniu limitów i trendu zmian wartości.
    /// Karta wartości zmiennej wyświetla informacje o przekroczeniu limitów w postaci koloru wartości, a informacje o trendzie zmian wartości w postaci ikony.
    /// </summary>
    public class Demo3Model : PageModel
    {
        public Demo3VariableModel mVariableModelA000 = new Demo3VariableModel("A000", "Temperatura spalin przed odemglaczem", "°C", "F0", 2, 10, 190, 200);
        public Demo3VariableModel mVariableModelA004 = new Demo3VariableModel("A004", "Temperatura kwasu siarkowego", "°C", "F0", 2, 10, 190, 200);
        public Demo3VariableModel mVariableModelA008 = new Demo3VariableModel("A008", "Temperatura wody ciepłej", "°C", "F1", 1, 8, 90, 98);

        public Demo3VariableModel mVariableModelA082 = new Demo3VariableModel("A082", "Przepływ kwasu siarkowego", "m³/h", "F0", 2, 10, 190, 200);
        public Demo3VariableModel mVariableModelA084 = new Demo3VariableModel("A084", "Poziom w zb. cyrkulacyjnym kwasu", "%", "F1", 2, 10, 190, 200);
        public Demo3VariableModel mVariableModelA086 = new Demo3VariableModel("A086", "Przepływ wody chłodzącej", "m³/h", "F0", 2, 10, 270, 290);

        public Demo3VariableModel mVariableModelUnknown = new Demo3VariableModel("UnknownVariable", "UnknownVariable - desc", "m³/h", "F0", 2, 10, 270, 290);


        /// <summary>
        /// Funkcja wywoływana przy generowaniu strony. Czyta wartosci kolejny zmiennych.
        /// </summary>
        public async Task OnGet()
        {
            await ReadVariableValue(mVariableModelA000);
            await ReadVariableValue(mVariableModelA004);
            await ReadVariableValue(mVariableModelA008);

            await ReadVariableValue(mVariableModelA082);
            await ReadVariableValue(mVariableModelA084);
            await ReadVariableValue(mVariableModelA086);

            await ReadVariableValue(mVariableModelUnknown);
        }


        /// <summary>
        /// Odczyt wartości bieżącej i średniej jednej zmiennej
        /// </summary>
        /// <param name="aVariableModel">Model zmiennej. Jego pole mName zawiera nazwę zmiennej.</param>
        async Task ReadVariableValue(Demo3VariableModel aVariableModel)
        {
            try
            {
                AsixRestClient asixRestClient = AsixRestClient.Create();

                // Odczyt wartości zmiennej z serwera REST
                ICollection<VariableValue> variableValues = await asixRestClient.GetVariableValueAsync(new string[] { aVariableModel.Name });
                VariableValue variableValue = variableValues.First();

                if (!aVariableModel.SetValue(variableValue))
                    return;


                // Odczyt średniej wartości zmiennej z serwera REST
                ICollection<VariableValue> variableAverages = await asixRestClient.GetVariableAggregateAsync(aVariableModel.Name, "Average", "15m", null, null, "60S");

                VariableValue variableAverage = variableAverages.First();

                // wypracowanie przez klasę modelu informacji o trendzie zmian wartości zmiennej
                aVariableModel.SetValueTrend(variableAverage);
            }
            catch (Exception e)
            {
                aVariableModel.ReadError = e.Message;
            }
        }
    }
}