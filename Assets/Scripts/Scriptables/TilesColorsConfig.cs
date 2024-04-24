using UnityEngine;

namespace Scriptables
{
	[CreateAssetMenu(order = 40)]
	public class TilesColorsConfig : ScriptableObject
	{
		[SerializeField] private Color _startDragColor;
		[SerializeField] private Color _readyToMergeColor;
		[SerializeField] private Color _notReadyToMergeColor;

		public Color StartDragColor => _startDragColor;
		public Color ReadyToMergeColor => _readyToMergeColor;
		public Color NotReadyToMergeColor => _notReadyToMergeColor;
	}
}