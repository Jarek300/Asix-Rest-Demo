using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Pages
{
    public class StaticDataModel : PageModel
    {
        private readonly IHttpClientFactory mClientFactory;

        Random mRandom = new Random();

        public List<List<string>> mVariableAttributeList = new List<List<string>>();
        public List<VariableState> mVariableStateList;
        public string mVariableA110;
        public string mVariableA110Status;

        public string mVariableA098;
        public string mVariableA098Status;

        public StaticDataModel(IHttpClientFactory aClientFactory)
        {
            mClientFactory = aClientFactory;
        }

        async public Task OnGet()
        {
            //SimulateVariables();
            VariableAttributes variableAttributes = await ReadVariableAttributes("A110");
            VariableState variableState = await ReadVariableState("A110");
        }

        void SimulateVariables()
        {
            double d = mRandom.NextDouble() * 600;
            mVariableA110 = d.ToString("F0");


            d = mRandom.NextDouble() * 25;
            mVariableA098 = d.ToString("F1");
        }


        public class VariableAttributes
        {
            public string name, description, unit;
        }

        async Task<VariableAttributes> ReadVariableAttributes(string aVariableName)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, 
                $"http://asport.askom.com.pl/asix/v1/variable/attribute?name={aVariableName}&attribute=Name&attribute=Description&attribute=Unit");

            var client = mClientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                VariableAttributes variableAttributes = new VariableAttributes();
                List<List<string>> content = await response.Content.ReadAsAsync<List<List<string>>>();
                
                List<string> attributes = content[0];
                variableAttributes.name = attributes[0];
                variableAttributes.description = attributes[1];
                variableAttributes.unit = attributes[2];
                return variableAttributes;
            }
            else
            {
                return null;
            }             
        }
            


        public class VariableState
        {
            public string id;
            public bool readSucceeded;
            public string readStatusString;
            public DateTime timeStamp;
            public uint quality;
            public object value;
        };


        async Task<VariableState> ReadVariableState(string aVariableName)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"http://asport.askom.com.pl/asix/v1/variable/value?name={aVariableName}");
            var client = mClientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                IEnumerable<VariableState> data = await response.Content.ReadAsAsync<IEnumerable<VariableState>>();
                mVariableStateList = data.ToList();

                return mVariableStateList[0];
                //if (variableState.readSucceeded)
                //{
                //    double d = (double)variableState.value;
                //    mVariableA110 = d.ToString("F0");
                //    mVariableA110Status = "";
                //}
                //else
                //{
                //    mVariableA110 = "?";
                //    mVariableA110Status = variableState.readStatusString;
                //}
            }
            else
            {
                return null;
                //mVariableA110Status = "Błąd odczytu nr " + response.StatusCode.ToString();
            }             

        }
    }
}