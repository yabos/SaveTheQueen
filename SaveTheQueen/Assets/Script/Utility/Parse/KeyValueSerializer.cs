using System.Collections.Generic;
using UnityEngine;
public class KeyValueSerializer
{
    private char tokenSeparator = ',';
    private char keyValueSeparator = ':';

    private char[] tokenSeparators = null;

    public bool loading = false;
    private string serializedText = "";
    private Dictionary<string, string> keyValueMap = new Dictionary<string, string>();

    public KeyValueSerializer()
    {
        tokenSeparators = new char[] { tokenSeparator };
    }

    public KeyValueSerializer(char tokenSeparator, char keyValueSeparator)
    {
        this.tokenSeparator = tokenSeparator;
        this.keyValueSeparator = keyValueSeparator;

        tokenSeparators = new char[] { tokenSeparator };
    }

    public string SerializedText
    {
        get
        {
            return serializedText;
        }
    }

    public void Serialize<T>(string key, ref T value)
    {
        if (loading)
        {
            string storedValue = GetValue(key);
            if (storedValue != null)
            {
                if (!StringConverter.TryConvert<T>(storedValue, out value))
                {
                    // error
                    Debug.LogError("Couldn't convert " + key);
                }
            }
            else
            {
                //value = default(T);
            }
        }
        else
        {
            if (serializedText != "")
                serializedText += tokenSeparator;
            serializedText += key + keyValueSeparator;
            if (value == null)
            {
                serializedText += default(T);
            }
            else if (value.GetType() == typeof(Vector3))
            {
                string v = value.ToString();
                v = v.Substring(1, v.Length - 2).Replace(',', '|');
                v = v.Replace(" ", "");
                serializedText += v;
            }
            else if (value.GetType() == typeof(Quaternion))
            {
                string v = value.ToString();
                v = v.Substring(1, v.Length - 2).Replace(',', '|');
                v = v.Replace(" ", "");
                serializedText += v;
            }
            else if (value.GetType() == typeof(Color))
            {
                object o = value;
                Color32 c = (Color)o;
                uint color = ((uint)c.r) << 24;
                color |= ((uint)c.g) << 16;
                color |= ((uint)c.b) << 8;
                color |= ((uint)c.a);
                serializedText += color.ToString();
            }
            else if (value.GetType().IsEnum)
            {
                // see to BuildConversionMap;
                serializedText += value.ToString();
            }
            else
            {
                serializedText += value.ToString();
            }
        }
    }

    private string GetValue(string key)
    {
        string value;
        if (!keyValueMap.TryGetValue(key, out value))
        {
            // error
            //Debug.LogError("Couldn't get value " + key);
            return null;
        }
        else
        {
            //Debug.LogWarning(key + " : " + value);
        }
        return value;
    }

    public void Deserialize(string serializedText)
    {
        loading = true;

        keyValueMap.Clear();

        if (serializedText == null)
            return;

        string[] tokens = serializedText.Split(tokenSeparators, System.StringSplitOptions.None);
        for (int i = 0; i < tokens.Length; ++i)
        {
            string token = tokens[i];
            string key, value;

            int separatorIndex = token.IndexOf(keyValueSeparator);
            if (separatorIndex != -1)
            {
                key = token.Substring(0, separatorIndex);
                value = token.Substring(separatorIndex + 1, token.Length - separatorIndex - 1);

                keyValueMap.Add(key, value);
            }
        }
    }

}