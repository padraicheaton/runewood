using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    public static T Instance;

    private void OnEnable()
    {
        if (Instance == null)
            Instance = this as T;
        else
        {
            string typename = typeof(T).Name;
            Debug.LogWarning($"More that one instance of {typename} found.");
        }
    }
}
