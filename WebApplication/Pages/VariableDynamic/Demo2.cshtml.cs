using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace WebApplication.Pages.VariableDynamic
{
    /// <summary>
    /// Model dla szablonu widoku zmiennej _Demo2Variable.cshtml
    /// </summary>
    public class VariableTemplate
    {
        public string mName;

        public VariableTemplate(string aName)
        {
            mName = aName;
        }
    }


    public class Demo2Model : PageModel
    {
        public Dictionary<string, VariableModel> mVariables = new Dictionary<string, VariableModel>();


        public Demo2Model()
        {
            mVariables["A000"] = new VariableModel("A000", "Temperatura spalin przed odemglaczem", "°C");
            mVariables["A004"] = new VariableModel("A004", "Temperatura kwasu siarkowego", "°C");
            mVariables["A008"] = new VariableModel("A008", "Temperatura wody ciepłej", "°C");

            mVariables["A082"] = new VariableModel("A082", "Przepływ kwasu siarkowego", "m³/h");
            mVariables["A084"] = new VariableModel("A084", "Poziom w zb. cyrkulacyjnym kwasu", "%");
            mVariables["A086"] = new VariableModel("A086", "Przepływ wody chłodzącej", "m³/h");

            mVariables["Nieznana"] = new VariableModel("Nieznana", "Nieznana zmienna", "");
        }

        public void OnGet()
        {

        }


        /// <summary>
        /// Odczyt wartości bieżącej zmiennych
        /// </summary>
        /// <param name="name">Nazwa zmiennej, może wystapić wielokrotnie</param>
        /// <returns></returns>
        public JsonResult OnGetVariables(string[] name)
        {
            if (name == null)
                return null;

            List<VariableModel> variables = new List<VariableModel>();
            foreach (string variableName in name)
            {
                VariableModel variableModel = null;
                if (!mVariables.ContainsKey(variableName))
                    variableModel = new VariableModel(variableName, "", "");
                else
                    variableModel = mVariables[variableName];

                ReadVariableValue(variableModel);
                variables.Add(variableModel);
            }

            return new JsonResult(variables.ToArray());
        }


        void ReadVariableValue(VariableModel aVariableModel)
        {
            try
            {
                AsixRestClient asixRestClient = new AsixRestClient();
                VariableState variableState = asixRestClient.ReadVariableState(aVariableModel.mName);

                if (!variableState.readSucceeded)
                {
                    aVariableModel.mReadError = variableState.readStatusString;
                    return;
                }


                aVariableModel.mDateTime = variableState.timeStamp;

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