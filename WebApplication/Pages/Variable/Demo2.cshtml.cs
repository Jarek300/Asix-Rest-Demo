using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace WebApplication.Pages.Variable
{
    public class VariableModel
    {
        public VariableModel(string aName, string aDecription, string aUnit)
        {
            mName = aName;
            mDecription = aDecription;
            mUnit = aUnit;
        }

        public string mName, mDecription, mUnit;
        public string mValueFormatted = "?", mReadError;
    }


    public class Demo2Model : PageModel
    {
        public VariableModel mVariableModelA000 = new VariableModel("A000", "Temperatura spalin przed odemglaczem", "°C");
        public VariableModel mVariableModelA004 = new VariableModel("A004", "Temperatura kwasu siarkowego", "°C");
        public VariableModel mVariableModelA008 = new VariableModel("A008", "Temperatura wody ciepłej", "°C");

        public VariableModel mVariableModelA082 = new VariableModel("A082", "Przepływ kwasu siarkowego", "m3/h");
        public VariableModel mVariableModelA084 = new VariableModel("A084", "Poziom w zb. cyrkulacyjnym kwasu", "%");
        public VariableModel mVariableModelA086 = new VariableModel("A086", "Przepływ wody chłodzącej", "m3/h");


        public void OnGet()
        {
            ReadVariableValue(mVariableModelA000);
            ReadVariableValue(mVariableModelA004);
            ReadVariableValue(mVariableModelA008);

            ReadVariableValue(mVariableModelA082);
            ReadVariableValue(mVariableModelA084);
            ReadVariableValue(mVariableModelA086);
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



        void ReadVariableValue(VariableModel aVariableModel)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri("http://asport.askom.com.pl");

                String uri = "/asix/v1/variable/value";
                uri = QueryHelpers.AddQueryString(uri, "name", aVariableModel.mName);


                HttpResponseMessage response = httpClient.GetAsync(uri).Result;
                if (!response.IsSuccessStatusCode)
                {
                    aVariableModel.mReadError = "Błąd http: " + response.StatusCode.ToString();
                    return;
                }             


                List<VariableState> variableStateList = response.Content.ReadAsAsync<List<VariableState>>().Result;
                VariableState variableState = variableStateList[0];


                if (!variableState.readSucceeded)
                {
                    aVariableModel.mReadError = variableState.readStatusString;
                    return;
                }


                switch (variableState.quality & 0xC0)
                {
                    case 0xC0:
                    {
                        double value = (double)variableState.value;
                        aVariableModel.mValueFormatted = value.ToString("F0");
                        break;
                    }

                    case 0x40:
                    {
                        double value = (double)variableState.value;
                        aVariableModel.mValueFormatted = value.ToString("F0") + "?";
                        break;
                    }

                    default:
                    {
                        aVariableModel.mValueFormatted = "?";
                        break;
                    }
                }               
            }
            catch (Exception e)
            {
                aVariableModel.mReadError = e.Message;
            }
        }
    }
}