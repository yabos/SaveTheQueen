using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Aniz;

namespace Aniz.NodeGraph.Level
{
    public class ActorLevel : Level<ActorLevelSetting>
    {
        public override eNodeStorageType StorageType
        {
            get { return eNodeStorageType.Actor; }
        }

        public override eNodeType NodeType
        {
            get { return eNodeType.ActorLevel; }
        }

        public override void BhvOnEnter()
        {
            base.BhvOnEnter();
        }

        public override void BhvOnLeave()
        {
            base.BhvOnLeave();
        }

        public override void SetParentTransform(GameObject gameObject, Transform parent)
        {
            base.SetParentTransform(gameObject, parent);
        }

        public override IEnumerator OnLoadLevel(System.Action<float> OnProgressAction)
        {
            OnProgressAction(1.0f);
            yield return null;
        }

        public override void OnStartLevel()
        {
            int childCount = GetNumChildren();

            for (int i = 0; i < childCount; ++i)
            {
                LevelBase level = GetChild(i) as LevelBase;
                if (level != null)
                {
                    level.OnStartLevel();
                }
            }
        }
    }
}