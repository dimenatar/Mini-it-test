using Data;
using Environment;
using Fruits;
using Progress;
using System;
using System.Collections.Generic;
using Tiles;
using Zenject;

namespace States
{
	public class InitialState : IState
	{
		private Level _level;
		private FruitSpawner _fruitSpawner;
		private LevelsDatasProvider _levelsDatasProvider;
		private DataController _dataController;
		private LevelDataController _levelDataController;

		public event Action Spawned;

		[Inject]
		public InitialState(
			LevelsDatasProvider levelsDatasProvider,
			DataController dataController,
			LevelDataController levelDataController,
			FruitSpawner fruitSpawner)
		{
			_fruitSpawner = fruitSpawner;
			_levelsDatasProvider = levelsDatasProvider;
			_dataController = dataController;
			_levelDataController = levelDataController;
		}

		public void SetSceneData(Level level)
		{
			_level = level;
		}

		public void Enter(object data)
		{
			_dataController.SetSavingState(true);
			_dataController.LoadProgresses();

			LevelData levelData = _levelsDatasProvider.LevelData;
			_levelDataController.SetLevelData(levelData);
			_levelDataController.LoadDataToProgresses();

			TileDatasHolder tileDatasHolder = levelData.tileDatasHolder;
			SetupGrid(levelData, _level);

			Spawned?.Invoke();
		}

		private void SetupGrid(LevelData levelData, Level level)
		{
			List<TileData> tileDatas = levelData.tileDatasHolder.TileDatas;

			if (tileDatas.Count == 0) return;

			List<Tile> tileBundle = level.TileHolder.TilesBundle;

			for (int i = 0; i < tileBundle.Count; i++)
			{
				if (tileDatas[i].FruitName != FruitName.None)
				{
					_fruitSpawner.SpawnFruit(tileBundle[i], tileDatas[i].FruitName);
				}
			}
		}

		public void Exit() { }
	}
}