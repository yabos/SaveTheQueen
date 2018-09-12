using UnityEngine;

namespace serialization.collections
{
    public class SerializableList<TVal> :
        System.Collections.Generic.List<TVal>,
        Serializable
    {
        /* Format
         * <List>
         *      <Item>
         *          <Value>Value</Value>
         *      </Item>
         *      <Item>
         *          <Value>Value</Value>
         *      </Item>
         * </List>
         */

        public void Serialize(SerializeTable table)
        {
            for (int i = 0; i < Count; ++i)
            {
                table.AddChild("Item").AddValue<TVal>("Value", this[i]);
            }
        }

        public static void Serialize<T>(T data, SerializeTable table) where T : SerializableList<T>
        {
            for (int i = 0; i < data.Count; ++i)
            {
                table.AddChild("Item").AddValue<T>("Value", data[i]);
            }
        }

        public void Deserialize(SerializeTable table)
        {
            System.Collections.Generic.List<SerializeTable> items = table.GetChildTables("Item");
            for (int i = 0; i < items.Count; ++i)
            {
                this.Add(items[i].GetValue<TVal>("Value"));
            }
        }

        public static void Deserialize<T>(T data, SerializeTable table) where T : SerializableList<T>
        {
            System.Collections.Generic.List<SerializeTable> items = table.GetChildTables("Item");
            for (int i = 0; i < items.Count; ++i)
            {
                data.Add(items[i].GetValue<T>("Value"));
            }
        }
    }
}