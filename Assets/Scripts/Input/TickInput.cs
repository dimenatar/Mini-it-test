using System;
using UnityEngine;
using Zenject;

public abstract class TickInput : IInput, ITickable
{
    public readonly Vector2 BASE_RESOLUTION = new Vector2(1080, 1920);

    private IUpdater _updater;

    public bool IsHolding { get; protected set; }
    public abstract bool IsEnabled { get; }

    public abstract event Action<float> Zoomed;
    public abstract event Action<Vector3> MouseDownOnUI;
    public abstract event Action<Vector3> MouseDownNonUI;
    public abstract event Action<Vector3> MouseUpOnUI;
    public abstract event Action<Vector3> MouseUpNonUI;
    public abstract event Action<Vector3> MouseDown;
    public abstract event Action<Vector3> MouseUp;

    public TickInput() { }

    [Inject]
    public void SetUpdater(IUpdater updater)
    {
        _updater = updater;
        _updater.AddTickable(this);
    }

    public abstract Vector3 GetTouchPosition();
    public abstract void Tick();
    public abstract void Enable();
    public abstract void Disable();
    public abstract bool IsTouchPresent();
}
