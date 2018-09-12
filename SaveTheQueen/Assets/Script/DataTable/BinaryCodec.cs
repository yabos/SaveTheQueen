using System.IO;

public class BinaryCodec
{
	public static bool DecodeUnicodeString(BinaryDecoder decoder, out string value)
    {
        return StringCodec.DecodeUnicodeString(decoder, out value);
    }
    
    public static bool EncodeUnicodeString(BinaryEncoder encoder, string value)
    {
        return StringCodec.EncodeUnicodeString(encoder, value);
    }
    
    public static uint SizeUnicodeString(string value)
    {
        return StringCodec.SizeUnicodeString(value);
    }
    
    public static bool Decode(BinaryDecoder decoder, out string value)
    {
        value = "";
        try
        {
            ushort size = decoder.DecodeUInt16();
            byte[] bytes = decoder.DecodeBytes(size);
            char[] array = new char[size];
            for (int i = 0; i < size; ++i)
            {
                array[i] = (char)bytes[i];
            }
            value = new string(array);
        }
        catch (System.Exception)
        {
            return false;
        }

        return true;
    }

    public static bool Decode(BinaryDecoder decoder, out bool value)
    {
        value = false;
        try
        {
            value = decoder.DecodeBoolean();
        }
        catch (System.Exception)
        {
            return false;
        }

        return true;
    }

    public static bool Decode(BinaryDecoder decoder, out short value)
    {
        value = 0;
        try
        {
            value = decoder.DecodeInt16();
        }
        catch (System.Exception)
        {
            return false;
        }

        return true;
    }

    public static bool Decode(BinaryDecoder decoder, out int value)
    {
        value = 0;
        try
        {
            value = decoder.DecodeInt32();
        }
        catch (System.Exception)
        {
            return false;
        }

        return true;
    }

    public static bool Decode(BinaryDecoder decoder, out long value)
    {
        value = 0;
        try
        {
            value = decoder.DecodeInt64();
        }
        catch (System.Exception)
        {
            return false;
        }

        return true;
    }

    public static bool Decode(BinaryDecoder decoder, out byte value)
    {
        value = 0;
        try
        {
            value = decoder.DecodeByte();
        }
        catch (System.Exception)
        {
            return false;
        }

        return true;
    }

    public static bool Decode(BinaryDecoder decoder, out ushort value)
    {
        value = 0;
        try
        {
            value = decoder.DecodeUInt16();
        }
        catch (System.Exception)
        {
            return false;
        }

        return true;
    }

    public static bool Decode(BinaryDecoder decoder, out uint value)
    {
        value = 0;
        try
        {
            value = decoder.DecodeUInt32();
        }
        catch (System.Exception)
        {
            return false;
        }

        return true;
    }

    public static bool Decode(BinaryDecoder decoder, out ulong value)
    {
        value = 0;
        try
        {
            value = decoder.DecodeUInt64();
        }
        catch (System.Exception)
        {
            return false;
        }

        return true;
    }

    public static bool Decode(BinaryDecoder decoder, out float value)
    {
        value = 0.0f;
        try
        {
            value = decoder.DecodeSingle();
        }
        catch (System.Exception)
        {
            return false;
        }

        return true;
    }

    public static bool Decode(BinaryDecoder decoder, out double value)
    {
        value = 0.0f;
        try
        {
            value = decoder.DecodeDouble();
        }
        catch (System.Exception)
        {
            return false;
        }

        return true;
    }

    public static bool Decode(BinaryDecoder decoder, out FixedPoint value)
    {
        value = new FixedPoint();
        try
        {
            value.RawValue = decoder.DecodeInt32();
        }
        catch (System.Exception)
        {
            return false;
        }

        return true;
    }
    
