using System;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace WebApplication.Pages.Variable
{
    public enum ValueTrend { NotAvailable, Up, Down, NoChange };

    public class Demo3VariableModel
    {
        public Demo3VariableModel(string aName, string aDecription, string aUnit, string aDotNetFormat)
        {
            mName = aName;
            mDecription = aDecription;
            mUnit = aUnit;
            mDotNetFormat = aDotNetFormat;
        }

        public string mReadError;

        public string mName, mDecription, mUnit;
        public string mDotNetFormat = "F0";
        public string mValueFormatted = "?";

        public string mAverageValueFormatted;
        public ValueTrend mValueTrend = ValueTrend.NotAvailable;
    }


    public class Demo3Model : PageModel
    {
        public Demo3VariableModel mVariableModelA000 = new Demo3VariableModel("A000", "Temperatura spalin przed odemglaczem", "°C", "F0");
        public Demo3VariableModel mVariableModelA004 = new Demo3VariableModel("A004", "Temperatura kwasu siarkowego", "°C", "F0");
        public Demo3VariableModel mVariableModelA008 = new Demo3VariableModel("A008", "Temperatura wody ciepłej", "°C", "F1");

        public Demo3VariableModel mVariableModelA082 = new Demo3VariableModel("A082", "Przepływ kwasu siarkowego", "m³/h", "F0");
        public Demo3VariableModel mVariableModelA084 = new Demo3VariableModel("A084", "Poziom w zb. cyrkulacyjnym kwasu", "%", "F1");
        public Demo3VariableModel mVariableModelA086 = new Demo3VariableModel("A086", "Przepływ wody chłodzącej", "m³/h", "F0");


        public void OnGet()
        {
            ReadVariableValue(mVariableModelA000);
            ReadVariableValue(mVariableModelA004);
            ReadVariableValue(mVariableModelA008);

            ReadVariableValue(mVariableModelA082);
            ReadVariableValue(mVariableModelA084);
            ReadVariableValue(mVariableModelA086);
        }



        void ReadVariableValue(Demo3VariableModel aVariableModel)
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
                    case 0x40:
                    {
                        double value = (double)variableState.value;
                        aVariableModel.mValueFormatted = value.ToString(aVariableModel.mDotNetFormat);
                        break;
                    }

                    default:
                    {
                        aVariableModel.mValueFormatted = "?";
                        return;  // - nie czytamy agregatu
                    }
                }               




                VariableState variableAverage = asixRestClient.ReadVariableAggregate(aVariableModel.mName, "Average", "15m");
                if (!variableAverage.readSucceeded || variableAverage.IsQualityBad())
                    return;

                double currentValue = (double)variableState.value;
                double averageValue = (double)variableAverage.value;
                aVariableModel.mAverageValueFormatted = averageValue.ToString(aVariableModel.mDotNetFormat);

                if (aVariableModel.mValueFormatted == aVariableModel.mAverageValueFormatted)
                    aVariableModel.mValueTrend = ValueTrend.NoChange;
                else
                if (currentValue < averageValue)
                    aVariableModel.mValueTrend = ValueTrend.Down;
                else
                    aVariableModel.mValueTrend = ValueTrend.Up;
            }
            catch (Exception e)
            {
                aVariableModel.mReadError = e.Message;
            }
        }
    }
}