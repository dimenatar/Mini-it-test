using Fruits;
using System;

[Serializable]
public class LevelData
{
    public FruitName initialFruit;
    public TileDatasHolder islandData;

    public LevelData() 
    {

    }

    public LevelData(TileDatasHolder islandData) : this()
    {
        this.islandData = islandData;
    }
}
