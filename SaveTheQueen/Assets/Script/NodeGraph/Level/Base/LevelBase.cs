using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Aniz.Graph;
using Lib.Pattern;
using UnityEngine;

namespace Aniz.NodeGraph.Level
{
    public class LevelSettingBase : GraphMono
    {
        #region Propeties

        public bool UseDebugLog = false;

        #endregion Propeties

        #region Log Methods
        public void Log(string msg)
        {
            msg = StringUtil.Format("<color=#ffffffff>{0}</color>", msg);
            if (UseDebugLog)
            {
                Debug.Log(msg);
            }
        }

        public void LogWarning(string msg)
        {
            msg = StringUtil.Format("<color=#ffff00ff>{0}</color>", msg);
            if (UseDebugLog)
            {
                Debug.LogWarning(msg);
            }
        }

        public void LogError(string msg)
        {
            msg = StringUtil.Format("<color=#ff0000ff>{0}</color>", msg);
            if (UseDebugLog)
            {
                Debug.LogError(msg);
            }
        }

        #endregion Log Methods
    }

    public abstract class LevelBase : GraphNodeGroup
    {
        #region Propeties


        public abstract eNodeStorageType StorageType { get; }

        public abstract GameObject gameObject { get; }

        public abstract Transform transform { get; }

        #endregion Propeties

        #region Methods


        public abstract void SetParentTransform(GameObject gameObject, Transform parent);

        public abstract IEnumerator OnLoadLevel(System.Action<float> OnProgressAction);

        public abstract void OnStartLevel();

        #endregion Methods
    }

    public abstract class Level<T> : LevelBase where T : LevelSettingBase
    {
        #region Propeties

        protected T m_setting = null;
        public T Setting
        {
            get { return m_setting; }
        }

        public override GameObject gameObject
        {
            get
            {
                if (Setting != null)
                {
                    return Setting.gameObject;
                }
                return null;
            }
        }

        public override Transform transform
        {
            get
            {
                if (Setting != null)
                {
                    return Setting.transform;
                }
                return null;
            }
        }

        #endregion Propeties

        #region Methods

        public override void BhvOnEnter()
        {
            base.BhvOnEnter();
        }

        public override void BhvOnLeave()
        {
            base.BhvOnLeave();

            if (m_setting != null)
            {
                GameObjectFactory.DestroyComponent(m_setting);
                m_setting = null;
            }
        }

        public override void SetParentTransform(GameObject gameObject, Transform parent)
        {
            if (gameObject == null)
            {
                throw new System.Exception("gameObject == null");
            }

            if (m_setting == null)
            {
                m_setting = ComponentFactory.GetComponent<T>(gameObject, IfNotExist.AddNew);
            }

            if (this.gameObject != null)
            {
                Name = this.gameObject.name;
            }

            if (transform != null)
            {
                transform.SetParent(parent);
            }
        }

        #endregion Methods
    }
}