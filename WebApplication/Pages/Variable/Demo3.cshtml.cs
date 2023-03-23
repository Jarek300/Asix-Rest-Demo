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
        public List<Demo3VariableModel> mVariableModelList = new();

        public Demo3VariableModel mVariableModelA000 = new("A000", "Temperatura spalin przed odemglaczem", "°C", "F0", 2, 10, 190, 200);
        public Demo3VariableModel mVariableModelA004 = new("A004", "Temperatura kwasu siarkowego", "°C", "F0", 2, 10, 190, 200);
        public Demo3VariableModel mVariableModelA008 = new("A008", "Temperatura wody ciepłej", "°C", "F1", 1, 8, 90, 98);
        public Demo3VariableModel mVariableModelA082 = new("A082", "Przepływ kwasu siarkowego", "m³/h", "F0", 2, 10, 190, 200);
        public Demo3VariableModel mVariableModelA084 = new("A084", "Poziom w zb. cyrkulacyjnym kwasu", "%", "F1", 2, 10, 190, 200);
        public Demo3VariableModel mVariableModelA086 = new("A086", "Przepływ wody chłodzącej", "m³/h", "F0", 2, 10, 270, 290);
        public Demo3VariableModel mVariableModelUnknown = new("UnknownVariable", "UnknownVariable - desc", "m³/h", "F0", 2, 10, 270, 290);


        public Demo3Model()
        {
            mVariableModelList.Add(mVariableModelA000);
            mVariableModelList.Add(mVariableModelA004);
            mVariableModelList.Add(mVariableModelA008);
            mVariableModelList.Add(mVariableModelA082);
            mVariableModelList.Add(mVariableModelA084);
            mVariableModelList.Add(mVariableModelA086);
            mVariableModelList.Add(mVariableModelUnknown);
        }


        /// <summary>
        /// Funkcja wywoływana przy generowaniu strony. Czyta wartosci kolejny zmiennych.
        /// Odczyt wartości bieżącej i średniej jednej zmiennej
        /// </summary>
        public async Task OnGet()
        {
            try
            {
                AsixRestClient asixRestClient = AsixRestClient.Create();

                // Odczyt wartości zmiennych z serwera REST
                string[] variableNames = mVariableModelList.Select(x => x.Name).ToArray();
                IList<VariableValue> variableValues = await asixRestClient.GetVariableValueAsync(variableNames);

                // Odczyt średniej wartości zmiennej z serwera REST
                AggregateRange[] aggregateRanges = mVariableModelList.Select(x => new AggregateRange(x.Name, "Average", "15m", "60S")).ToArray();
                IList<VariableValue> variableAverages = await asixRestClient.PostGetVariableAggregateAsync(aggregateRanges);


                for (int i = 0; i < mVariableModelList.Count; i++)
                {
                    Demo3VariableModel aVariableModel = mVariableModelList[i];

                    VariableValue variableValue = variableValues[i];
                    aVariableModel.SetValue(variableValue);

                    VariableValue variableAverage = variableAverages[i];
                    aVariableModel.SetValueTrend(variableAverage);
                }
            }
            catch (Exception e)
            {
                for (int i = 0; i < mVariableModelList.Count; i++)
                {
                    Demo3VariableModel aVariableModel = mVariableModelList[i];
                    aVariableModel.SetError(e.Message);
                }
            }
        }
    }
}
