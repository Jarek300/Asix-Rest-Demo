using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Asix;

namespace WebApplication.Pages.VariableDynamic
{
    /// <summary>
    /// Model dla szablonu widoku zmiennej _Demo2Variable.cshtml
    /// </summary>
    public class VariableTemplate
    {
        /// <summary>
        /// Nazwa zmiennej. Identyfikuje główny DIV szablonu zmiennej
        /// </summary>
        public string mName;

        public VariableTemplate(string aName)
        {
            mName = aName;
        }
    }


    public class Demo2Model : PageModel
    {
        public Dictionary<string, VariableModel> mVariables = new Dictionary<string, VariableModel>();


        public Demo2Model()
        {
            mVariables["A000"] = new VariableModel("A000", "Temperatura spalin przed odemglaczem", "°C");
            mVariables["A004"] = new VariableModel("A004", "Temperatura kwasu siarkowego", "°C");
            mVariables["A008"] = new VariableModel("A008", "Temperatura wody ciepłej", "°C");

            mVariables["A082"] = new VariableModel("A082", "Przepływ kwasu siarkowego", "m³/h");
            mVariables["A084"] = new VariableModel("A084", "Poziom w zb. cyrkulacyjnym kwasu", "%");
            mVariables["A086"] = new VariableModel("A086", "Przepływ wody chłodzącej", "m³/h");

            mVariables["Nieznana"] = new VariableModel("Nieznana", "Nieznana zmienna", "");
        }

        /// <summary>
        /// Funkcja wywoływana przy pobieraniu strony przez przeglądarkę
        /// Pusta, ponieważ wszystkie operacje odbywają się po stronie klienta
        /// </summary>
        public void OnGet()
        {

        }


        /// <summary>
        /// Odczyt wartości bieżącej zmiennych
        /// </summary>
        /// <param name="name">Nazwa zmiennej, może wystapić wielokrotnie</param>
        /// <returns>Json zawierający tablicę obiektów VariableModel</returns>
        public async Task<JsonResult> OnGetVariables(string[] name)
        {
            List<VariableModel> variables = new List<VariableModel>();

            if (name == null)
                return new JsonResult(variables.ToArray());

            foreach (string variableName in name)
            {
                VariableModel variableModel;
                if (!mVariables.ContainsKey(variableName))
                    variableModel = new VariableModel(variableName, "", "");
                else
                    variableModel = mVariables[variableName];

                await ReadVariableValue(variableModel);
                variables.Add(variableModel);
            }

            return new JsonResult(variables.ToArray());
        }


        async Task ReadVariableValue(VariableModel aVariableModel)
        {
            try
            {
                AsixRestClient asixRestClient = AsixRestClient.Create();
                ICollection<VariableValue> variableStates = await asixRestClient.GetVariableValueAsync(new string[] { aVariableModel.mName });
                VariableValue variableState = variableStates.First();

                aVariableModel.mDateTime = variableState.TimeStamp;

                switch (variableState.Quality & 0xC0)
                {
                    case 0xC0:
                        {
                            double value = (double)variableState.Value;
                            aVariableModel.mValueFormatted = value.ToString("F0");
                            break;
                        }

                    case 0x40:
                        {
                            double value = (double)variableState.Value;
                            aVariableModel.mValueFormatted = value.ToString("F0") + "?";
                            break;
                        }

                    default:
                        {
                            aVariableModel.mValueFormatted = "?";
                            break;
                        }
                }
            }
            catch (Exception e)
            {
                aVariableModel.mReadError = e.Message;
            }
        }

    }
}