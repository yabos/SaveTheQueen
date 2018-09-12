using System;
using System.IO;
using UnityEngine;

namespace Aniz.Utils
{
    public static class ColorUtil
    {
        public static uint GetColor(Color color)
        {
            Color32 value = color;
            return ((uint)value.r << 24) | ((uint)value.g << 16) | ((uint)value.b << 8) | (uint)value.a;
        }

        public static Color GetColor(uint color)
        {
            uint r = (color >> 24) & 0xff;
            uint g = (color >> 16) & 0xff;
            uint b = (color >> 8) & 0xff;
            uint a = color & 0xff;

            return new Color(r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f);
        }
    }

    public static class DateTimeUtil
    {
        public static bool Equals(System.DateTime x, System.DateTime y)
        {
            return (x.Second == y.Second && x.Minute == y.Minute && x.Hour == y.Hour && x.Day == y.Day && x.Month == y.Month && x.Year == y.Year);
        }
    }

    public static class DeviceSleeperUtil
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        static private int s_flag = 128;  // "WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON"
        static private AndroidJavaObject s_currentActivity;

        static void SetWindowFlag()
        {
            AndroidJavaObject playerWindow = s_currentActivity.Call<AndroidJavaObject>("getWindow");
            if (playerWindow != null)
            {
                playerWindow.Call("addFlags", s_flag);
            }
        }

        static void ClearWindowFlag()
        {
            AndroidJavaObject playerWindow = s_currentActivity.Call<AndroidJavaObject>("getWindow");
            if (playerWindow != null)
            {
                playerWindow.Call("clearFlags", s_flag);
            }
        }

        static bool GetCurrentActivity()
        {
            AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            if (playerClass != null)
            {
                s_currentActivity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");
            }
            else
            {
                s_currentActivity = null;
            }

            return (s_currentActivity != null);
        }
#endif

