using UnityEditor;

public class OpenScriptEditor
{
	[MenuItem("Window/Open Script Editor &1")]
	public static void OpenScriptCompiler()
	{
		UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal("", 1);
	}
}