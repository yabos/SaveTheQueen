

using System;
using System.Collections.Generic;

using table.db;
using UnityEngine;

namespace Aniz.Data
{
    public class ResData : BinaryScriptData
    {
        public enum eType
        {
            Map,
            Object,
            Item,
            Weapon,
            Armor,
            Character,
        }

        private DB_SpriteDataList m_dbSpriteDataList = new DB_SpriteDataList();
        private Dictionary<string, DB_SpriteData> m_dicSprite = new Dictionary<string, DB_SpriteData>(StringComparer.InvariantCulture);
        private List<DB_SpriteData> m_lstMonster = new List<DB_SpriteData>();

        public override void Release()
        {
            m_dbSpriteDataList.maps.Clear();
            m_dbSpriteDataList.objects.Clear();
            m_dbSpriteDataList.items.Clear();
            m_dbSpriteDataList.weapons.Clear();
            m_dbSpriteDataList.armors.Clear();
            m_dbSpriteDataList.characters.Clear();
        }

        public DB_SpriteData GetSpriteData(string name)
        {
            if (m_dicSprite.ContainsKey(name))
            {
                return m_dicSprite[name];
            }
            return null;
        }

        public List<DB_SpriteData> GetCharacterSpriteList()
        {
            return m_dbSpriteDataList.characters;
        }

        private void AddSpriteData(List<DB_SpriteData> dbSprite, string subpath = "")
        {
            for (int i = 0; i < dbSprite.Count; i++)
            {
                dbSprite[i].subPath = subpath;
                m_dicSprite.Add(dbSprite[i].spriteName, dbSprite[i]);
            }
        }

        private void PostProcess()
        {
            AddSpriteData(m_dbSpriteDataList.maps, "Auto/");
            AddSpriteData(m_dbSpriteDataList.items);
            AddSpriteData(m_dbSpriteDataList.objects);
            AddSpriteData(m_dbSpriteDataList.armors);
            AddSpriteData(m_dbSpriteDataList.weapons);
            AddSpriteData(m_dbSpriteDataList.characters, "Character/");

            for (int i = 0; i < m_dbSpriteDataList.characters.Count; i++)
            {
                if (m_dbSpriteDataList.characters[i].objType == E_ObjectType.Monster
                    || m_dbSpriteDataList.characters[i].option.Equals("battle"))
                {
                    m_lstMonster.Add(m_dbSpriteDataList.characters[i]);
                }
            }
        }

        public string GetRandomMonster()
        {
            return m_lstMonster[UnityEngine.Random.Range(0, m_lstMonster.Count)].spriteName;
        }

        protected override bool ReadData(BinaryDecoder decoder)
        {
            bool succ = true;
            if (m_dbSpriteDataList.Decode(decoder) == false)
            {
                succ = false;
            }

            PostProcess();

            return succ;
        }

    }
}