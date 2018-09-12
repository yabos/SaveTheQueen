using UnityEngine;

namespace serialization.collections
{
	public class SerializableDictionary<TKey, TVal> :
		System.Collections.Generic.Dictionary<TKey, TVal>,
		Serializable
	{
		/* Format
         * <Dictionary>
         *      <Item>
         *          <Key>Key</Key>
         *          <Value>Value</Value>
         *      </Item>
         *      <Item>
         *          <Key>Key</Key>
         *          <Value>Value</Value>
         *      </Item>
         * </Dictionary>
         */

		public void Serialize(SerializeTable table)
		{
			SerializableDictionary<TKey, TVal>.Enumerator it = GetEnumerator();
			while (it.MoveNext())
			{
				SerializeTable item = table.AddChild("Item");
				item.AddValue<TKey>("Key", it.Current.Key);
				item.AddValue<TVal>("Value", it.Current.Value);
			}
		}

		public void Deserialize(SerializeTable table)
		{
			System.Collections.Generic.List<SerializeTable> items = table.GetChildTables("Item");

			for (int i = 0; i < items.Count; ++i)
			{
				SerializeTable item = items[i];
				Add(item.GetValue<TKey>("Key"), item.GetValue<TVal>("Value"));
			}
		}
	}
}