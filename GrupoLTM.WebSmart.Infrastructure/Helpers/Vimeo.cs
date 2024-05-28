using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;

namespace GrupoLTM.WebSmart.Infrastructure.Helpers
{
    public class Vimeo
    {
        public string GetVimeoThumbnail(string idVimeoVideo)
        {
            string strThumbnail = "";

            string strURL = "http://vimeo.com/api/v2/video/" + idVimeoVideo + ".xml";

            XmlDocument doc = new XmlDocument();
            doc.Load(strURL);

            XmlNode VideosListNode = doc.SelectSingleNode("/videos/video");

            if (VideosListNode.ChildNodes.Count > 0)
            {
                strThumbnail = VideosListNode.ChildNodes[13].InnerText;
            }
            
            return strThumbnail;
        }
    }
}
