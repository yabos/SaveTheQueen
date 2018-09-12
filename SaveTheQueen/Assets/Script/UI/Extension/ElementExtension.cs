using UnityEngine;
using System.Collections;
using System.Linq;
using Lib.Pattern;

namespace UIExtension
{
    public static class ElementExtension
    {
        public static T[] GetElementArray<T>(T source, int count) where T : MonoBehaviour
        {
            T[] values = new T[count];
            T[] t = Lib.Pattern.ComponentFactory.GetChildComponents<T>(source.transform.parent.gameObject, Lib.Pattern.IfNotExist.ReturnNull);

            if (t != null && t.Any() && t.Count() > count)
            {
                for (int i = 0; i < t.Count(); i++)
                {
                    if (i < count)
                    {
                        values[i] = t[i];
                    }
                    else
                    {
                        GameObjectFactory.Destroy(t[i].gameObject);
                    }
                }
            }
            else if (t != null && t.Any() && t.Count() < count)
            {
                for (int i = 0; i < count; i++)
                {
                    if (i < t.Count())
                    {
                        values[i] = t[i];
                    }
                    else
                    {
                        values[i] = (UnityEngine.GameObject.Instantiate(source, source.transform.parent) as T);

                        values[i].transform.localScale = Vector3.one;
                        values[i].transform.localPosition = Vector3.zero;
                    }
                }
            }
            else if (t != null && t.Any() && t.Count() == count)
            {
                for (int i = 0; i < count; i++)
                {
                    values[i] = t[i];
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    values[i] = (UnityEngine.GameObject.Instantiate(source, source.transform.parent) as T);

                    values[i].transform.localScale = Vector3.one;
                    values[i].transform.localPosition = Vector3.zero;
                }
            }

            return values;
        }
    }
}
