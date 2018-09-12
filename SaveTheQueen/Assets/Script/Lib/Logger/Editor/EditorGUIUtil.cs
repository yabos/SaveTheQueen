using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections.Generic;
//using TAGENIGMA.TreeView;

public static class EditorGUIUtil
{
    static Texture2D mContrastTex;

    static public Texture2D blankTexture
    {
        get
        {
            return EditorGUIUtility.whiteTexture;
        }
    }

    static public Texture2D contrastTexture
    {
        get
        {
            if (mContrastTex == null) mContrastTex = CreateCheckerTex(
                new Color(0f, 0f, 0f, 0.5f),
                new Color(1f, 1f, 1f, 0.5f));
            return mContrastTex;
        }
    }


    static Texture2D CreateCheckerTex(Color c0, Color c1)
    {
        Texture2D tex = new Texture2D(16, 16);
        tex.name = "[Generated] Checker Texture";
        tex.hideFlags = HideFlags.DontSave;

        for (int y = 0; y < 8; ++y) for (int x = 0; x < 8; ++x) tex.SetPixel(x, y, c1);
        for (int y = 8; y < 16; ++y) for (int x = 0; x < 8; ++x) tex.SetPixel(x, y, c0);
        for (int y = 0; y < 8; ++y) for (int x = 8; x < 16; ++x) tex.SetPixel(x, y, c0);
        for (int y = 8; y < 16; ++y) for (int x = 8; x < 16; ++x) tex.SetPixel(x, y, c1);

        tex.Apply();
        tex.filterMode = FilterMode.Point;
        return tex;
    }


    static public void DrawTiledTexture(Rect rect, Texture tex)
    {
        GUI.BeginGroup(rect);
        {
            int width = Mathf.RoundToInt(rect.width);
            int height = Mathf.RoundToInt(rect.height);

            for (int y = 0; y < height; y += tex.height)
            {
                for (int x = 0; x < width; x += tex.width)
                {
                    GUI.DrawTexture(new Rect(x, y, tex.width, tex.height), tex);
                }
            }
        }
        GUI.EndGroup();
    }


    static public void DrawOutline(Rect rect)
    {
        if (Event.current.type == EventType.Repaint)
        {
            Texture2D tex = contrastTexture;
            GUI.color = Color.white;
            DrawTiledTexture(new Rect(rect.xMin, rect.yMax, 1f, -rect.height), tex);
            DrawTiledTexture(new Rect(rect.xMax, rect.yMax, 1f, -rect.height), tex);
            DrawTiledTexture(new Rect(rect.xMin, rect.yMin, rect.width, 1f), tex);
            DrawTiledTexture(new Rect(rect.xMin, rect.yMax, rect.width, 1f), tex);
        }
    }


    static public void DrawOutline(Rect rect, Color color)
    {
        if (Event.current.type == EventType.Repaint)
        {
            Texture2D tex = blankTexture;
            GUI.color = color;
            GUI.DrawTexture(new Rect(rect.xMin, rect.yMin, 1f, rect.height), tex);
            GUI.DrawTexture(new Rect(rect.xMax, rect.yMin, 1f, rect.height), tex);
            GUI.DrawTexture(new Rect(rect.xMin, rect.yMin, rect.width, 1f), tex);
            GUI.DrawTexture(new Rect(rect.xMin, rect.yMax, rect.width, 1f), tex);
            GUI.color = Color.white;
        }
    }

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

    public static void Label(string text, bool selected, int width, int padding)
    {
        GUIStyle guiStyle = null;
        if (selected)
        {
            guiStyle = EditorGUIStyle.Label.SelectedStyle;
        }
        else
        {
            guiStyle = EditorGUIStyle.Label.UnselectedStyle;
        }
        int oldPadding = guiStyle.padding.left;
        guiStyle.padding.left = padding;

        if (width != 0)
        {
            GUILayout.Label(text, guiStyle, GUILayout.Width(width));
        }
        else
        {
            GUILayout.Label(text, guiStyle);
        }
        guiStyle.padding.left = oldPadding;
    }


