using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

public static class CustomEditorTools
{
	static bool m_endHorizontal = false;
	static bool m_lock = false;

	static public bool DrawHeader(string text) { return DrawHeader(text, text, false, CustomEditorSetting.Minimalistic); }

	static public bool DrawHeader(string text, string key, bool forceOn, bool minimalistic)
	{
		bool state = EditorPrefs.GetBool(key, true);

		if (!minimalistic && m_lock) GUILayout.Space(3f);
		if (!forceOn && !state) GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
		GUILayout.BeginHorizontal();
		GUI.changed = false;

		if (minimalistic && !m_lock)
		{
			if (state) text = "\u25BC" + (char)0x200a + text;
			else text = "\u25BA" + (char)0x200a + text;

			GUILayout.BeginHorizontal();
			GUI.contentColor = EditorGUIUtility.isProSkin ? new Color(1f, 1f, 1f, 0.7f) : new Color(0f, 0f, 0f, 0.7f);
			if (!GUILayout.Toggle(true, text, "PreToolbar2", GUILayout.MinWidth(20f))) state = !state;
			GUI.contentColor = Color.white;
			GUILayout.EndHorizontal();
		}
		else
		{
			text = "<b><size=11>" + text + "</size></b>";
			if (state) text = "\u25BC " + text;
			else text = "\u25BA " + text;
			if (!GUILayout.Toggle(true, text, "dragtab", GUILayout.MinWidth(20f)))
			{
				if (!m_lock)
				{
					state = !state;
				}
			}
		}

		if (GUI.changed) EditorPrefs.SetBool(key, state);

		if (!minimalistic && m_lock) GUILayout.Space(2f);
		GUILayout.EndHorizontal();
		GUI.backgroundColor = Color.white;
		if (!forceOn && !state) GUILayout.Space(3f);
		return state;
	}

	static public void BeginContents() { BeginContents(CustomEditorSetting.Minimalistic); }

	static public void BeginContents(bool minimalistic)
	{
		if (!minimalistic)
		{
			m_endHorizontal = true;
			GUILayout.BeginHorizontal();
			EditorGUILayout.BeginHorizontal("AS TextArea", GUILayout.MinHeight(10f));
		}
		else
		{
			m_endHorizontal = false;
			EditorGUILayout.BeginHorizontal(GUILayout.MinHeight(10f));
			GUILayout.Space(10f);
		}
		GUILayout.BeginVertical();
		GUILayout.Space(2f);
	}

	static public void EndContents()
	{
		GUILayout.Space(3f);
		GUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();

		if (m_endHorizontal)
		{
			GUILayout.Space(3f);
			GUILayout.EndHorizontal();
		}

		GUILayout.Space(3f);
	}

	static public void LockContents()
	{
		m_lock = true;
	}

	static public void UnLockContents()
	{
		m_lock = false;
	}
}
