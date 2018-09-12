using Aniz.Resource;
using UnityEngine;

namespace Aniz.Resource.Unit
{
	public class TextureResouce : IResource
	{
		private GameObject m_gameObject;
		private Texture m_gameTexture;

		public TextureResouce(Object obj, bool isAssetBundle) : base(obj, isAssetBundle)
		{

		}

		public override eResourceType Type
		{
			get { return eResourceType.Texture; }
		}

		public Texture GameTexture
		{
			get { return m_gameTexture; }
		}

		protected override bool InitUpdate()
		{
			m_gameTexture = (Texture)ResourceData;
			return true;
		}

		public override void UnLoad(bool unloadAllLoadedObjects)
		{
			if (m_gameTexture != null)
			{
				//Texture.Destroy(m_gameTexture);
				m_gameTexture = null;
			}
			base.UnLoad(unloadAllLoadedObjects);
		}

		//         public override GameObject UnityGameObject
		//         {
		//             get { return m_gameObject; }
		//         }

	}
}