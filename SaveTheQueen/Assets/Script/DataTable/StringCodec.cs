using System.IO;

public class StringCodec
{

    //public static bool DecodeUTF8String(BinaryDecoder decoder, out string value)
    //{
    //    try
    //    {
    //        int size = decoder.DecodeInt32();
    //        value = System.Text.Encoding.UTF8.GetString(decoder.DecodeBytes(size));
    //    }
    //    catch (System.Exception)
    //    {
    //        value = string.Empty;
    //        return false;
    //    }

    //    return true;
    //}

    public static bool DecodeUnicodeString(BinaryDecoder decoder, out string value)
    {
        try
        {
            ushort length = decoder.DecodeUInt16();
            value = System.Text.Encoding.Unicode.GetString(decoder.DecodeBytes(length * 2));
        }
        catch (System.Exception)
        {
            value = string.Empty;
            return false;
        }

        return true;
    }

    //public static bool EncodeUTF8String(BinaryEncoder encoder, string value)
    //{
    //    try
    //    {
    //        byte[] encoded_str = System.Text.Encoding.UTF8.GetBytes(value);
    //        encoder.Encode((uint)encoded_str.Length);
    //        encoder.Encode(encoded_str);
    //    }
    //    catch (System.Exception)
    //    {
    //        return false;
    //    }

    //    return true;
    //}

    public static bool EncodeUnicodeString(BinaryEncoder encoder, string value)
    {
        try
        {
            encoder.Encode((ushort)value.Length);
            byte[] encoded_str = System.Text.Encoding.Unicode.GetBytes(value);
            encoder.Encode(encoded_str);
        }
        catch (System.Exception)
        {
            return false;
        }

        return true;
    }
    
    //public static uint SizeUTF8String(string value)
    //{
    //    uint size = sizeof(ushort);
    //    size += (uint)System.Text.Encoding.UTF8.GetByteCount(value);
    //    return size;
    //}
    public static uint SizeUnicodeString(string value)
    {
        uint size = sizeof(ushort);
        size += (ushort)System.Text.Encoding.Unicode.GetByteCount(value);
        return size;
    }

}
