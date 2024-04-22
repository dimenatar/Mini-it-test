using UnityEngine;

public interface IDraggable
{
    public Vector3 Offset { get; }

    public void ReceivePosition(Vector3 position);
    public void StartDrag();
    public void StopDrag();
}
