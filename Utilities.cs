using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LetsMT.MTProvider
{
    class Utilities
    {
        public static T DeserializeObject<T>(string serialized)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            return (T)xmlSerializer.Deserialize(new StringReader(serialized));
        }
    }
}
