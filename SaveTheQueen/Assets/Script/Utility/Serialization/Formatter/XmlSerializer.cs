#if !UNITY_FLASH || UNITY_EDITOR
using System.Collections.Generic;
using System.Xml;
using serialization;

//TODO: replace system.xml to another which is not requiered reflection.
namespace serialization
{
    public class XmlSerializer : ISerializer
    {
        public XmlDocument doc;

        public override void OnSerializeTable(SerializeTable rootTable)
        {
            doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-16", null);
            doc.AppendChild(docNode);

            SerializeTable(rootTable, doc);
        }

        public override void OnDeserializeTable(SerializeTable rootTable)
        {
            DeserializeTable(rootTable, doc.DocumentElement);
        }

        private void SerializeTable(SerializeTable table, XmlNode parentNode)
        {
            if (0 == table.KeyValueMap.Count && 0 == table.Children.Count)
            {
                return;
            }

            XmlNode currentNode = doc.CreateElement(table.Name);
            parentNode.AppendChild(currentNode);

            using (Dictionary<string, SerializeTable.Value>.Enumerator next = table.KeyValueMap.GetEnumerator())
            {
                while (next.MoveNext())
                {
                    XmlNode child = doc.CreateElement(next.Current.Key);
                    child.InnerText = next.Current.Value.obj.ToString();

                    //XmlAttribute type = doc.CreateAttribute("type");
                    //type.Value = next.Current.Value.type.ToString();
                    //child.Attributes.Append(type);

                    currentNode.AppendChild(child);
                }
            }
            foreach (SerializeTable node in table.Children)
            {
                SerializeTable(node, currentNode);
            }
        }

        private void DeserializeTable(SerializeTable table, XmlNode currentNode)
        {
            XmlNode child = currentNode.FirstChild;
            while (null != child)
            {
                if (child.NodeType == XmlNodeType.Element)
                {
                    if (1 == child.ChildNodes.Count && child.FirstChild.NodeType == XmlNodeType.Text)
                    {
                        //string type = child.Attributes["type"].Value;
                        table.AddValue<string>(child.Name, child.InnerText);
                    }
                    else
                    {
                        DeserializeTable(table.AddChild(child.Name), child);
                    }
                }
                
                child = child.NextSibling;
            }
        }

        public virtual void SaveToFile(string filePath)
        {
            doc.Save(filePath);
        }

        public virtual void LoadFromFile(string filePath)
        {
            doc = new XmlDocument();
            doc.Load(filePath);
        }

        public virtual void LoadFromString(string xml, Serializable value)
        {
            doc = new XmlDocument();
            doc.LoadXml(xml);
            base.Deserialize(value);
        }
    }
}
#endif //!UNITY_FLASH || UNITY_EDITOR