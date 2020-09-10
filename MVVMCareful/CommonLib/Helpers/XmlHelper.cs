using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CommonLib.Helpers
{
    public class XmlHelper
    {
        /// <summary>  
        /// 序列化  
        /// </summary>   
        /// <param name="obj">对象</param>  
        /// <returns></returns>  
        public static string Serializer<T>(object obj)
        {
            MemoryStream Stream = new MemoryStream();
            XmlSerializer xml = new XmlSerializer(typeof(T));
            xml.Serialize(Stream, obj);
            Stream.Position = 0;
            StreamReader sr = new StreamReader(Stream);
            string str = sr.ReadToEnd();

            sr.Dispose();
            Stream.Dispose();

            return str;
        }
        /// <summary>  
        /// 反序列化  
        /// </summary>  
        /// <param name="xml">XML字符串</param>  
        /// <returns></returns>  
        public static T Deserialize<T>(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
                return default(T);
            using (StringReader sr = new StringReader(xml))
            {
                XmlSerializer xmldes = new XmlSerializer(typeof(T));
                return (T)xmldes.Deserialize(sr);
            }
        }
        /// <summary>
        /// 写入xml文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="xml"></param>
        public static void WriterXml(string path, string xml)
        {
            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            System.IO.StreamWriter file = new System.IO.StreamWriter(fs);
            file.Write(xml);
            file.Flush();
            file.Close();
        }
        /// <summary>
        /// 读取xml文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReadXml(string path)
        {
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
            System.IO.StreamReader sr = new System.IO.StreamReader(fs);
            string str = sr.ReadToEnd();
            sr.Close();
            return str;
        }
    }
}