    public static void Label(string text, bool selected, int width, int padding, Color textColor)
    {
        GUIStyle guiStyle = null;
        if (selected)
        {
            guiStyle = EditorGUIStyle.Label.SelectedStyle;
        }
        else
        {
            guiStyle = EditorGUIStyle.Label.UnselectedStyle;
        }
        int oldPadding = guiStyle.padding.left;
        guiStyle.padding.left = padding;
        guiStyle.normal.textColor = textColor;

        if (width != 0)
        {
            GUILayout.Label(text, guiStyle, GUILayout.Width(width));
        }
        else
        {
            GUILayout.Label(text, guiStyle);
        }
        guiStyle.padding.left = oldPadding;
    }

    public static int IntTextField(int value, int width)
    {
        string text = value.ToString();
        if (width != 0)
        {
            text = GUILayout.TextField(text, GUILayout.Width(width));
        }
        else
        {
            text = GUILayout.TextField(text);
        }

        if (text == string.Empty)
        {
            value = 0;
        }
        else
        {
            int.TryParse(text, out value);
        }

        return value;
    }

    public static float FloatTextField(float value, int width)
    {
        string text = value.ToString();
        if (width != 0)
        {
            text = GUILayout.TextField(text, GUILayout.Width(width));
        }
        else
        {
            text = GUILayout.TextField(text);
        }

        if (text == string.Empty)
        {
            value = 0;
        }
        else
        {
            float.TryParse(text, out value);
        }

        return value;
    }

    /// <summary>
    /// Display EditorGUI elements to set the value of a field.
    /// </summary>
    /// <param name="fieldName">The label to show</param>
    /// <param name="field">The FieldInfo object</param>
    /// <param name="fieldOwner">The object whose field value should be set</param>
    /// <returns>true if the field was something we could display and edit, false otherwise</returns>
    public static bool LayoutFieldInfo(string fieldName, System.Reflection.FieldInfo field, object fieldOwner)
    {
        if (field.FieldType.IsEnum)
        {
            field.SetValue(fieldOwner, EditorGUILayout.EnumPopup(fieldName, (System.Enum)field.GetValue(fieldOwner)));
            return true;
        }

        switch (field.FieldType.Name)
        {
            case "Int32":
                field.SetValue(fieldOwner, EditorGUILayout.IntField(fieldName, (int)field.GetValue(fieldOwner)));
                return true;
            case "Single":
                field.SetValue(fieldOwner, EditorGUILayout.FloatField(fieldName, (float)field.GetValue(fieldOwner)));
                return true;
            case "Boolean":
                field.SetValue(fieldOwner, EditorGUILayout.Toggle(fieldName, (bool)field.GetValue(fieldOwner)));
                return true;
            case "Vector3":
                field.SetValue(fieldOwner, EditorGUILayout.Vector3Field(fieldName, (Vector3)field.GetValue(fieldOwner)));
                return true;
            case "Color":
                field.SetValue(fieldOwner, EditorGUILayout.ColorField(new GUIContent(fieldName), (Color)field.GetValue(fieldOwner), true, true, true, null));
                return true;
            case "String":
                {
                    string value = (string)field.GetValue(fieldOwner);
                    if (value == null)
                        value = "";
                    field.SetValue(fieldOwner, EditorGUILayout.TextField(fieldName, value));
                }
                return true;
            //case "Texture2D":
            //	field.SetValue(fieldOwner, EditorGUILayout.ObjectField(fieldName, (Texture2D)field.GetValue(fieldOwner), typeof(Texture2D), true));
            //	return true;
            default:
                return false;
        }
    }


    public static Color GetTextColorByEditorSkin()
    {
        return EditorGUIUtility.isProSkin ? Color.white : Color.black;
    }

    public static Color GetHighlightTextColorByEditorSkin()
    {
        return EditorGUIUtility.isProSkin ? Color.yellow : Color.blue;
    }

    public static Color GetSelectedTextColorByEditorSkin(bool selected)
    {
        return selected ? Color.green : GetHighlightTextColorByEditorSkin();
    }
}

public static class EditorGUIEventUtil
{

    public static bool IsLastRectClicked(int button)
    {
        Rect rect = GUILayoutUtility.GetLastRect();
        if (Event.current.type == EventType.MouseDown && Event.current.button == button)
        {
            Vector2 mousePos = Event.current.mousePosition;
            if (rect.Contains(mousePos))
            {
                Event.current.Use();
                return true;
            }
        }
        return false;
    }

