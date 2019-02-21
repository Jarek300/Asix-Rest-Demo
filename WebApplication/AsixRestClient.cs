using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;

namespace WebApplication
{
    public class VariableAttributes
    {
        public string id = "";
        public bool readSucceeded = false;
        public string readStatusString;

        public List<string> mAttributeNameList;
        public List<string> mAttributeValueList;
    };


    public class ServerAttributes
    {
        public bool readSucceeded = false;
        public string readStatusString;

        public List<string> mAttributeNameList;
    };


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
        const string mServerBaseAddress = "http://asport.askom.com.pl";


        public VariableAttributes ReadVariableAttributes(string aVariableName, List<string> aAttributeNameList)
        {
            VariableAttributes variableAttributes = new VariableAttributes();
            variableAttributes.id = aVariableName;
            variableAttributes.mAttributeNameList = aAttributeNameList;


            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(mServerBaseAddress);


            String uri = "/asix/v1/variable/attribute";

            uri = QueryHelpers.AddQueryString(uri, "name", aVariableName);

            foreach (var i in aAttributeNameList)
                uri = QueryHelpers.AddQueryString(uri, "attribute", i);


            HttpResponseMessage response = httpClient.GetAsync(uri).Result;
            if (!response.IsSuccessStatusCode)
            {
                variableAttributes.readSucceeded = false;
                variableAttributes.readStatusString = "Błąd http odczytu atrybutów: " + response.StatusCode.ToString();
                return variableAttributes;
            }             


                
            List<List<string>> content = response.Content.ReadAsAsync<List<List<string>>>().Result;                
            variableAttributes.mAttributeValueList = content[0];
            if (variableAttributes.mAttributeValueList == null)
            {
                variableAttributes.readSucceeded = false;
                variableAttributes.readStatusString = "Błąd odczytu atrybutów : nieznana zmienna";
                return variableAttributes;
            }


            variableAttributes.readSucceeded = true;
            return variableAttributes;
        }





        public ServerAttributes ReadAttributeNames()
        {
            ServerAttributes serverAttributes = new ServerAttributes();

            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(mServerBaseAddress);


                HttpResponseMessage response = httpClient.GetAsync("/asix/v1/server/variable/attribute").Result;
                if (!response.IsSuccessStatusCode)
                {
                    serverAttributes.readSucceeded = false;
                    serverAttributes.readStatusString = "Błąd http odczytu nazw atrybutów: " + response.StatusCode.ToString();
                    return serverAttributes;
                }             

                
                serverAttributes.readSucceeded = true;
                serverAttributes.mAttributeNameList = response.Content.ReadAsAsync<List<string>>().Result;
                return serverAttributes;
            }
            catch (Exception e)
            {
                serverAttributes.readSucceeded = false;
                serverAttributes.readStatusString = "Błąd: " + e.Message;
                return serverAttributes;
            }
            

        }



        public VariableState ReadVariableState(string aVariableName)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(mServerBaseAddress);

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
