using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ReportPublisherConfig.Core
{
    public class XmlSerialization
    {
        // I found the following here: http://stackoverflow.com/questions/1081325/c-sharp-how-to-xml-deserialize-object-itself
        // There might be a easier way but this seems to work.
        public string Serialize<T>(T obj, Encoding encoding)
        {
            var serializer = new XmlSerializer(typeof(T));
            TextWriter textWriter = new StringWriterWithEncoding(new StringBuilder(), encoding);
            serializer.Serialize(textWriter, obj);

            return textWriter.ToString();
        }

        private class StringWriterWithEncoding : StringWriter
        {
            readonly Encoding encoding;

            public StringWriterWithEncoding(StringBuilder builder, Encoding encoding)
                : base(builder)
            {
                this.encoding = encoding;
            }

            public override Encoding Encoding
            {
                get { return encoding; }
            }
        }

        public T Deserialize<T>(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return default(T);
            }

            using (var textReader = new StringReader(xml))
            using (var xmlReader = XmlReader.Create(textReader))
            {
                return (T)new XmlSerializer(typeof(T)).Deserialize(xmlReader);
            }
        }
    }
}