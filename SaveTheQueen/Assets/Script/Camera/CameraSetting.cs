using System;
using UnityEngine;
using System.Collections;
using Aniz.Cam.Info;
using Aniz;

namespace Aniz.Cam
{
    [DisallowMultipleComponent]
    public class CameraSetting : ManagerSettingBase
    {

        [SerializeField]
        private CameraStock m_cameraStock = new CameraStock();
        [SerializeField]
        private LimitStock m_limitStock = new LimitStock();

        public CameraStock CameraStock
        {
            get { return m_cameraStock; }
        }

        public LimitStock LimitStock
        {
            get { return m_limitStock; }
        }

    }
}