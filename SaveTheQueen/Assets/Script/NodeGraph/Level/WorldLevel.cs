using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aniz.NodeGraph.Level;
using Aniz;
using Aniz.Graph;
using Lib.Pattern;
using UnityEngine;

namespace Aniz.NodeGraph.Level
{
    public class WorldLevel : Level<WorldLevelSetting>
    {
        private GameObject m_worldGameObject;

        public override eNodeStorageType StorageType
        {
            get { return eNodeStorageType.Scene; }
        }

        public override eNodeType NodeType
        {
            get { return eNodeType.WorldLevel; }
        }

        protected TileMapLevel m_tileMapLevel = null;

        public TileMapLevel TileMapLevel
        {
            get
            {
                return m_tileMapLevel;
            }
        }
        
        protected ActorLevel m_actorLevel = null;

        public ActorLevel ActorLevel
        {
            get
            {
                return m_actorLevel;
            }
        }

        public override void BhvOnEnter()
        {
            base.BhvOnEnter();
        }

        public override void BhvOnLeave()
        {
            base.BhvOnLeave();
        }


        public override IEnumerator OnLoadLevel(System.Action<float> OnProgressAction)
        {
            yield return null;
        }
        public override void OnStartLevel()
        {
        }

        public void CreateFirstLevel()
        {
            {
                Name = "World";
                m_worldGameObject = new GameObject(Name);
            }

            {
                // environment level setting
                if (CreateLevelWithTag<TileMapLevel>("TileMapLevel", eNodeType.TileMapLevel, m_worldGameObject.transform, ref m_tileMapLevel))
                {
                    AddLevel(m_tileMapLevel);
                }
            }
            
            {
                // actor level setting
                if (CreateLevelWithTag<ActorLevel>("ActorLevel", eNodeType.ActorLevel, m_worldGameObject.transform, ref m_actorLevel))
                {
                    AddLevel(m_actorLevel);
                }
            }
        }

        public void ClearFirstLevel()
        {
            if (m_actorLevel != null)
            {
                RemoveLevel(m_actorLevel);
                m_actorLevel = null;
            }
            
            if (m_tileMapLevel != null)
            {
                RemoveLevel(m_tileMapLevel);
                m_tileMapLevel = null;
            }

            if (m_worldGameObject != null)
            {
                GameObjectFactory.Destroy(m_worldGameObject);
                m_worldGameObject = null;
            }
        }

        public static bool CreateLevel<T>(GameObject settingGameObject, eNodeType nodeType, Transform parent, ref T level) where T : LevelBase, new()
        {
            if (level != null)
            {
                return false;
            }

            if (settingGameObject == null)
            {
                string[] gameObjectNames = StringUtil.Split(nodeType.ToString(), ".");
                if (gameObjectNames.Any() == false)
                {
                    throw new System.Exception(StringUtil.Format("null is gameObjectNames", nodeType.ToString()));
                }

                settingGameObject = new GameObject(gameObjectNames[gameObjectNames.Length - 1]);
                settingGameObject.tag = string.IsNullOrEmpty(parent.tag) ? settingGameObject.name : parent.tag;
            }

            level = (T)System.Activator.CreateInstance(typeof(T));
            level.SetParentTransform(settingGameObject, parent);
            return (level != null) ? true : false;
        }

        public static bool CreateLevelWithTag<T>(string tag, eNodeType nodeType, Transform parent, ref T level) where T : LevelBase, new()
        {
            if (level != null)
            {
                return false;
            }

            GameObject settingGameObject = null;
            GameObject[] settingGameObjects = GameObject.FindGameObjectsWithTag(tag);
            if (settingGameObjects.Length == 1)
            {
                settingGameObject = settingGameObjects[0];
            }
            else
            {
                if (settingGameObjects.Length > 1)
                {
                    throw new System.Exception(StringUtil.Format("{0}.Length != 1", tag));
                }
            }

            return CreateLevel<T>(settingGameObject, nodeType, parent, ref level);
        }

        public void AddLevel(LevelBase level)
        {
            if (level != null && FindLevel(level) == null)
            {
                AttachChild(level);
            }
        }

        public void RemoveLevel(LevelBase level)
        {
            if (level != null && FindLevel(level) != null)
            {
                DetachChild(level);
            }
        }

        private LevelBase FindLevel(LevelBase level)
        {
            for (int i = 0; i < children.Count; i++)
            {
                if (children[i] is LevelBase)
                {
                    LevelBase childrenLevel = children[i] as LevelBase;
                    if (childrenLevel == level)
                    {
                        return childrenLevel;
                    }
                }
            }
            return null;
        }

        public LevelBase FindLevel(eNodeStorageType eNodeStorage)
        {
            for (int i = 0; i < children.Count; i++)
            {
                if (children[i] is LevelBase)
                {
                    LevelBase childrenLevel = children[i] as LevelBase;
                    if (childrenLevel.StorageType == eNodeStorage)
                    {
                        return childrenLevel;
                    }
                }
            }
            return null;
        }
    }

}