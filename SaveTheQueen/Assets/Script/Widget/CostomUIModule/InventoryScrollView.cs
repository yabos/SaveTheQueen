using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Lib.uGui;

namespace Aniz.Widget.Module
{

    // 임시 코드 테스트용( 테이블 데이타로 교체 )
    [System.Serializable]
    public class InventoryData
    {
        public int IDX;
        public string Name;

        public InventoryData(int idx, string name)
        {
            IDX = idx;
            Name = name;
        }
    }


    [System.Serializable]
    public class InventoryScrollView : UIModuleGridScrollView<InventoryData>
    {
        public void SetInfo(List<InventoryData> list)
        {
            base.OnEnterModule();

            OnScrollEnter(list);
        }

        public override void OnExitModule()
        {
            base.OnExitModule();
        }

        protected override void OnInit()
        {
            base.OnInit();
        }

        public override void OnSelectEvent<T1>(T1 value = default(T1))
        {
        }

        public override void OnDestroyModule()
        {
            base.OnDestroyModule();
        }
    }

}