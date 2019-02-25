using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;


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
    }


    public class Demo1Model : PageModel
    {
        public List<VariableModel> mVariableModelList = new List<VariableModel>();
        public VariableModel mVariableModelA000 = new VariableModel("A000", "Temperatura spalin przed odemglaczem", "°C");
        public VariableModel mVariableModelA004 = new VariableModel("A004", "Temperatura kwasu siarkowego", "°C");
        public VariableModel mVariableModelA008 = new VariableModel("A008", "Temperatura wody ciepłej", "°C");

        public VariableModel mVariableModelA082 = new VariableModel("A082", "Przepływ kwasu siarkowego", "m³/h");
        public VariableModel mVariableModelA084 = new VariableModel("A084", "Poziom w zb. cyrkulacyjnym kwasu", "%");
        public VariableModel mVariableModelA086 = new VariableModel("A086", "Przepływ wody chłodzącej", "m³/h");



        public Demo1Model()
        {
            mVariableModelList.Add(mVariableModelA000);
            mVariableModelList.Add(mVariableModelA004);
            mVariableModelList.Add(mVariableModelA008);
            mVariableModelList.Add(mVariableModelA082);
            mVariableModelList.Add(mVariableModelA084);
            mVariableModelList.Add(mVariableModelA086);
        }



        public void OnGet()
        {
            ReadVariableValues();
        }



        public PartialViewResult OnGetVariableDeck()
        {
            ReadVariableValues();

            return new PartialViewResult {
                ViewName = "_Demo1VariableDeck",
                ViewData = new ViewDataDictionary<VariableModel[]>(ViewData, mVariableModelList.ToArray())
            };
        }



        void ReadVariableValues()
        {
            ReadVariableValue(mVariableModelA000);
            ReadVariableValue(mVariableModelA004);
            ReadVariableValue(mVariableModelA008);

            ReadVariableValue(mVariableModelA082);
            ReadVariableValue(mVariableModelA084);
            ReadVariableValue(mVariableModelA086);
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