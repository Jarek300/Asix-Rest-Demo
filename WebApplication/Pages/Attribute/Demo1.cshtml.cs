using Microsoft.AspNetCore.Mvc.RazorPages;
using Asix;


namespace WebApplication.Pages.Attribute
{
    /// <summary>
    /// Przykład odczytu atrybutów jednej zmiennej
    /// </summary>
    public class Demo1Model : PageModel
    {
        /// <summary>
        /// Nazwa zmiennej, której atrybuty są czytane
        /// </summary>
        public string mVariableName = "A000";

        /// <summary>
        /// Lista nazw atrybutów, które są czytane
        /// </summary>
        List<string> mAttributeNameList = new List<string> { "Name", "Description", "Unit", "Szafa", "Listwa" };

        /// <summary>
        /// Opis ewentualnego błędu odczytu
        /// </summary>
        public string mReadError = "";

        /// <summary>
        /// Przeczytane wartości atrybutów zmiennej
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
                IList<IList<string>> variableAttributesCollection = await asixRestClient.GetVariableAttributeAsync(new string[] { mVariableName }, mAttributeNameList);
                IList<string>? variableAttributes = variableAttributesCollection.FirstOrDefault();

                if (variableAttributes != null)
                {
                    mVariableAttributes = variableAttributes.ToList();
                }
                else
                {
                    mReadError = "Błąd odczytu atrybutów : nieznana zmienna";
                }
            }
            catch (Exception e)
            {
                mReadError = e.Message;
            }
        }
    }
}