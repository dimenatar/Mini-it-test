using System;
using UnityEngine;

public class LevelEvents : MonoBehaviour
{
    public event Action Awaken;
    public event Action Started;
    public event Action UnloadingScene;
    public event Action Disabled;
    public event Action Destroyed;

    public event Action<bool> PauseStateChanged;

    private void Awake()
    {
        Awaken?.Invoke();
    }

    private void Start()
    {
        Started?.Invoke();
    }

    private void OnDisable()
    {
        Disabled?.Invoke();
    }

    private void OnDestroy()
    {
        Destroyed?.Invoke();
    }

    private void OnApplicationPause(bool pause)
    {
        PauseStateChanged?.Invoke(pause);
    }

    public void InvokeUnloadingScene()
    {
        UnloadingScene?.Invoke();
    }
}
