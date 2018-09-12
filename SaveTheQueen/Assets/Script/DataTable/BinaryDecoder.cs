using System.IO;

public class BinaryDecoder
{

    private Stream stream = null;

    public BinaryDecoder(Stream stream)
    {
        this.stream = stream;
    }

    public bool DecodeBoolean()
    {
        byte[] bytes = DecodeBytes(1);
        return System.BitConverter.ToBoolean(bytes, 0);
    }

    public byte DecodeByte()
    {
        byte[] bytes = DecodeBytes(1);
        return bytes[0];
    }

    public byte[] DecodeBytes(int count)
    {
        byte[] bytes = new byte[count];
        int readBytes = stream.Read(bytes, 0, count);
        if (readBytes != count)
        {
            
        }
        return bytes;
    }

    public char DecodeChar()
    {
        byte[] bytes = DecodeBytes(1);
        return System.BitConverter.ToChar(bytes, 0);
    }

    public double DecodeDouble()
    {
        byte[] bytes = DecodeBytes(8);
        return System.BitConverter.ToDouble(bytes, 0);
    }

    public short DecodeInt16()
    {
        byte[] bytes = DecodeBytes(2);
        return System.BitConverter.ToInt16(bytes, 0);
    }

    public int DecodeInt32()
    {
        byte[] bytes = DecodeBytes(4);
        return System.BitConverter.ToInt32(bytes, 0);
    }

    public long DecodeInt64()
    {
        byte[] bytes = DecodeBytes(8);
        return System.BitConverter.ToInt64(bytes, 0);
    }

    public float DecodeSingle()
    {
        byte[] bytes = DecodeBytes(4);
        return System.BitConverter.ToSingle(bytes, 0);
    }

    public ushort DecodeUInt16()
    {
        byte[] bytes = DecodeBytes(2);
        return System.BitConverter.ToUInt16(bytes, 0);
    }

    public uint DecodeUInt32()
    {
        byte[] bytes = DecodeBytes(4);
        return System.BitConverter.ToUInt32(bytes, 0);
    }

    public ulong DecodeUInt64()
    {
        uint low32 = DecodeUInt32();
        uint high32 = DecodeUInt32();
        return high32 << 32 | low32;
    }
}
