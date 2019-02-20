using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace WebApplication.Pages.VariableDynamic
{
    public class VariableModel
    {
        public string mValueFormatted, mReadError;
    }


    public class Demo1Model : PageModel
    {
        public VariableModel mVariableModelA000;

        public void OnGet()
        {
            mVariableModelA000 = ReadVariableValue();
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



        VariableModel ReadVariableValue()
        {
            VariableModel variableModel = new VariableModel();

            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri("http://asport.askom.com.pl");


                HttpResponseMessage response = httpClient.GetAsync("/asix/v1/variable/value?name=A1101").Result;
                if (!response.IsSuccessStatusCode)
                {
                    variableModel.mReadError = "Błąd http: " + response.StatusCode.ToString();
                    return variableModel;
                }             


                List<VariableState> variableStateList = response.Content.ReadAsAsync<List<VariableState>>().Result;
                VariableState variableState = variableStateList[0];


                if (!variableState.readSucceeded)
                {
                    variableModel.mReadError = variableState.readStatusString;
                    return variableModel;
                }


                switch (variableState.quality & 0xC0)
                {
                    case 0xC0:
                    {
                        double value = (double)variableState.value;
                        variableModel.mValueFormatted = value.ToString("F0");
                        break;
                    }

                    case 0x40:
                    {
                        double value = (double)variableState.value;
                        variableModel.mValueFormatted = value.ToString("F0") + "?";
                        break;
                    }

                    default:
                    {
                        variableModel.mValueFormatted = "?";
                        break;
                    }
                }               

                return variableModel;
            }
            catch (Exception e)
            {
                variableModel.mReadError = e.Message;
                return variableModel;
            }

        }


    }
}