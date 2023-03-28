using Asix;


namespace WebApplication.Code
{
    /// <summary>
    /// Klasa przechowująca model jednej zmiennej
    /// </summary>

    public class VariableModel
    {
        /// <summary>
        ///  Atrybuty zmiennej
        /// </summary>
        public string Name { get; set; }
        public string Decription { get; set; } = "";
        public string Unit { get; set; } = "";

        /// <summary>
        /// Ewentualny błąd odczytu wartości zmiennej
        /// </summary>
        public string ReadError { get; set; } = "";


        /// <summary>
        /// Sformatowana wartość zmiennej
        /// </summary>
        public string ValueFormatted { get; set; } = "?";

        /// <summary>
        /// Stempel czasu wartości zmiennej
        /// </summary>
        public DateTime DateTime { get; set; }


        public VariableModel(string aName)
        {
            Name = aName;
        }


        public void SetVariableValue(VariableValue aVariableValue)
        {
            try
            {
                if (!aVariableValue.ReadSucceeded)
                {
                    ReadError = aVariableValue.ReadStatusString;
                    return;
                }

                DateTime = aVariableValue.TimeStamp;

                switch (aVariableValue.Quality & 0xC0)
                {
                    case 0xC0:
                        {
                            double value = (double)aVariableValue.Value;
                            ValueFormatted = value.ToString("F0");
                            break;
                        }

                    case 0x40:
                        {
                            double value = (double)aVariableValue.Value;
                            ValueFormatted = value.ToString("F0") + "?";
                            break;
                        }

                    default:
                        {
                            ValueFormatted = "?";
                            break;
                        }
                }
            }
            catch (Exception e)
            {
                ReadError = e.Message;
            }
        }


        internal void SetError(string aReadError)
        {
            ReadError = aReadError;
        }
    }
}