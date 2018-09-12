using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lib.uGui
{

    public static class UIModuleModelUtils
    {
        public static Vector3 NormalizeRotation(Vector3 rotation)
        {
            return new Vector3(NormalizeAngle(rotation.x), NormalizeAngle(rotation.y), NormalizeAngle(rotation.z));
        }

        public static float NormalizeAngle(float value)
        {
            value = value % 360;

            if (value < 0)
            {
                value += 360;
            }

            return value;
        }


        private static Dictionary<UIModuleModelObject, Vector3> g_targetContainers = new Dictionary<UIModuleModelObject, Vector3>();

        internal static void RegisterTargetContainerPosition(UIModuleModelObject modelObject, Vector3 position)
        {
            if (g_targetContainers.ContainsKey(modelObject))
            {
                return;
            }

            g_targetContainers.Add(modelObject, position);
        }

        internal static Vector3 GetTargetContainerPosition(UIModuleModelObject modelObject)
        {
            if (g_targetContainers.ContainsKey(modelObject)) return g_targetContainers[modelObject];

            return GetNextFreeTargetContainerPosition();
        }

        internal static Vector3 GetNextFreeTargetContainerPosition()
        {
            if (!g_targetContainers.Any()) return Vector3.zero;

            var lastXInUse = g_targetContainers.Max(v => v.Value.x);
            return new Vector3(lastXInUse + 250f, 0f, 0f);
        }

        internal static void UnRegisterTargetContainer(UIModuleModelObject modelObject)
        {
            g_targetContainers.Remove(modelObject);
        }

#if UNITY_EDITOR

        [UnityEditor.InitializeOnLoadMethod]
        public static void ManageLayer()
        {
            var layer = LayerMask.NameToLayer("UIModuleModelObject");
            if (layer != -1) return;

            var tagManager = new UnityEditor.SerializedObject(UnityEditor.AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset"));
            var layers = tagManager.FindProperty("layers");

            if (layers == null || !layers.isArray)
            {
                Debug.LogWarning("[Warning] Unable to set up layers.");
                return;
            }

            bool set = false;
            for (var i = 8; i < layers.arraySize; i++)
            {
                var element = layers.GetArrayElementAtIndex(i);

                if (element.stringValue == "")
                {
                    element.stringValue = "UIModuleModelObject";
                    set = true;
                    break;
                }
            }

            if (set)
            {
                Debug.Log("Layer 'UIModuleModelObject' created.");
                tagManager.ApplyModifiedProperties();
            }
            else
            {
                Debug.LogWarning("[Warning] Unable to create Layer 'UIModuleModelObject'.");
            }
        }

#endif
    }
}