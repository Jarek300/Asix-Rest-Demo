using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace WebApplication.Pages.Attribute
{
    public class Demo2Model : PageModel
    {
        /// <summary>
        /// Nazwa zmiennej, której atrybuty są czytane
        /// </summary>
        public string mVariableName = "A000";

        /// <summary>
        /// Opis ewentualnego błędu odczytu
        /// </summary>
        public string mReadError;

        /// <summary>
        /// Lista nazw wszystkich atrybutów w bazie definicji zmiennych
        /// </summary>
        public List<string> mAttributeNames;

        /// <summary>
        /// Lista wartości atrybutów
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

                // Odczyt z serwera REST nazw wszystkich atrybutów
                ServerAttributes serverAttributes = asixRestClient.ReadAttributeNames();
                if (!serverAttributes.readSucceeded)
                {
                    mReadError = serverAttributes.readStatusString;
                    return;
                }             
               
                mAttributeNames = serverAttributes.mAttributeNameList;                



                // Odczyt z serwera REST wartości atrybutów atrybutów zmiennej
                VariableAttributes variableAttributes = asixRestClient.ReadVariableAttributes(mVariableName, mAttributeNames);
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