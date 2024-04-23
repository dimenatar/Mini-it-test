using Fruits;
using System.Collections.Generic;
using UnityEngine;

namespace Scriptables
{
	[CreateAssetMenu(order = 46)]
	public class FruitDataBundle : ScriptableObject
	{
		[SerializeField] private List<FruitData> _datas;

		public List<FruitData> Datas => new List<FruitData>(_datas);
	}
}