
namespace XOR
{
    public class XOREncryption
    {
        public static byte[] Encrypt(byte[] data, byte[] key)
        {
            byte[] encryptedBytes = new byte[data.Length];
            for (int i = 0; i < data.Length; ++i)
            {
                encryptedBytes[i] = (byte)((data[i] ^ key[i % key.Length]) + key.Length);
            }

            return encryptedBytes;
        }

        public static byte[] Decrypt(byte[] data, byte[] key)
        {
            byte[] decryptedBytes = new byte[data.Length];
            for (int i = 0; i < data.Length; ++i)
            {
                decryptedBytes[i] = (byte)((data[i] - key.Length) ^ key[i % key.Length]);
            }

            return decryptedBytes;
        }
    }
}

