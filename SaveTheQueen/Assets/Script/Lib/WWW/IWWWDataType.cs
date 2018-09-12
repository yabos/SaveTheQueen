using UnityEngine;

namespace Lib.www
{
    public interface IWWWDataType
    {
        // defines the way that an IWWWDataType object populates data from given WWWQuery.WWWResult and www
        void GetDataFromWWW(WWWQuery.WWWResult result, WWW www);
    }
}