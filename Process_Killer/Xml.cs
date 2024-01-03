using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace e_Verify_BACK_OFFICE_Service_Interface
{
    public static class Xml
    {
        public static string ToXml(this object objectToSerialize)
        {
            var mem = new MemoryStream();
            var ser = new XmlSerializer(objectToSerialize.GetType());
            ser.Serialize(mem, objectToSerialize);
            var utf8 = new UTF8Encoding();
            return utf8.GetString(mem.GetBuffer(), 0, (int)mem.Length);
        }
    }
}
