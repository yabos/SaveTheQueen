using UnityEngine;
using Lib.Pattern;
using UIExtension;

namespace Lib.uGui
{
    public interface IScrollReceiver
    {
        void OnSetInfoEvent<T>(T value = default(T));
        void OnSelectEvent<T>(T value = default(T));
        void OnDoubleClickEvent<T>(T value = default(T));
    }

    public class ScrollItem : MonoBehaviour, System.IDisposable
    {

        protected long m_itemIDX;

        public long ItemIDX
        {
            get { return m_itemIDX; }
        }

        private bool m_isAlive;

        [HideInInspector]
        public RectTransform Owner;

        public bool Alive
        {
            get { return m_isAlive; }
        }

        protected IScrollReceiver m_receiver = null;
        protected IUIAction m_actionmodule = null;

        protected float m_preferredSize = 0.0f;

        private CanvasGroup m_canvasGroup = null;

        protected RectTransform m_rectTransform = null;

        protected Vector2 m_initSizeDelta = Vector2.zero;

        protected UnityEngine.UI.Button m_button;

        void Awake()
        {
            OnAwake();
            m_button = GetComponent<UnityEngine.UI.Button>();
            if (m_button != null)
            {
                m_button.AddOnClick(OnSelectButtonClk);
            }
        }

        void Start()
        {
            OnStart();
        }

        public virtual void OnAwake()
        {
        }

        public virtual void OnStart()
        {
        }

        #region ActionModule

        public void ConnectActionModule(IUIAction module)
        {
            m_actionmodule = module;
        }

        public void OnMessageCallback(string key, params object[] args)
        {
            if (m_actionmodule == null)
                return;

            m_actionmodule.MessageCallback(key, args);
        }

        #endregion

        #region PreferredSize

        public virtual float GetPreferredSize()
        {
            return 0.0f;
        }

        #endregion

        public virtual void SetInfo<T>(T data, IScrollReceiver receiver) where T : class
        {
            this.m_receiver = receiver;

            if (m_rectTransform == null)
            {
                m_rectTransform = ComponentFactory.GetComponent<RectTransform>(gameObject, IfNotExist.ReturnNull);

                if (m_rectTransform != null)
                {
                    m_initSizeDelta = m_rectTransform.sizeDelta;
                }
            }
        }

        public virtual void ReplaceText()
        {
        }

        public void UpdateOwner()
        {
            Owner = this.transform as RectTransform;

            Owner.anchoredPosition3D = Vector3.zero;
            Owner.localScale = Vector3.one;
        }

        public RectTransform GetOwner()
        {
            if (Owner == null)
                Owner = this.transform as RectTransform;

            return Owner;
        }

        public void SetPosition(Vector2 pos)
        {
            Owner.anchoredPosition = pos;
        }

        public virtual void SetAlive(bool isActive)
        {
            if (m_isAlive == isActive)
                return;

            m_isAlive = isActive;

            if (m_canvasGroup == null)
            {
                m_canvasGroup = ComponentFactory.GetComponent<CanvasGroup>(this.gameObject, IfNotExist.AddNew);
            }

            m_canvasGroup.alpha = (isActive == true) ? 1 : 0;
            m_canvasGroup.blocksRaycasts = isActive;
            m_canvasGroup.interactable = isActive;

            this.gameObject.SetActive(true);
        }

        protected virtual void OnDestroy()
        {
            m_receiver = null;

            m_actionmodule = null;

            Dispose();
        }

        public virtual void Dispose()
        {
        }

        private void OnSelectButtonClk()
        {
            if (m_receiver != null)
            {
                m_receiver.OnSelectEvent(this);
            }
        }
    }

}