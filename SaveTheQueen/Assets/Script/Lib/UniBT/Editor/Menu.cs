using UnityEditor;

namespace Lib.UniBt.Editor
{
    public static class Menu
    {
        [MenuItem("Joker/AI Editor(UniBt)/Behavior Trees Editor", false)]
        public static void OpenEditorWindow()
        {
            BehaviorTreesEditor.ShowEditorWindow();
        }

        [MenuItem("Joker/AI Editor(UniBt)/Code Pack Maker", false)]
        public static void OpenCodePackMaker()
        {
            EditorWindow.GetWindow<CodePackMaker>(false, "Code Pack Maker", true).Show();
        }
    }
}
