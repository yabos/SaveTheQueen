using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.NetworkSystem;

namespace Aniz.Resource.Unit
{
    public class SpriteAssetResource : IResource
    {
        protected Dictionary<string, Sprite> m_dictionary = new Dictionary<string, Sprite>();
        protected Sprite[] m_allsprite;

        public SpriteAssetResource(Sprite[] allobj, bool isAssetBundle)
            : base(null, isAssetBundle)
        {
            m_allsprite = allobj;
        }

        public override eResourceType Type
        {
            get { return eResourceType.SpriteAsset; }
        }

        public Sprite[] Allsprite
        {
            get { return m_allsprite; }
        }

        public Sprite GetSprite(string name)
        {
            if (m_dictionary.ContainsKey(name))
            {
                return m_dictionary[name];
            }
            return null;
        }

        protected override bool InitUpdate()
        {
            if (m_allsprite != null)
            {
                for (int i = 0; i < m_allsprite.Length; i++)
                {
                    m_dictionary.Add(m_allsprite[i].name, m_allsprite[i]);
                }
            }

            return true;
        }

        public Sprite GetSprite(int index)
        {
            if (m_allsprite.Length > index)
            {
                return m_allsprite[index];
            }
            return null;
        }

        public Sprite GetRandom()
        {
            return m_allsprite[Random.Range(0, m_allsprite.Length)];
        }

        public override void UnLoad(bool unloadAllLoadedObjects)
        {
            m_dictionary.Clear();
            if (m_allsprite != null)
            {
                m_allsprite = null;
            }
        }
    }
}