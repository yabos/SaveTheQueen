using UnityEngine;

namespace Lib.www
{
    public class WWWTextData : IWWWDataType
    {
        public string text
        {
            get;
            private set;
        }

        public WWWTextData()
        {
            text = text = string.Empty;
        }

        public void GetDataFromWWW(WWWQuery.WWWResult result, WWW www)
        {

            if (result == WWWQuery.WWWResult.Success)
            {
                using (System.IO.StreamReader sw = new System.IO.StreamReader(new System.IO.MemoryStream(www.bytes)))
                {
                    text = sw.ReadToEnd();
                }
            }
            else if (result == WWWQuery.WWWResult.Failed)
            {
                text = www.error;
            }
            else // WWWQuery.WWWResult.Timeout
            {
                text = string.Empty;
            }
        }
    }
}