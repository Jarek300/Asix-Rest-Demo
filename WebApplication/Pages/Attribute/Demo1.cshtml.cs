using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Pages.Attribute
{
    public class Demo1Model : PageModel
    {
        public string mReadError;
        public List<string> mVariableAttributes;

        public void OnGet()
        {
            try
            {
                ReadVariableAttributes();
            }
            catch (Exception e)
            {
                mReadError = e.Message;
            }
        }


        void ReadVariableAttributes()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://asport.askom.com.pl");


            HttpResponseMessage response = httpClient.GetAsync("/asix/v1/variable/attribute?name=A000&attribute=Name&attribute=Description&attribute=Unit&attribute=Szafa&attribute=Listwa").Result;
            if (!response.IsSuccessStatusCode)
            {
                mReadError = "Błąd http odczytu atrybutów: " + response.StatusCode.ToString();
                return;
            }             


                
            List<List<string>> content = response.Content.ReadAsAsync<List<List<string>>>().Result;                
            mVariableAttributes = content[0];
            if (mVariableAttributes == null)
            {
                mReadError = "Błąd odczytu atrybutów : nieznana zmienna";
                return;
            }
        }

    }
}