using System;
using UnityEngine;

public interface IInput : IActivatable
{
    public event Action<float> Zoomed;
    public event Action<Vector3> MouseDown;
    public event Action<Vector3> MouseDownOnUI;
    public event Action<Vector3> MouseDownNonUI;

    public event Action<Vector3> MouseUp;
    public event Action<Vector3> MouseUpOnUI;
    public event Action<Vector3> MouseUpNonUI;

    public bool IsHolding { get; }
    public Vector3 GetTouchPosition();
    public bool IsTouchPresent();
}