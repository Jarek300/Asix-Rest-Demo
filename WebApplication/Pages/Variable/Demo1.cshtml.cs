using System;
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


        void ReadVariableValue()
        {
            AsixRestClient asixRestClient = new AsixRestClient();
            VariableState variableState = asixRestClient.ReadVariableState("A110");

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