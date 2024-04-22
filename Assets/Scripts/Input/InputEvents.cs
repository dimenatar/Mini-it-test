using System;
using UnityEngine;
using Zenject;

public class InputEvents : IActivatable
{
    private Camera _camera;
    private IInput _input;
    private bool _isEnabled = true;

    public bool IsEnabled => _isEnabled;

    [Inject]
    public InputEvents(IInput input)
    {
        _input = input;

        _input.MouseDownNonUI += OnMouseDown;
        _input.MouseUpNonUI += OnMouseUp;
    }

    public void SetCamera(Camera camera)
    {
        _camera = camera;
    }

    private void OnMouseUp(Vector3 position)
    {
        if (_isEnabled)
        {
            if (Physics.Raycast(_camera.ScreenPointToRay(position), out RaycastHit raycastHit))
            {
                if (raycastHit.collider.TryGetComponent(out IMouseUpListener mouseUpListener))
                {
                    mouseUpListener.MouseUp();
                }
            }
        }
    }

    public void Dispose()
    {
        
    }

    private void OnMouseDown(Vector3 position)
    {
        if (_isEnabled)
        {
            if (Physics.Raycast(_camera.ScreenPointToRay(position), out RaycastHit raycastHit))
            {
                if (raycastHit.collider.TryGetComponent(out IMouseDownListener mouseDownListener))
                {
                    mouseDownListener.MouseDown();
                }
            }
        }
    }

    public void Enable()
    {
        _isEnabled = true;
    }

    public void Disable()
    {
        _isEnabled = false;
    }
}
