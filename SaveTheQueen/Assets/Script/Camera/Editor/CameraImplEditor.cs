using Aniz.Cam;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraImpl))]
public class CameraImplEditor : Editor
{
    GUIContent _tooltip;

    MonoScript _script;

    void OnEnable()
    {
        if (target != null)
        {
            _script = MonoScript.FromMonoBehaviour((CameraImpl)target);
        }
    }

    void AddSpace()
    {
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
    }

    public override void OnInspectorGUI()
    {

        CameraImpl cameraImpl = (CameraImpl)target;

        serializedObject.Update();

        // Show script link
        GUI.enabled = false;
        _script = EditorGUILayout.ObjectField("Script", _script, typeof(MonoScript), false) as MonoScript;
        GUI.enabled = true;

        _tooltip = new GUIContent("Main", "");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("IsMain"), _tooltip);

        _tooltip = new GUIContent("CameraStock", "");
        EditorGUILayout.LabelField(_tooltip);

        _tooltip = new GUIContent("Distance", "거리");
        EditorGUILayout.Slider(serializedObject.FindProperty("EditorDistance"), 0f, 100f, _tooltip);
        _tooltip = new GUIContent("Vertical", "");
        EditorGUILayout.Slider(serializedObject.FindProperty("EditorVertical"), -90f, 90f, _tooltip);
        _tooltip = new GUIContent("Horizon", "");
        EditorGUILayout.Slider(serializedObject.FindProperty("EditorHorizon"), -90f, 90f, _tooltip);
        _tooltip = new GUIContent("Fov", "");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("EditorFov"), _tooltip);
        _tooltip = new GUIContent("HeightOffset", "");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("EditorHeightOffset"), _tooltip);

        cameraImpl.EditorUpdateStock();



        serializedObject.ApplyModifiedProperties();
    }
}
