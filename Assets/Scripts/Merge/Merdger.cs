using Environment;
using Extensions;
using Fruits;
using Scriptables;
using System;
using System.Collections.Generic;
using Tiles;
using Zenject;

namespace Merdge
{
	public class Merdger
    {
        private FruitEvolutionConfig _buildingEvolutionConfig;
        private float _delayToMergde;

        private FruitSpawner _buildingCreator;
        private InputActivator _inputActivator;
        private LevelProvider _levelProvider;

        private List<FruitsEvolutionFormula> _formulas;

        [Inject]
        public Merdger(FruitEvolutionConfig buildingEvolutionConfig, float delayToMergde, FruitSpawner buildingCreator, InputActivator inputActivator, LevelProvider levelProvider)
        {
            _buildingEvolutionConfig = buildingEvolutionConfig;
            _delayToMergde = delayToMergde;
            _buildingCreator = buildingCreator;
            _inputActivator = inputActivator;
            _levelProvider = levelProvider;

            _formulas = _buildingEvolutionConfig.BuildingEvolutionFormulas;
        }

        public event Action<Fruit> Merged;

        public bool IsReadyToMerdge(FruitName fruitName)
        {
            FruitsEvolutionFormula? formula = _formulas.Find(formula => formula.Component == fruitName);
            return !Extension.IsDefault(formula.Value);
        }

        public bool IsReadyToMerdge(FruitData first, FruitData second)
        {
            if (first.FruitName == second.FruitName)
            {
                return IsReadyToMerdge(first.FruitName);
            }
            return false;
        }

        public FruitName GetResult(params Fruit[] fruits)
        {
            return _formulas.Find(formula => formula.Component == fruits[0].FruitData.FruitName).Result;
        }

        public void Merdge(Tile tile, List<Fruit> buildings)
        {
            Merdge(tile, buildings.ToArray());
        }

        public void Merdge(Tile tile, params Fruit[] fruits)
        {
            _inputActivator.DisableInput();
            MonobehaviourExtensions.DODelayed(() => MerdgeDelayed(tile, fruits), _delayToMergde);
        }

        private void MerdgeDelayed(Tile tile, params Fruit[] fruits)
        {
            //var result = GetNextBuilding(buildings[0].BuildingData);
            var result = _formulas.Find(formula => formula.Component == fruits[0].FruitData.FruitName).Result;

            _inputActivator.EnableInput();
            var resultFruit = _buildingCreator.SpawnFruit(tile, result);
            //var resultBuilding = _factory.CreateBuilding(result, tile.transform.position, tile.transform);
            for (int i = 0; i < fruits.Length; i++)
            {
                UnityEngine.Object.Destroy(fruits[i].gameObject);
            }

            Merged?.Invoke(resultFruit);
        }
    }
}