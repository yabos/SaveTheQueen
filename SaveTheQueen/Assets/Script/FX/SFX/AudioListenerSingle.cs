using UnityEngine;

namespace Aniz.SFX
{
    /// <summary>
    /// Scene에 단일 audio listener를 유지
    /// Root에 위치하다가 hero 생성시 하위로 이동. Lobby로 돌아올때 root로 이동
    /// </summary>
    [RequireComponent(typeof(AudioListener))]
    public class AudioListenerSingle : UnitySingleton<AudioListenerSingle>
    {
        public AudioListener listener { get; private set; }


        public override void Init() { }

        void Awake()
        {
            DontDestroyOnLoad(gameObject);

            listener = GetComponent<AudioListener>();

            if (listener == null)
            {
                Debug.LogError("Falied to find audio listener : AudioListenerSingle");
                return;
            }

            AudioListener[] listeners = GameObject.FindObjectsOfType(typeof(AudioListener)) as AudioListener[];
            for (int i = 0; i < listeners.Length; ++i)
            {
                if (listeners[i] == listener || !UnityAssist.GetActive(listeners[i]))
                    continue;

                Debug.LogWarning("Detect another activated audio listener : AudioListenerSingle");
                break;
            }
        }

        public void Reset()
        {
            transform.SetParent(null, false);
            DontDestroyOnLoad(gameObject);

            var behaviours = GetComponents<Behaviour>();
            foreach (var behaviour in behaviours)
                behaviour.enabled = true;
        }
    }
}


