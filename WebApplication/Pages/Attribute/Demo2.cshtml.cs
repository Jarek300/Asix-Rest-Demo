using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace WebApplication.Pages.Attribute
{
    public class Demo2Model : PageModel
    {
        public string mReadError;
        public List<string> mAttributeNames;
        public List<string> mVariableAttributes;

        public void OnGet()
        {
            try
            {
                ReadAttributeNames();

                if (string.IsNullOrEmpty(mReadError))
                    ReadVariableAttributes();
            }
            catch (Exception e)
            {
                mReadError = e.Message;
            }
        }



        void ReadAttributeNames()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://asport.askom.com.pl");


            HttpResponseMessage response = httpClient.GetAsync("/asix/v1/server/variable/attribute").Result;
            if (!response.IsSuccessStatusCode)
            {
                mReadError = "Błąd http odczytu nazw atrybutów: " + response.StatusCode.ToString();
                return;
            }             

                
            mAttributeNames = response.Content.ReadAsAsync<List<string>>().Result;                
        }



        void ReadVariableAttributes()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://asport.askom.com.pl");


            String uri = "/asix/v1/variable/attribute";

            uri = QueryHelpers.AddQueryString(uri, "name", "A000");

            foreach (var i in mAttributeNames)
                uri = QueryHelpers.AddQueryString(uri, "attribute", i);

            HttpResponseMessage response = httpClient.GetAsync(uri).Result;
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