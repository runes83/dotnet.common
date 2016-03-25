namespace dotnet.common.numbers
{
    public static class NumberExtensions
    {
        public static int ParseInt(this string text,int defaultValue=0)
        {
            int result = defaultValue;
            if(!int.TryParse(text,out result))
                return defaultValue;

            return result;
        }

        public static double ParseDouble(this string text, double defaultValue = 0)
        {
            double result = defaultValue;
            if (!double.TryParse(text, out result))
                return defaultValue;

            return result;
        }

        public static short ParseShort(this string text, short defaultValue = 0)
        {
            short result = defaultValue;
            if (!short.TryParse(text, out result))
                return defaultValue;

            return result;
        }
    }
}
