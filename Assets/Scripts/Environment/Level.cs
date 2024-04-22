using Extensions;
using System;
using System.Linq;
using Tiles;
using UnityEngine;
using Zenject;

public class Level : MonoBehaviour, ILevelProgress
{
	[SerializeField] private TileHolder _tileHolder;

	private LevelsDatasProvider _levelDataController;

	public TileHolder TileHolder => _tileHolder;

	public event Action TileUnavalailable;
	public event Action TileAvailable;

	[Inject]
	private void Construct(LevelsDatasProvider levelDataController)
	{
		_levelDataController = levelDataController;
	}

	public Tile GetFreeTile()
	{
		var tile = GetFreeTile(_tileHolder);
		return tile;
	}

	public bool IsSpace()
	{
		return _tileHolder.TilesBundle.Any(tile => tile.IsAvailable());
	}

	private Tile GetFreeTile(TileHolder tileHolder)
	{
		var freeTile = tileHolder.TilesBundle.GetRandom(tile => tile.IsAvailable());
		return freeTile;
	}

	public void LoadProgress(LevelData levelData) { }

	public void SaveProgress(LevelData levelData)
	{
		_levelDataController.SaveLevelData(_tileHolder);
	}
}