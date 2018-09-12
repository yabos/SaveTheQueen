using System.Collections;
using System.Linq;
using Aniz;
using Aniz.Graph;
using Lib.Event;
using UnityEngine;
using Lib.Pattern;
using Lib.uGui;

namespace Aniz.Widget
{

    public abstract class WidgetBase : NotifyHanlderBehaviour, IGraphUpdatable
    {
        public UnityEngine.Canvas Canvas = null;

        public CanvasGroup CanvasGroup = null;

        public bool IsVolatileObject = false;

        public bool IsPopupType = false;

        public bool IsSetCanvasLayer = true;

        public bool IsOverlayCanvas = false;

        protected bool m_isActive = false;

        public string UniqueName { get; private set; }
        public string WidgetName { get; private set; }
        public int WidgetID { get; private set; }

        public bool IsActive
        {
            get { return m_isActive; }
        }

        public bool IsGameOjectActive
        {
            get { return gameObject.activeSelf; }
        }

        //뒤로가기 지원 UI만... true로..
        public abstract bool IsFlow { get; }

        //ui getcomponent 하는 곳을 enter 종료할때는 leave로 사용하는 것이 좋을듯한데..
        void Awake()
        {
            string typeName = this.GetType().ToString();
            string[] split = typeName.Split('.');
            if (split.Length > 0)
            {
                WidgetName = split[split.Length - 1];
            }

            WidgetID = IDFactory.GenerateUIID();
            BhvOnEnter();
        }

        void OnDestroy()
        {
            BhvOnLeave();
            IDFactory.DeleteUIID(WidgetID);
            WidgetID = 0;
        }

        #region IBhvUpdatable

        public abstract void BhvOnEnter();
        public abstract void BhvOnLeave();

        public virtual void BhvFixedUpdate(float dt)
        {
        }

        public virtual void BhvLateFixedUpdate(float dt)
        {
        }

        public virtual void BhvUpdate(float dt)
        {
        }

        public virtual void BhvLateUpdate(float dt)
        {
        }

        #endregion // "IBhvUpdatable"

        #region IEventHandler

        public override eNotifyHandler GetHandlerType()
        {
            return eNotifyHandler.Widget;
        }

        public override void ConnectHandler()
        {
            Global.NotificationMgr.ConnectHandler(this);
        }

        public override void DisconnectHandler()
        {
            Global.NotificationMgr.DisconnectHandler(this);
        }

        //public override void OnConnectHandler()
        //{
        //    base.OnConnectHandler();
        //}

        //public override void OnDisconnectHandler()
        //{
        //    base.OnDisconnectHandler();
        //}


        public virtual bool OnMessage(IMessage message)
        {
            return false;
        }


        #endregion IEventHandler

        #region Methods

        public void InitializeWidget(string name)
        {
            UniqueName = name;
            // temp by wani
            {
                UnityEngine.UI.CanvasScaler[] canvasScalers =
                    ComponentFactory.GetChildComponents<UnityEngine.UI.CanvasScaler>(gameObject, IfNotExist.ReturnNull);

                if (canvasScalers != null && canvasScalers.Any())
                {
                    for (int i = 0; i < canvasScalers.Length; ++i)
                    {
                        if (canvasScalers[i].uiScaleMode == UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize)
                        {
                            //canvasScalers[i].referenceResolution = new Vector2(Screen.width, Screen.height);
                            //canvasScalers[i].referenceResolution = new Vector2(1280, 720);
                        }
                    }
                }
            }

            if (IsPopupType)
            {
                CanvasGroup = ComponentFactory.GetComponent<CanvasGroup>(gameObject, IfNotExist.AddNew);
                CanvasGroup.alpha = 0;
            }
        }

        public virtual void FinalizeWidget()
        {
            if (IsPopupType)
            {
                CanvasGroup = ComponentFactory.GetComponent<CanvasGroup>(gameObject, IfNotExist.AddNew);
                CanvasGroup.alpha = 1;
            }
            gameObject.SetActive(false);
        }

        private Coroutine FadeCoroutine = null;

