
namespace Aniz.Data
{
    public class StatusEffectData : BinaryScriptData
    {
        public override void Release()
        {
        }

        protected override bool ReadData(BinaryDecoder decoder)
        {
            return true;
        }
    }
}