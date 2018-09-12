using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lib.uGui
{
    public class UIModuleCoolTimeIcon : MonoBehaviour, IUIModule
    {
        public UnityEngine.UI.Image BackgroundIcon = null;
        public UnityEngine.UI.Image CoolTimeIcon = null;

        private Coroutine m_progressCoroutine = null;

        #region IUIModule

        public virtual void OnEnterModule()
        {
        }

        public virtual void OnExitModule()
        {

        }

        public virtual void OnRefreshModule()
        {

        }

        public virtual void OnDestroyModule()
        {

        }

        #endregion IUIModule

        private IEnumerator CoolTimeProgressCoroutine(UnityEngine.UI.Image coolTimeProgressImage, float duration,
            bool inverse, System.Action completed)
        {
            float t = 0.0f;
            while (t < 1.0f)
            {
                yield return new WaitForEndOfFrame();
                t = Mathf.Clamp01(t + Time.deltaTime / duration);

                if (coolTimeProgressImage != null)
                {
                    coolTimeProgressImage.fillAmount = (inverse == true) ? 1.0f - t : t;
                }
            }

            yield return new WaitForEndOfFrame();

            if (completed != null)
            {
                completed();
            }
        }

        public void InitCoolTimeIcon(int skillID)
        {

        }

        public void SetCoolTimeIcon(float duration, bool inverse, System.Action completed)
        {
            if (m_progressCoroutine != null)
            {
                StopCoroutine(m_progressCoroutine);
                m_progressCoroutine = null;
            }

            if (CoolTimeIcon != null)
            {
                CoolTimeIcon.gameObject.SetActive(true);
                CoolTimeIcon.fillAmount = (inverse == true) ? 1.0f : 0.0f;
            }

            m_progressCoroutine = StartCoroutine(CoolTimeProgressCoroutine(CoolTimeIcon, duration, inverse, () =>
            {
                m_progressCoroutine = null;

                if (CoolTimeIcon != null)
                {
                    CoolTimeIcon.gameObject.SetActive(false);
                }

                if (completed != null)
                {
                    completed();
                }
            }));
        }
    }

}