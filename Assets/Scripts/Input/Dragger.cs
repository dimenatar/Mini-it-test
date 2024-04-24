using Fruits;
using System;
using System.Collections;
using Tiles;
using UnityEngine;
using Updaters;
using Zenject;

namespace UserInput
{
	public class Dragger : MonoBehaviour, IActivatable, Updaters.ITickable
	{
		[SerializeField] private Camera _camera;
		[SerializeField] private IDraggable _currentDraggable = null;
		[SerializeField] private float _delayToDrag = 0.3f;
		[SerializeField] private LayerMask _groundLayer;
		[SerializeField] private LayerMask _tileLayer;

		private bool _isEnabled = true;
		private IInput _input;
		private Tile _pointingTile;

		public bool IsEnabled => _isEnabled;
		public bool IsDragging { get; private set; }

		public event Action<Tile> PointingTileChanged;
		public event Action<IDraggable> StartedDragging;
		public event Action<IDraggable> StoppedDragging;

		[Inject]
		private void Construct(IInput input)
		{
			_input = input;
			_input.MouseDownNonUI += MouseDown;
			_input.MouseUp += MouseUp;
			print(input);
		}

		private void OnDestroy()
		{
			_input.MouseDownNonUI -= MouseDown;
			_input.MouseUp -= MouseUp;
		}

		private void MouseUp(Vector3 obj)
		{
			if (!IsEnabled) return;

			if (_currentDraggable != null && IsDragging)
			{
				IsDragging = false;
				StoppedDragging?.Invoke(_currentDraggable);
			}
			else if (_currentDraggable != null)
			{
				StopAllCoroutines();
			}

			_currentDraggable = null;
			_pointingTile = null;
		}

		private void MouseDown(Vector3 obj)
		{
			if (!IsEnabled || IsDragging) return;
			StopAllCoroutines();
			var ray = GetRay(_camera, _input);
			if (IsCastDraggable(ray, out IDraggable castedDraggable))
			{
				_currentDraggable = castedDraggable;
				StartCoroutine(WaitForDrug(_currentDraggable, () => StartDrag(_currentDraggable), _camera, _input, _delayToDrag));
			}
		}

		private IEnumerator WaitForDrug(IDraggable draggable, Action callback, Camera camera, IInput input, float delay)
		{
			float timer = 0;
			while (timer < delay)
			{
				var result = IsCastDraggable(GetRay(camera, input), out IDraggable foundDraggable);
				if (!result || draggable != foundDraggable)
				{
					yield break;
				}
				else yield return null;
				timer += Time.deltaTime;
			}
			callback?.Invoke();
		}

		private bool IsCastDraggable(Ray ray, out IDraggable draggable)
		{
			draggable = null;

			if (Physics.Raycast(ray, out RaycastHit raycastHit))
			{
				if (raycastHit.collider.TryGetComponent(out IDraggable foundDraggable))
				{
					draggable = foundDraggable;
					return true;
				}
			}
			return false;
		}

		private Ray GetRay(Camera camera, IInput input)
		{
			return camera.ScreenPointToRay(input.GetTouchPosition());
		}

		private void StartDrag(IDraggable draggable)
		{
			IsDragging = true;
			StartedDragging?.Invoke(draggable);
		}

		public void Enable()
		{
			_isEnabled = true;
		}

		public void Disable()
		{
			if (IsDragging)
			{
				StopAllCoroutines();
				IsDragging = false;
				if (_currentDraggable != null)
				{
					StoppedDragging?.Invoke(_currentDraggable);
				}
				_pointingTile = null;
				_currentDraggable = null;
			}
			_isEnabled = false;


		}

		public void Tick()
		{
			if (IsDragging && IsEnabled)
			{
				var ray = GetRay(_camera, _input);
				if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, _tileLayer))
				{
					if (raycastHit.collider.TryGetComponent(out Tile tile))
					{
						if (_pointingTile != tile)
						{
							_pointingTile = tile;
							PointingTileChanged?.Invoke(tile);
						}
					}
				}
				if (Physics.Raycast(ray, out RaycastHit raycastHit1, float.MaxValue, _groundLayer))
				{
					_currentDraggable.ReceivePosition(raycastHit1.point);
				}
			}
		}
	}
}