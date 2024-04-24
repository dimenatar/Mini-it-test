using System;
using System.Collections.Generic;

namespace Data
{
	[Serializable]
	public class TileDatasHolder
	{
		[NonSerialized]
		private List<TileData> _tileDatas;

		public List<TileData> TileDatas => new List<TileData>(_tileDatas);

		public TileDatasHolder()
		{
			_tileDatas = new List<TileData>();
		}

		public TileDatasHolder(List<TileData> tileDatas)
		{
			_tileDatas = tileDatas;
		}
	}
}