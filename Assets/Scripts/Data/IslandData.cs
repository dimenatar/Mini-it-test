using System;
using System.Collections.Generic;

[System.Serializable]
public class IslandData
{
	[NonSerialized]
	private List<TileData> _tileDatas;

	public List<TileData> TileDatas => new List<TileData>(_tileDatas);

	public IslandData(List<TileData> tileDatas)
	{
		_tileDatas = tileDatas;
	}
}