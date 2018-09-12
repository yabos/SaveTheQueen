

using System;
using System.Reflection;

public static class GUISearchFieldUtil
{
    public static System.Object DoInvoke(Type type, string methodName, System.Object[] parameters)
    {
        Type[] types = new Type[parameters.Length];
        for (int i = 0; i < parameters.Length; i++)
        {
            types[i] = parameters[i].GetType();
        }
        MethodInfo method = type.GetMethod(methodName, (BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public), null, types, null);
        return DoInvokeInternal(type, method, parameters);
    }

    private static System.Object DoInvokeInternal(Type type, MethodInfo method, System.Object[] parameters)
    {
        if (method.IsStatic)
        {
            return method.Invoke(null, parameters);
        }

        System.Object obj = type.InvokeMember(null,
        BindingFlags.DeclaredOnly |
        BindingFlags.Public | BindingFlags.NonPublic |
        BindingFlags.Instance | BindingFlags.CreateInstance, null, null, new System.Object[0]);
        return method.Invoke(obj, parameters);
    }
}