        static public void AllowDeviceToSleep(bool bAllow)
        {
            if (bAllow)
            {
                Screen.sleepTimeout = SleepTimeout.SystemSetting;

#if UNITY_ANDROID && !UNITY_EDITOR
                if (GetCurrentActivity())
                {
                    s_currentActivity.Call("runOnUiThread", (AndroidJavaRunnable)ClearWindowFlag);
                }
#endif
            }
            else
            {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;

#if UNITY_ANDROID && !UNITY_EDITOR
                if (GetCurrentActivity())
                {
                    s_currentActivity.Call("runOnUiThread", (AndroidJavaRunnable)SetWindowFlag);
                }
#endif
            }
        }
    }

    static public class FileUtil
    {
        static public string GetFileName(string srcPath, bool bRemoveExtension)
        {
            string name = srcPath;
            int lastSlashIndex;

            lastSlashIndex = name.LastIndexOfAny(new char[] { '/', '\\' });
            if (lastSlashIndex >= 0)
            {
                name = name.Substring(lastSlashIndex + 1);
            }

            if (bRemoveExtension)
            {
                int lastDotIndex = name.LastIndexOf('.');
                if (lastDotIndex >= 0)
                {
                    name = name.Substring(0, lastDotIndex);
                }
            }

            return name;
        }

        static public string GetFileExt(string srcPath)
        {
            if (null == srcPath)
            {
                return null;
            }

            int lastDotIndex = srcPath.LastIndexOfAny(new char[] { '/', '\\', '.' });

            if (lastDotIndex < 0 ||
                srcPath[lastDotIndex] == '/' ||
                srcPath[lastDotIndex] == '\\')
            {
                return string.Empty;
            }
            // The extension of the specified path (including the period ".")
            return srcPath.Substring(lastDotIndex);
        }

        static public string GetDirectory(string srcPath)
        {
            int lastSlashIndex = srcPath.LastIndexOfAny(new char[] { '/', '\\' });
            string directory = (lastSlashIndex > 0) ? srcPath.Substring(0, lastSlashIndex) : string.Empty;
            return directory;
        }

        static public string CombinePath(string lhPath, string rhPath)
        {
            if ((lhPath == null) || (rhPath == null))
            {
                throw new System.ArgumentNullException((lhPath == null) ? "lhPath" : "rhPath");
            }
            if (rhPath.Length == 0)
            {
                return lhPath;
            }
            if (lhPath.Length == 0)
            {
                return rhPath;
            }

            string output = lhPath + (lhPath.EndsWith(Path.DirectorySeparatorChar.ToString()) ? rhPath : Path.DirectorySeparatorChar + rhPath);

            return output;
        }
    }

    public static class URLUtil
    {

        /// <summary>
        /// Combine the specified url part1 and part2.
        /// e.g.
        /// CombineUrl("http://www.163.com/", "/logo.jpg"),
        /// return "http://www.163.com/logo.jpg";
        /// </summary>
        /// <returns>The Combined URL.</returns>
        public static string CombineUrl(string part1, string part2)
        {
            if (part2.StartsWith("/"))
            {
                part1 = CheckUrlSplash(part1, 0);
            }
            else
            {
                part1 = CheckUrlSplash(part1, 1);
            }

            return part1 + part2;
        }

        /// <summary>
        /// part1 appends the part2 derectly.
        /// </summary>
        public static string AppendUrl(string part1, string part2)
        {
            if (part2.StartsWith("?") || part2.StartsWith("."))
            {
                part1 = CheckUrlSplash(part1, 0);
            }
            return part1 + part2;
        }

        /// <summary>
        /// Checks the ending interrogation of URL.
        /// if reserve==true, reserve 1 '?', else remove all ending '?'.
        /// </summary>
        public static string CheckUrlInterrogation(string url, bool reserve)
        {
            string ret = url;

            if (reserve == true)
            {
                if (!ret.EndsWith("?"))
                {
                    ret += "?";
                }
            }
            else
            {
                if (ret.EndsWith("?"))
                {
                    while (ret.EndsWith("?"))
                    {
                        ret = ret.Substring(0, ret.Length - 1);
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Checks the URL splash.
        /// if reserveSplash == true: reserve 1 ending '/';
        /// reserveSplash == false: remove all ending '/'.
        /// </summary>
        /// <returns>The URL splash.</returns>
        public static string CheckUrlSplash(string url, bool reserveSplash)
        {
            int spCount = reserveSplash ? 1 : 0;

            return CheckUrlSplash(url, spCount);
        }

        /// <summary>
        /// Checks the URL splash.
        /// if reservedSplashCount <= 0, remove all ending '/',
        /// </summary>
        /// <returns>The URL splash.</returns>
        public static string CheckUrlSplash(string url, int reservedSplashCount)
        {
            string ret = url;

            if (!string.IsNullOrEmpty(ret))
            {
                while (ret.EndsWith("/"))
                {
                    ret = ret.Substring(0, ret.Length - 1);
                }

                if (reservedSplashCount > 0)
                {
                    ret += new string('/', reservedSplashCount);
                }
            }
            else
            {
                Debug.LogWarning("--the url you check is null or empty...");
            }

            return ret;
        }
    }

    public static class DataParserAssist
    {
        static public T EnumParser<T>(string s)
        {
            return (T)Enum.Parse(typeof(T), s);
        }

        static public uint UintParser(string s)
        {
            uint data;
            uint.TryParse(s, out data);
            return data;
        }

        static public float FloatParser(string s)
        {
            float data;
            float.TryParse(s, out data);
            return data;
        }

        static public int IntParser(string s)
        {
            int data;
            int.TryParse(s, out data);
            return data;
        }

        static public bool BoolParser(string s)
        {
            int data;
            int.TryParse(s, out data);
            if (data >= 1)
            {
                return true;
            }
            return false;
        }

        static public Vector3 Vector3Parser(string s)
        {
            Vector3 data = Vector3.zero;
            string[] split = s.Split(',');
            if (split.Length >= 2)
            {
                data.x = FloatParser(split[0]);
                data.y = FloatParser(split[1]);
                data.z = FloatParser(split[2]);
            }
            return data;
        }

        static public string ToVector3String(Vector3 vec3)
        {
            return string.Format("{0},{1},{2}", vec3.x, vec3.y, vec3.z);
        }
    }
}