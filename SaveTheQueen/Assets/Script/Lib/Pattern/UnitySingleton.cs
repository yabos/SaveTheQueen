using UnityEngine;

/// <summary>
///
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class UnitySingleton<T> : MonoBehaviour where T : UnitySingleton<T>
{
    private static T m_instance = null;
    private static bool m_shutdown = false;

    public static T Instance
    {
        get
        {
            if (m_shutdown)
            {
                return null;
            }

            // Instance requiered for the first time, we look for it
            if (m_instance == null)
            {
                m_instance = GameObject.FindObjectOfType(typeof(T)) as T;

                // Object not found, we create a temporary one
                if (m_instance == null)
                {
                    m_instance = new GameObject("Temp Instance of " + typeof(T).ToString(), typeof(T)).GetComponent<T>();
                    DontDestroyOnLoad(m_instance);
                }

                // Problem during the creation, this should not happen
                if (m_instance == null)
                {
                    Debug.LogError("Problem during the creation of " + typeof(T).ToString());
                }

                //instance.Init();
            }

            return m_instance;
        }
    }

    public abstract void Init();

    private void OnApplicationQuit()
    {
        if (m_instance != null)
        {
            GameObject.Destroy(m_instance);
        }
        m_instance = null;
        m_shutdown = true;
    }
}