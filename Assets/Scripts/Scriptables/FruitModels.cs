using AYellowpaper.SerializedCollections;
using Fruits;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(order = 44)]
public class FruitModels : ScriptableObject
{
	[SerializeField] private SerializedDictionary<FruitName, Fruit> _modelByName;

	public Dictionary<FruitName, Fruit> Models => new Dictionary<FruitName, Fruit>(_modelByName);
}
