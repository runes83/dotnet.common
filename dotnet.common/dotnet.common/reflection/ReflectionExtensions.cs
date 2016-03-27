using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using dotnet.common.strings;

namespace dotnet.common.reflection
{
    public static class ReflectionExtensions
    {
        /// <summary>
        ///     Sets property to object by use of reflection
        /// </summary>
        /// <param name="objectValue">The object to set property value on</param>
        /// <param name="propertyName">The name of the property to set value on</param>
        /// <param name="value">Value to set</param>
        /// <param name="throwExceptionIfPropertyNotExist">Throw exception if property does not exist or the value is mismatch</param>
        public static void SetByName(this object objectValue, string propertyName, dynamic value,
            bool throwExceptionIfPropertyNotExist = false)
        {
            try
            {
                var property = objectValue.GetType().GetProperty(propertyName);
                if (property != null)
                    property.SetValue(objectValue, value, null);
                else
                {
                    if (throwExceptionIfPropertyNotExist)
                        throw new Exception("Property does not exist with name: {0}".FormatWith(propertyName));
                }
            }
            catch (Exception e)
            {
                if (throwExceptionIfPropertyNotExist)
                    throw e;
            }
        }

        /// <summary>
        ///     Gets property value from  object by use of reflection
        /// </summary>
        /// <param name="objectValue">The object to set property value on</param>
        /// <param name="propertyName">The name of the property to set value on</param>
        /// <param name="throwExceptionIfPropertyNotExist">Throw exception if property does not exist</param>
        /// <returns>Value of the property or null if exception (if throwExceptionIfPropertyNotExist is not set to true)</returns>
        public static dynamic GetByName(this object objectValue, string propertyName,
            bool throwExceptionIfPropertyNotExist = false)
        {
            try
            {
                var property = objectValue.GetType().GetProperty(propertyName);
                if (property != null)
                    return property.GetValue(objectValue, null);
                if (throwExceptionIfPropertyNotExist)
                    throw new Exception("Property does not exist with name: {0}".FormatWith(propertyName));
            }
            catch (Exception e)
            {
                if (throwExceptionIfPropertyNotExist)
                    throw e;
            }

            return null;
        }

        /// <summary>
        /// List all the properties of the given object as a Dictionary with name and value
        /// </summary>
        /// <param name="objectValue"></param>
        /// <returns>A dicitionart with name value of all the properties of the give object</returns>
        public static Dictionary<string, dynamic> ListProperties(this object objectValue)
        {
            return objectValue
                .GetType()
                .GetProperties()
                .ToDictionary<PropertyInfo, string, dynamic>(
                        property => property.Name, property => 
                        property.GetValue(objectValue, null)
                );
        }
    }
}