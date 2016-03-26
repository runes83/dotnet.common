using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace dotnet.common.serialization
{
    public static class SerializationExtensions
    {
        /// <summary>
        ///     Serialize object to XML and returns the result as bytes
        /// </summary>
        /// <param name="value">object to be serialized</param>
        /// <returns>Byte array of XML</returns>
        public static byte[] SerializeToXMLBytes(this object value)
        {
            using (var ms = new MemoryStream())
            {
                new XmlSerializer(value.GetType()).Serialize(ms, value);
                return ms.ToArray();
            }
        }

        /// <summary>
        ///     Serialize object to XML and returns the result as string (UTF-8)
        /// </summary>
        /// <param name="value">object to be serialized</param>
        /// <returns>String of XML</returns>
        public static string SerializeToXMLString(this object value)
        {
            using (var ms = new MemoryStream())
            {
                new XmlSerializer(value.GetType()).Serialize(ms, value);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        public static byte[] SerializeToCSVString<T>(this IEnumerable<T> list)
        {
            //using (var ms = new MemoryStream())
            //{
            //    return new CsvExport<T>(list.ToList()).ExportToBytes();
            //}
            return null;
        }
    }

    public class CsvExport<T> where T : class
    {
        public List<T> Objects;

        public CsvExport(List<T> objects)
        {
            Objects = objects;
        }

        public string Export()
        {
            return Export(true);
        }

        public Stream ExportStream(bool includeHeaderLine)
        {
            var ms = new MemoryStream();
            using (var streamWriter = new StreamWriter(ms, Encoding.UTF8))
            {
                streamWriter.Write(Export(includeHeaderLine));
                streamWriter.Flush();
                streamWriter.Close();
            }
            return ms;
        }

        public string Export(bool includeHeaderLine)
        {
            var sb = new StringBuilder();
            //Get properties using reflection.
            IList<PropertyInfo> propertyInfos = typeof (T).GetProperties();

            if (includeHeaderLine)
            {
                //add header line.
                foreach (var propertyInfo in propertyInfos)
                {
                    sb.Append(propertyInfo.Name).Append(";");
                }
                sb.Remove(sb.Length - 1, 1).AppendLine();
            }

            //add value for each property.
            foreach (var obj in Objects)
            {
                foreach (var propertyInfo in propertyInfos)
                {
                    try
                    {
                        sb.Append(MakeValueCsvFriendly(propertyInfo.GetValue(obj, null))).Append(";");
                    }
                    catch (Exception e)
                    {
                    }
                }
                sb.Remove(sb.Length - 1, 1).AppendLine();
            }


            sb.Insert(0, '\uFEFF');
            return sb.ToString();
        }

        //export to a file.
        public void ExportToFile(string path)
        {
            File.WriteAllText(path, Export());
        }

        //export as binary data.
        public byte[] ExportToBytes()
        {
            return Encoding.UTF8.GetBytes(Export());
        }

        //get the csv value for field.
        private string MakeValueCsvFriendly(object value)
        {
            if (value == null) return "";
            if (value is Nullable) return "";

            if (value is DateTime)
            {
                if (((DateTime) value).TimeOfDay.TotalSeconds == 0)
                    return ((DateTime) value).ToString("dd.MM.yyyy");
                return ((DateTime) value).ToString("dd.MM.yyyy HH:mm:ss");
            }
            var output = value.ToString();

            if (output.Contains(",") || output.Contains("\""))
                output = '"' + output.Replace("\"", "\"\"") + '"';

            return output;
        }
    }
}