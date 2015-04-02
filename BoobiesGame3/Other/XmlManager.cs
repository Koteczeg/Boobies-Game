using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace BoobiesGame3
{
    public class XmlManager
    {
        public string path;

        public XmlManager(string path)
        {
            this.path = path;
        }

        public GameElements Load()
        {
            GameElements gameElements;
            using (TextReader reader = new StreamReader(path))
            {
                XmlSerializer xml = new XmlSerializer(typeof(GameElements));
                gameElements = (GameElements)xml.Deserialize(reader);
            }
            return gameElements;
        }
    }
}
