using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


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
        public VariableAggregateArchive mVariableAggregateArchiveA000;
        public VariableAggregateArchive mVariableAggregateArchiveA004;


        // Wywoływane przy ładowaniu strony
        public void OnGet()
        {
            ReadData();
        }



        // Wywoływane po naciśnięciu na stronie przycisku 'Czytaj'
        public void OnPost()
        {
            ReadData();
        }


        // Odczyt danych agregowanych
        void ReadData()
        {
            AsixRestClient asixRestClient = new AsixRestClient();

            string periodStartOpc = DateReadTime.Date.ToString("o");   // format ISO, też obsługiwany
            mVariableAggregateArchiveA000 = asixRestClient.ReadVariableAggregateArchive("A000", "Average", periodStartOpc, "1D", "1H");
            mVariableAggregateArchiveA004 = asixRestClient.ReadVariableAggregateArchive("A004", "Average", periodStartOpc, "1D", "1H");
        }
    }
}