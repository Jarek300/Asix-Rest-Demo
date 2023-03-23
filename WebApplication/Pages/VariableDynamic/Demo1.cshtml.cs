using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Asix;
using WebApplication.Code;

namespace WebApplication.Pages.VariableDynamic
{

    /// <summary>
    /// Przykład odczytu wartości wielu zmiennych, wartości zmiennych są odświeżane bez przeładowywania strony.
    /// Skrypt po stronie klienta periodycznie pobiera z serwera nowy stan kart zawierających wizualizację zmiennych (w postaci kodu HTML) i wstawia go do strony w przeglądarce
    /// </summary>
    public class Demo1Model : PageModel
    {
        public List<VariableModel> mVariableModelList = new List<VariableModel>();


        public Demo1Model()
        {
            mVariableModelList.Add(new VariableModel("A000", "Temperatura spalin przed odemglaczem", "°C"));
            mVariableModelList.Add(new VariableModel("A004", "Temperatura kwasu siarkowego", "°C"));
            mVariableModelList.Add(new VariableModel("A008", "Temperatura wody ciepłej", "°C"));
            mVariableModelList.Add(new VariableModel("A082", "Przepływ kwasu siarkowego", "m³/h"));
            mVariableModelList.Add(new VariableModel("A084", "Poziom w zb. cyrkulacyjnym kwasu", "%"));
            mVariableModelList.Add(new VariableModel("A086", "Przepływ wody chłodzącej", "m³/h"));
        }



        /// <summary>
        /// Funkcja wywoływana przy generowaniu strony. Czyta wartosci kolejny zmiennych.
        /// </summary>
        public async Task OnGet()
        {
            await ReadVariableValues();
        }



        /// <summary>
        /// Funkcja wywoływana przy odświeżaniu fragmentu strony. Czyta wartosci kolejny zmiennych i zwraca widok częsciowy zdefiniowany w pliku _Demo1VariableDeck.cshtml.
        /// </summary>
        public async Task<PartialViewResult> OnGetVariableDeck()
        {
            await ReadVariableValues();

            return new PartialViewResult
            {
                ViewName = "_Demo1VariableDeck",
                ViewData = new ViewDataDictionary<VariableModel[]>(ViewData, mVariableModelList.ToArray())
            };
        }


        /// <summary>
        /// Odczyt wartości zmiennych
        /// </summary>
        async Task ReadVariableValues()
        {
            try
            {
                AsixRestClient asixRestClient = AsixRestClient.Create();
                string[] variableNames = mVariableModelList.Select(x => x.Name).ToArray();
                IList<VariableValue> variableValues = await asixRestClient.GetVariableValueAsync(variableNames);

                for (int i = 0; i < mVariableModelList.Count; i++)
                {
                    mVariableModelList[i].SetVariableValue(variableValues[i]);
                }
            }
            catch (Exception e)
            {
                for (int i = 0; i < mVariableModelList.Count; i++)
                {
                    mVariableModelList[i].ReadError = e.Message;
                }
            }
        }
    }
}