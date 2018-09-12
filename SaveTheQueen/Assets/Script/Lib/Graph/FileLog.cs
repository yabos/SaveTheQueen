#if UNITY_EDITOR
#define DEBUG_FILE_LOG
#endif

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;

namespace Aniz.Graph
{
    public static class FileLog
    {
        private static Dictionary<string, StreamWriter> catWriterDic = new Dictionary<string, StreamWriter>();

        private static StreamWriter GetWriter(string category)
        {
            StreamWriter writer;
            if (!catWriterDic.TryGetValue(category, out writer))
            {
                writer = File.CreateText(Application.persistentDataPath + "/" + category);
                writer.AutoFlush = true;
                catWriterDic.Add(category, writer);
            }

            Debug.Assert(writer != null);
            return writer;
        }

        [System.Diagnostics.Conditional("DEBUG_FILE_LOG")]
        public static void Log(string category, string msg)
        {
            GetWriter(category).WriteLine(msg);
        }

        [System.Diagnostics.Conditional("DEBUG_FILE_LOG")]
        public static void Log(string category, string prefix, Vector3 v3)
        {
            string msg = "";
            Ut.Vector3ToString(prefix, v3, ref msg);
            Log(category, msg);
        }

        [System.Diagnostics.Conditional("DEBUG_FILE_LOG")]
        public static void Log(string category, string prefix, Quaternion q)
        {
            string msg = "";
            Ut.QuaternionToString(prefix, q, ref msg);
            Log(category, msg);
        }

        [System.Diagnostics.Conditional("DEBUG_FILE_LOG")]
        public static void CloseCategory(string category)
        {
            StreamWriter writer;
            if (catWriterDic.TryGetValue(category, out writer))
            {
                writer.Close();
                catWriterDic.Remove(category);
            }
        }

        [System.Diagnostics.Conditional("DEBUG_FILE_LOG")]
        public static void CloseCategoryAll()
        {
            foreach (KeyValuePair<string, StreamWriter> entry in catWriterDic)
            {
                entry.Value.Close();
            }

            catWriterDic.Clear();
        }
    }
}
