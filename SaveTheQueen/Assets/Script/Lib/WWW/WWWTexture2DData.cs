using UnityEngine;

namespace Lib.www
{

    public class WWWTexture2DData : IWWWDataType
    {
        public Texture2D texture
        {
            get;
            private set;
        }

        public void GetDataFromWWW(WWWQuery.WWWResult result, WWW www)
        {
            if (result == WWWQuery.WWWResult.Success)
            {
                texture = www.texture;
            }
            else
            {
                texture = null;
            }
        }
    }
}