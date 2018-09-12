using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CustomEditorSetting
{
	public static bool Minimalistic
	{
		get { return GetBool("Minimalistic", false); }
		set { SetBool("Minimalistic", value); }
	}

	public static bool GetBool(string name, bool defaultValue) { return EditorPrefs.GetBool(name, defaultValue); }

	public static void SetBool(string name, bool val) { EditorPrefs.SetBool(name, val); }
}
