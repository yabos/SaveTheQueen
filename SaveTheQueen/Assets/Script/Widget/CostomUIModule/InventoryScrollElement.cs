using UnityEngine;
using System.Collections;
using Lib.uGui;

namespace Aniz.Widget.Module
{
    public class InventoryScrollElement : ScrollItem
    {
        public UnityEngine.UI.Text IndexText;
        public UnityEngine.UI.Text NameText;

        public override void SetInfo<T>(T data, IScrollReceiver receiver)
        {
            base.SetInfo<T>(data, receiver);

            InventoryData inventoryData = data as InventoryData;

            if (IndexText != null)
            {
                IndexText.text = inventoryData.IDX.ToString();
            }

            if (NameText != null)
            {
                NameText.text = inventoryData.Name;
            }
        }
    }

}