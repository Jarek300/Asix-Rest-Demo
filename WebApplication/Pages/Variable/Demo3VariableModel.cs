using Asix;

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
        /// <summary>
        ///  Atrybuty zmiennej
        /// </summary>
        public string Name, Decription, Unit;

        /// <summary>
        /// Format wartości zmiennej
        /// </summary>
        public string DotNetFormat = "F0";

        /// <summary>
        /// Wartości limitów zmiennej
        /// </summary>
        public double LimitLoLo, LimitLo, LimitHi, LimitHiHi;

        /// <summary>
        /// Ewentualny błąd odczytu wartości zmiennej
        /// </summary>
        public string ReadError = "";

        /// <summary>
        /// Jakość zmiennej
        /// </summary>
        public ValueQuality ValueQuality = ValueQuality.Bad;


        /// <summary>
        /// Wartość zmiennej
        /// </summary>
        public double Value = 0;

        /// <summary>
        /// Sformatowana wartość zmiennej
        /// </summary>
        public string ValueFormatted = "?";

        /// <summary>
        /// Sformatowana wartość średniej zmiennej za ostatnie 15 minut
        /// </summary>
        public string AverageValueFormatted = "";

        /// <summary>
        /// Trend zmian wartości zmiennej
        /// </summary>
        public ValueTrend ValueTrend = ValueTrend.NotAvailable;

        /// <summary>
        /// Informacja o przekroczneiu limitów przez wartość zmiennej
        /// </summary>
        public ValueLimit ValueLimit = ValueLimit.Normal;



        public Demo3VariableModel(string aName, string aDecription, string aUnit, string aDotNetFormat, double aLimitLoLo, double aLimitLo, double aLimitHi, double aLimitHiHi)
        {
            Name = aName;
            Decription = aDecription;
            Unit = aUnit;
            DotNetFormat = aDotNetFormat;

            LimitLoLo = aLimitLoLo;
            LimitLo = aLimitLo;
            LimitHi = aLimitHi;
            LimitHiHi = aLimitHiHi;
        }


        /// <summary>
        /// Tworzenie modelu zmiennej
        /// </summary>
        public bool SetValue(VariableValue aVariableValue)
        {
            if (!aVariableValue.ReadSucceeded)
            {
                ReadError = aVariableValue.ReadStatusString;
                return false;
            }


            SetValueQuality(aVariableValue.Quality);

            if (ValueQuality == ValueQuality.Bad)
                return false;

            SetFormatedValueAndValueLimit(aVariableValue.Value);
            return true;
        }


        /// <summary>
        /// Tworzenie modelu zmiennej - analiza jakośći wartość zmienej
        /// </summary>
        /// <param name="aOpcQuality">Jakość zmiennej</param>
        void SetValueQuality(int aOpcQuality)
        {
            switch (aOpcQuality & 0xC0)
            {
                case 0xC0:
                    ValueQuality = ValueQuality.Good;
                    break;

                case 0x40:
                    ValueQuality = ValueQuality.Uncertain;
                    break;

                default:
                    ValueQuality = ValueQuality.Bad;
                    break;
            }
        }


        /// <summary>
        /// Tworzenie modelu zmiennej - analiza wartości zmienej i wartości limitów
        /// </summary>
        /// <param name="aValue">Wartośc zmiennej</param>
        void SetFormatedValueAndValueLimit(object aValue)
        {
            Value = Convert.ToDouble(aValue);
            ValueFormatted = Value.ToString(DotNetFormat);

            if (Value <= LimitLoLo)
                ValueLimit = ValueLimit.LoLo;
            else if (Value <= LimitLo)
                ValueLimit = ValueLimit.Lo;
            else if (Value >= LimitHiHi)
                ValueLimit = ValueLimit.HiHi;
            else if (Value >= LimitHi)
                ValueLimit = ValueLimit.Hi;
            else
                ValueLimit = ValueLimit.Normal;
        }


        /// <summary>
        /// Tworzenie modelu zmiennej - analiza wartości wartość zmienej bieżącej i średniej - określenie trendu zmian wartości
        /// </summary>
        /// <param name="aValue">Wartość bieżąca zmiennej</param>
        /// <param name="aAverageValue">Wartość średnia zmiennej</param>
        public void SetValueTrend(VariableValue aVariableAverage)
        {
            if (!aVariableAverage.ReadSucceeded || AsixRestClient.IsQualityBad(aVariableAverage.Quality))
            {
                ValueTrend = ValueTrend.NotAvailable;
                return;
            }

            double averageValue = Convert.ToDouble(aVariableAverage.Value);

            AverageValueFormatted = averageValue.ToString(DotNetFormat);

            if (ValueFormatted == AverageValueFormatted)
                ValueTrend = ValueTrend.NoChange;
            else
            if (Value < averageValue)
                ValueTrend = ValueTrend.Down;
            else
                ValueTrend = ValueTrend.Up;
        }


        internal void SetError(string aErrorMessage)
        {
            ReadError = aErrorMessage;
        }
    }
}