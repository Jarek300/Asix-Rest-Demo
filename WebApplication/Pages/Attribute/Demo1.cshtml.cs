using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace WebApplication.Pages.Attribute
{
    public class Demo1Model : PageModel
    {
        List<string> mAttributeNameList = new List<string> { "Name", "Description", "Unit", "Szafa", "Listwa" };

        public string mReadError;
        public List<string> mVariableAttributes;

        public void OnGet()
        {
            try
            {
                AsixRestClient asixRestClient = new AsixRestClient();
                VariableAttributes variableAttributes = asixRestClient.ReadVariableAttributes("A000", mAttributeNameList);

                if (variableAttributes.readSucceeded)
                {
                    mVariableAttributes = variableAttributes.mAttributeValueList;
                }
                else
                {
                    mReadError = variableAttributes.readStatusString;
                }
            }
            catch (Exception e)
            {
                mReadError = e.Message;
            }
        }

    }
}