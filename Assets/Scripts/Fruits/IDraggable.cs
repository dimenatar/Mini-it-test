using UnityEngine;

namespace Fruits
{
	public interface IDraggable
	{
		public Vector3 Offset { get; }

		public void ReceivePosition(Vector3 position);
	}
}