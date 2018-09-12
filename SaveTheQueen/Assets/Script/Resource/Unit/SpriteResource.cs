using UnityEngine;

namespace Aniz.Resource.Unit
{
    public class SpriteResource : IResource
    {
        private Sprite m_sprite;

        public SpriteResource(Object obj, bool isAssetBundle)
            : base(obj, isAssetBundle)
        {
            m_sprite = obj as Sprite;
        }


        public override eResourceType Type
        {
            get { return eResourceType.Sprite; }
        }

        public Sprite Sprite
        {
            get { return m_sprite; }
        }

        protected override bool InitUpdate()
        {
            if (m_resourceData is Sprite)
            {

            }

            return true;
        }

        public override void UnLoad(bool unloadAllLoadedObjects)
        {
            if (m_sprite != null)
            {
                m_sprite = null;
                //TextAsset.Destroy(m_texObject);
                //GameObjectFactory.Destroy(m_texObject);
            }
            base.UnLoad(unloadAllLoadedObjects);
        }
    }
}