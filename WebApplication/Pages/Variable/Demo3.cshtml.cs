using System;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace WebApplication.Pages.Variable
{
    public enum ValueQuality { Good, Uncertain, Bad };
    public enum ValueLimit { LoLo, Lo, Normal, Hi, HiHi };
    public enum ValueTrend { NotAvailable, Up, Down, NoChange };


    public class Demo3VariableModel
    {
        public Demo3VariableModel(string aName, string aDecription, string aUnit, string aDotNetFormat, double aLimitLoLo, double aLimitLo, double aLimitHi, double aLimitHiHi)
        {
            mName = aName;
            mDecription = aDecription;
            mUnit = aUnit;
            mDotNetFormat = aDotNetFormat;

            mLimitLoLo = aLimitLoLo;
            mLimitLo = aLimitLo;
            mLimitHi = aLimitHi;
            mLimitHiHi = aLimitHiHi;
        }


        public string mName, mDecription, mUnit;
        public string mDotNetFormat = "F0";
        public double mLimitLoLo, mLimitLo, mLimitHi, mLimitHiHi;

        public string mReadError;
        public ValueQuality mValueQuality = ValueQuality.Bad;
        public string mValueFormatted = "?";
        public string mAverageValueFormatted;

        public ValueTrend mValueTrend = ValueTrend.NotAvailable;

        public ValueLimit mValueLimit = ValueLimit.Normal;


        public void SetValueQuality(uint aOpcQuality)
        {
            switch (aOpcQuality & 0xC0)
            {
                case 0xC0:
                    mValueQuality = ValueQuality.Good;
                    break;

                case 0x40:
                    mValueQuality = ValueQuality.Uncertain;
                    break;

                default:
                    mValueQuality = ValueQuality.Bad;
                    break;
            }               
        }

        public void SetFormatedValueAndValueLimit(object aValue)
        {
            double currentValue = Convert.ToDouble(aValue);
            mValueFormatted = currentValue.ToString(mDotNetFormat);

            if (currentValue <= mLimitLoLo)
                mValueLimit = ValueLimit.LoLo;
            else if (currentValue <= mLimitLo)
                mValueLimit = ValueLimit.Lo;
            else if (currentValue >= mLimitHiHi)
                mValueLimit = ValueLimit.HiHi;
            else if (currentValue >= mLimitHi)
                mValueLimit = ValueLimit.Hi;
            else
                mValueLimit = ValueLimit.Normal;
        }


        public void SetValueTrend(object aValue, object aAverageValue)
        {
            double currentValue = Convert.ToDouble(aValue);
            double averageValue = Convert.ToDouble(aAverageValue);

            mAverageValueFormatted = averageValue.ToString(mDotNetFormat);

            if (mValueFormatted == mAverageValueFormatted)
                mValueTrend = ValueTrend.NoChange;
            else
            if (currentValue < averageValue)
                mValueTrend = ValueTrend.Down;
            else
                mValueTrend = ValueTrend.Up;

        }
    }



    public class Demo3Model : PageModel
    {
        public Demo3VariableModel mVariableModelA000 = new Demo3VariableModel("A000", "Temperatura spalin przed odemglaczem", "°C", "F0", 2, 10, 190, 200);
        public Demo3VariableModel mVariableModelA004 = new Demo3VariableModel("A004", "Temperatura kwasu siarkowego", "°C", "F0", 2, 10, 190, 200);
        public Demo3VariableModel mVariableModelA008 = new Demo3VariableModel("A008", "Temperatura wody ciepłej", "°C", "F1", 1, 8, 90, 98);

        public Demo3VariableModel mVariableModelA082 = new Demo3VariableModel("A082", "Przepływ kwasu siarkowego", "m³/h", "F0", 2, 10, 190, 200 );
        public Demo3VariableModel mVariableModelA084 = new Demo3VariableModel("A084", "Poziom w zb. cyrkulacyjnym kwasu", "%", "F1", 2, 10, 190, 200);
        public Demo3VariableModel mVariableModelA086 = new Demo3VariableModel("A086", "Przepływ wody chłodzącej", "m³/h", "F0", 2, 10, 270, 290);


        public void OnGet()
        {
            ReadVariableValue(mVariableModelA000);
            ReadVariableValue(mVariableModelA004);
            ReadVariableValue(mVariableModelA008);

            ReadVariableValue(mVariableModelA082);
            ReadVariableValue(mVariableModelA084);
            ReadVariableValue(mVariableModelA086);
        }


        public void testOnGet()
        {
            mVariableModelA000.mValueQuality = ValueQuality.Bad;

            mVariableModelA004.mValueQuality = ValueQuality.Uncertain;
            mVariableModelA004.mValueFormatted = "123,4";
            mVariableModelA004.mValueLimit = ValueLimit.LoLo;
            mVariableModelA004.mValueTrend = ValueTrend.NotAvailable;

            mVariableModelA008.mValueQuality = ValueQuality.Good;
            mVariableModelA008.mValueFormatted = "123";
            mVariableModelA008.mValueLimit = ValueLimit.Lo;
            mVariableModelA008.mValueTrend = ValueTrend.Down;

            mVariableModelA082.mValueQuality = ValueQuality.Good;
            mVariableModelA082.mValueFormatted = "11";
            mVariableModelA082.mValueLimit = ValueLimit.Normal;
            mVariableModelA082.mValueTrend = ValueTrend.Up;

            mVariableModelA084.mValueQuality = ValueQuality.Good;
            mVariableModelA084.mValueFormatted = "12";
            mVariableModelA084.mValueLimit = ValueLimit.Normal;
            mVariableModelA084.mValueTrend = ValueTrend.NoChange;


            mVariableModelA086.mReadError = "Test błędu odczytu";
            mVariableModelA086.mValueQuality = ValueQuality.Good;
            mVariableModelA086.mValueFormatted = "-123";
            mVariableModelA086.mValueLimit = ValueLimit.Normal;
            mVariableModelA086.mValueTrend = ValueTrend.NoChange;
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


                aVariableModel.SetValueQuality(variableState.quality);

                if (aVariableModel.mValueQuality == ValueQuality.Bad)
                    return;

                aVariableModel.SetFormatedValueAndValueLimit(variableState.value);


                VariableState variableAverage = asixRestClient.ReadVariableAggregate(aVariableModel.mName, "Average", "15m");
                if (!variableAverage.readSucceeded || variableAverage.IsQualityBad())
                    return;

                aVariableModel.SetValueTrend(variableState.value, variableAverage.value);
            }
            catch (Exception e)
            {
                aVariableModel.mReadError = e.Message;
            }
        }
    }
}