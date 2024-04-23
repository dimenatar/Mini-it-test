using Scriptables;
using System.Collections.Generic;
using Tiles;
using UnityEngine;

namespace Fruits
{
	public class FruitSpawner
    {
        private Dictionary<FruitName, Fruit> _models;

        public FruitSpawner(FruitModels fruitModels)
        {
            _models = fruitModels.Models;
        }

        public Fruit SpawnFruit(Tile tile, FruitName fruitName)
        {
            var original = _models[fruitName];
            var copy = Object.Instantiate(original);
            tile.SetTileContent(copy);
            copy.SetTile(tile);
            copy.transform.SetParent(tile.transform);
            copy.ReceivePosition(tile.GetBildingPoint());

            return copy;
        }
    }
}