using System;
using System.Collections.Generic;
using UnityEngine;

namespace Aniz.Resource.Unit
{
    public class SpritePackerResource : IResource
    {
        private Sprite[] m_sprites;
        private Dictionary<string, Sprite> m_dicSprites = new Dictionary<string, Sprite>(StringComparer.CurrentCulture);

        public SpritePackerResource(UnityEngine.Object obj, UnityEngine.Object[] sprites, bool isAssetBundle) : base(obj, isAssetBundle)
        {
            m_sprites = (Sprite[])sprites;
        }

        public override eResourceType Type
        {
            get { return eResourceType.SpritePacker; }
        }

        public Dictionary<string, Sprite> DicSprites
        {
            get { return m_dicSprites; }
        }

        protected override bool InitUpdate()
        {
            for (int i = 0; i < m_sprites.Length; i++)
            {
                m_dicSprites.Add(m_sprites[i].name, m_sprites[i]);
            }

            return true;
        }

        public Sprite GetSprite(string name)
        {
            if (m_dicSprites.ContainsKey(name))
            {
                return m_dicSprites[name];
            }
            return null;
        }

        public override void UnLoad(bool unloadAllLoadedObjects)
        {
            if (m_sprites != null)
            {
                m_sprites = null;
            }
            base.UnLoad(unloadAllLoadedObjects);
        }
    }
}