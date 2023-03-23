using Microsoft.AspNetCore.Mvc.RazorPages;
using Asix;


namespace WebApplication.Pages.Variable
{
    /// <summary>
    /// Przykład odczytu wartości jednej zmiennej
    /// </summary>
    public class Demo1Model : PageModel
    {
        /// <summary>
        /// Przeczytana i sformatowana wartości zmiennej
        /// </summary>
        public string mValueFormatted = "";

        /// <summary>
        /// Opis ewentualnego błędu odczytu
        /// </summary>
        public string mReadError = "";


        /// <summary>
        /// Funkcja wywoływana przy generowaniu strony
        /// </summary>
        public async Task OnGet()
        {
            try
            {
                await ReadVariableValue();
            }
            catch (Exception e)
            {
                mReadError = e.Message;
            }
        }


        async Task ReadVariableValue()
        {
            AsixRestClient asixRestClient = AsixRestClient.Create();

            // Odczyt wartości zmiennej
            ICollection<VariableValue> variableValues = await asixRestClient.GetVariableValueAsync(new string[] { "A110" });
            VariableValue variableState = variableValues.First();


            // Sprawdzenie czy nie wystąpił błąd odczytu
            if (!variableState.ReadSucceeded)
            {
                mReadError = variableState.ReadStatusString;
                return;
            }


            // Formatowanie wartości zmiennej
            if (AsixRestClient.IsQualityGood(variableState.Quality))
            {
                // Formatowanie wartości o jakości dobrej
                double value = (double)variableState.Value;
                mValueFormatted = value.ToString("F0");
            }
            else if (AsixRestClient.IsQualityUncertain(variableState.Quality))
            {
                // Formatowanie wartości o jakości niepewnej
                double value = (double)variableState.Value;
                mValueFormatted = value.ToString("F0") + "?";
            }
            else
            {
                // Dla wartości o jakości złej wyświetlimy pytajnik
                mValueFormatted = "?";
            }
        }
    }
}