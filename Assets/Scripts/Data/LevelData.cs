using Fruits;
using System;

[Serializable]
public class LevelData
{
    public FruitName initialFruit;
    public TileDatasHolder tileDatasHolder;

    public LevelData() 
    {
        tileDatasHolder = new TileDatasHolder();
    }

    public LevelData(TileDatasHolder tileDatasHolder) : this()
    {
        this.tileDatasHolder = tileDatasHolder;
    }
}
