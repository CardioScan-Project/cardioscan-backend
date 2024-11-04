using System.Globalization;

namespace ChatBot.API.Utils
{
    public static class AuxiliaryMethods
    {
        public static DateTime StringToDatetime(string fechaString)
        {
            string formato = "dd/MM/yyyy";

            try
            {
                return DateTime.ParseExact(fechaString, formato, CultureInfo.InvariantCulture);
            }
            catch (FormatException ex)
            {
                throw new ArgumentException("El formato de fecha proporcionado no es válido. Debe ser dd/mm/yyyy.", ex);
            }
        }
    }
}
