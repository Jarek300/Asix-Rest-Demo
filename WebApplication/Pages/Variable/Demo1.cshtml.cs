using System;
using Microsoft.AspNetCore.Mvc.RazorPages;


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
        public string mValueFormatted;

        /// <summary>
        /// Opis ewentualnego błędu odczytu
        /// </summary>
        public string mReadError;


        /// <summary>
        /// Funkcja wywoływana przy generowaniu strony
        /// </summary>
        public void OnGet()
        {
            try
            {
                ReadVariableValue();
            }
            catch (Exception e)
            {
                mReadError = e.Message;
            }
        }


        void ReadVariableValue()
        {
            AsixRestClient asixRestClient = new AsixRestClient();

            // Odczyt wartości zmiennej
            VariableState variableState = asixRestClient.ReadVariableState("A110");


            // Sprawdzenie czy nie wystąpił błąd odczytu
            if (!variableState.readSucceeded)
            {
                mReadError = variableState.readStatusString;
                return;
            }


            // Formatowanie wartości zmiennej
            if (variableState.IsQualityGood())
            {
                // Formatowanie wartości o jakości dobrej
                double value = (double)variableState.value;
                mValueFormatted = value.ToString("F0");
            }
            else if (variableState.IsQualityUncertain())
            {
                // Formatowanie wartości o jakości niepewnej
                double value = (double)variableState.value;
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