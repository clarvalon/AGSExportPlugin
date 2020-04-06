using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AGSExportPlugin
{
    [XmlRoot]
    public class AGSExtraInfo
    {
        [XmlElement("Fonts")]
        public List<AGSExtraInfoFont> Fonts;

        public AGSExtraInfo()
        {
        }

        public AGSExtraInfo(bool init)
        {
            if (init)
                Initialise();
        }

        public void Initialise()
        {
            Fonts = new List<AGSExtraInfoFont>();
        }
    }

    
    public class AGSExtraInfoFont
    {
        public int ID;
        public int FontHeight;
    }
}
