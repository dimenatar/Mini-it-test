using Fruits;
using System;
using System.Collections.Generic;
using Tiles;
using Zenject;

public class InitialState : IState
{
    private FruitName _initialFruit;

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

        //LevelData levelData = _levelsDatasProvider.LevelDatas.levelDatas.Find(data => data.stageType == _stageType);
        //_levelDataController.SetLevelData(levelData);
        //_levelDataController.LoadDataToProgresses();

        //IslandData islandData = levelData.islandData;
        //SetupIsland(levelData, _level, _initialFruit);

        Spawned?.Invoke();
    }

    private void SetupIsland(LevelData levelData, Level level, FruitName initialFruit)
    {
        //List<Tile> tileBundle = level.Island.Tiles.TilesBundle;
        //List<TileData> tileDatas = levelData.islandData.tileDatas;

        //for (int i = 0; i < tileBundle.Count; i++)
        //{
        //    tileBundle[i].Initialise(tileDatas[i].ID);
        //}
    }

    public void Exit() { }
}
