using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;


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
                AsixRestClient asixRestClient = new AsixRestClient();


                ServerAttributes serverAttributes = asixRestClient.ReadAttributeNames();
                if (!serverAttributes.readSucceeded)
                {
                    mReadError = serverAttributes.readStatusString;
                    return;
                }             
               
                mAttributeNames = serverAttributes.mAttributeNameList;                



                VariableAttributes variableAttributes = asixRestClient.ReadVariableAttributes("A000", mAttributeNames);
                if (!variableAttributes.readSucceeded)
                {
                    mReadError = variableAttributes.readStatusString;
                    return;
                }

                mVariableAttributes = variableAttributes.mAttributeValueList;
            }
            catch (Exception e)
            {
                mReadError = e.Message;
            }
        }
    }
}