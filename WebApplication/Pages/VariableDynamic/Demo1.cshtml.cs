using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Asix;


namespace WebApplication.Pages.VariableDynamic
{
    /// <summary>
    /// Klasa przechowująca model jednej zmiennej
    /// </summary>
    public class VariableModel
    {
        public VariableModel(string aName, string aDecription, string aUnit)
        {
            mName = aName;
            mDecription = aDecription;
            mUnit = aUnit;
        }

        /// <summary>
        ///  Atrybuty zmiennej
        /// </summary>
        public string mName { get; set; }
        public string mDecription { get; set; }
        public string mUnit { get; set; }

        /// <summary>
        /// Sformatowaną wartość zmiennej
        /// </summary>
        public string mValueFormatted { get; set; } = "?";

        /// <summary>
        /// Ewentualny błąd odczytu wartości zmiennej
        /// </summary>
        public string mReadError { get; set; } = "";

        /// <summary>
        /// Stemple czasu wartości zmiennej
        /// </summary>
        public DateTimeOffset mDateTime { get; set; }
    }



    /// <summary>
    /// Przykład odczytu wartości wielu zmiennych, wartości zmiennych są odświeżane bez przeładowywania strony.
    /// Skrypt po stronie klienta periodycznie pobiera z serwera nowy stan kart zawierających wizualizację zmiennych (w postaci kodu HTML) i wstawia go do strony w przeglądarce
    /// </summary>
    public class Demo1Model : PageModel
    {
        public List<VariableModel> mVariableModelList = new List<VariableModel>();
        public VariableModel mVariableModelA000 = new VariableModel("A000", "Temperatura spalin przed odemglaczem", "°C");
        public VariableModel mVariableModelA004 = new VariableModel("A004", "Temperatura kwasu siarkowego", "°C");
        public VariableModel mVariableModelA008 = new VariableModel("A008", "Temperatura wody ciepłej", "°C");

        public VariableModel mVariableModelA082 = new VariableModel("A082", "Przepływ kwasu siarkowego", "m³/h");
        public VariableModel mVariableModelA084 = new VariableModel("A084", "Poziom w zb. cyrkulacyjnym kwasu", "%");
        public VariableModel mVariableModelA086 = new VariableModel("A086", "Przepływ wody chłodzącej", "m³/h");



        public Demo1Model()
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
        /// Funkcja wywoływana przy odświeżaniu fragmentu strony. Czyta wartosci kolejny zmiennych i zwraca widok częsciowy zdefiniowany w pliku _Demo1VariableDeck.cshtml.
        /// </summary>
        public async Task<PartialViewResult> OnGetVariableDeck()
        {
            await ReadVariableValues();

            return new PartialViewResult {
                ViewName = "_Demo1VariableDeck",
                ViewData = new ViewDataDictionary<VariableModel[]>(ViewData, mVariableModelList.ToArray())
            };
        }



        /// <summary>
        /// Odczyt wartości zmiennych
        /// </summary>
        async Task ReadVariableValues()
        {
            await ReadVariableValue(mVariableModelA000);
            await ReadVariableValue(mVariableModelA004);
            await ReadVariableValue(mVariableModelA008);

            await ReadVariableValue(mVariableModelA082);
            await ReadVariableValue(mVariableModelA084);
            await ReadVariableValue(mVariableModelA086);
        }


        /// <summary>
        /// Odczyt wartości jednej zmiennej
        /// </summary>
        async Task ReadVariableValue(VariableModel aVariableModel)
        {
            try
            {
                AsixRestClient asixRestClient = AsixRestClient.Create();
                ICollection<VariableValue> VariableValues = await asixRestClient.GetVariableValueAsync(new string[] { aVariableModel.mName });
                VariableValue variableState = VariableValues.First();

                if (!variableState.ReadSucceeded)
                {
                    aVariableModel.mReadError = variableState.ReadStatusString;
                    return;
                }


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