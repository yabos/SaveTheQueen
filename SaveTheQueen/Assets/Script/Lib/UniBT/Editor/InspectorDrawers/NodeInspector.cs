using UnityEditor;

namespace Lib.UniBt.Editor.Inspector
{
    [CustomEditor(typeof(Node), true)]
    public class NodeInspector : UnityEditor.Editor
    {
        protected Node node;

        public virtual void OnEnable()
        {
            node = target as Node;
        }

        public virtual void OnDisable()
        {
        }

        public override void OnInspectorGUI()
        {
            string name = node.Name;
            string description = string.IsNullOrEmpty(node.description) ? name : node.description;
            BehaviorTreesEditorUtility.BeginInspectorGUI(ref name, ref description);
            if (name != node.Name)
            {
                node.Name = name;
                node.description = description;
                
                //AssetCreator.SaveAIAsset();
            }
            BehaviorTreesEditorUtility.EndInspectorGUI(node);
        }
    }
}
