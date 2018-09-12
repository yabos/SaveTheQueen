using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace serialization
{
	public class SerializeTable // : TreeNode<SerializeTable> // We need to keep this class non-generic due to Mono AOT Compiler limitations
	{
		public class Value
		{
			public Value(System.Object obj, System.Type type)
			{
				this.obj = obj;
				this.type = type;
			}

			public System.Object obj;
			public System.Type type;

			public override string ToString()
			{
				return obj.ToString();
			}
		}

		private Dictionary<string, Value> keyValueMap = null;
		public Dictionary<string, Value> KeyValueMap
		{
			get
			{
				if (null == keyValueMap)
				{
					keyValueMap = new Dictionary<string, Value>();
				}

				return keyValueMap;
			}
		}

		public string Name { get; set; }
		public SerializeTable Parent { get; set; }

		private List<SerializeTable> _children;
		public List<SerializeTable> Children
		{
			get
			{
				if (_children == null)
				{
					_children = new List<SerializeTable>();
				}
				return _children;
			}

			set
			{
				_children = value;
			}
		}

		public Boolean IsRoot
		{
			get { return Parent == null; }
		}

		public Boolean IsLeaf
		{
			get { return Children.Count == 0; }
		}

		public int Level
		{
			get
			{
				if (IsRoot)
					return 0;
				return Parent.Level + 1;
			}
		}

		public SerializeTable(string name)
		{
			Name = name;
		}

		public SerializeTable AddChild(SerializeTable child)
		{
			return AddChild(child.Name, child);
		}

		public SerializeTable AddChild(string name)
		{
			return AddChild(name, new SerializeTable(name));
		}

		public SerializeTable AddChild(string name, SerializeTable child)
		{
			child.Parent = this;
			Children.Add(child);
			return child;
		}

		public bool RemoveChild(SerializeTable child)
		{
			return Children.Remove(child);
		}

		public SerializeTable GetChildTable(string name)
		{
			return Children.Find(x => x.Name == name);
		}

		public bool TryGetChild(string name, out SerializeTable value)
		{
			value = GetChildTable(name);
			return (null != value);
		}

		public List<SerializeTable> GetChildTables(string name)
		{
			return Children.FindAll(x => x.Name == name);
		}

		public bool TryGetChildren(string name, out List<SerializeTable> value)
		{
			value = GetChildTables(name);
			return (value != null);
		}

		/// <summary>
		/// serializes to this table
		/// </summary>
		/// <param name="value"></param>
		public void AddSerializableValue(Serializable value)
		{
			value.Serialize(this);
		}

		/// <summary>
		/// serializes to child of this table
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public void AddSerializableValue(string name, Serializable value)
		{
			value.Serialize(AddChild(name));
		}

		public void AddValue<T>(string name, T value)
		{
			if (value == null)
			{
				return;
			}

			Serializable serializableValue = value as Serializable;
			if (null != serializableValue)
			{
				serializableValue.Serialize(AddChild(name));
			}
			else
			{
				if (KeyValueMap.ContainsKey(name))
				{
					throw new System.Exception("A value has already been associated with name.");
				}

				KeyValueMap.Add(name, new Value(value, typeof(T)));
			}
		}

		public T GetValue<T>(string name)
		{
			if (typeof(Serializable).IsAssignableFrom(typeof(T)))
			{
				SerializeTable table;
				if (TryGetChild(name, out table))
				{
					Serializable v3 = Activator.CreateInstance<T>() as Serializable;
					v3.Deserialize(table);
					return (T)v3;
				}
				else
				{
					//					Debug.LogWarning(string.Format("Can't find key value(serializable): {0}.{1}", this.Name, name));
					return default(T);
				}
			}

			Value value;

			if (!KeyValueMap.TryGetValue(name, out value))
			{
				//               Debug.LogWarning(string.Format("Can't find key value: {0}.{1}", this.Name, name));
				return default(T);
			}

			if (value.type == typeof(string))
			{
				T convertedValue;

				if (StringConverter.TryConvert<T>((string)value.obj, out convertedValue))
				{
					return convertedValue;
				}
			}

			//if (typeof(System.Enum).IsAssignableFrom(typeof(T)))
			//{
			//    int convertedValue;
			//    StringConverter.TryConvert<int>((string)value.obj, out convertedValue);
			//    return (T)System.Enum.ToObject(typeof(T), convertedValue);
			//}

			object numberic = value.obj;
			if (value.obj is System.Single)
			{
				numberic = System.Convert.ToSingle(value.obj);
			}

			if (value.obj is System.Double)
			{
				numberic = System.Convert.ToDouble(value.obj);
			}

			if (value.obj is System.Int16 || value.obj is System.UInt16 ||
				value.obj is System.Int32 || value.obj is System.UInt32)
			{
				numberic = System.Convert.ToInt32(value.obj);
			}

			if (value.obj is System.Boolean)
			{
				numberic = System.Convert.ToBoolean(value.obj);
			}

			if (value.obj is System.Int64 || value.obj is System.UInt64)
			{
				numberic = System.Convert.ToInt64(value.obj);
			}

			return (T)numberic;
		}

		public T GetValue<T>(string name, T defaultValue)
		{
			if (typeof(Serializable).IsAssignableFrom(typeof(T)))
			{
				SerializeTable table;
				if (TryGetChild(name, out table))
				{
					Serializable v3 = Activator.CreateInstance<T>() as Serializable;
					v3.Deserialize(table);
					return (T)v3;
				}
				else
				{
					//					Debug.LogWarning(string.Format("Can't find key value(serializable): {0}.{1}", this.Name, name));
				}

				return defaultValue;
			}

			Value value;

			if (!KeyValueMap.TryGetValue(name, out value))
			{
				//				Debug.LogWarning(string.Format("Can't find key value: {0}.{1}", this.Name, name));
				return defaultValue;
			}

			if (value.type == typeof(string))
			{
				T convertedValue;

				if (StringConverter.TryConvert<T>((string)value.obj, out convertedValue))
				{
					return convertedValue;
				}
			}

			//if (typeof(System.Enum).IsAssignableFrom(typeof(T)))
			//{
			//    int convertedValue;
			//    StringConverter.TryConvert<int>((string)value.obj, out convertedValue);
			//    return (T)System.Enum.ToObject(typeof(T), convertedValue);
			//}

			object numberic = value.obj;
			if (value.obj is System.Single)
			{
				numberic = System.Convert.ToSingle(value.obj);
			}

			if (value.obj is System.Double)
			{
				numberic = System.Convert.ToDouble(value.obj);
			}

			if (value.obj is System.Int16 || value.obj is System.UInt16 ||
				value.obj is System.Int32 || value.obj is System.UInt32)
			{
				numberic = System.Convert.ToInt32(value.obj);
			}

			if (value.obj is System.Boolean)
			{
				numberic = System.Convert.ToBoolean(value.obj);
			}

			if (value.obj is System.Int64 || value.obj is System.UInt64)
			{
				numberic = System.Convert.ToInt64(value.obj);
			}

			return (T)numberic;
		}
	}
}