using System;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace WebApplication.Pages.Variable
{
    public class Demo2VariableModel
    {
        public Demo2VariableModel(string aName, string aDecription, string aUnit)
        {
            mName = aName;
            mDecription = aDecription;
            mUnit = aUnit;
        }

        public string mName, mDecription, mUnit;
        public string mValueFormatted = "?", mReadError;
    }


    public class Demo2Model : PageModel
    {
        public Demo2VariableModel mVariableModelA000 = new Demo2VariableModel("A000", "Temperatura spalin przed odemglaczem", "°C");
        public Demo2VariableModel mVariableModelA004 = new Demo2VariableModel("A004", "Temperatura kwasu siarkowego", "°C");
        public Demo2VariableModel mVariableModelA008 = new Demo2VariableModel("A008", "Temperatura wody ciepłej", "°C");

        public Demo2VariableModel mVariableModelA082 = new Demo2VariableModel("A082", "Przepływ kwasu siarkowego", "m³/h");
        public Demo2VariableModel mVariableModelA084 = new Demo2VariableModel("A084", "Poziom w zb. cyrkulacyjnym kwasu", "%");
        public Demo2VariableModel mVariableModelA086 = new Demo2VariableModel("A086", "Przepływ wody chłodzącej", "m³/h");


        public void OnGet()
        {
            ReadVariableValue(mVariableModelA000);
            ReadVariableValue(mVariableModelA004);
            ReadVariableValue(mVariableModelA008);

            ReadVariableValue(mVariableModelA082);
            ReadVariableValue(mVariableModelA084);
            ReadVariableValue(mVariableModelA086);
        }



        void ReadVariableValue(Demo2VariableModel aVariableModel)
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