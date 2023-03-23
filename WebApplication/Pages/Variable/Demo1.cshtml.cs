using Microsoft.AspNetCore.Mvc.RazorPages;
using Asix;
using WebApplication.Code;

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
        public VariableModel VariableModel = new("A110", "Przepływ gazu do pieca", "m³/h");


        /// <summary>
        /// Funkcja wywoływana przy generowaniu strony
        /// </summary>
        public async Task OnGet()
        {
            try
            {
                AsixRestClient asixRestClient = AsixRestClient.Create();

                ICollection<VariableValue> variableValues = await asixRestClient.GetVariableValueAsync(new string[] { "A110" });
                VariableValue variableState = variableValues.First();
                VariableModel.SetVariableValue(variableState);
            }
            catch (Exception e)
            {
                VariableModel.SetError(e.Message);
            }
        }
    }
}