using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;

namespace TimeTableOne.Common
{
    public static class LiveTileTemplateHelper
    {
        public static XmlDocument AppendTextElement(this XmlDocument doc, int id, string message)
        {
            doc.GetElementsByTagName("text")[id].InnerText = message;
            return doc;
        }
    }
}
