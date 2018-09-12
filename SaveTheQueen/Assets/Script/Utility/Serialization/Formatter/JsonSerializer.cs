#if (!UNITY_FLASH && !UNITY_WEBPLAYER) || UNITY_EDITOR
#define ENABLE_SERIALIZER
#endif

using System.Collections;
using System.Collections.Generic;
using System.Text;
using LitJson;

namespace serialization
{
    public class JsonSerializer : ISerializer
    {
        StringBuilder stream = new StringBuilder();
        LitJson.JsonReader reader = null;
        LitJson.JsonWriter writer = null;
        private bool PrettyPrint = true;

        public virtual void SaveToFile(string filePath, Serializable value, bool bPrettyPrint = true)
        {
            PrettyPrint = bPrettyPrint;
#if ENABLE_SERIALIZER
            base.Serialize(value);

            System.IO.StreamWriter streamWriter = null;
            try
            {
                System.IO.FileInfo xmlFile = new System.IO.FileInfo(filePath);
                streamWriter = xmlFile.CreateText();

                streamWriter.WriteLine(stream);

                streamWriter.Close();
            }
            finally
            {
                if ((streamWriter != null))
                {
                    streamWriter.Dispose();
                }
            }
#endif // ENABLE_SERIALIZER
        }

        public virtual void LoadFromString(string json, Serializable value)
        {
            reader = new JsonReader(json);
            base.Deserialize(value);
        }

        public virtual void LoadFromFile(string filePath, Serializable value)
        {
#if ENABLE_SERIALIZER
            System.IO.FileStream file = null;
            System.IO.StreamReader sr = null;
            try
            {
                file = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                sr = new System.IO.StreamReader(file);

                LoadFromString(sr.ReadToEnd(), value);

                sr.Close();
                file.Close();
            }
            finally
            {
                if ((file != null))
                {
                    file.Dispose();
                }
                if ((sr != null))
                {
                    sr.Dispose();
                }
            }
#endif //!UNITY_FLASH && !UNITY_WEBPLAYER
        }

        public override void OnSerializeTable(SerializeTable rootTable)
        {
            writer = new JsonWriter(stream);
            writer.PrettyPrint = PrettyPrint;

            writer.WriteObjectStart();
            SerializeTable(rootTable, writer);
            writer.WriteObjectEnd();
        }

        public override void OnDeserializeTable(SerializeTable rootTable)
        {
            reader.Read();  // read blank node.
            DeserializeTable(rootTable, reader);
        }

        private void SerializeTable(SerializeTable table, LitJson.JsonWriter writer)
        {
            if (0 == table.KeyValueMap.Count && 0 == table.Children.Count)
            {
                return;
            }

            writer.WritePropertyName(table.Name);
            writer.WriteObjectStart();
            using (Dictionary<string, SerializeTable.Value>.Enumerator next = table.KeyValueMap.GetEnumerator())
            {
                while (next.MoveNext())
                {
                    writer.WritePropertyName(next.Current.Key);
                    //writer.Write(next.Current.Value.obj);
                    WriteValue(next.Current.Value.obj, writer, 0);
                }
            }

            foreach (SerializeTable node in table.Children)
            {
                SerializeTable(node, writer);
            }

            writer.WriteObjectEnd();
        }

        private static void WriteValue(object obj, JsonWriter writer, int depth)
        {
            if (obj == null)
            {
                writer.Write(null);
                return;
            }

            if (obj is System.String)
            {
                writer.Write(System.Convert.ToString(obj));
                return;
            }

            if (obj is System.Single)
            {
                writer.Write(System.Convert.ToSingle(obj));
                return;
            }

            if (obj is System.Double)
            {
                writer.Write(System.Convert.ToDouble(obj));
                return;
            }


            if (obj is System.Int16 || obj is System.UInt16 || obj is System.Int32)
            {
                writer.Write(System.Convert.ToInt32(obj));
                return;
            }

            if (obj is System.Int64 || obj is System.UInt32)
            {
                writer.Write(System.Convert.ToInt64(obj));
                return;
            }

            if (obj is System.UInt64)
            {
                writer.Write(System.Convert.ToUInt64(obj));
                return;
            }

            if (obj is System.Boolean)
            {
                writer.Write(System.Convert.ToBoolean(obj));
                return;
            }

            if (obj is System.Enum)
            {
                writer.Write((int)obj);
                return;
            }

            if (obj is System.Array)
            {
                writer.WriteArrayStart();

                foreach (object elem in (System.Array)obj)
                    WriteValue(elem, writer, depth + 1);

                writer.WriteArrayEnd();

                return;
            }

            if (obj is System.Collections.IList)
            {
                writer.WriteArrayStart();
                foreach (object elem in (System.Collections.IList)obj)
                    WriteValue(elem, writer, depth + 1);
                writer.WriteArrayEnd();

                return;
            }

            if (obj is System.Collections.IDictionary)
            {
                writer.WriteObjectStart();
                foreach (System.Collections.DictionaryEntry entry in (System.Collections.IDictionary)obj)
                {
                    writer.WritePropertyName((string)entry.Key);
                    WriteValue(entry.Value, writer, depth + 1);
                }
                writer.WriteObjectEnd();

                return;
            }

            throw new System.NotSupportedException(typeof(object).ToString());
        }

        private void DeserializeTable(SerializeTable table, LitJson.JsonReader reader)
        {
            string propertyName = string.Empty;
            while (reader.Read())
            {
                switch (reader.Token)
                {
                    case JsonToken.ObjectStart:
                        if (table.Name == propertyName && table.IsRoot)
                        {   // nothing to do
                        }
                        else
                        {
                            DeserializeTable(table.AddChild(propertyName), reader);
                        }
                        //propertyName = string.Empty;
                        break;

                    case JsonToken.PropertyName:
                        propertyName = (string)reader.Value;
                        break;

                    case JsonToken.ObjectEnd:
                        return;


                    case JsonToken.ArrayStart:
                        // for arrary and list
                        DeserializeTable(table.AddChild(propertyName), reader);
                        break;
                    case JsonToken.ArrayEnd:
                        // for arrary and list
                        return;

                    case JsonToken.Int:
                        //table.AddValue<int>(propertyName, (int)reader.Value);
                        table.AddValue<string>(propertyName, reader.Value.ToString());
                        propertyName = string.Empty;
                        break;

                    case JsonToken.Long:
                        //table.AddValue<int>(propertyName, (int)reader.Value);
                        table.AddValue<string>(propertyName, reader.Value.ToString());
                        propertyName = string.Empty;
                        break;

                    // case JsonToken.Single:
                    // 	 //table.AddValue<float>(propertyName, (float)reader.Value);
                    // 	 table.AddValue<string>(propertyName, reader.Value.ToString());
                    // 	 propertyName = string.Empty;
                    // 	 break;

                    case JsonToken.Double:
                        //table.AddValue<double>(propertyName, (double)reader.Value);
                        table.AddValue<string>(propertyName, reader.Value.ToString());
                        propertyName = string.Empty;
                        break;

                    case JsonToken.String:
                        //table.AddValue<string>(propertyName, (string)reader.Value);
                        table.AddValue<string>(propertyName, reader.Value.ToString());
                        propertyName = string.Empty;
                        break;

                    case JsonToken.Boolean:
                        //table.AddValue<bool>(propertyName, (bool)reader.Value);
                        table.AddValue<string>(propertyName, reader.Value.ToString());
                        propertyName = string.Empty;
                        break;

                    case JsonToken.Null:
                        break;
                }
            }
        }
    }
}
