using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace WebApplication.Pages.Archive
{
    public class Demo1Model : PageModel
    {
        [BindProperty]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd}")]
        public DateTime DateReadTime { get; set; } = DateTime.Today;

        public VariableAggregateArchive mVariableAggregateArchiveA000;
        public VariableAggregateArchive mVariableAggregateArchiveA004;


        public void OnGet()
        {
            ReadData();
        }



        public void OnPost()
        {
            ReadData();
        }



        void ReadData()
        {
            AsixRestClient asixRestClient = new AsixRestClient();

            string periodStartOpc = DateReadTime.Date.ToString("o");   // format ISO, też obsługiwany
            mVariableAggregateArchiveA000 = asixRestClient.ReadVariableAggregateArchive("A000", "Average", periodStartOpc, "1D", "1H");
            mVariableAggregateArchiveA004 = asixRestClient.ReadVariableAggregateArchive("A004", "Average", periodStartOpc, "1D", "1H");
        }

    }
}