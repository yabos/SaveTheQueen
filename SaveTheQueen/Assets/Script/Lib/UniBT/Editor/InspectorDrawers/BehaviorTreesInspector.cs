using UnityEngine;
using UnityEditor;
using Lib.UniBt.Editor;

namespace Lib.UniBt.Editor.Inspector
{
    [CustomEditor(typeof(BehaviorTrees))]
    public sealed class BehaviorTreesInspector : NodeInspector
    {
        public override void OnEnable()
        {
            base.OnEnable();
            EditorApplication.projectWindowItemOnGUI += OnDoubleClick;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            EditorApplication.projectWindowItemOnGUI -= OnDoubleClick;
        }

        public override void OnInspectorGUI()
        {
            if (node.parent != null)
            {
                base.OnInspectorGUI();
            }
        }

        public void OnDoubleClick(string guid, Rect rect)
        {
            if (UnityEngine.Event.current.type == EventType.MouseDown &&
                UnityEngine.Event.current.clickCount == 2 &&
                rect.Contains(UnityEngine.Event.current.mousePosition))
            {
                BehaviorTreesEditor.ShowEditorWindow();
                BehaviorTreesEditor.SelectBehaviorTrees(target as BehaviorTrees);
            }
        }
    }
}
