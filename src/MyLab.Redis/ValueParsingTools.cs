using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace MyLab.Redis
{
    /// <summary>
    /// Containes method to parsing values
    /// </summary>
    public class ValueParsingTools
    {

        /// <summary>
        /// Converts value to string
        /// </summary>
        public static string ObjectToString<T>(T obj)
        {
            if (obj == null)
                return null;

            var type = obj.GetType();
            var typeInfo = type.GetTypeInfo();

            if (typeInfo.IsPrimitive ||
                type == typeof(Guid) ||
                type == typeof(DateTime) ||
                type == typeof(string))
                return obj.ToString();

            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None
            });
        }

        /// <summary>
        /// Parses string
        /// </summary>
        public static T ParseCleanString<T>(string cleanString)
        {
            if (cleanString == null)
                return default(T);

            if (typeof(T) == typeof(int)) return (T)(object)int.Parse(cleanString);
            if (typeof(T) == typeof(bool)) return (T)(object)bool.Parse(cleanString);
            if (typeof(T) == typeof(double)) return (T)(object)double.Parse(cleanString);
            if (typeof(T) == typeof(DateTime)) return (T)(object)DateTime.Parse(cleanString);
            if (typeof(T) == typeof(Guid)) return (T)(object)Guid.Parse(cleanString);
            if (typeof(T) == typeof(string)) return (T)(object)cleanString;

            return JsonConvert.DeserializeObject<T>(cleanString, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
        }
    }
}
