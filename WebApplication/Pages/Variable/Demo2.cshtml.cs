using Microsoft.AspNetCore.Mvc.RazorPages;
using Asix;
using WebApplication.Code;

namespace WebApplication.Pages.Variable
{
    /// <summary>
    /// Przykład odczytu wartości wielu zmiennych
    /// </summary>
    public class Demo2Model : PageModel
    {
        public List<VariableModel> mVariableModelList = new List<VariableModel>();

        public VariableModel mVariableModelA000 = new("A000", "Temperatura spalin przed odemglaczem", "°C");
        public VariableModel mVariableModelA004 = new("A004", "Temperatura kwasu siarkowego", "°C");
        public VariableModel mVariableModelA008 = new("A008", "Temperatura wody ciepłej", "°C");
        public VariableModel mVariableModelA082 = new("A082", "Przepływ kwasu siarkowego", "m³/h");
        public VariableModel mVariableModelA084 = new("A084", "Poziom w zb. cyrkulacyjnym kwasu", "%");
        public VariableModel mVariableModelA086 = new("A086", "Przepływ wody chłodzącej", "m³/h");


        public Demo2Model()
        {
            mVariableModelList.Add(mVariableModelA000);
            mVariableModelList.Add(mVariableModelA004);
            mVariableModelList.Add(mVariableModelA008);
            mVariableModelList.Add(mVariableModelA082);
            mVariableModelList.Add(mVariableModelA084);
            mVariableModelList.Add(mVariableModelA086);
        }


        /// <summary>
        /// Funkcja wywoływana przy generowaniu strony. Czyta wartosci kolejny zmiennych.
        /// </summary>
        public async Task OnGet()
        {
            await ReadVariableValues();
        }



        /// <summary>
        /// Odczyt wartości jednej zmiennej
        /// </summary>
        /// <param name="aVariableModel">Model zmiennej. Jego pole mName zawiera nazwe zmiennej.</param>
        async Task ReadVariableValues()
        {
            try
            {
                AsixRestClient asixRestClient = AsixRestClient.Create();

                // Odczyt wartości zmiennej z serwera REST
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
                    mVariableModelList[i].SetError(e.Message);
                }
            }
        }
    }
}