    public static bool IsRectClicked(Rect rect, int button)
    {
        if (Event.current.type == EventType.MouseDown && Event.current.button == button)
        {
            Vector2 mousePos = Event.current.mousePosition;
            if (rect.Contains(mousePos))
            {
                Event.current.Use();
                return true;
            }
        }
        return false;
    }

    public static bool IsRectDblClicked(Rect rect, int button)
    {
        if (Event.current.type == EventType.MouseDown && Event.current.button == button && Event.current.clickCount == 2)
        {
            Vector2 mousePos = Event.current.mousePosition;
            if (rect.Contains(mousePos))
            {
                Event.current.Use();
                return true;
            }
        }
        return false;
    }

    public static bool IsLastRectClicked()
    {
        Rect rect = GUILayoutUtility.GetLastRect();
        if (Event.current.type == EventType.MouseDown)
        {
            Vector2 mousePos = Event.current.mousePosition;
            if (rect.Contains(mousePos))
            {
                Event.current.Use();
                return true;
            }
        }
        return false;
    }

    public static bool ListKeyHandler(out KeyCode keyCode, ref int index, int count)
    {
        keyCode = KeyCode.None;
        if (Event.current.type == EventType.KeyDown)
        {
            keyCode = Event.current.keyCode;
            if (Event.current.keyCode == KeyCode.UpArrow)
            {
                if (index == 0)
                    return false;

                index = Mathf.Max(index - 1, 0);
                Event.current.Use();
                return true;
            }
            else if (Event.current.keyCode == KeyCode.DownArrow)
            {
                if (index == count - 1)
                    return false;

                index = Mathf.Min(index + 1, count - 1);
                Event.current.Use();
                return true;
            }
            else if (Event.current.keyCode == KeyCode.PageUp)
            {
                if (index == 0)
                    return false;

                index = Mathf.Max(index - 10, 0);
                Event.current.Use();
                return true;
            }
            else if (Event.current.keyCode == KeyCode.PageDown)
            {
                if (index == count - 1)
                    return false;

                index = Mathf.Min(index + 10, count - 1);
                Event.current.Use();
                return true;
            }
            //else if (Event.current.keyCode == KeyCode.Home)
            //{
            //    index = 0;
            //    Event.current.Use();
            //    return true;
            //}
            //else if (Event.current.keyCode == KeyCode.End)
            //{
            //    index = count - 1;
            //    Event.current.Use();
            //    return true;
            //}
        }
        return false;
    }

    public static void ResetFocus()
    {
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            GUIUtility.keyboardControl = 0;
            Event.current.Use();
        }
    }

}

public class ToolbarSelectionGrid
{
    class SelectionUnit
    {
        public string name = "";
        public bool toggle = false;
    }

    private List<SelectionUnit> selectionUnitList = new List<SelectionUnit>();
    private int selectionUnitIndex = -1;
    public int SelectionUnitIndex
    {
        get
        {
            return selectionUnitIndex;
        }
    }

    public void AddSelectionUnit(string name)
    {
        SelectionUnit selectionUnit = new SelectionUnit();
        selectionUnit.name = name;
        selectionUnit.toggle = false;
        selectionUnitList.Add(selectionUnit);
    }

    public void SelectUnit(string name)
    {
        for (int i = 0; i < selectionUnitList.Count; ++i)
        {
            if (selectionUnitList[i].name == name)
            {
                selectionUnitList[i].toggle = true;
                selectionUnitIndex = i;
            }
            else
            {
                selectionUnitList[i].toggle = false;
            }
        }
    }

