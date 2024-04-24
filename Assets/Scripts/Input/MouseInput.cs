using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UserInput
{
	public class MouseInput : TickInput
	{
		private bool _isEnabled = true;

		public override bool IsEnabled => _isEnabled;

		public override event Action<float> Zoomed;
		public override event Action<Vector3> MouseDown;
		public override event Action<Vector3> MouseDownOnUI;
		public override event Action<Vector3> MouseDownNonUI;

		public override event Action<Vector3> MouseUp;
		public override event Action<Vector3> MouseUpOnUI;
		public override event Action<Vector3> MouseUpNonUI;

		public override void Disable()
		{
			IsHolding = false;
			_isEnabled = false;
		}

		public override void Enable()
		{
			_isEnabled = true;
		}

		public override Vector3 GetTouchPosition()
		{
			if (!IsHolding) return Vector3.zero;
			return Input.mousePosition;
		}

		public override bool IsTouchPresent()
		{
			return Input.GetMouseButton(0);
		}

		public override void Tick()
		{
			if (Input.GetMouseButtonDown(0))
			{
				IsHolding = true;

				if (EventSystem.current.IsPointerOverGameObject())
				{
					MouseDownOnUI?.Invoke(Input.mousePosition);
				}
				else
				{
					MouseDownNonUI?.Invoke(Input.mousePosition);
				}
				MouseDown?.Invoke(Input.mousePosition);
			}
			else if (Input.GetMouseButtonUp(0) && IsHolding)
			{
				IsHolding = false;
				MouseUp?.Invoke(Input.mousePosition);
				if (EventSystem.current.IsPointerOverGameObject())
				{
					MouseUpOnUI?.Invoke(Input.mousePosition);
				}
				else
				{
					MouseUpNonUI?.Invoke(Input.mousePosition);
				}
			}
		}
	}
}