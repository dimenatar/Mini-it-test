using Fruits;
using System;

[Serializable]
public class LevelData
{
    public FruitName initialFruit;
    public IslandData islandData;

    public LevelData() 
    {

    }

    public LevelData(IslandData islandData) : this()
    {
        this.islandData = islandData;
    }
}
