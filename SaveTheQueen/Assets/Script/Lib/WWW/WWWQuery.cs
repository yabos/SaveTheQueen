using System.Collections;
using UnityEngine;

namespace Lib.www
{
    public class WWWQuery
    {
        public enum WWWResult
        {
            Failed,
            Success,
            Timeout
        }

        private WWW m_www;
        public int RetryCount { get; set; }
        private bool Timeout { get; set; }

        public delegate void OnWWWHandler<DataType>(WWWResult result, DataType data, params object[] callbackArgs) where DataType : IWWWDataType;

        public static void StartWWW<DataType>(MonoBehaviour scheduler, string url, OnWWWHandler<DataType> onWWWHandler, float time, int retryCount, params object[] callbackArgs) where DataType : IWWWDataType, new()
        {
            WWWQuery handler = new WWWQuery();
            handler.RetryCount = retryCount > 0 ? retryCount : 1;
            scheduler.StartCoroutine(handler.StartWWW_Internal<DataType>(scheduler, url, onWWWHandler, time, callbackArgs));
        }

        private IEnumerator StartWWW_Internal<DataType>(MonoBehaviour scheduler, string url, OnWWWHandler<DataType> onWWWHandler, float time, params object[] callbackArgs) where DataType : IWWWDataType, new()
        {
            WWWResult result = WWWResult.Failed;

            for (int i = 0; i < RetryCount; ++i)
            {
                if (null != m_www)
                {
                    m_www.Dispose();
                    m_www = null;
                }

                m_www = new WWW(url);
                // Start Timer
                scheduler.StartCoroutine(WaitWWW<DataType>(url, time, onWWWHandler, callbackArgs));

                yield return m_www;

                if (!Timeout)
                {
                    if (m_www.error != null)
                    {
                        Debug.LogWarning(string.Format("[WWWQuery] failed #{0}({1}): {2}", i, m_www.error, url));
                        result = WWWResult.Failed;
                    }
                    else
                    {
                        result = WWWResult.Success;
                        break;
                    }
                }

                yield return new WaitForSeconds(3.0f);
            }

            if (!Timeout)
            {
                if (onWWWHandler != null)
                {
                    DataType wwwData = new DataType();
                    wwwData.GetDataFromWWW(result, m_www);
                    onWWWHandler(result, wwwData, callbackArgs);
                }
            }

            if (null != m_www)
            {
                m_www.Dispose();
                m_www = null;
            }
        }

        private IEnumerator WaitWWW<DataType>(string url, float time, OnWWWHandler<DataType> onWWWHandler, params object[] callbackArgs) where DataType : IWWWDataType, new()
        {
            // 		if (time < TableSettings.MinHTTPTimeoutTime)
            // 		{
            // 			Debug.LogWarning("WWW timeout is too small. time(" + time + ") Threshold(" + TableSettings.MinHTTPTimeoutTime + ")");
            // 			time = TableSettings.MinHTTPTimeoutTime;
            // 		}

            yield return new WaitForSeconds(time);

            if (m_www != null && !m_www.isDone)
            {
                Timeout = true;

                m_www.Dispose();
                m_www = null;

                Debug.LogWarning("[WWWQuery] timeout(" + time + "): " + url);

                if (onWWWHandler != null)
                {
                    DataType wwwData = new DataType();
                    wwwData.GetDataFromWWW(WWWResult.Timeout, null);

                    onWWWHandler(WWWResult.Timeout, wwwData, callbackArgs);
                }
            }
        }

    }
}