using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace WebApplication.Pages.Archive
{
    public class Demo2Model : PageModel
    {
        [BindProperty]
        [DataType(DataType.DateTime)]
        public DateTime DateReadTime { get; set; } = DateTime.Today;

        public VariableRawArchive mVariableRawArchiveA000;


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
            string periodStartOpc = DateReadTime.ToString("o");   // format ISO, też obsługiwany przez REST server
            mVariableRawArchiveA000 = asixRestClient.ReadVariableRawArchive("A000", periodStartOpc, "1H");
        }
    }
}