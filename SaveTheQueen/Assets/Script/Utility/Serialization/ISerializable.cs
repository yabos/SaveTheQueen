using UnityEngine;
using System.Collections;
using System.IO;

public interface ISerializable  {

    void Serialize(Stream stream);
    void Deserialize(Stream stream);
}
