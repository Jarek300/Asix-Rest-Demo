using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;

namespace WebApplication
{
    public class VariableState
    {
        public string id = "";
        public bool readSucceeded = false;
        public string readStatusString;
        public DateTime timeStamp = DateTime.UtcNow;
        public uint quality = 0;
        public object value = null;
    };


    public class AsixRestClient
    {
        const string mBaseAddress = "http://asport.askom.com.pl";

        public VariableState ReadVariableState(string aVariableName)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(mBaseAddress);

                String uri = "/asix/v1/variable/value";
                uri = QueryHelpers.AddQueryString(uri, "name", aVariableName);


                HttpResponseMessage response = httpClient.GetAsync(uri).Result;
                if (!response.IsSuccessStatusCode)
                {
                    VariableState variableState = new VariableState();
                    variableState.id = aVariableName;
                    variableState.readSucceeded = false;
                    variableState.readStatusString = "Błąd http: " + response.StatusCode.ToString();
                    return variableState;
                }             


                List<VariableState> variableStateList = response.Content.ReadAsAsync<List<VariableState>>().Result;
                return variableStateList[0];
            }
            catch (Exception e)
            {
                VariableState variableState = new VariableState();
                variableState.id = aVariableName;
                variableState.readSucceeded = false;
                variableState.readStatusString = "Błąd: " + e.Message;
                return variableState;
            }
        }
    }
}
