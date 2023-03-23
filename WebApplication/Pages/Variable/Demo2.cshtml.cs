using Microsoft.AspNetCore.Mvc.RazorPages;
using Asix;


namespace WebApplication.Pages.Variable
{
    /// <summary>
    /// Klasa przechowująca model jednej zmiennej
    /// </summary>
    public class Demo2VariableModel
    {
        public Demo2VariableModel(string aName, string aDecription, string aUnit)
        {
            mName = aName;
            mDecription = aDecription;
            mUnit = aUnit;
        }

        /// <summary>
        ///  Atrybuty zmiennej
        /// </summary>
        public string mName, mDecription, mUnit;

        /// <summary>
        /// Sformatowaną wartość zmiennej
        /// </summary>
        public string mValueFormatted = "?";

        /// <summary>
        /// Ewentualny błąd odczytu wartości zmiennej
        /// </summary>
        public string mReadError = "";
    }



    /// <summary>
    /// Przykład odczytu wartości wielu zmiennych
    /// </summary>
    public class Demo2Model : PageModel
    {
        /// <summary>
        /// Model zmiennej A000
        /// </summary>
        public Demo2VariableModel mVariableModelA000 = new Demo2VariableModel("A000", "Temperatura spalin przed odemglaczem", "°C");

        public Demo2VariableModel mVariableModelA004 = new Demo2VariableModel("A004", "Temperatura kwasu siarkowego", "°C");
        public Demo2VariableModel mVariableModelA008 = new Demo2VariableModel("A008", "Temperatura wody ciepłej", "°C");

        public Demo2VariableModel mVariableModelA082 = new Demo2VariableModel("A082", "Przepływ kwasu siarkowego", "m³/h");
        public Demo2VariableModel mVariableModelA084 = new Demo2VariableModel("A084", "Poziom w zb. cyrkulacyjnym kwasu", "%");
        public Demo2VariableModel mVariableModelA086 = new Demo2VariableModel("A086", "Przepływ wody chłodzącej", "m³/h");


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
        }



        /// <summary>
        /// Odczyt wartości jednej zmiennej
        /// </summary>
        /// <param name="aVariableModel">Model zmiennej. Jego pole mName zawiera nazwe zmiennej.</param>
        async Task ReadVariableValue(Demo2VariableModel aVariableModel)
        {
            try
            {
                AsixRestClient asixRestClient = AsixRestClient.Create();

                // Odczyt wartości zmiennej z serwera REST
                ICollection<VariableValue> variableStates = await asixRestClient.GetVariableValueAsync(new string[] { aVariableModel.mName });
                VariableValue variableState = variableStates.First();


                // Obsług błędu odczytu
                if (!variableState.ReadSucceeded)
                {
                    aVariableModel.mReadError = variableState.ReadStatusString;
                    return;
                }


                // Formatowanie wartości zmiennej
                if (AsixRestClient.IsQualityGood(variableState.Quality))
                {
                    // Formatowanie wartości o jakości dobrej
                    double value = (double)variableState.Value;
                    aVariableModel.mValueFormatted = value.ToString("F0");
                }
                else if (AsixRestClient.IsQualityUncertain(variableState.Quality))
                {
                    // Formatowanie wartości o jakości niepewnej
                    double value = (double)variableState.Value;
                    aVariableModel.mValueFormatted = value.ToString("F0") + "?";
                }
                else
                {
                    // Dla wartości o jakości złej wyświetlimy pytajnik
                    aVariableModel.mValueFormatted = "?";
                }
            }
            catch (Exception e)
            {
                aVariableModel.mReadError = e.Message;
            }
        }
    }
}