using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Asix;


namespace WebApplication.Pages.Archive
{
    public class Demo2Model : PageModel
    {
        [BindProperty]
        [DataType(DataType.DateTime)]
        public DateTime DateReadTime { get; set; } = DateTime.Today;

        public ReadRawResult mVariableRawArchiveA000 = new();


        public async Task OnGet()
        {
            await ReadData();
        }


        public async Task OnPost()
        {
            await ReadData();
        }


        async Task ReadData()
        {
            AsixRestClient asixRestClient = AsixRestClient.Create();
            string periodStartOpc = DateReadTime.ToString("o");   // format ISO, też obsługiwany przez REST server
            mVariableRawArchiveA000 = await asixRestClient.GetVariableArchiveRawAsync("A000", periodStartOpc, "1H", null, null, null, null);
        }
    }
}