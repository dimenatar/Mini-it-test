using Fruits;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Tiles;
using UnityEditor;
using Zenject;

public class LevelsDatasProvider : IProgress
{
    private LevelData _levelData;
    private FruitName _initialFruit;

    public LevelData LevelData => _levelData;
    public bool IsLoaded { get; private set; }

	[Inject]
	public LevelsDatasProvider(DataController saveController, FruitName initialFruit)
	{
		saveController.AddProgress(this);
		_initialFruit = initialFruit;
	}

	public void LoadProgress(DictionaryProgressManager dictionaryProgressManager)
    {
        if (dictionaryProgressManager.ContainsKey(Tags.LEVEL_DATAS))
        {
			_levelData = dictionaryProgressManager.GetObject<LevelData>(Tags.LEVEL_DATAS);
        }
        else
        {
			_levelData = new LevelData();
            CreateInitialData();
        }


		_levelData.initialFruit = _initialFruit;
        

        IsLoaded = true;
    }

    public void SaveProgress(DictionaryProgressManager dictionaryProgressManager)
    {
        dictionaryProgressManager.SaveValue(Tags.LEVEL_DATAS, LevelData);
    }

    public void SaveLevelData(TileHolder tileHolder)
    {

        List<TileData> tileDatas = new List<TileData>();
        TileDatasHolder islandData;
        for (int i = 0; i < tileHolder.TilesBundle.Count; i++)
        {
            //BuildingName name = _tiles.TilesBundle[i].IsFree ? BuildingName.None : _tiles.TilesBundle[i].TileContent.BuildingName;
            Tile tile = tileHolder.TilesBundle[i];
            TileData tileData = CreateTileData(tile);
            tileDatas.Add(tileData);
        }
        islandData = new TileDatasHolder(tileDatas);

        _levelData.tileDatasHolder = islandData;
    }

	private TileData CreateTileData(Tile tile)
	{
        FruitName fruitName = FruitName.None;

        if (tile.TileContent is Fruit fruit)
        {
            fruitName = fruit.FruitData.FruitName;
        }

		return new TileData(tile.ID, fruitName);
	}

	private void CreateInitialData()
	{

	}


	public void ClearProgress()
    {
        IsLoaded = false;
        _levelData = new LevelData();
        CreateInitialData();
    }
}