        protected abstract void ShowWidget(IUIDataParams data);
        protected abstract void HideWidget();

        public void Show(float activeTime = 0.0f, IUIDataParams data = null, bool flowCommand = false)
        {
            if (FadeCoroutine != null)
            {
                StopCoroutine(FadeCoroutine);
                FadeCoroutine = null;
            }

            m_isActive = true;

            if (CanvasGroup != null)
            {
                CanvasGroup.blocksRaycasts = IsActive;
                CanvasGroup.interactable = IsActive;
            }

            gameObject.SetActive(IsActive);
            ShowWidget(data);

            if (IsFlow && IsPopupType == false && flowCommand == false)
            {
                Global.WidgetMgr.AddFlow(this, activeTime, data);
            }

            if (activeTime != 0.0f)
            {
                if (CanvasGroup != null)
                {
                    CanvasGroup.alpha = 0.0f;
                }

                if (IsGameOjectActive == true)
                {
                    FadeCoroutine = StartCoroutine(CanvasFadeCoroutine(activeTime, false, () =>
                    {
                        if (CanvasGroup != null)
                        {
                            CanvasGroup.alpha = 1.0f;
                        }
                        FadeCoroutine = null;
                    }));
                }
                else
                {
                    if (CanvasGroup != null)
                    {
                        CanvasGroup.alpha = 1.0f;
                    }
                    FadeCoroutine = null;
                }
            }
            else
            {
                if (CanvasGroup != null)
                {
                    CanvasGroup.alpha = 1.0f;
                }
            }
        }

        public void Hide(float deactiveTime = 0.0f)
        {
            if (FadeCoroutine != null)
            {
                StopCoroutine(FadeCoroutine);
                FadeCoroutine = null;
            }

            m_isActive = false;

            if (CanvasGroup != null)
            {
                CanvasGroup.blocksRaycasts = IsActive;
                CanvasGroup.interactable = IsActive;
            }

            if (deactiveTime != 0.0f)
            {
                if (CanvasGroup != null)
                {
                    CanvasGroup.alpha = 1.0f;
                }

                if (IsGameOjectActive == true)
                {
                    FadeCoroutine = StartCoroutine(CanvasFadeCoroutine(deactiveTime, true, () =>
                    {
                        gameObject.SetActive(IsActive);
                        FadeCoroutine = null;
                    }));
                }
                else
                {
                    if (CanvasGroup != null)
                    {
                        CanvasGroup.alpha = 0.0f;
                    }

                    FadeCoroutine = null;
                }
            }
            else
            {
                if (CanvasGroup != null)
                {
                    CanvasGroup.alpha = 0.0f;
                }

                gameObject.SetActive(IsActive);
            }
            HideWidget();
        }

        private IEnumerator CanvasFadeCoroutine(float duration, bool inverse, System.Action completed)
        {
            float t = 0.0f;
            while (t < 1.0f)
            {
                yield return new WaitForEndOfFrame();
                t = Mathf.Clamp01(t + Time.deltaTime / duration);

                if (CanvasGroup != null)
                {
                    CanvasGroup.alpha = (inverse == true) ? 1.0f - t : t;
                }
            }

            if (completed != null)
            {
                completed();
            }
        }

        public void SetCamera(Camera camera, int layer)
        {
            if (IsSetCanvasLayer == false)
            {
                return;
            }

            if (Canvas == null)
            {
                Canvas = ComponentFactory.GetChildComponent<UnityEngine.Canvas>(gameObject, IfNotExist.ReturnNull);
            }

            if (Canvas != null)
            {
                if (camera != null)
                {
                    Canvas.worldCamera = camera;
                    Canvas.renderMode = RenderMode.ScreenSpaceCamera;
                }
                else
                {
                    if (Canvas.worldCamera == null)
                    {
                        IsOverlayCanvas = true;
                        Canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    }
                }
                Canvas.sortingOrder = layer;
            }
        }

        public void BackFlow()
        {
            Global.WidgetMgr.BackFlow();
        }

        public void Close()
        {
            Hide();
        }

        #endregion Methods
    }

}