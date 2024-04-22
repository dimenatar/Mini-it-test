using UnityEngine;

namespace Fruits
{
	[System.Serializable]
	public struct FruitData
	{
		[SerializeField] private FruitName _fruitName;
		[SerializeField] private int _globalLevel;

		public FruitName FruitName => _fruitName;
		public int GlobalLevel => _globalLevel;
	}
}