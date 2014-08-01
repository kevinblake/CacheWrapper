using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Puzzlebox.Caching.Helpers
{
    public static class SerializationHelper
    {
        public static T Deserialize<T>(string filename)
        {
            var xs = new XmlSerializer(typeof(T));
            using (var rd = new StreamReader(filename))
            {
                return (T)xs.Deserialize(rd);
            }
        }

        public static T DeserializeFromString<T>(string data)
        {
            var xs = new XmlSerializer(typeof(T));
            using (var rd = new StreamReader(data.ToStream()))
            {
                return (T)xs.Deserialize(rd);
            }
        }

        public static T DeserializeFromString<T>(string data, string elementName)
        {
            var xs = new XmlSerializer(typeof(T));
            using (var rd = new StreamReader(data.ToStream()))
            {
                return (T)xs.Deserialize(rd);
            }
        }

        public static T LoadFromDisk<T>(string path)
        {
            return Deserialize<T>(path);
        }

        public static void SaveToDisk<T>(string path, T obj)
        {
            var objAsStr = Serialize(obj);
            File.WriteAllText(path, objAsStr);
        }

        public static string Serialize<T>(T obj)
        {
            var ws = new XmlWriterSettings { NewLineHandling = NewLineHandling.Entitize };
            var sb = new StringBuilder();

            var ser = new XmlSerializer(typeof(T));
            using (var wr = XmlWriter.Create(sb, ws))
            {
                ser.Serialize(wr, obj);
            }

            return sb.ToString();
        }

        public static Stream ToStream(this string inputString)
        {
            var byteArray = Encoding.ASCII.GetBytes(inputString);
            var stream = new MemoryStream(byteArray);
            return stream;
        }
    }
}