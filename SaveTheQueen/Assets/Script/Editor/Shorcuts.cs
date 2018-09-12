using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

public class Shortcuts : EditorWindow
{
	[MenuItem("Joker/Shortcuts/Open Window > Lighting %L")]
	public static void OpenLightingWindow()
	{
		EditorApplication.ExecuteMenuItem("Window/Lighting");
	}
}
