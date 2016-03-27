using System;

namespace dotnet.common
{
    public static class MiscParsingExtensions
    {
        /// <summary>
        ///     Parse string to Guid and returns null value if the parsing errors without throwing exceptions
        /// </summary>
        /// <param name="text">String to parse for Guid</param>
        /// <returns>Guid value parsed from the string if parsing error returns null</returns>
        public static Guid? ParseGuid(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;

            try
            {
                Guid result;
                if (!Guid.TryParse(text, out result))
                {
                    return result ==  Guid.Empty ? null: (Guid?)result;
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}