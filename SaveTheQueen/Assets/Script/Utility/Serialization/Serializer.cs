using System.Text;

namespace serialization
{
    public abstract class ISerializer
    {
        private SerializeTable rootTable = null;
        protected SerializeTable RootTable
        {
            get
            {
                if (null == rootTable)
                {
                    rootTable = new SerializeTable("__root__");
                }

                return rootTable;
            }
        }

        /// <summary>
        /// Required to implement serialization rule.
        /// </summary>
        public abstract void OnSerializeTable(SerializeTable rootTable);
        /// <summary>
        /// Required to implement deserialization rule.
        /// </summary>
        public abstract void OnDeserializeTable(SerializeTable rootTable);

        #region Serialization
        public void Serialize(Serializable value)
        {
            RootTable.Name = value.GetType().Name;
            value.Serialize(RootTable);
            OnSerializeTable(RootTable);
        }

        public void Deserialize(Serializable value)
        {
            RootTable.Name = value.GetType().Name;
            OnDeserializeTable(RootTable);
            value.Deserialize(RootTable);
        }
        #endregion
    }
}