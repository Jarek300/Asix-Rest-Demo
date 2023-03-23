using Microsoft.AspNetCore.Mvc.RazorPages;
using Asix;


namespace WebApplication.Pages.Attribute
{
    public class Demo2Model : PageModel
    {
        /// <summary>
        /// Nazwa zmiennej, której atrybuty są czytane
        /// </summary>
        public string mVariableName = "A000";

        /// <summary>
        /// Opis ewentualnego błędu odczytu
        /// </summary>
        public string mReadError = "";

        /// <summary>
        /// Lista nazw wszystkich atrybutów w bazie definicji zmiennych
        /// </summary>
        public List<string> mAttributeNames = new();

        /// <summary>
        /// Lista wartości atrybutów
        /// </summary>
        public List<string> mVariableAttributes = new();


        /// <summary>
        /// Funkcja wywoływana przy generowaniu strony
        /// </summary>
        public async Task OnGet()
        {
            try
            {
                AsixRestClient asixRestClient = AsixRestClient.Create();

                // Odczyt z serwera REST nazw wszystkich atrybutów
                var serverAttributeNames = await asixRestClient.GetServerAttributeNamesAsync();
                mAttributeNames = serverAttributeNames.ToList();


                // Odczyt z serwera REST wartości atrybutów atrybutów zmiennej
                IList<IList<string>> variableAttributes = await asixRestClient.GetVariableAttributeAsync(new string[] { mVariableName }, mAttributeNames);
                mVariableAttributes = variableAttributes.First().ToList();
            }
            catch (Exception e)
            {
                mReadError = e.Message;
            }
        }
    }
}