    public static bool Encode(BinaryEncoder encoder, string value)
    {
        try
        {
            ushort size = (ushort)value.Length;
            encoder.Encode(size);

			byte[] bytes = new byte[value.Length];
            for(int i = 0; i < value.Length; ++i)
            {
                bytes[i] = (byte)value[i];
            }
            encoder.Encode(bytes);
        }
        catch (System.Exception)
        {
            return false;
        }

        return true;
    }

    public static bool Encode(BinaryEncoder encoder, bool value)
    {
        try
        {
            encoder.Encode(value);
        }
        catch (System.Exception)
        {
            return false;
        }

        return true;
    }

    public static bool Encode(BinaryEncoder encoder, sbyte value)
    {
        try
        {
            encoder.Encode(value);
        }
        catch (System.Exception)
        {
            return false;
        }

        return true;
    }

    public static bool Encode(BinaryEncoder encoder, short value)
    {
        try
        {
            encoder.Encode(value);
        }
        catch (System.Exception)
        {
            return false;
        }

        return true;
    }

    public static bool Encode(BinaryEncoder encoder, int value)
    {
        try
        {
            encoder.Encode(value);
        }
        catch (System.Exception)
        {
            return false;
        }

        return true;
    }

    public static bool Encode(BinaryEncoder encoder, long value)
    {
        try
        {
            encoder.Encode(value);
        }
        catch (System.Exception)
        {
            return false;
        }

        return true;
    }

    public static bool Encode(BinaryEncoder encoder, byte value)
    {
        try
        {
            encoder.Encode(value);
        }
        catch (System.Exception)
        {
            return false;
        }

        return true;
    }

    public static bool Encode(BinaryEncoder encoder, ushort value)
    {
        try
        {
            encoder.Encode(value);
        }
        catch (System.Exception)
        {
            return false;
        }

        return true;
    }

    public static bool Encode(BinaryEncoder encoder, uint value)
    {
        try
        {
            encoder.Encode(value);
        }
        catch (System.Exception)
        {
            return false;
        }

        return true;
    }

    public static bool Encode(BinaryEncoder encoder, ulong value)
    {
        try
        {
            encoder.Encode(value);
        }
        catch (System.Exception)
        {
            return false;
        }

        return true;
    }

    public static bool Encode(BinaryEncoder encoder, float value)
    {
        try
        {
            encoder.Encode(value);
        }
        catch (System.Exception)
        {
            return false;
        }

        return true;
    }

    public static bool Encode(BinaryEncoder encoder, double value)
    {
        try
        {
            encoder.Encode(value);
        }
        catch (System.Exception)
        {
            return false;
        }

        return true;
    }

    public static bool Encode(BinaryEncoder encoder, byte[] value)
    {
        try
        {
            encoder.Encode((uint)value.Length);
            encoder.Encode(value);
        }
        catch (System.Exception)
        {
            return false;
        }

        return true;
    }

    public static bool Encode(BinaryEncoder encoder, FixedPoint value)
    {
        try
        {
            encoder.Encode(value.RawValue);
        }
        catch (System.Exception)
        {
            return false;
        }

        return true;
    }

    // Size
    public static uint Size(bool value)
    {
        return sizeof(bool);
    }
    public static uint Size(sbyte value)
    {
        return sizeof(sbyte);
    }
    public static uint Size(short value)
    {
        return sizeof(short);
    }
    public static uint Size(int value)
    {
        return sizeof(int);
    }
    public static uint Size(long value)
    {
        return sizeof(long);
    }
    public static uint Size(byte value)
    {
        return sizeof(byte);
    }
    public static uint Size(ushort value)
    {
        return sizeof(ushort);
    }
    public static uint Size(uint value)
    {
        return sizeof(uint);
    }
    public static uint Size(ulong value)
    {
        return sizeof(ulong);
    }
    public static uint Size(float value)
    {
        return sizeof(float);
    }
    public static uint Size(double value)
    {
        return sizeof(double);
    }
    public static uint Size(string value)
    {
        uint size = sizeof(ushort);
        size += (ushort)value.Length;
        return size;
    }
}
