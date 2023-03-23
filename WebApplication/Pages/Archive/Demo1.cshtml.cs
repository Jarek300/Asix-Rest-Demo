using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Asix;


namespace WebApplication.Pages.Archive
{
    /// <summary>
    /// Przykład odczytu danych historycznych agregowanych
    /// </summary>
    public class Demo1Model : PageModel
    {
        // Pole przechowujące początek okresu odczytu danych historycznych, na stronie dostępny edytor pola
        [BindProperty]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd}")]
        public DateTime DateReadTime { get; set; } = DateTime.Today;

        // Pola przechowujące odczytane dane
        public ReadProcessedResult mVariableAggregateArchiveA000 = new();
        public ReadProcessedResult mVariableAggregateArchiveA004 = new();


        // Wywoływane przy ładowaniu strony
        public async Task OnGet()
        {
            await ReadData();
        }



        // Wywoływane po naciśnięciu na stronie przycisku 'Czytaj'
        public async Task OnPost()
        {
            await ReadData();
        }


        // Odczyt danych agregowanych
        async Task ReadData()
        {
            AsixRestClient asixRestClient = AsixRestClient.Create();

            string periodStartOpc = DateReadTime.Date.ToString("o");   // format ISO, też obsługiwany
            mVariableAggregateArchiveA000 = await asixRestClient.GetVariableArchiveProcessedAsync("A000", "Average", periodStartOpc, "1D", "1H", null);
            mVariableAggregateArchiveA004 = await asixRestClient.GetVariableArchiveProcessedAsync("A004", "Average", periodStartOpc, "1D", "1H", null);
        }
    }
}