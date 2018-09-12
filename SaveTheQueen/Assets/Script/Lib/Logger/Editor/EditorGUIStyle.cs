using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections.Generic;

public static class EditorGUIStyle
{

	private static Color SelectedBGColor = new Color(61.0f / 255.0f, 128.0f / 255.0f, 223.0f / 255.0f);
	private static Color UnfocusBGColor = new Color(143.0f / 255.0f, 143.0f / 255.0f, 143.0f / 255.0f);
	private static Color DarkGreyColor = new Color(86.0f / 255.0f, 86.0f / 255.0f, 86.0f / 255.0f);

	private static Dictionary<Color, Texture2D> coloredTexture = new Dictionary<Color, Texture2D>();
	public static Texture2D GetTexture(Color color)
	{
		Texture2D texture = null;
		if (!coloredTexture.TryGetValue(color, out texture))
		{
			texture = CreateTexture(color);
		}
		else if (texture == null)
		{
			coloredTexture.Remove(color);
			texture = CreateTexture(color);
		}
		return texture;
	}

	private static Texture2D CreateTexture(Color color)
	{
		Texture2D texture = new Texture2D(1, 1);
		texture.SetPixel(0, 0, color);
		texture.wrapMode = TextureWrapMode.Repeat;
		texture.Apply();
		UnityEngine.Object.DontDestroyOnLoad(texture);
		coloredTexture.Add(color, texture);
		return texture;
	}

	private static Texture2D SelectedBackground
	{
		get { return GetTexture(SelectedBGColor); }
	}

	private static Texture2D UnfocusBackground
	{
		get { return GetTexture(UnfocusBGColor); }
	}

	private static Texture2D DarkGreyTexture
	{
		get { return GetTexture(DarkGreyColor); }
	}

	private static GUIStyle darkGreyBackground = null;
	public static GUIStyle DarkGreyBackground
	{
		get
		{
			if (darkGreyBackground == null)
			{
				darkGreyBackground = new GUIStyle()
				{
					normal =
					{
						background = DarkGreyTexture
					}
				};
			}
			return darkGreyBackground;
		}
	}

	public static class Label
	{
		private static RectOffset padding =
			new RectOffset
			{
				top = 2,
				left = 2,
				bottom = 2,
				right = 2
			};

		private static GUIStyle redLabel = null;
		public static GUIStyle Red
		{
			get
			{
				if (redLabel == null)
				{
					redLabel = new GUIStyle("label")
					{
						alignment = TextAnchor.MiddleCenter,
						normal =
						{
							textColor = Color.red
						}
					};
				}
				return redLabel;
			}
		}

		private static GUIStyle blueLabel = null;
		public static GUIStyle Blue
		{
			get
			{
				if (blueLabel == null)
				{
					blueLabel = new GUIStyle("label")
					{
						alignment = TextAnchor.MiddleCenter,
						normal =
						{
							textColor = Color.blue
						}
					};
				}
				return blueLabel;
			}
		}

		private static GUIStyle greenLabel = null;
		public static GUIStyle Green
		{
			get
			{
				if (greenLabel == null)
				{
					greenLabel = new GUIStyle("label")
					{
						alignment = TextAnchor.MiddleCenter,
						normal =
						{
							textColor = Color.green
						}
					};
				}
				return greenLabel;
			}
		}

		private static GUIStyle unfocusStyle;
		public static GUIStyle UnfocusStyle
		{
			get
			{
				if (unfocusStyle == null)
				{
					unfocusStyle = new GUIStyle()
					{
						wordWrap = false,
						alignment = TextAnchor.MiddleLeft,
						padding = padding,
						normal =
						{
							textColor = Color.white,
							background = UnfocusBackground
						}
					};
				}
				return unfocusStyle;
			}
		}

		private static GUIStyle selectedStyle;
		public static GUIStyle SelectedStyle
		{
			get
			{
				if (selectedStyle == null)
				{
					selectedStyle = new GUIStyle()
					{
						wordWrap = false,
						alignment = TextAnchor.MiddleLeft,
						padding = padding,
						normal =
						{
							textColor = Color.white,
							background = SelectedBackground
						}
					};
				}

				if (selectedStyle.normal.background == null)
				{
					selectedStyle.normal.background = SelectedBackground;
				}

				return selectedStyle;
			}
		}

		private static GUIStyle unselectedStyle;
		public static GUIStyle UnselectedStyle
		{
			get
			{
				if (unselectedStyle == null)
				{
					unselectedStyle = new GUIStyle()
					{
						wordWrap = false,
						alignment = TextAnchor.MiddleLeft,
						padding = padding,
						normal =
						{
							textColor = Color.black
						}
					};
				}
				return unselectedStyle;
			}
		}

	}

}