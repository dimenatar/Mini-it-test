using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UserInput
{
	public class MobileInput : TickInput
	{
		private float _prevDelta = float.MaxValue;
		private bool _isEnabled = true;

		private HashSet<int> _nonUITaps;
		private List<int> _zoomFingers;

		public override bool IsEnabled => _isEnabled;
		public MobileInput() : base()
		{
			_nonUITaps = new HashSet<int>();
			_zoomFingers = new List<int>();
		}

		public override event Action<float> Zoomed;
		public override event Action<Vector3> MouseDown;
		public override event Action<Vector3> MouseDownOnUI;
		public override event Action<Vector3> MouseDownNonUI;

		public override event Action<Vector3> MouseUpOnUI;
		public override event Action<Vector3> MouseUpNonUI;
		public override event Action<Vector3> MouseUp;

		public override void Tick()
		{
			if (!_isEnabled) return;

			IsHolding = Input.touchCount > 0;

			if (Input.touchCount > 0)
			{
				Touch firstTouch = Input.GetTouch(0);

				if (firstTouch.phase == TouchPhase.Began)
				{
					IsHolding = true;
					MouseDown?.Invoke(firstTouch.position);
				}
				else if (firstTouch.phase == TouchPhase.Ended || firstTouch.phase == TouchPhase.Canceled)
				{
					IsHolding = false;
					MouseUp?.Invoke(firstTouch.position);
				}

				ManageZoom();

				foreach (var touch in Input.touches)
				{

					if (touch.phase == TouchPhase.Began)
					{
						if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
						{
							MouseDownOnUI?.Invoke(touch.position);
						}
						else
						{
							_nonUITaps.Add(touch.fingerId);
							MouseDownNonUI?.Invoke(touch.position);
						}
					}
					else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
					{
						if (_nonUITaps.Contains(touch.fingerId))
						{
							MouseUpNonUI?.Invoke(touch.position);
						}
						else
						{
							MouseUpOnUI?.Invoke(touch.position);
						}
						_nonUITaps.Remove(touch.fingerId);
					}
					else if (touch.phase == TouchPhase.Moved)
					{
						_nonUITaps.Remove(touch.fingerId);
					}
				}
			}
		}

		private void ManageZoom()
		{
			if (Input.touchCount >= 2)
			{
				var touch1 = Input.GetTouch(0);
				var touch2 = Input.GetTouch(1);

				List<int> currentTouches = new List<int>() { touch1.fingerId, touch2.fingerId };

				var touch1Position = touch1.position;
				var touch2Position = touch2.position;

				if (touch1.phase != TouchPhase.Moved && touch2.phase != TouchPhase.Moved && touch1.phase != TouchPhase.Began
					&& touch2.phase != TouchPhase.Began || _zoomFingers.Count > 0 && !_zoomFingers.SequenceEqual(currentTouches))
				{
					ClearZoomData();
					return;
				}

				Vector2 touch1Normalized = new Vector2(touch1Position.x / Screen.width * BASE_RESOLUTION.x, touch1Position.y / Screen.height * BASE_RESOLUTION.y);
				Vector2 touch2Normalized = new Vector2(touch2Position.x / Screen.width * BASE_RESOLUTION.x, touch2Position.y / Screen.height * BASE_RESOLUTION.y);
				var delta = Mathf.Abs((touch1Normalized - touch2Normalized).magnitude);

				if (_prevDelta != float.MaxValue && (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved))
				{
					Zoomed?.Invoke(-(delta - _prevDelta));
				}
				_prevDelta = delta;
			}
			else
			{
				ClearZoomData();
			}
		}

		private void ClearZoomData()
		{
			_prevDelta = float.MaxValue;
			_zoomFingers.Clear();
		}

		public override Vector3 GetTouchPosition()
		{
			if (Input.touchCount == 0) return Vector3.zero;
			return Input.GetTouch(0).position;
		}

		public override void Enable()
		{
			_isEnabled = true;
		}

		public override void Disable()
		{
			IsHolding = false;
			_isEnabled = false;
		}

		public override bool IsTouchPresent()
		{
			return Input.touchCount > 0;
		}
	}
}