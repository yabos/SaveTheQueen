using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using Lib.uGui;

namespace Aniz.Widget.Module
{
    public class CommandScrollElement : ScrollItem, IPointerDownHandler
    {
        protected CommandData m_commandData = null;

        public CommandData Data
        {
            get { return m_commandData; }
        }

        public GameObject SelectedEffectObject;

        public UnityEngine.UI.Text CommandMessageText;

        public override void SetInfo<T>(T data, IScrollReceiver receiver)
        {
            base.SetInfo<T>(data, receiver);

            m_commandData = data as CommandData;

            if (CommandMessageText != null)
            {
                CommandMessageText.text = m_commandData.Message;
            }

            if (m_receiver != null)
            {
                m_receiver.OnSetInfoEvent(this);
            }
        }

        public void SetSelectedEffectObject(bool value)
        {
            if (SelectedEffectObject != null)
            {
                SelectedEffectObject.gameObject.SetActive(value);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.clickCount == 2)
            {
                Debug.Log("Double Click");
                eventData.clickCount = 0;

                if (m_receiver != null)
                {
                    m_receiver.OnDoubleClickEvent(this);
                }
            }
        }

        public void OnSelectButtonClk()
        {
            if (m_receiver != null)
            {
                m_receiver.OnSelectEvent(this);
            }
        }
    }
}