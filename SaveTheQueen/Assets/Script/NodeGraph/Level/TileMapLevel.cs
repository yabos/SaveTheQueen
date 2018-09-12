using UnityEngine;
using System.Collections;
using System.Linq;
using Aniz;
using Aniz.Basis;
using Aniz.Data.Map;
using Aniz.Data.Map.Info;
using System.Collections.Generic;
using Aniz.Data;

namespace Aniz.NodeGraph.Level
{
    public class TileMapLevel : Level<TileMapLevelSetting>
    {

        public override eNodeStorageType StorageType
        {
            get { return eNodeStorageType.Scene; }
        }


        public override eNodeType NodeType
        {
            get { return eNodeType.TileMapLevel; }
        }



        public override void BhvOnEnter()
        {


            base.BhvOnEnter();
        }

        public override void BhvOnLeave()
        {
            base.BhvOnLeave();

        }

        public void CreateTileMap()
        {

        }

        public void OpenMap(string mapname, System.Action<float> OnProgressAction)
        {
           

            m_setting.StartCoroutine(OnLoadLevel(OnProgressAction));
        }

        public void ReleaseMap()
        {
        }
        
        public void UserFogUpdate(int X, int Y)
        {
        }
        
        public override void SetParentTransform(GameObject gameObject, Transform parent)
        {
            base.SetParentTransform(gameObject, parent);
        }

        public override IEnumerator OnLoadLevel(System.Action<float> OnProgressAction)
        {
           
            yield return new WaitForEndOfFrame();
        }

        public override void OnStartLevel()
        {

        }
    }
}