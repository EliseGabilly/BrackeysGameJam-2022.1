using UnityEngine;

/// <summary>
/// Static instance is similar to a singleton that overrides the current instance
/// </summary>
public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour {
    public static T Instance { get; private set; }
    protected virtual void Awake() => Instance = this as T;

    protected virtual void OnApplicationQuit() {
        Instance = null;
        Destroy(gameObject);
    }
}

/// <summary>
/// Singleton that destroy any new versions and keep the original instance 
/// </summary>
public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour {
    protected override void Awake() {
        if (Instance != null) Destroy(gameObject);
        base.Awake();
    }
}

public abstract class PersistentSingleton<T> : MonoBehaviour where T : MonoBehaviour {
    public static T Instance { get; private set; }

    protected virtual void Awake() {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else {
            Instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
    }
}

