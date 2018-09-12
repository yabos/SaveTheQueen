using System;
using System.IO;

public interface IDatabase
{
    UInt32 GetSize();
    bool Encode(BinaryEncoder encoder);
    bool Decode(BinaryDecoder decoder);
}
