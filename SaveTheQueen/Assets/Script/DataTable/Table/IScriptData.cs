using System.IO;
using Aniz.Resource;
using Aniz.Resource.Unit;
using XOR;

namespace Aniz.Data
{
    public interface IScriptData
    {
        bool Load(string filename);
        void Release();
    }

    public abstract class BinaryScriptData : IScriptData
    {
        private static readonly byte[] encryptKey = System.Text.Encoding.UTF8.GetBytes("MoreC0mplexMoleculesTargeted");

        public bool Load(string filename)
        {
            ScriptResource scriptResource = Global.ResourceMgr.CreateScriptResource(filename, ePath.Data);
            bool result = false;
            byte[] bytes = XOREncryption.Decrypt(scriptResource.TexObject.bytes, encryptKey);
            using (Stream stream = new MemoryStream(bytes))
            {
                BinaryDecoder decoder = new BinaryDecoder(stream);
                result = ReadData(decoder);
            }

            Global.ResourceMgr.DestoryResource(scriptResource);

            return result;
        }

        public abstract void Release();
        protected abstract bool ReadData(BinaryDecoder decoder);

    }
}