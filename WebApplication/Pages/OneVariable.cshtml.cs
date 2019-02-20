using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Pages
{
    public class OneVariableModel : PageModel
    {
        private readonly IHttpClientFactory mClientFactory;

        public string mGeneralReadError;

        public string mDescription, mUnit, mFormat;
        public string mValueFormatted, mVariableReadError;
        public VariableState mVariableState;


        public OneVariableModel(IHttpClientFactory aClientFactory)
        {
            mClientFactory = aClientFactory;
        }


        async public Task OnGet()
        {
            try
            {
                if (!await ReadVariableAttributes())
                    return;

                await ReadVariableState();
            }
            catch (Exception e)
            {
                mGeneralReadError = e.Message;
            }
        }


        async Task<bool> ReadVariableAttributes()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, 
                $"http://asport.askom.com.pl/asix/v1/variable/attribute?name=A000&attribute=Name&attribute=Description&attribute=Unit&attribute=Format");

            var client = mClientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                mGeneralReadError = "Błąd odczytu atrybutów nr: " + response.StatusCode.ToString();
                return false;
            }             

                
            List<List<string>> content = await response.Content.ReadAsAsync<List<List<string>>>();
                
            List<string> attributes = content[0];
            if (attributes == null)
            {
                mGeneralReadError = "Błąd odczytu atrybutów : nieznana zmienna";
                return false;
            }

            mDescription = attributes[1];
            mUnit = attributes[2];
            mFormat = attributes[3];
            return true;
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


        async Task ReadVariableState()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"http://asport.askom.com.pl/asix/v1/variable/value?name=A0001");

            var client = mClientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                mGeneralReadError = "Błąd odczytu wartości zmienej nr " + response.StatusCode.ToString();
            }             

            List<VariableState> content = await response.Content.ReadAsAsync<List<VariableState>>();
            mVariableState = content[0];

            if (!mVariableState.readSucceeded)
            {
                mValueFormatted = $"Błąd odczytu: {mVariableState.readStatusString}";
                return;
            }

            switch (mVariableState.quality & 0xC0)
            {
                case 0xC0:
                {
                    double d = (double)mVariableState.value;
                    mValueFormatted = d.ToString(mFormat);
                    break;
                }

                case 0x40:
                {
                    double d = (double)mVariableState.value;
                    mValueFormatted = d.ToString(mFormat) + "?";
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