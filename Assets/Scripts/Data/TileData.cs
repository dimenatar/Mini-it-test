using Fruits;

public class TileData
{
	public TileData(int iD, FruitName fruitName)
	{
		ID = iD;
		FruitName = fruitName;
	}

	public int ID { get; private set; }
	public FruitName FruitName { get; private set; }
}
