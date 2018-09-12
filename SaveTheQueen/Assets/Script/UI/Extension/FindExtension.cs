using UnityEngine;
using UnityEngine.UI;
using Lib.Pattern;

namespace UIExtension
{
    public static class FindExtension
    {
        public static Text FindText(this Transform root, string path)
        {
            if (root.IsNull() == true)
                return null;

            Transform target = FindTransform(root, path);

            if (target.IsNull() == true)
                return null;

            return ComponentFactory.GetComponent<Text>(target.gameObject, IfNotExist.ReturnNull);
        }

        public static Text FindTextChild(this Transform target)
        {
            if (target.IsNull() == true)
                return null;

            return ComponentFactory.GetChildComponent<Text>(target.gameObject, IfNotExist.ReturnNull);
        }

        public static Image FindImage(this Transform target)
        {
            if (target.IsNull() == true)
                return null;

            return ComponentFactory.GetComponent<Image>(target.gameObject, IfNotExist.ReturnNull);
        }

        public static Image FindImage(this Transform root, string path)
        {
            if (root.IsNull() == true)
                return null;

            Transform target = FindTransform(root, path);

            if (target.IsNull() == true)
                return null;

            return ComponentFactory.GetComponent<Image>(target.gameObject, IfNotExist.ReturnNull);
        }

        public static Toggle FindToggle(this Transform target)
        {
            if (target.IsNull() == true)
                return null;

            return ComponentFactory.GetComponent<Toggle>(target.gameObject, IfNotExist.ReturnNull);
        }

        public static Transform FindTransform(this Transform target, string path)
        {
            if (target.IsNull() == true)
                return null;

            return target.Find(path);
        }

        public static bool IsNull(this Transform target, bool log = false)
        {
            bool isNull = (target == null);

            if (isNull & log == true)
                Debug.LogError("Transform not found.");

            return isNull;
        }

        public static bool IsNull(this GameObject target, bool log = false)
        {
            bool isNull = (target == null);

            if (isNull & log == true)
                Debug.LogError("Transform not found.");

            return isNull;
        }
    }
}


