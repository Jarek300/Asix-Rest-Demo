using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.WebUtilities;

namespace WebApplication.Pages.VariableDynamic
{
    public class VariableModel
    {
        public VariableModel(string aName, string aDecription, string aUnit)
        {
            mName = aName;
            mDecription = aDecription;
            mUnit = aUnit;
        }

        public string mName, mDecription, mUnit;
        public string mValueFormatted = "?", mReadError;
        public DateTime mDateTime;

        public bool mAjax = false;
    }


    public class Demo1Model : PageModel
    {
        public Dictionary<string, VariableModel> mVariableModelDict = new Dictionary<string, VariableModel>();
        public VariableModel mVariableModelA000 = new VariableModel("A000", "Temperatura spalin przed odemglaczem", "°C");
        public VariableModel mVariableModelA004 = new VariableModel("A004", "Temperatura kwasu siarkowego", "°C");
        public VariableModel mVariableModelA008 = new VariableModel("A008", "Temperatura wody ciepłej", "°C");

        public VariableModel mVariableModelA082 = new VariableModel("A082", "Przepływ kwasu siarkowego", "m³/h");
        public VariableModel mVariableModelA084 = new VariableModel("A084", "Poziom w zb. cyrkulacyjnym kwasu", "%");
        public VariableModel mVariableModelA086 = new VariableModel("A086", "Przepływ wody chłodzącej", "m³/h");

        public Demo1Model()
        {
            mVariableModelDict.Add(mVariableModelA000.mName, mVariableModelA000);
            mVariableModelDict.Add(mVariableModelA004.mName, mVariableModelA004);
            mVariableModelDict.Add(mVariableModelA008.mName, mVariableModelA008);
            mVariableModelDict.Add(mVariableModelA082.mName, mVariableModelA082);
            mVariableModelDict.Add(mVariableModelA084.mName, mVariableModelA084);
            mVariableModelDict.Add(mVariableModelA086.mName, mVariableModelA086);
        }

        public void OnGet()
        {
            ReadVariableValue(mVariableModelA000);
            ReadVariableValue(mVariableModelA004);
            ReadVariableValue(mVariableModelA008);

            ReadVariableValue(mVariableModelA082);
            ReadVariableValue(mVariableModelA084);
            ReadVariableValue(mVariableModelA086);
        }



        public PartialViewResult OnGetVariableCard(string name)
        {
            mVariableModelDict.TryGetValue(name, out VariableModel variableModel);
            if (variableModel != null)
            {
                ReadVariableValue(variableModel);
            }
            else
            {
                variableModel = new VariableModel(name, "", "");
                variableModel.mReadError = "Nieobsługiwana zmienna";
            }

            //return Partial("_Demo1Variable", mVariableModelA000);

            variableModel.mAjax = true;

            return new PartialViewResult {
                ViewName = "_Demo1Variable",
                ViewData = new ViewDataDictionary<VariableModel>(ViewData, variableModel)
            };
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