    public void OnGUI()
    {
        if (selectionUnitList.Count == 1)
        {
            selectionUnitList[0].toggle = GUILayout.Toggle(selectionUnitList[0].toggle, selectionUnitList[0].name, EditorStyles.miniButton);
        }
        else if (selectionUnitList.Count >= 2)
        {
            EditorGUILayout.BeginHorizontal();

            OnGUIToggle(0, EditorStyles.miniButtonLeft);
            for (int i = 1; i < selectionUnitList.Count - 1; ++i)
            {
                OnGUIToggle(i, EditorStyles.miniButtonMid);
            }
            OnGUIToggle(selectionUnitList.Count - 1, EditorStyles.miniButtonRight);

            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < selectionUnitList.Count; ++i)
            {
                if (i != selectionUnitIndex)
                {
                    selectionUnitList[i].toggle = false;
                }
            }
        }
    }

    private void OnGUIToggle(int index, GUIStyle guiStyle)
    {
        bool result = GUILayout.Toggle(selectionUnitList[index].toggle, selectionUnitList[index].name, guiStyle);
        if (result != selectionUnitList[index].toggle)
        {
            selectionUnitList[index].toggle = true;
            selectionUnitIndex = index;
        }
    }

}

public class ToolbarToggleGrid
{
    class ToggleUnit
    {
        public string name = "";
        public bool toggle = false;
    }

    private List<ToggleUnit> toggleUnitList = new List<ToggleUnit>();
    private int toggleUnitMask = 0;
    public int ToggleUnitMask
    {
        get
        {
            return toggleUnitMask;
        }
    }

    public void AddToggleUnit(string name)
    {
        ToggleUnit toggleUnit = new ToggleUnit();
        toggleUnit.name = name;
        toggleUnit.toggle = false;
        toggleUnitList.Add(toggleUnit);
    }

    public void SelectUnit(int toggleUnitMask)
    {
        this.toggleUnitMask = toggleUnitMask;
        for (int i = 0; i < toggleUnitList.Count; ++i)
        {
            int mask = (1 << i);
            if ((mask & toggleUnitMask) == mask)
            {
                toggleUnitList[i].toggle = true;
            }
            else
            {
                toggleUnitList[i].toggle = false;
            }
        }
    }

    public void OnGUI()
    {
        if (toggleUnitList.Count == 1)
        {
            toggleUnitList[0].toggle = GUILayout.Toggle(toggleUnitList[0].toggle, toggleUnitList[0].name);
        }
        else if (toggleUnitList.Count >= 2)
        {
            EditorGUILayout.BeginHorizontal();

            OnGUIToggle(0, EditorStyles.miniButtonLeft);
            for (int i = 1; i < toggleUnitList.Count - 1; ++i)
            {
                OnGUIToggle(i, EditorStyles.miniButtonMid);
            }
            OnGUIToggle(toggleUnitList.Count - 1, EditorStyles.miniButtonRight);

            EditorGUILayout.EndHorizontal();
        }
    }

    private void OnGUIToggle(int index, GUIStyle guiStyle)
    {
        bool result = GUILayout.Toggle(toggleUnitList[index].toggle, toggleUnitList[index].name, guiStyle);
        if (result != toggleUnitList[index].toggle)
        {
            toggleUnitList[index].toggle = result;
            int mask = (1 << index);
            if (toggleUnitList[index].toggle)
            {
                toggleUnitMask |= mask;
            }
            else
            {
                toggleUnitMask &= ~mask;
            }
        }
    }

}

public class GraphMenu
{
    public delegate void OnClickMenu();

    public bool foldout = false;
    public string name = "";
    public List<GraphMenu> childMenuList;
    public OnClickMenu onClickMenu = null;

    public GraphMenu AddMenu(string menuName, OnClickMenu onClickMenu)
    {
        GraphMenu menuItem = new GraphMenu();
        menuItem.name = menuName;
        menuItem.onClickMenu = onClickMenu;

        if (childMenuList == null)
            childMenuList = new List<GraphMenu>();
        childMenuList.Add(menuItem);
        return menuItem;
    }
}

public static class SceneGUIUtil
{

    public static void DrawArrow(Vector3 from, Vector3 to)
    {
        from.y += 0.01f;
        to.y += 0.01f;
        Gizmos.DrawLine(from, to);
        Vector3 dir = to - from;
        Vector3 pos = Quaternion.Euler(0, 160, 0) * dir;
        pos.Normalize();
        pos *= 0.2f;
        pos += to;
        Gizmos.DrawLine(to, pos);
        pos = Quaternion.Euler(0, -160, 0) * dir;
        pos.Normalize();
        pos *= 0.2f;
        pos += to;
        Gizmos.DrawLine(to, pos);
    }

}