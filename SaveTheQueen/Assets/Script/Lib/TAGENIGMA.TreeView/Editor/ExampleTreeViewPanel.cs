using UnityEditor;
using UnityEngine;

namespace TAGENIGMA.TreeView
{
    public class ExampleTreeViewPanel : EditorWindow
    {
        [MenuItem("TreeView/Show Example Panel")]
        public static void ShowExampleTreeViewPanel()
        {
            CreateTreeView();
            RefreshPanel();
        }

        static ExampleTreeViewPanel m_instance = null;

        public static ExampleTreeViewPanel GetPanel()
        {
            if (null == m_instance)
            {
                m_instance = EditorWindow.GetWindow<ExampleTreeViewPanel>(false, "TreeView Panel", false);
            }

            return m_instance;

        }

        public static void RefreshPanel()
        {
            ExampleTreeViewPanel panel = GetPanel();
            panel.Repaint();
        }

        static GameObject m_treeViewGO = null;
        static TreeViewControl m_treeViewControl = null;
        const string GO_NAME = "MyTreeViewPanel";
        static void CreateTreeView()
        {
            if (null == m_treeViewGO)
            {
                m_treeViewGO = GameObject.Find(GO_NAME);
            }
            if (null != m_treeViewGO)
            {
                m_treeViewControl = m_treeViewGO.GetComponent<TreeViewControl>();
            }
            if (null != m_treeViewControl)
            {
                return;
            }
            m_treeViewGO = new GameObject(GO_NAME);
            Selection.activeGameObject = m_treeViewGO;
            m_treeViewControl = TreeViewInspector.AddTreeView(m_treeViewGO);
            m_treeViewControl.DisplayInInspector = false;
            m_treeViewControl.DisplayOnGame = false;
            m_treeViewControl.DisplayOnScene = false;
            Example.PopulateExampleData(m_treeViewControl);
        }

        void OnEnable()
        {
            wantsMouseMove = true;
        }

        void OnGUI()
        {
            if (null == m_treeViewControl)
            {
                return;
            }
            wantsMouseMove = true;
            if (null != Event.current &&
                Event.current.type == EventType.MouseMove)
            {
                Repaint();
            }
            m_treeViewControl.DisplayTreeView(TreeViewControl.DisplayTypes.USE_SCROLL_VIEW);
        }
    }
}