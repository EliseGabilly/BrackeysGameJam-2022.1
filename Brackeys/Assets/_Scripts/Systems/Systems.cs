using UnityEngine;

/// <summary>
/// Persistent class to keep one main object as persitent (with sub-systems as children)
/// </summary>
public class Systems : MonoBehaviour {

    public static Systems Instance;

    void Awake() {
        if (!Instance) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
}
