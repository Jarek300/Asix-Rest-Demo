using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Asix;
using WebApplication.Code;

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

            try
            {
                if (name == null)
                    return new JsonResult(variables.ToArray());

                AsixRestClient asixRestClient = AsixRestClient.Create();
                IList<VariableValue> variableValues = await asixRestClient.GetVariableValueAsync(name);

                for (int i = 0; i < name.Length; i++)
                {
                    VariableModel variableModel;

                    if (!mVariables.ContainsKey(name[i]))
                        variableModel = new VariableModel(name[i], "", "");
                    else
                        variableModel = mVariables[name[i]];

                    variableModel.SetVariableValue(variableValues[i]);
                    variables.Add(variableModel);
                }

                return new JsonResult(variables.ToArray());
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                if (ex.InnerException != null)
                    message += " " + ex.InnerException.Message;

                for (int i = 0; i < name.Length; i++)
                {
                    VariableModel variableModel;

                    if (!mVariables.ContainsKey(name[i]))
                        variableModel = new VariableModel(name[i], "", "");
                    else
                        variableModel = mVariables[name[i]];

                    variableModel.SetError(ex.Message);
                    variables.Add(variableModel);
                }


                return new JsonResult(variables.ToArray());

            }
        }
    }
}