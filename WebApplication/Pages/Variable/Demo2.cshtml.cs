using System;
using Microsoft.AspNetCore.Mvc.RazorPages;


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
        public string mReadError;
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
        public void OnGet()
        {
            ReadVariableValue(mVariableModelA000);
            ReadVariableValue(mVariableModelA004);
            ReadVariableValue(mVariableModelA008);

            ReadVariableValue(mVariableModelA082);
            ReadVariableValue(mVariableModelA084);
            ReadVariableValue(mVariableModelA086);
        }



        /// <summary>
        /// Odczyt wartości jednej zmiennej
        /// </summary>
        /// <param name="aVariableModel">Model zmiennej. Jego pole mName zawiera nazwe zmiennej.</param>
        void ReadVariableValue(Demo2VariableModel aVariableModel)
        {
            try
            {
                AsixRestClient asixRestClient = new AsixRestClient();

                // Odczyt wartości zmiennej z serwera REST
                VariableState variableState = asixRestClient.ReadVariableState(aVariableModel.mName);


                // Obsług błędu odczytu
                if (!variableState.readSucceeded)
                {
                    aVariableModel.mReadError = variableState.readStatusString;
                    return;
                }


                // Formatowanie wartości zmiennej
                if (variableState.IsQualityGood())
                {
                    // Formatowanie wartości o jakości dobrej
                    double value = (double)variableState.value;
                    aVariableModel.mValueFormatted = value.ToString("F0");
                }
                else if (variableState.IsQualityUncertain())
                {
                    // Formatowanie wartości o jakości niepewnej
                    double value = (double)variableState.value;
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