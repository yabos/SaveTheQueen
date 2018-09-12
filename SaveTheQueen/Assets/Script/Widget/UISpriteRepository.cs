using System;
using System.Collections.Generic;
using Aniz.Resource;
using Aniz.Resource.Unit;
#if UNITY_EDITOR
using UnityEditorInternal;
#endif //UNITY_EDITOR
using UnityEngine;

namespace Aniz.Widget
{
    public class UISpriteRepository : Lib.Pattern.IRepository<string, Sprite>
    {
        private Dictionary<string, Sprite> m_sprites = new Dictionary<string, Sprite>(StringComparer.CurrentCultureIgnoreCase);

        public void Initialize()
        {
        }

        public bool Get(string index, out Sprite t2)
        {
            if (m_sprites.TryGetValue(index, out t2))
            {
                return true;
            }

            SpriteResource resource = Global.ResourceMgr.CreateSpriteResource(index, ePath.UISprite, true);
            if (resource != null)
            {
                t2 = resource.Sprite;

                m_sprites.Add(index, t2);
                return true;
            }

            return false;
        }

        public void Insert(Sprite node)
        {
            m_sprites.Add(node.name, node);
        }

        public bool Remove(string index)
        {
            return m_sprites.Remove(index);
        }


        public void Terminate()
        {
            m_sprites.Clear();
        }
    }
}