
using UnityEngine;
using UnityEditor;

namespace Lib.UniBt.Editor.Inspector
{
    [CustomEditor(typeof(Service), true)]
    public class ServiceInspector : UnityEditor.Editor
    {
        protected Service service;

        public virtual void OnEnable()
        {
            service = target as Service;
        }

        public override void OnInspectorGUI()
        {
            string name = service.Name;
            string description = string.IsNullOrEmpty(service.comment) ? name : service.comment;
            BehaviorTreesEditorUtility.BeginInspectorGUI(ref name, ref description);
            if (name != service.Name)
            {
                service.Name = name;
                //AssetCreator.SaveAIAsset();
            }
            GUILayout.Space(7f);
            if (BehaviorTreesEditorUtility.DrawHeader("Target Code", false))
            {
                BehaviorTreesEditorUtility.DrawTargetScript(OnSelected, serializedObject);
                if (service.targetScript != null && BehaviorTreesEditorUtility.DrawTargetMethod(service.targetScript.GetType(), typeof(void), ref service.targetMethod))
                {
                    UpdateName();
                    UpdateComment();
                    BehaviorTreesEditor.RepaintAll();
                    AssetCreator.SaveAIAsset();
                    EditorGUILayout.Space();
                }
            }
            GUILayout.Space(7f);
            if (BehaviorTreesEditorUtility.DrawHeader("Service", false))
            {
                DrawTick();
            }
            BehaviorTreesEditorUtility.EndInspectorGUI(service);
        }

        private void OnSelected(Object obj)
        {
            serializedObject.Update();
            SerializedProperty sp = serializedObject.FindProperty("targetScript");
            sp.objectReferenceValue = obj;
            serializedObject.ApplyModifiedProperties();
            service.targetScript = obj as MonoBehaviour;
        }

        private void DrawTick()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(7f);
            float tick = EditorGUILayout.FloatField("Interval", service.tick);
            if (tick != service.tick)
            {
                // tick 이 0이면 한번만 실행 하도록.
                //if (tick <= 0)
                //    tick = 0.1f;
                service.tick = tick;
                UpdateComment();
                AssetCreator.SaveAIAsset();
            }
            GUILayout.EndHorizontal();
        }

        private void UpdateName()
        {
            string name = "Service";
            if (service.targetScript != null)
                name = string.IsNullOrEmpty(service.targetMethod) ? service.targetScript.name : service.targetMethod;
            service.Name = name;
        }

        private void UpdateComment()
        {
            string comment = "Empty Service";
            if (service.targetScript != null && !string.IsNullOrEmpty(service.targetMethod))
                comment = service.targetScript.name + "." + service.targetMethod;
            service.comment += ": tick every " + service.tick + "s";
            service.comment = comment;
        }
    }
}
