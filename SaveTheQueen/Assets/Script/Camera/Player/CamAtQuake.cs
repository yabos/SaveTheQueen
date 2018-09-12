using System;
using UnityEngine;
using System.Collections.Generic;
using Aniz.Cam.Info;
using Aniz.Cam.Quake;

namespace Aniz.Cam.Player
{


    public class CamAtQuake : ICameraPlayer
    {
        private List<AtQuakeUnit.Stock> m_lstEffectAtQuakes = new List<AtQuakeUnit.Stock>();
        private Vector3 m_quake;

        public E_CameraPlayer CameraPlayer
        {
            get { return E_CameraPlayer.EyeQuakeEffect; }
        }

        public void SetQuakeUnit(AtQuakeUnit.Stock effectQuakeStock)
        {
            for (int i = 0; i < m_lstEffectAtQuakes.Count; i++)
            {
                AtQuakeUnit.Stock quakeStock = m_lstEffectAtQuakes[i];
                if (quakeStock.Name == effectQuakeStock.Name)
                {
                    return;
                }
            }
            m_lstEffectAtQuakes.Add(effectQuakeStock);
        }
        public void RemoveQuakeUnit(AtQuakeUnit.Stock effectQuakeStock)
        {
            for (int i = 0; i < m_lstEffectAtQuakes.Count; i++)
            {
                AtQuakeUnit.Stock quakeStock = m_lstEffectAtQuakes[i];
                if (quakeStock.Name == effectQuakeStock.Name)
                {
                    m_lstEffectAtQuakes.Remove(quakeStock);
                    return;
                }
            }

        }

        private void ProcessQuakeUnit()
        {
            m_quake = Vector3.zero;
            for (int i = 0; i < m_lstEffectAtQuakes.Count; i++)
            {
                AtQuakeUnit.Stock effectAtQuakeStock = m_lstEffectAtQuakes[i];
                if (effectAtQuakeStock.Power > 0)
                {
                    m_quake += effectAtQuakeStock.Direction * effectAtQuakeStock.Power;
                }
            }
        }

        public void Update(ref CameraUpdateInfo cameraUpdateInfo, float deltaTime)
        {
            if (m_lstEffectAtQuakes.Count <= 0)
                return;

            m_lstEffectAtQuakes.Sort(new AtQuakeUnit.Stock.Sort());

            ProcessQuakeUnit();

            cameraUpdateInfo.At += m_quake;
        }
    }
}