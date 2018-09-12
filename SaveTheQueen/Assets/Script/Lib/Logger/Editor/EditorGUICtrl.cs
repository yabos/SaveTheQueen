using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace EditorGUICtrl
{

	public class EditorGUIStyle
	{
		private static Color SelectedBGColor = new Color(61.0f / 255.0f, 128.0f / 255.0f, 223.0f / 255.0f);

		public static GUIStyle[] label = null;
		public static GUIStyle[] selected = null;

		public static GUIStyle header = new GUIStyle("OL Title");

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

		private EditorGUIStyle()
		{
			label = new GUIStyle[3];
			selected = new GUIStyle[3];
			for (int i = 0; i < 3; ++i)
			{
				TextAnchor alignment;
				if ((TextAlignment)i == TextAlignment.Left)
				{
					alignment = TextAnchor.MiddleLeft;
				}
				else if ((TextAlignment)i == TextAlignment.Center)
				{
					alignment = TextAnchor.MiddleCenter;
				}
				else // if((TextAlignment)i == TextAlignment.Right)
				{
					alignment = TextAnchor.MiddleRight;
				}

				label[i] = new GUIStyle("label");
				label[i].alignment = alignment;
				label[i].padding = padding;
				label[i].margin = margin;
				label[i].overflow = overflow;

				selected[i] = new GUIStyle("label");
				selected[i].alignment = alignment;
				selected[i].padding = padding;
				selected[i].margin = margin;
				selected[i].overflow = overflow;
				selected[i].normal.textColor = Color.white;
				selected[i].normal.background = SelectedBackground;
			}

			header.alignment = TextAnchor.MiddleCenter;
		}

		public static void Restore()
		{
			for (int i = 0; i < 3; ++i)
			{
				selected[i].normal.background = SelectedBackground;
			}
		}

		private static EditorGUIStyle instance = null;
		public static void Use()
		{
			if (instance == null)
			{
				instance = new EditorGUIStyle();
			}
		}

		private static RectOffset padding =
			new RectOffset
			{
				top = 2,
				left = 2,
				bottom = 2,
				right = 2
			};

		private static RectOffset margin =
			new RectOffset
			{
				top = 0,
				left = 0,
				bottom = 0,
				right = 0
			};

		private static RectOffset overflow =
			new RectOffset
			{
				top = 0,
				left = 0,
				bottom = 0,
				right = 0
			};

		private static Texture2D SelectedBackground
		{
			get { return GetTexture(SelectedBGColor); }
		}

	}

	public class ListCtrl
	{
		public delegate void OnClickColumn(Column column);
		public delegate void OnClickRow(Row row);

		public enum ColumnAlignment
		{
			Left,
			Center,
			Right,
			None,
		}

		[System.Serializable]
		public class Column
		{
			private bool visible = true;
			private string title = string.Empty;
			private int width = -1;
			private ColumnAlignment alignment = ColumnAlignment.None;
			private OnClickColumn onLeftClickColumn = null;
			private OnClickColumn onRightClickColumn = null;

			public bool Visible
			{
				get { return visible; }
				set { visible = value; }
			}

			public Column(string title, int width)
			{
				this.title = title;
				this.width = width;
			}

			public string Title
			{
				get { return title; }
				set { title = value; }
			}

			public int Width
			{
				get { return width; }
				set { width = value; }
			}

			public ColumnAlignment Alignment
			{
				get { return alignment; }
				set { alignment = value; }
			}

			public void SetOnLeftClickColumn(OnClickColumn onLeftClickColumn)
			{
				this.onLeftClickColumn = onLeftClickColumn;
			}

			public void OnLeftClickColumn()
			{
				if (onLeftClickColumn != null)
				{
					onLeftClickColumn(this);
				}
			}

			public void SetOnRightClickColumn(OnClickColumn onRightClickColumn)
			{
				this.onRightClickColumn = onRightClickColumn;
			}

			public void OnRightClickColumn()
			{
				if (onRightClickColumn != null)
				{
					onRightClickColumn(this);
				}
			}

		}

		[System.Serializable]
		public class Row
		{
			private bool visible = true;
			private bool selected = false;
			private List<Item> itemList = new List<Item>();

			public bool Visible
			{
				get { return visible; }
				set { visible = value; }
			}

			public bool Selected
			{
				get { return selected; }
				set { selected = value; }
			}

			public List<Item> ItemList
			{
				get { return itemList; }
			}

			public Item AddItem(string text)
			{
				Item item = new Item(text);
				itemList.Add(item);
				return item;
			}

			public Item AddItem(string text, object data)
			{
				Item item = new Item(text);
				item.SetData(data);
				itemList.Add(item);
				return item;
			}

			public Item GetMainItem()
			{
				return GetItem(0);
			}

			public Item GetItem(int index)
			{
				return itemList[index];
			}

		}

		[System.Serializable]
		public class Item
		{
			private string text;
			public string Text
			{
				get { return text; }
			}

			private object data;
			public object Data
			{
				get { return data; }
			}

			public Item(string text)
			{
				SetText(text);
			}

			public void SetText(string text)
			{
				this.text = text;
			}

			public void SetData(object data)
			{
				this.data = data;
			}

		}

		public interface RowProvider
		{
			int GetCount();
			Row GetRow(int index);
		}

		private class DefaultRowProvider : RowProvider
		{
			private List<Row> rowList = null;
			public DefaultRowProvider(List<Row> rowList)
			{
				this.rowList = rowList;
			}

			public int GetCount()
			{
				return rowList.Count;
			}

			public Row GetRow(int index)
			{
				return rowList[index];
			}
		}

		public enum Style
		{
			Label,
			Button,
			ToggleButton,
			SelectOneRow,
			SelectMultipleRows,
		}

		private Style style = Style.Label;
		private ColumnAlignment alignment = ColumnAlignment.Left;

		private List<Column> columnList = new List<Column>();
		private List<Row> rowList = new List<Row>();
		private OnClickRow onClickRow = null;
		private OnClickRow onDblClickRow = null;
		private OnClickRow onRightClickRow = null;
		private RowProvider rowProvider = null;

		private Rect[] labelRect = null;
		private bool expandWidth = false;
		private int totalWidth = 0;
		private Vector2 scrollView = Vector2.zero;

        public List<Column> ColumnList
		{
			get { return columnList; }
		}

		public List<Row> RowList
		{
			get { return rowList; }
		}

		public Row GetRow(int index)
		{
			return rowList[index];
		}

		public ListCtrl()
		{
			rowProvider = new DefaultRowProvider(rowList);
		}

		public ListCtrl(RowProvider rowProvider)
		{
			this.rowProvider = rowProvider;
		}

		public void SetStyle(Style style)
		{
			this.style = style;
		}

		public void SetOnClickRow(OnClickRow onClickRow)
		{
			this.onClickRow = onClickRow;
		}

		public void SetOnDblClickRow(OnClickRow onClickRow)
		{
			this.onDblClickRow = onClickRow;
		}

		public void SetOnRightClickRow(OnClickRow onClickRow)
		{
			this.onRightClickRow = onClickRow;
		}

		public Column AddColumn(string title, int width)
		{
			Column column = new Column(title, width);
			columnList.Add(column);

			//
			labelRect = new Rect[columnList.Count];
			totalWidth = -7; // box border offset
			for (int i = 0; i < columnList.Count; ++i)
			{
				totalWidth += columnList[i].Width;
				if (columnList[i].Width == -1)
				{
					expandWidth = true;
					break;
				}
			}

			return column;
		}

		public Column AddColumn(string title)
		{
			return AddColumn(title, -1);
		}

		private bool scrollLock = false;
		private bool handleKeyboard = true;
		private int goPage = -1;

		public bool ScrollLock
		{
			get { return scrollLock; }
			set { scrollLock = value; }
		}

		public bool HandleKeyboard
		{
			get { return handleKeyboard; }
			set { handleKeyboard = value; }
		}

		public int GoPage
		{
			get { return goPage; }
			set { goPage = value; }
		}

		public Row AddRow()
		{
			if (!scrollLock)
                scrollView.y = int.MaxValue;

			Row row = new Row();
			rowList.Add(row);
			return row;
		}

		public Row AddRow(string text, object data)
		{
			Row row = AddRow();

			Item item = row.AddItem(text);
			item.SetData(data);
			return row;
		}

		public void ClearRows()
		{
			rowList.Clear();
			rowCursor = -1;
		}

		public void Clear()
		{
			ClearRows();
		}

		public void OnGUI(int viewCount = 0, float canvasHeight = 0f)
		{
			EditorGUIStyle.Use();

			EditorGUILayout.BeginVertical("box");
			{
				OnGUIHeader();

				GUILayout.Space(2);

                if (0 < canvasHeight)
                    scrollView = EditorGUILayout.BeginScrollView(scrollView, GUILayout.MaxHeight(canvasHeight));
                else
                    scrollView = EditorGUILayout.BeginScrollView(scrollView);
                {
                    OnGUIItemList(viewCount, canvasHeight);
                }
				EditorGUILayout.EndScrollView();
			}
			EditorGUILayout.EndVertical();
		}

		private void OnGUIHeader()
		{
			EditorGUILayout.BeginHorizontal();
			{
                GUI.color = Color.green;
				for (int i = 0; i < columnList.Count; ++i)
				{
					Column column = columnList[i];
					if (column.Width != -1)
					{
						GUILayout.Label(column.Title, EditorGUIStyle.header, GUILayout.Width(column.Width));
					}
					else
					{
						GUILayout.Label(column.Title, EditorGUIStyle.header);
					}
					labelRect[i] = GUILayoutUtility.GetLastRect();
					if (i == columnList.Count - 1)
					{
						labelRect[i].width -= 12;
					}

					if (EditorGUIEventUtil.IsRectClicked(labelRect[i], 0))
					{
						column.OnLeftClickColumn();
					}

					if (EditorGUIEventUtil.IsRectClicked(labelRect[i], 1))
					{
						column.OnRightClickColumn();
					}
				}
                GUI.color = Color.white;
            }
			EditorGUILayout.EndHorizontal();
		}

		private void OnGUIItemList(int viewCount, float canvasHeight = 0f)
		{
            KeyCode keyCode = KeyCode.None;
            GUI.color = Color.yellow;
            if (style == Style.Label)
			{
				for (int r = 0; r < rowProvider.GetCount(); ++r)
				{
					Row row = rowProvider.GetRow(r);
					if (!row.Visible)
						continue;
					OnGUILayoutRow(row, false, false);
				}
			}
			else if (style == Style.Button)
			{
				for (int r = 0; r < rowProvider.GetCount(); ++r)
				{
					Row row = rowProvider.GetRow(r);
					if (!row.Visible)
						continue;

					if (expandWidth)
					{
						if (GUILayout.Button(string.Empty))
						{
							if (onClickRow != null)
							{
								onClickRow(row);
							}
						}
					}
					else
					{
						if (GUILayout.Button(string.Empty, GUILayout.Width(totalWidth)))
						{
							if (onClickRow != null)
							{
								onClickRow(row);
							}
						}
					}

					Rect rowRect = GUILayoutUtility.GetLastRect();
					OnGUIRow(row, rowRect.y);
				}
			}
			else if (style == Style.ToggleButton)
			{
				for (int r = 0; r < rowList.Count; ++r)
				{
					Row row = rowList[r];
					if (!row.Visible)
						continue;

					bool oldSelected = row.Selected;
					row.Selected = GUILayout.Toggle(row.Selected, string.Empty, "button");
					if (row.Selected != oldSelected)
					{
						if (onClickRow != null)
						{
							onClickRow(row);
						}
					}

					Rect rowRect = GUILayoutUtility.GetLastRect();
					OnGUIRow(row, rowRect.y);
				}
			}
			else if (style == Style.SelectOneRow)
			{
				int cursor = -1;
				int count = 0;
				int rowstart = 0;
				int rowEnd = rowList.Count;
				if (viewCount > 0)
				{
					if (viewCount < rowList.Count)
					{
						rowstart = rowList.Count - viewCount;
					}
				}

				if (goPage > 0 && goPage != rowList.Count / viewCount)
				{
					int tempstart = viewCount * goPage;
					if (rowEnd < tempstart)
					{
						return;
					}
					else
					{
						rowstart = tempstart;
						if (rowEnd > tempstart + viewCount)
						{
							rowEnd = tempstart + viewCount;
						}
					}
				}

				for (int r = rowstart; r < rowEnd; ++r)
				{
					Row row = rowList[r];
					if (!row.Visible)
						continue;

					Rect rect = OnGUILayoutRow(row, false, row.Selected);

					if (EditorGUIEventUtil.IsRectDblClicked(rect, 0))
					{
						if (onDblClickRow != null)
						{
							onDblClickRow(row);
						}
					}

					if (EditorGUIEventUtil.IsRectClicked(rect, 1))
					{
						if (onRightClickRow != null)
						{
							onRightClickRow(row);
						}
					}

					if (EditorGUIEventUtil.IsRectClicked(rect, 0))
					{
                        for (int i = 0; i < rowList.Count; ++i)
						{
                            rowList[i].Selected = rowList[i] == row;
						}

						rowCursor = r;
						if (onClickRow != null)
						{
							onClickRow(row);
						}
					}

					if (row.Selected)
					{
						cursor = count;
					}
					count++;
				}

				if (HandleKeyboard && EditorGUIEventUtil.ListKeyHandler(out keyCode, ref cursor, count))
				{
                    ScrollTo(keyCode, canvasHeight);
                    int counter = 0;
					for (int i = 0; i < rowList.Count; ++i)
					{
						if (!rowList[i].Visible)
							continue;
						bool selected = counter == cursor;
						rowList[i].Selected = selected;

						if (selected)
						{
							rowCursor = i;
							if (onClickRow != null)
							{
								onClickRow(rowList[i]);
							}
						}
						counter++;
					}
                }
			}
			else if (style == Style.SelectMultipleRows)
			{
				int cursor = rowCursor;
				int count = 0;
				for (int r = 0; r < rowList.Count; ++r)
				{
					Row row = rowList[r];
					if (!row.Visible)
						continue;

					Rect rect = OnGUILayoutRow(row, false, row.Selected);

					if (EditorGUIEventUtil.IsRectDblClicked(rect, 0))
					{
						if (onDblClickRow != null)
						{
							onDblClickRow(row);
						}
					}

					if (EditorGUIEventUtil.IsRectClicked(rect, 1))
					{
						if (onRightClickRow != null)
						{
							onRightClickRow(row);
						}
					}

					if (EditorGUIEventUtil.IsRectClicked(rect, 0))
					{
						if (Event.current.control)
						{
							row.Selected = !row.Selected;
						}
						else if (Event.current.shift && rowCursor != -1)
						{
							if (r < cursor)
							{
								for (int i = r; i < cursor; ++i)
								{
									rowList[i].Selected = true;
								}
							}
							else
							{
								for (int i = cursor; i <= r; ++i)
								{
									rowList[i].Selected = true;
								}
							}
						}
						else
						{
							rowCursor = r;
							for (int i = 0; i < rowList.Count; ++i)
							{
								rowList[i].Selected = rowList[i] == row;
							}
						}
						if (onClickRow != null)
						{
							onClickRow(row);
						}
					}
					count++;
				}

				if (HandleKeyboard && EditorGUIEventUtil.ListKeyHandler(out keyCode, ref rowCursor, count))
				{
					for (int i = 0; i < rowList.Count; ++i)
					{
						rowList[i].Selected = i == rowCursor;
					}
				}
			}
            GUI.color = Color.white;
        }

        private void ScrollTo(KeyCode keyCode, float canvasHeight)
        {
            int heightOffset = (int)canvasHeight / RowList.Count;
            Vector2 scrollOffSet = keyCode == KeyCode.UpArrow ? new Vector2(0, -25f + heightOffset) : new Vector2(0, 25f - heightOffset);

            scrollView += scrollOffSet;
        }

        private int rowCursor = -1;
		public int GetRowCursor()
		{
			return rowCursor;
		}

		public Row GetSelectedRow()
		{
			if (rowCursor != -1)
				return rowList[rowCursor];
			return null;
		}

		public Item GetSelectedItem()
		{
			if (rowCursor != -1)
				return rowList[rowCursor].GetMainItem();
			return null;
		}

		public List<Item> GetSelectedItems()
		{
			return rowList.Where(item => item.Selected).Select(item => item.GetMainItem()).ToList();
		}

		public void ResetCursor()
		{
			if (rowCursor != -1)
			{
				rowList[rowCursor].Selected = false;
				rowCursor = -1;
			}
		}

		public void SelectRow(int rowIndex)
		{
			ResetCursor();
			rowCursor = rowIndex;
			if (rowCursor != -1)
			{
				Row row = rowList[rowCursor];
				row.Selected = true;
				if (onClickRow != null)
				{
					onClickRow(row);
				}
			}
		}

		private GUIStyle GetLabelStyle(ColumnAlignment alignment, bool bold, bool selected)
		{
			if (selected)
			{
				if (EditorGUIStyle.selected[0].normal.background == null)
				{
					EditorGUIStyle.Restore();
				}

				if (alignment == ColumnAlignment.Left)
					return EditorGUIStyle.selected[0];
				else if (alignment == ColumnAlignment.Center)
					return EditorGUIStyle.selected[1];
				else //if (alignment == ColumnAlignment.Right)
					return EditorGUIStyle.selected[2];
			}
			else
			{
				if (alignment == ColumnAlignment.Left)
					return EditorGUIStyle.label[0];
				else if (alignment == ColumnAlignment.Center)
					return EditorGUIStyle.label[1];
				else //if (alignment == ColumnAlignment.Right)
					return EditorGUIStyle.label[2];
			}
		}

		private Rect OnGUILayoutRow(Row row, bool bold, bool selected)
		{
			List<Item> itemList = row.ItemList;

			Rect rect = EditorGUILayout.BeginHorizontal();
			{
				for (int c = 0; c < columnList.Count; ++c)
				{
					Column column = columnList[c];

					GUIStyle columnGuiStyle = GetLabelStyle(column.Alignment != ColumnAlignment.None ? column.Alignment : alignment, bold, selected);
					if (column.Width != -1)
					{
						GUILayout.Label(c < itemList.Count ? itemList[c].Text : string.Empty, columnGuiStyle, GUILayout.Width(column.Width));
					}
					else
					{
						GUILayout.Label(c < itemList.Count ? itemList[c].Text : string.Empty, columnGuiStyle);
					}
				}
			}
			EditorGUILayout.EndHorizontal();
			return rect;
		}

		private void OnGUIRow(Row row, float y)
		{
			List<Item> itemList = row.ItemList;

			for (int c = 0; c < columnList.Count; ++c)
			{
				Rect rect = new Rect();
				rect.x = labelRect[c].x;
				rect.y = y;
				rect.width = labelRect[c].width;
				rect.height = labelRect[c].height;

				Column column = columnList[c];
				GUIStyle columnGuiStyle = GetLabelStyle(column.Alignment != ColumnAlignment.None ? column.Alignment : alignment, false, false);
				GUI.Label(rect, c < itemList.Count ? itemList[c].Text : string.Empty, columnGuiStyle);
			}
		}

	}

	public class ToolbarCtrl
	{
		public ToolbarCtrl(System.Type enumType)
		{
			// enumType.
		}
	}

	public class TreeCtrl
	{
		//private bool FunctionHeader(string header, bool expandedByDefault)
		//{
		//    GUILayout.Space(5);
		//    bool expanded = GetFunctionIsIncluded(m_BaseClass, header, expandedByDefault);
		//    bool expandedNew = GUILayout.Toggle(expanded, header, EditorStyles.foldout);
		//    if (expandedNew != expanded)
		//        SetFunctionIsIncluded(m_BaseClass, header, expandedNew);
		//    return expandedNew;
		//}
	}

}
