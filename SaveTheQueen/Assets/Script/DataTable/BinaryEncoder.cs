using System.IO;

public class BinaryEncoder
{

    private Stream stream = null;

    public BinaryEncoder(Stream stream)
    {
        this.stream = stream;
    }

    public void Encode(bool value)
    {
        byte[] bytes = System.BitConverter.GetBytes(value);
        stream.Write(bytes, 0, bytes.Length);
    }

    public void Encode(byte value)
    {
        byte[] bytes = System.BitConverter.GetBytes(value);
        stream.Write(bytes, 0, bytes.Length);
    }

    public void Encode(byte[] buffer)
    {
        stream.Write(buffer, 0, buffer.Length);
    }

    public void Encode(char ch)
    {
        byte[] bytes = System.BitConverter.GetBytes(ch);
        stream.Write(bytes, 0, bytes.Length);
    }

    public void Encode(double value)
    {
        byte[] bytes = System.BitConverter.GetBytes(value);
        stream.Write(bytes, 0, bytes.Length);
    }

    public void Encode(float value)
    {
        byte[] bytes = System.BitConverter.GetBytes(value);
        stream.Write(bytes, 0, bytes.Length);
    }

    public void Encode(short value)
    {
        byte[] bytes = System.BitConverter.GetBytes(value);
        stream.Write(bytes, 0, bytes.Length);
    }

    public void Encode(int value)
    {
        byte[] bytes = System.BitConverter.GetBytes(value);
        stream.Write(bytes, 0, bytes.Length);
    }

    public void Encode(long value)
    {
        byte[] bytes = System.BitConverter.GetBytes(value);
        stream.Write(bytes, 0, bytes.Length);
    }

    public void Encode(ushort value)
    {
        byte[] bytes = System.BitConverter.GetBytes(value);
        stream.Write(bytes, 0, bytes.Length);
    }

    public void Encode(uint value)
    {
        byte[] bytes = System.BitConverter.GetBytes(value);
        stream.Write(bytes, 0, bytes.Length);
    }

    public void Encode(ulong value)
    {
        uint high32 = (uint)(value >> 32);
        uint low32 = (uint)(value & 0xffffffff);

        Encode(low32);
        Encode(high32);
    }

}
