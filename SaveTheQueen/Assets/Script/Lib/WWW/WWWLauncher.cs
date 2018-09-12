using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Lib.www
{
    public class WWWLauncher : MonoBehaviour
    {
        private static WWWLauncher m_instance = null;
        private static bool m_shutdown = false;

        public delegate void OnCallbackDownload(WWW www_);
        class Task
        {
            public OnCallbackDownload callback;
            public string url;
        }

        private List<Task> m_messageQueue = new List<Task>();

        public static WWWLauncher Instance
        {
            get
            {
                if (m_shutdown)
                {
                    return null;
                }

                // Instance requiered for the first time, we look for it
                if (m_instance == null)
                {
                    m_instance = GameObject.FindObjectOfType(typeof(WWWLauncher)) as WWWLauncher;

                    // Object not found, we create a temporary one
                    if (m_instance == null)
                    {
                        m_instance = new GameObject("Temp Instance of " + typeof(WWWLauncher).ToString(), typeof(WWWLauncher)).GetComponent<WWWLauncher>();
                        DontDestroyOnLoad(m_instance);
                    }

                    // Problem during the creation, this should not happen
                    if (m_instance == null)
                    {
                        Debug.LogError("Problem during the creation of " + typeof(WWWLauncher).ToString());
                    }
                }

                return m_instance;
            }
        }

        public void Init()
        {
            if (Application.isPlaying)
            {
                DontDestroyOnLoad(gameObject);
            }
        }


        void Update()
        {
            // process (at most) one message from queue
            Task task = null;
            lock (m_messageQueue)
            {
                if (m_messageQueue.Count > 0)
                {
                    task = m_messageQueue[0];
                    m_messageQueue.RemoveAt(0);
                }
            }

            if (null != task)
            {
                StartCoroutine(WWWGet_Internal(task.url, task.callback));
            }

        }

        private void OnApplicationQuit()
        {
            if (m_instance != null)
            {
                GameObject.Destroy(m_instance);
            }
            m_instance = null;
            m_shutdown = true;
        }

        public static void WWWGet(string getUrl, OnCallbackDownload callback = null)
        {
            Instance.StartCoroutine(WWWGet_Internal(getUrl, callback));
        }

        public static void WWWQueueGet(string getUrl, OnCallbackDownload callback = null)
        {
            lock (Instance.m_messageQueue)
            {
                Task task = new Task();
                task.url = getUrl;
                task.callback = callback;

                Instance.m_messageQueue.Add(task);
            }
        }

        public static void WWWPost(string postUrl, WWWForm postForm)
        {
            Instance.StartCoroutine(WWWPost_Internal(postUrl, postForm));
        }

        private static IEnumerator WWWGet_Internal(string getUrl, OnCallbackDownload callback)
        {
            WWW www = new WWW(getUrl);
            yield return www;

            if (null != callback)
            {
                callback(www);
            }

            www.Dispose();
        }

        private static IEnumerator WWWPost_Internal(string postUrl, WWWForm postForm)
        {
            WWW www = new WWW(postUrl, postForm);
            yield return www;

            www.Dispose();
        }

        public static void StartWWW(string url, WWWQuery.OnWWWHandler<WWWTextData> onWWWHandler, float time = 10.0f, int retryCount = 0, params object[] callbackArgs)
        {
            WWWQuery.StartWWW<WWWTextData>(Instance, url, onWWWHandler, time, retryCount, callbackArgs);
        }

        public static void StartWWW(string url, WWWQuery.OnWWWHandler<WWWTexture2DData> onWWWHandler, float time = 10.0f, int retryCount = 0, params object[] callbackArgs)
        {
            WWWQuery.StartWWW<WWWTexture2DData>(Instance, url, onWWWHandler, time, retryCount, callbackArgs);
        }

        public static void StartWWW<DataType>(string url, WWWQuery.OnWWWHandler<DataType> onWWWHandler, float time, int retryCount = 0, params object[] callbackArgs) where DataType : IWWWDataType, new()
        {
            WWWQuery.StartWWW<DataType>(Instance, url, onWWWHandler, time, retryCount, callbackArgs);
        }


    }

    public static class WWWExtension
    {
        public static void AddFieldEx(this WWWForm form, string fieldName, string value)
        {
            form.AddField(fieldName, value ?? "");
        }

        public static void AddBinaryDataEx(this WWWForm form, string fieldName, byte[] contents)
        {
            form.AddBinaryData(fieldName, contents);
        }

        public static void AddFieldEx(this WWWForm form, string fieldName, int value)
        {
            form.AddField(fieldName, value);
        }
    }

}