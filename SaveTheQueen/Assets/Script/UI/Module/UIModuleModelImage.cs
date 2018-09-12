using UnityEngine;
using System.Collections;

namespace Lib.uGui
{

    [RequireComponent(typeof(RectTransform)), DisallowMultipleComponent, ExecuteInEditMode]
    public class UIModuleModelImage : UnityEngine.UI.Image, UnityEngine.UI.ILayoutElement
    {
        void UnityEngine.UI.ILayoutElement.CalculateLayoutInputHorizontal()
        {
        }

        void UnityEngine.UI.ILayoutElement.CalculateLayoutInputVertical()
        {
        }

        float UnityEngine.UI.ILayoutElement.flexibleHeight
        {
            get { return 1; }
        }

        float UnityEngine.UI.ILayoutElement.flexibleWidth
        {
            get { return 1; }
        }

        int UnityEngine.UI.ILayoutElement.layoutPriority
        {
            get { return -1; }
        }

        float UnityEngine.UI.ILayoutElement.minHeight
        {
            get { return 0; }
        }

        float UnityEngine.UI.ILayoutElement.minWidth
        {
            get { return 0; }
        }

        float UnityEngine.UI.ILayoutElement.preferredHeight
        {
            get { return 0; }
        }

        float UnityEngine.UI.ILayoutElement.preferredWidth
        {
            get { return 0; }
        }
    }
}