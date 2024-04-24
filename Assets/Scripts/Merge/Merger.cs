using Cysharp.Threading.Tasks;
using Environment;
using Extensions;
using Fruits;
using Scriptables;
using System;
using System.Collections.Generic;
using Tiles;
using Zenject;

namespace Merge
{
	public class Merger
    {
        private FruitEvolutionConfig _buildingEvolutionConfig;
        private float _delayToMergde;

        private FruitSpawner _buildingCreator;
        private InputActivator _inputActivator;
        private LevelProvider _levelProvider;

        private List<FruitsEvolutionFormula> _formulas;

        [Inject]
        public Merger(FruitEvolutionConfig buildingEvolutionConfig, float delayToMergde, FruitSpawner buildingCreator, InputActivator inputActivator, LevelProvider levelProvider)
        {
            _buildingEvolutionConfig = buildingEvolutionConfig;
            _delayToMergde = delayToMergde;
            _buildingCreator = buildingCreator;
            _inputActivator = inputActivator;
            _levelProvider = levelProvider;

            _formulas = _buildingEvolutionConfig.BuildingEvolutionFormulas;
        }

        public event Action<Fruit> Merged;

        public bool IsReadyToMerge(FruitName fruitName)
        {
            FruitsEvolutionFormula? formula = _formulas.Find(formula => formula.Component == fruitName);
            return !Extension.IsDefault(formula.Value);
        }

        public bool IsReadyToMerge(FruitData first, FruitData second)
        {
            if (first.FruitName == second.FruitName)
            {
                return IsReadyToMerge(first.FruitName);
            }
            return false;
        }

        public FruitName GetResult(params Fruit[] fruits)
        {
            return _formulas.Find(formula => formula.Component == fruits[0].FruitData.FruitName).Result;
        }

        public void Merge(Tile tile, List<Fruit> buildings)
        {
            Merge(tile, buildings.ToArray());
        }

        public async void Merge(Tile tile, params Fruit[] fruits)
        {
            _inputActivator.DisableInput();

			for (int i = 0; i < fruits.Length; i++)
			{
                fruits[i].ScaleOut();
			}

			await UniTask.WaitForSeconds(_delayToMergde);

			var result = _formulas.Find(formula => formula.Component == fruits[0].FruitData.FruitName).Result;

			var resultFruit = _buildingCreator.SpawnFruit(tile, result);
            resultFruit.ScaleIn();

			for (int i = 0; i < fruits.Length; i++)
			{
				UnityEngine.Object.Destroy(fruits[i].gameObject);
			}

			_inputActivator.EnableInput();
			Merged?.Invoke(resultFruit);

		}
    }
}