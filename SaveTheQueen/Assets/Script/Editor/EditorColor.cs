using UnityEditor;
using UnityEngine;

public static class EditorColor
{
    public static Color GetColor(int r, int g, int b)
    {
        return new Color((float)r / 255.0f, (float)g / 255.0f, (float)b / 255.0f);
    }

    // 에디터전용? Color Preset
    public static Color BackgroundColor = GetColor(90, 100, 116);
    public static Color CurrentStateColor = GetColor(240, 77, 49);
    public static Color StateContextColor = GetColor(48, 58, 72);
    public static Color TitleColor = GetColor(135, 145, 161);
    public static Color TitleTextColor = GetColor(0, 0, 0);
    public static Color SelectedTitleColor = GetColor(252, 173, 62);
    public static Color GlobalTransitionColor = GetColor(200, 200, 200);
    public static Color TransitionColor = GetColor(240, 240, 240);
    public static Color TransitionStartColor = GetColor(245, 176, 162);

    public static Color InEditingTransitionLineColor = GetColor(20, 20, 20);
    public static Color BlinkTransitionLineColor = GetColor(240, 177, 149);
    public static Color TransitionInLineColor = GetColor(77, 240, 49);
    public static Color TransitionOutLineColor = GetColor(240, 77, 49);
    public static Color TransitionLineShadowColor = GetColor(100, 100, 100);

    public static Color TransitionLineColor
    {
        get
        {
            if (EditorGUIUtility.isProSkin)
                return GetColor(255, 255, 255);
            else
                return GetColor(0, 0, 0);
        }
    }
}