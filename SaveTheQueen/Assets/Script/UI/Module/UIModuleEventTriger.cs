using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Lib.uGui
{
    public class UIModuleEventTriger : MonoBehaviour, IUIModule, IPointerClickHandler, IPointerDownHandler,
        IPointerEnterHandler, IPointerUpHandler, IPointerExitHandler
    {
        [SerializeField]
        UnityEngine.Events.UnityEvent onClick = new UnityEngine.Events.UnityEvent();
        [SerializeField]
        UnityEngine.Events.UnityEvent onDown = new UnityEngine.Events.UnityEvent();
        [SerializeField]
        UnityEngine.Events.UnityEvent onEnter = new UnityEngine.Events.UnityEvent();
        [SerializeField]
        UnityEngine.Events.UnityEvent onUp = new UnityEngine.Events.UnityEvent();
        [SerializeField]
        UnityEngine.Events.UnityEvent onExit = new UnityEngine.Events.UnityEvent();

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

        public void OnPointerClick(PointerEventData eventData)
        {
            onClick.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            onDown.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            onEnter.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            onUp.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            onExit.Invoke();
        }
    }
}