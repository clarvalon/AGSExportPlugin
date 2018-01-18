#region License
/* AGSExportPlugin 
 * Copyright 2010-2018 - Dan Alexander
 *
 * Released under the MIT License.  See LICENSE for details. */
#endregion

using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Text;

namespace Clarvalon.XAGE.Global
{
    /// <summary>
    /// Class used to serialize generic classes to XML
    /// </summary>
    public static class GenericXmlSerializer
    {
        /// <summary>
        /// Generic Serialization - save any object to disk (XML format)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename">Destination filename of item to be serialized</param>
        /// <param name="item">Item to be serialized</param>
        public static void SerializeToFile<T>(string filename, T item)
        {
            SerializeToFile<T>(filename, item, null, null);
        }

        /// <summary>
        /// Generic Serialization - save any object to disk (XML format)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename">Destination filename of item to be serialized</param>
        /// <param name="item">Item to be serialized</param>
        /// <param name="xmlSerializer">Pre-prepared serializer</param>
        /// <param name="xmlWriterSettings">XML Writer settings - used to override default behaviour</param>
        public static void SerializeToFile<T>(string filename, T item, XmlSerializer xmlSerializer, XmlWriterSettings xmlWriterSettings = null)
        {
            // Create Serializer if not supplied
            if (xmlSerializer == null)
                xmlSerializer = new XmlSerializer(typeof(T));

            if (xmlWriterSettings == null)
            {
                 TextWriter w = new StreamWriter(filename);
                 xmlSerializer.Serialize(w, item);
                 w.Close();
            }
            else
            {
                XmlWriter w = XmlWriter.Create(filename, xmlWriterSettings);
                xmlSerializer.Serialize(w, item);
                w.Close();
            }
        }

        /// <summary>
        ///  Generic Serialization - convert any object to string (XML format)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">Item to be serialized</param>
        /// <param name="xmlSerializer">Pre-prepared serializer</param>
        /// <param name="xmlWriterSettings">XML Writer settings - used to override default behaviour</param>
        /// <returns></returns>
        public static string SerializeToString<T>(T item, XmlSerializer xmlSerializer, XmlWriterSettings xmlWriterSettings = null)
        {
            // Create Serializer if not supplied
            if (xmlSerializer == null)
                xmlSerializer = new XmlSerializer(typeof(T));

            if (xmlWriterSettings == null)
            {
                Utf8StringWriter sw = new Utf8StringWriter();
                xmlSerializer.Serialize(sw, item);
                return sw.ToString();
            }
            else
            {
                Utf8StringWriter sw = new Utf8StringWriter();
                XmlWriter w = XmlWriter.Create(sw, xmlWriterSettings);
                xmlSerializer.Serialize(w, item);
                w.Close();
                return sw.ToString();
            }
        }

        /// <summary>
        /// Generic DeSerialization - load any object from disk (XML format)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename">Target filename of item to be deserialized</param>
        /// <returns></returns>
        public static T DeSerializeFromFile<T>(string filename)
        {
            return DeSerializeFromFile<T>(filename, null);
        }

        /// <summary>
        /// Generic DeSerialization - load any object from disk (XML format)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename">Target filename of item to be deserialized</param>
        /// <param name="xmlSerializer">Pre-prepared serializer</param>
        /// <returns></returns>
        public static T DeSerializeFromFile<T>(string filename, XmlSerializer xmlSerializer)
        {
            if (xmlSerializer == null)
                xmlSerializer = new XmlSerializer(typeof(T));
            TextReader r = new StreamReader(filename);
            T item = (T)xmlSerializer.Deserialize(r);
            r.Close();
            return item;
        }
    }
}