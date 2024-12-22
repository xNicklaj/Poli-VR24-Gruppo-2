using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour
    where T : Singleton<T>
{
    public static T Instance { get; private set; }

    void Awake()
    {
        // Destroy this object if we already have a Singleton defined
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = (T)this;
        DoAwake();
    }

    // Virtual method to allow implementations to use Awake
    protected virtual void DoAwake() { }
}
