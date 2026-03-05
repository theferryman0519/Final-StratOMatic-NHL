using UnityEngine;

namespace SoM.Core
{
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(-10000)]
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static bool _shuttingDown;
        private static readonly object _lock = new();

        public static T Inst
        {
            get
            {
                if (_shuttingDown) return null;
                if (_instance != null) return _instance;

                lock (_lock)
                {
                    if (_instance != null) return _instance;

                    _instance = FindFirstObjectByType<T>(FindObjectsInactive.Include);
                    if (_instance == null)
                    {
                        Debug.LogError($"[Singleton<{typeof(T).Name}>] Instance not found. Add it to the Bootstrap scene.");
                    }
                    return _instance;
                }
            }
        }

        protected virtual void Awake()
        {
            if (_instance != null && _instance != (T)(object)this)
            {
                Debug.LogWarning($"[Singleton<{typeof(T).Name}>] Duplicate found on {gameObject.scene.name}. Destroying duplicate.");
                Destroy(gameObject);
                return;
            }

            _instance = (T)(object)this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnApplicationQuit() => _shuttingDown = true;
    }
}
