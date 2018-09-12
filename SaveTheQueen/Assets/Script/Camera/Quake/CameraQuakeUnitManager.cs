using System;
using System.Collections.Generic;
using Aniz.Cam.Info;
using Aniz.Cam.Quake;
using Lib.Event;
using Lib.Pattern;
using UnityEngine;

namespace Aniz.Cam
{
    public class CameraQuakeUnitManager : NotifyHanlder, IBaseClass
    {
        private Dictionary<string, CameraQuakeUnit> m_cameraEffectUnits = new Dictionary<string, CameraQuakeUnit>();

        public override string HandlerName { get; set; }

        private bool m_NotifyEnd = false;

        public override void ConnectHandler()
        {
            Global.NotificationMgr.ConnectHandler(this);
        }

        public override void DisconnectHandler()
        {
            Global.NotificationMgr.DisconnectHandler(this);
        }

        public void Initialize()
        {
            HandlerName = "CameraQuakeUnitManager";
            ConnectHandler();
        }

        public void Terminate()
        {
            m_cameraEffectUnits.Clear();
            DisconnectHandler();
        }


        public void OnUpdate(float dt)
        {
            bool play = false;
            using (var itor = m_cameraEffectUnits.GetEnumerator())
            {
                while (itor.MoveNext())
                {
                    if (itor.Current.Value.IsFinish)
                        continue;

                    if (itor.Current.Value.Process(dt))
                    {
                        play = true;
                    }
                }
            }


            m_NotifyEnd = !play;
        }


        public bool LoadAtEffect(string cameraData)
        {
            AtQuakeInfo atCameraData;
            //bool succ = DataManager.Instance.CameraTable.GetAtData(cameraData, out atCameraData);
            //bool succ = CameraTable.Instance.GetAtData(cameraData, out atCameraData);

            //if (succ == false)
            //{
            //	return false;
            //}

            //if (m_cameraEffectUnits.ContainsKey(atCameraData.EffectName))
            //{
            //	return true;
            //}

            //CameraQuakeUnit cameraQuakeUnit = new AtQuakeUnit();
            //cameraQuakeUnit.Capture();
            //cameraQuakeUnit.Initialize(atCameraData);

            //m_cameraEffectUnits.Add(atCameraData.EffectName, cameraQuakeUnit);

            return true;
        }

        public void PlayAtEffect(Transform parent, string cameraData)
        {
            if (m_cameraEffectUnits.ContainsKey(cameraData))
            {
                CameraQuakeUnit quakeUnit = m_cameraEffectUnits[cameraData];

                if (parent != null)
                {
                    quakeUnit.SetParent(parent);
                }
                quakeUnit.Play(Vector3.zero);
            }
        }

        public void StopAtEffect(string cameraData)
        {
            if (m_cameraEffectUnits.ContainsKey(cameraData))
            {
                CameraQuakeUnit quakeUnit = m_cameraEffectUnits[cameraData];

                quakeUnit.Terminate();
            }
        }

        public override void OnNotify(INotify notify)
        {

        }


        /*
        public void OnHandleEvent(IGameNormalEventSender sender, E_GameNormalEvent e, params System.Object[] args)
        {
            switch (e)
            {
                case E_GameNormalEvent.Load_Camera_Shaking:
                    {
                        if (Application.isPlaying)
                        {
                            string name = (string)args[0];
                            LoadAtEffect(name);
                        }
                    }
                    break;
                case E_GameNormalEvent.Target_Camera_Shaking:
                    {
                        Transform Owner = (Transform)args[0];
                        string name = (string)args[1];
                        PlayAtEffect(Owner, name);
                    }
                    break;
                case E_GameNormalEvent.Trigger_Camera_Shaking:
                    {
                        Transform Owner = (Transform)args[0];
                        string name = (string)args[1];
                        bool play = (bool)args[2];
                        if (play)
                        {
                            //PlayAtEffect(PawnCharacterManager.Instance.MainHero.gameObject, name);
                            PlayAtEffect(Owner, name);
                        }
                        else
                        {
                            StopAtEffect(name);
                        }
                    }
                    break;

            }
        }
        */
    }
}