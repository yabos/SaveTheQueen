using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Aniz.Resource.Unit
{

	public class ScriptResource : IResource
	{
		private TextAsset m_texObject;
		private List<string[]> m_lstValue = new List<string[]>();
		//private string m_strTitle;
		private int m_nRows = 0;
		private int m_nCols = 0;

		public ScriptResource(Object obj, bool isAssetBundle)
			: base(obj, isAssetBundle)
		{

		}

		public override eResourceType Type
		{
			get { return eResourceType.Script; }
		}

		public TextAsset TexObject
		{
			get { return m_texObject; }
		}

		public List<string[]> LstValue
		{
			get { return m_lstValue; }
		}

		protected override bool InitUpdate()
		{
			m_texObject = (TextAsset)ResourceData;
			return true;
		}

		public bool LoadCSV()
		{
			try
			{
				string[] lineData = m_texObject.text.Split('\n');

				char[] Separators = new char[] { ',' };
				foreach (string line in lineData)
				{
					string[] arLine = line.Split(Separators, System.StringSplitOptions.None);
					if (m_nRows == 0)
					{
						m_nRows++;
						m_nCols = arLine.Length;
						continue;
					}
					m_nRows++;
					if (arLine.Length < m_nCols)
					{
						Debug.Log(string.Format("Err Line {0} : cols {1} - {2} \n", m_nRows, m_nCols, line));
						continue;
					}
					if (arLine != null)
					{
						LstValue.Add(arLine);
					}
				}
			}
			catch
			{
				return false;
			}
			return true;
		}

		public override void UnLoad(bool unloadAllLoadedObjects)
		{
			if (m_texObject != null)
			{
				m_texObject = null;
				//TextAsset.Destroy(m_texObject);
				//GameObject.Destroy(m_texObject);


			}
		}


		//public override GameObject UnityGameObject
		//{
		//	get { return null; }
		//}
	}
}