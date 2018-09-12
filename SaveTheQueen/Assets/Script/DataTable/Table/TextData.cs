using table.db;

namespace Aniz.Data
{
    public class TextData : BinaryScriptData
    {
        private DB_UITextList m_dbUiTextList = new DB_UITextList();

        public override void Release()
        {
        }

        protected override bool ReadData(BinaryDecoder decoder)
        {
            if (m_dbUiTextList.Decode(decoder))
                return true;

            return false;
        }
    }
}