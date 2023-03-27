using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Asix;

namespace WebApplication.Pages.Alarm
{
    public class Demo2Model : PageModel
    {
        // Pole przechowujące początek okresu odczytu alarmów, na stronie dostępny edytor pola
        [BindProperty]
        [DataType(DataType.DateTime)]
        public DateTime DateReadTime { get; set; } = DateTime.Today;


        // Pole przechowujące długość okresu odczytu alarmów, na stronie dostępny edytor pola
        [BindProperty]
        public string PeriodLength { get; set; } = "1:00:00";


        // Pole przechowujące dostępne długości okresu odczytu alarmów. Używane na stronie do inicjalizacji pola wyboru długości okresu
        public List<SelectListItem> Periods { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "1.00:00:00", Text = "1 dzień" },
            new SelectListItem { Value = "1:00:00", Text = "1 godzina" },
            new SelectListItem { Value = "0:15:00", Text = "15 minut" },
            new SelectListItem { Value = "0:5:00", Text = "5 minut" },
            new SelectListItem { Value = "0:1:00", Text = "1 minuta"  },
        };


        public HistoricalAlarmsReadResult mHistAlarmArchive = new();


        // Wywoływane przy ładowaniu strony
        public async Task OnGet()
        {
            await ReadData();
        }


        // Wywoływane po naciśnięciu na stronie przycisku 'Odśwież'
        public async Task OnPost()
        {
            await ReadData();
        }


        // Odczyt z archiwum alarmów
        async Task ReadData()
        {
            AsixRestClient asixRestClient = AsixRestClient.Create();
            mHistAlarmArchive = await asixRestClient.GetAlarmArchiveAsync(
                AsixRestClient.AlarmDomainName, DateReadTime.ToString("o"), TimeSpan.Parse(PeriodLength).ToString(),
                Array.Empty<string>(), Array.Empty<string>(), Array.Empty<string>(), null, null, null, null);
        }
    }
}
