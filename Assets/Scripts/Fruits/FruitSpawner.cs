using Scriptables;
using System.Collections.Generic;
using Tiles;
using UnityEngine;

namespace Fruits
{
	public class FruitSpawner
    {
        private Dictionary<FruitName, Fruit> _models;
        private List<FruitData> _fruitDatas;

        public FruitSpawner(FruitModels fruitModels, FruitDataBundle fruitDataBundle)
        {
            _models = fruitModels.Models;
            _fruitDatas = fruitDataBundle.Datas;
        }

        public Fruit SpawnFruit(Tile tile, FruitName fruitName)
        {
            var original = _models[fruitName];
            var copy = Object.Instantiate(original);

            tile.SetTileContent(copy);
            copy.SetTile(tile);
            copy.ReceivePosition(tile.GetBildingPoint());

            var fruitData = _fruitDatas.Find(data => data.FruitName == fruitName);
            copy.SetFruitData(fruitData);

            return copy;
        }
    }
}