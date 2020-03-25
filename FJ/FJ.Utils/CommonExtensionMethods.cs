using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace FJ.Utils
{
    public static class CommonExtensionMethods
    {
        /// <summary>
        /// Checks if given key value is some of the values given as parameter
        /// </summary>
        /// <typeparam name="T">Type of key value to be looked for</typeparam>
        /// <param name="key">Key value to be looked for</param>
        /// <param name="allowedValues">Values the key will be checked agains</param>
        /// <returns>Key equals with at least one of the given values</returns>
        public static bool IsIn<T>(this T key, params T[] allowedValues)
        {
            return allowedValues != null && allowedValues.Contains(key);
        }

        /// <summary>
        /// Wraps this object instance into IEnumerable
        /// </summary>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <param name="item">The instance to be wrapped</param>
        /// <returns>IEnumerable consisting only this item</returns>
        public static IEnumerable<T> ToMany<T>(this T item)
        {
            return new[] { item };
        }

        /// <summary>
        /// Converts any object into comparable byte array
        /// </summary>
        /// <param name="obj">Object to be converted</param>
        /// <returns>Serialized byte array from object</returns>
        public static byte[] ToByteArray(this object obj)
        {
            if (obj == null)
            {
                return null;
            }
            
            var formatter = new BinaryFormatter();
            using var memStream = new MemoryStream();
            
            formatter.Serialize(memStream, obj);
            return memStream.ToArray();
        }

        /// <summary>
        /// Serializes object into string based on class contents
        /// </summary>
        /// <param name="obj">Object to serialize</param>
        /// <returns>Serialized object as string</returns>
        [Obsolete("Do this with Newtonsoft's JsonSerializer instead", false)]
        public static string SerializeToString(this object obj)
        {
            var serializer = new XmlSerializer(obj.GetType());
            using TextWriter writer = new StringWriter();
            
            serializer.Serialize(writer, obj);
            return writer.ToString();
        }
    }
}
