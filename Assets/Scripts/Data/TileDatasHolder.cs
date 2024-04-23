using System;
using System.Collections.Generic;

[System.Serializable]
public class TileDatasHolder
{
	[NonSerialized]
	private List<TileData> _tileDatas;

	public List<TileData> TileDatas => new List<TileData>(_tileDatas);

	public TileDatasHolder(List<TileData> tileDatas)
	{
		_tileDatas = tileDatas;
	}
}