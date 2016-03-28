using System;
using System.Linq;
using System.Security;
using dotnet.common.strings;

namespace dotnet.common.misc
{
    public static class ConfigManagerExtensions
    {
        public static T GetValue<T>(this System.Collections.Specialized.NameValueCollection nameValuePairs, string configKey, T defaultValue) where T : IConvertible
        {
            T retValue = default(T);

            if (string.IsNullOrWhiteSpace(configKey))
                return retValue;

            if (nameValuePairs != null && nameValuePairs.AllKeys.Contains(configKey))
            {
                string tmpValue = nameValuePairs[configKey];
                if (string.IsNullOrWhiteSpace(tmpValue))
                    return retValue;

                retValue = (T)Convert.ChangeType(tmpValue, typeof(T));
            }
            else
            {
                return defaultValue;
            }

            return retValue;
        }

        public static SecureString GetValueASecureString(this System.Collections.Specialized.NameValueCollection nameValuePairs, string configKey)
        {
            if (nameValuePairs!=null && nameValuePairs.AllKeys.Contains(configKey))
            {
                return nameValuePairs[configKey].ToSecureString();
            }
            return null;
        }
    }
}