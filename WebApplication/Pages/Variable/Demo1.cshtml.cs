using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace WebApplication.Pages.Variable
{
    public class Demo1Model : PageModel
    {
        public string mValueFormatted, mReadError;


        public void OnGet()
        {
            try
            {
                ReadVariableValue();
            }
            catch (Exception e)
            {
                mReadError = e.Message;
            }
        }


        class VariableState
        {
            public string id;
            public bool readSucceeded;
            public string readStatusString;
            public DateTime timeStamp;
            public uint quality;
            public object value;
        };



        void ReadVariableValue()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://asport.askom.com.pl");


            HttpResponseMessage response = httpClient.GetAsync("/asix/v1/variable/value?name=A110").Result;
            if (!response.IsSuccessStatusCode)
            {
                mReadError = "Błąd http: " + response.StatusCode.ToString();
                return;
            }             


            List<VariableState> variableStateList = response.Content.ReadAsAsync<List<VariableState>>().Result;
            VariableState variableState = variableStateList[0];


            if (!variableState.readSucceeded)
            {
                mReadError = variableState.readStatusString;
                return;
            }


            switch (variableState.quality & 0xC0)
            {
                case 0xC0:
                {
                    double value = (double)variableState.value;
                    mValueFormatted = value.ToString("F0");
                    break;
                }

                case 0x40:
                {
                    double value = (double)variableState.value;
                    mValueFormatted = value.ToString("F0") + "?";
                    break;
                }

                default:
                {
                    mValueFormatted = "?";
                    break;
                }
            }               
        }
    }
}