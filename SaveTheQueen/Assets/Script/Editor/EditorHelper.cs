using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class EditorHelper
{
    private const BindingFlags Binding_FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
    private const BindingFlags Binding_FLAGS_IGNORECASE = Binding_FLAGS | BindingFlags.IgnoreCase;

    public static T GetTartgetObject<T>(SerializedProperty prop) where T : new()
    {
        return (T)GetTargetObjectOfProperty(prop);
    }

    public static MethodInfo GetMethodInfo(object obj, string methodName)
    {
        return obj.GetType().GetMethod(methodName, Binding_FLAGS);
    }

    public static PropertyInfo GetPropertyInfo(object obj, string propertyName)
    {
        return obj.GetType().GetProperty(propertyName, Binding_FLAGS);
    }

    private static object GetTargetObjectOfProperty(SerializedProperty prop)
    {
        var path = prop.propertyPath.Replace(".Array.data[", "[");
        object obj = prop.serializedObject.targetObject;
        var elements = path.Split('.');
        foreach (var element in elements)
        {
            if (element.Contains("["))
            {
                var elementName = element.Substring(0, element.IndexOf("["));
                var index = System.Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                obj = GetValue_Imp(obj, elementName, index);
            }
            else
            {
                obj = GetValue_Imp(obj, element);
            }
        }
        return obj;
    }

    private static object GetValue_Imp(object source, string name)
    {
        if (source == null)
        {
            return null;
        }

        var type = source.GetType();

        while (type != null)
        {
            var f = type.GetField(name, Binding_FLAGS);
            if (f != null)
            {
                return f.GetValue(source);
            }

            var p = type.GetProperty(name, Binding_FLAGS_IGNORECASE);
            if (p != null)
            {
                return p.GetValue(source, null);
            }

            type = type.BaseType;
        }
        return null;
    }

    private static object GetValue_Imp(object source, string name, int index)
    {
        var enumerable = GetValue_Imp(source, name) as System.Collections.IEnumerable;
        if (enumerable == null)
        {
            return null;
        }

        var enm = enumerable.GetEnumerator();

        for (int i = 0; i <= index; i++)
        {
            if (!enm.MoveNext())
            {
                return null;
            }
        }
        return enm.Current;
    }

    public static void GetArrayElement(SerializedProperty prop, int index, string PropertyRelative,
        ref SerializedProperty element, ref SerializedProperty targetObj, ref GameObject targetGO)
    {
        element = prop.GetArrayElementAtIndex(index);
        targetObj = element.FindPropertyRelative(PropertyRelative);
        targetGO = (GameObject)targetObj.objectReferenceValue;
    }

    public static void LookAtSelectionObject(GameObject selection, bool lookAt = true, Vector3 assignPos = new Vector3())
    {
        EditorGUIUtility.PingObject(selection);
        Selection.activeObject = selection;

        if (lookAt)
        {
            SceneView sv = SceneView.lastActiveSceneView;
            if (null != sv)
            {
                if (false == assignPos.Equals(Vector3.zero))
                    sv.LookAt(assignPos, Quaternion.Euler(45f, 0f, 0f));
                else
                    sv.LookAt(selection.transform.position, Quaternion.Euler(45f, 0f, 0f));
            }
        }
    }

    public static void UnloadResources()
    {
        EditorUtility.UnloadUnusedAssetsImmediate();
        Resources.UnloadUnusedAssets();
    }

    public static List<T> FindObjectsInScene<T>() where T : Object
    {
        T[] objects = Resources.FindObjectsOfTypeAll(typeof(T)) as T[];

        List<T> list = new List<T>();

        foreach (T o in objects)
        {
            if (o.hideFlags == 0)
            {
                string path = AssetDatabase.GetAssetPath(o);
                if (string.IsNullOrEmpty(path))
                {
                    //Debug.Log(o.name);
                    list.Add(o);
                }
            }
        }
        return list;
    }

    public static List<T> FindComponentsInScene<T>() where T : Component
    {
        T[] comps = Resources.FindObjectsOfTypeAll(typeof(T)) as T[];

        List<T> list = new List<T>();

        foreach (T comp in comps)
        {
            if (comp == null)
                continue;
            if (comp.gameObject == null)
                continue;
            if (comp.gameObject.hideFlags == 0)
            {
                string path = AssetDatabase.GetAssetPath(comp.gameObject);
                if (string.IsNullOrEmpty(path))
                {
                    //Debug.Log(comp.name);
                    list.Add(comp);
                }
            }
        }
        return list;
    }

    public static List<Object> FindComponentsInScene(System.Type type)
    {
        Object[] objects = Resources.FindObjectsOfTypeAll(type);

        List<Object> list = new List<Object>();

        foreach (Object o in objects)
        {
            Component comp = o as Component;
            if (comp == null)
                continue;
            if (comp.gameObject == null)
                continue;
            if (comp.gameObject.hideFlags == 0)
            {
                string path = AssetDatabase.GetAssetPath(comp.gameObject);
                if (string.IsNullOrEmpty(path))
                {
                    //Debug.Log(comp.name);
                    list.Add(comp);
                }
            }
        }
        return list;
    }

    public static List<T> FindComponentsInResources<T>() where T : Component
    {
        T[] comps = Resources.FindObjectsOfTypeAll(typeof(T)) as T[];

        List<T> list = new List<T>();

        foreach (T comp in comps)
        {
            if (comp.gameObject.hideFlags == 0)
            {
                string path = AssetDatabase.GetAssetPath(comp.gameObject);
                if (!string.IsNullOrEmpty(path))
                {
                    //Debug.Log(comp.name);
                    list.Add(comp);
                }
            }
        }
        return list;
    }
}