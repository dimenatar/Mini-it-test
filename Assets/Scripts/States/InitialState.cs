using Fruits;
using System;
using System.Collections.Generic;
using Tiles;
using Zenject;

public class InitialState : IState
{
    private Level _level;
    private FruitSpawner _fruitSpawner;
    private LevelsDatasProvider _levelsDatasProvider;
    private DataController _dataController;
    private LevelDataController _levelDataController;

    public event Action<IState> StateFinished;
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

    public void SetSceneData(
        Level level)
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

        TileDatasHolder tileDatasHolder = levelData.islandData;
		SetupGrid(levelData, _level);

        Spawned?.Invoke();
    }

    private void SetupGrid(LevelData levelData, Level level)
    {
        List<Tile> tileBundle = level.TileHolder.TilesBundle;
        List<TileData> tileDatas = levelData.islandData.TileDatas;

        for (int i = 0; i < tileBundle.Count; i++)
        {
            if (tileDatas[i].FruitName != FruitName.None)
            {
                _fruitSpawner.CreateBuilding(tileBundle[i], tileDatas[i].FruitName);
            }
        }
    }

    public void Exit() { }
}
