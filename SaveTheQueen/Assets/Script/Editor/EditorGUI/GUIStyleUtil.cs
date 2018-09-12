

using UnityEngine;

public static class GUIStyleUtil
{
	private static GUIStyle _errorStyle;
	public static GUIStyle ErrorStyle
	{
		get
		{
			if (_errorStyle == null)
			{
				_errorStyle = new GUIStyle(GUI.skin.box) { wordWrap = true, alignment = TextAnchor.UpperLeft };

				Color c = Color.red;
				c.a = 0.75f;
				_errorStyle.normal.textColor = c;
			}
			return _errorStyle;
		}
	}

	private static GUIStyle _warningStyle;
	public static GUIStyle WarningStyle
	{
		get
		{
			if (_warningStyle == null)
			{
				_warningStyle = new GUIStyle(GUI.skin.box) { wordWrap = true, alignment = TextAnchor.UpperLeft };

				Color c = Color.yellow;
				c.a = 0.75f;
				_warningStyle.normal.textColor = c;
			}
			return _warningStyle;
		}
	}

	private static GUIStyle _helpStyle;
	public static GUIStyle HelpStyle
	{
		get
		{
			if (_helpStyle == null)
			{
				_helpStyle = new GUIStyle(GUI.skin.box) { wordWrap = true, alignment = TextAnchor.UpperLeft };

				Color c = Color.white;
				c.a = 0.75f;
				_helpStyle.normal.textColor = c;
			}
			return _helpStyle;
		}
	}

	private static GUIStyle _selectedStyle;
	public static GUIStyle SelectedStyle
	{
		get
		{
			if (_selectedStyle == null)
			{
				_selectedStyle = new GUIStyle(GUI.skin.box) { wordWrap = true, alignment = TextAnchor.UpperLeft, normal = { textColor = ControlUtil.StandardHighlight } };
			}
			return _selectedStyle;
		}
	}

	private static GUIStyle _unselectedStyle;
	public static GUIStyle UnselectedStyle
	{
		get
		{
			if (_unselectedStyle == null)
			{
				Color c = Color.white;
				c.a = 0.75f;
				_unselectedStyle = new GUIStyle(GUI.skin.box) { wordWrap = true, alignment = TextAnchor.UpperLeft, normal = { textColor = c } };
			}
			return _unselectedStyle;
		}
	}

}