using UnityEngine;
using System;

namespace serialization
{
    public interface Serializable
    {
        void Serialize(SerializeTable table);
        void Deserialize(SerializeTable table);
    }

}
