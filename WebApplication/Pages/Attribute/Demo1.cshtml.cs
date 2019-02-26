using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace WebApplication.Pages.Attribute
{
    /// <summary>
    /// Przykład odczytu atrybutów jednej zmiennej
    /// </summary>
    public class Demo1Model : PageModel
    {
        /// <summary>
        /// Nazwa zmiennej, której atrybuty są czytane
        /// </summary>
        public string mVariableName = "A000";

        /// <summary>
        /// Lista nazw atrybutów, które są czytane
        /// </summary>
        List<string> mAttributeNameList = new List<string> { "Name", "Description", "Unit", "Szafa", "Listwa" };

        /// <summary>
        /// Opis ewentualnego błędu odczytu
        /// </summary>
        public string mReadError;

        /// <summary>
        /// Przeczytane wartości atrybutów zmiennej
        /// </summary>
        public List<string> mVariableAttributes;


        /// <summary>
        /// Funkcja wywoływana przy generowaniu strony
        /// </summary>
        public void OnGet()
        {
            try
            {
                AsixRestClient asixRestClient = new AsixRestClient();
                VariableAttributes variableAttributes = asixRestClient.ReadVariableAttributes(mVariableName, mAttributeNameList);

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