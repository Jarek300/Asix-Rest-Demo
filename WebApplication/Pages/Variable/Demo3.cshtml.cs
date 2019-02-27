using System;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace WebApplication.Pages.Variable
{
    /// <summary>
    /// Jakość zmiennej
    /// </summary>
    public enum ValueQuality { Good, Uncertain, Bad };

    /// <summary>
    /// Informacja o przekroczeniu limitów przez zmienną
    /// </summary>
    public enum ValueLimit { LoLo, Lo, Normal, Hi, HiHi };

    /// <summary>
    /// Trend zmian wartości zmiennej (wyliczany dla okresu ostatnich 15 minut)
    /// </summary>
    public enum ValueTrend { NotAvailable, Up, Down, NoChange };


    /// <summary>
    /// Klasa przechowująca model jednej zmiennej
    /// </summary>
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


        /// <summary>
        ///  Atrybuty zmiennej
        /// </summary>
        public string mName, mDecription, mUnit;

        /// <summary>
        /// Format wartości zmiennej
        /// </summary>
        public string mDotNetFormat = "F0";

        /// <summary>
        /// Wartości limitów zmiennej
        /// </summary>
        public double mLimitLoLo, mLimitLo, mLimitHi, mLimitHiHi;

        /// <summary>
        /// Ewentualny błąd odczytu wartości zmiennej
        /// </summary>
        public string mReadError;

        /// <summary>
        /// Jakość zmiennej
        /// </summary>
        public ValueQuality mValueQuality = ValueQuality.Bad;

        /// <summary>
        /// Sformatowana wartość zmiennej
        /// </summary>
        public string mValueFormatted = "?";

        /// <summary>
        /// Sformatowana wartość średniej zmiennej za ostatnie 15 minut
        /// </summary>
        public string mAverageValueFormatted;

        /// <summary>
        /// Trend zmian wartości zmiennej
        /// </summary>
        public ValueTrend mValueTrend = ValueTrend.NotAvailable;

        /// <summary>
        /// Informacja o przekroczneiu limitów przez wartość zmiennej
        /// </summary>
        public ValueLimit mValueLimit = ValueLimit.Normal;


        /// <summary>
        /// Tworzenie modelu zmiennej - analiza jakośći wartość zmienej
        /// </summary>
        /// <param name="aOpcQuality">Jakość zmiennej</param>
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

        /// <summary>
        /// Tworzenie modelu zmiennej - analiza wartości zmienej i wartości limitów
        /// </summary>
        /// <param name="aValue">Wartośc zmiennej</param>
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


        /// <summary>
        /// Tworzenie modelu zmiennej - analiza wartości wartość zmienej bieżącej i średniej - określenie trendu zmian wartości 
        /// </summary>
        /// <param name="aValue">Wartość bieżąca zmiennej</param>
        /// <param name="aAverageValue">Wartość średnia zmiennej</param>
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



    /// <summary>
    /// Przykład odczytu wartości wielu zmiennych i wypracowania dodatkowych informacji o zmiennej - informacji o przekroczeniu limitów i trendu zmian wartości.
    /// Karta wartości zmiennej wyświetla informacje o przekroczeniu limitów w postaci koloru wartości, a informacje o trendzie zmian wartości w postaci ikony.
    /// </summary>
    public class Demo3Model : PageModel
    {
        public Demo3VariableModel mVariableModelA000 = new Demo3VariableModel("A000", "Temperatura spalin przed odemglaczem", "°C", "F0", 2, 10, 190, 200);
        public Demo3VariableModel mVariableModelA004 = new Demo3VariableModel("A004", "Temperatura kwasu siarkowego", "°C", "F0", 2, 10, 190, 200);
        public Demo3VariableModel mVariableModelA008 = new Demo3VariableModel("A008", "Temperatura wody ciepłej", "°C", "F1", 1, 8, 90, 98);

        public Demo3VariableModel mVariableModelA082 = new Demo3VariableModel("A082", "Przepływ kwasu siarkowego", "m³/h", "F0", 2, 10, 190, 200 );
        public Demo3VariableModel mVariableModelA084 = new Demo3VariableModel("A084", "Poziom w zb. cyrkulacyjnym kwasu", "%", "F1", 2, 10, 190, 200);
        public Demo3VariableModel mVariableModelA086 = new Demo3VariableModel("A086", "Przepływ wody chłodzącej", "m³/h", "F0", 2, 10, 270, 290);


        /// <summary>
        /// Funkcja wywoływana przy generowaniu strony. Czyta wartosci kolejny zmiennych.
        /// </summary>
        public void OnGet()
        {
            ReadVariableValue(mVariableModelA000);
            ReadVariableValue(mVariableModelA004);
            ReadVariableValue(mVariableModelA008);

            ReadVariableValue(mVariableModelA082);
            ReadVariableValue(mVariableModelA084);
            ReadVariableValue(mVariableModelA086);
        }


        /// <summary>
        /// Funkcja używana do testowania poprawności generowania wizualizacji zmiennej w karcie
        /// </summary>
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


        /// <summary>
        /// Odczyt wartości bieżącej i średniej jednej zmiennej
        /// </summary>
        /// <param name="aVariableModel">Model zmiennej. Jego pole mName zawiera nazwę zmiennej.</param>
        void ReadVariableValue(Demo3VariableModel aVariableModel)
        {
            try
            {
                AsixRestClient asixRestClient = new AsixRestClient();

                // Odczyt wartości zmiennej z serwera REST
                VariableState variableState = asixRestClient.ReadVariableState(aVariableModel.mName);
                if (!variableState.readSucceeded)
                {
                    aVariableModel.mReadError = variableState.readStatusString;
                    return;
                }


                // wypracowanie przez klasę modelu informacji o jakości zmiennej
                aVariableModel.SetValueQuality(variableState.quality);

                if (aVariableModel.mValueQuality == ValueQuality.Bad)
                    return;


                // wypracowanie przez klasę modelu informacji o wartości zmiennej i limitach
                aVariableModel.SetFormatedValueAndValueLimit(variableState.value);


                // Odczyt średniej wartości zmiennej z serwera REST
                VariableState variableAverage = asixRestClient.ReadVariableAggregate(aVariableModel.mName, "Average", "15m");
                if (!variableAverage.readSucceeded || variableAverage.IsQualityBad())
                    return;


                // wypracowanie przez klasę modelu informacji o trendzie zmian wartości zmiennej
                aVariableModel.SetValueTrend(variableState.value, variableAverage.value);
            }
            catch (Exception e)
            {
                aVariableModel.mReadError = e.Message;
            }
        }
    }
}