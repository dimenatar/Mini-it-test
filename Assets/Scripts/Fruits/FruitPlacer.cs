using Extensions;
using Merge;
using System.Collections.Generic;
using Tiles;
using UnityEngine;
using UserInput;
using Zenject;

namespace Fruits
{
	public class FruitPlacer : MonoBehaviour
	{
		[SerializeField] private TileHolder _tileHolder;

		private Dragger _dragger;
		private Merger _merdger;
		private Vibrations _vibrations;

		private IDraggable _currentDraggable;
		private Tile _startTile;
		private Tile _pointingTile;

		private List<Tile> _visitedTiles;

		[Inject]
		private void Construct(Dragger dragger, Merger merdger, Vibrations vibrations)
		{
			_dragger = dragger;
			_merdger = merdger;
			_vibrations = vibrations;
		}

		private void Awake()
		{
			_visitedTiles = new List<Tile>();
			_dragger.PointingTileChanged += OnPointingTileChanged;
			_dragger.StoppedDragging += OnStoppedDragging;
			_dragger.StartedDragging += OnStartedDragging;
		}

		private void OnDestroy()
		{
			_dragger.PointingTileChanged -= OnPointingTileChanged;
			_dragger.StoppedDragging -= OnStoppedDragging;
			_dragger.StartedDragging -= OnStartedDragging;
		}

		private void OnStartedDragging(IDraggable draggable)
		{
			_visitedTiles.Clear();
			_currentDraggable = draggable;
			_startTile = GetTileWithDraggable(draggable);
			_visitedTiles.Add(_startTile);
			_pointingTile = _startTile;
		}

		private void OnStoppedDragging(IDraggable draggable)
		{
			bool isFruit = draggable is Fruit fruit;
			if (!isFruit) return;
			bool isPlaced = false;
			fruit = isFruit ? draggable as Fruit : null;

			if (_pointingTile.TileContent == draggable)
			{
				ReturnToPrevTile(draggable, fruit);
				return;
			}

			if (_pointingTile.TileContent is Fruit tileFruit)
			{
				if (_merdger.IsReadyToMerge(fruit.FruitData, tileFruit.FruitData))
				{
					draggable.ReceivePosition(_pointingTile.GetBildingPoint());

					_visitedTiles.ForEach(tile => tile.ClearTile());
					_vibrations.VibrateSoft();
					_merdger.Merge(_pointingTile, fruit, tileFruit);
				}
				else
				{
					_vibrations.VibrateMedium();
					ReturnToPrevTile(draggable, fruit);
				}
				isPlaced = true;
			}
			else
			{
				ReturnToPrevTile(draggable, fruit);
				isPlaced = true;
			}

			if (!isPlaced)
			{
				ReturnToPrevTile(draggable, fruit);
			}

			_visitedTiles.Clear();
			_pointingTile = null;
		}

		private void ReturnToPrevTile(IDraggable draggable, Fruit fruit)
		{
			draggable.ReceivePosition(_visitedTiles[^1].GetBildingPoint());
			_visitedTiles[^1].SetTileContent(fruit);
		}

		private void OnPointingTileChanged(Tile tile)
		{
			_pointingTile = tile;
			if (tile.IsAvailable())
			{
				if (_currentDraggable is ITileContent tileContent)
				{
					tileContent.SetTile(tile);
					tile.SetTileContent(tileContent);
				}

				_visitedTiles.Add(tile);
				if (_visitedTiles.Count >= 2)
				{
					_visitedTiles[^2].ClearTile();
				}
			}
		}

		private Tile GetTileWithDraggable(IDraggable draggable)
		{
			if (draggable is not Fruit fruit) throw new System.Exception("draggable isn't fruit");

			return fruit.Tile;
		}

		private void PrintList(List<Tile> tiles)
		{
			string line = "";
			for (int i = 0; i < tiles.Count; i++)
			{
				try
				{
					line += $"{tiles[i].gameObject.name} -> ";
				}
				catch (System.Exception e)
				{
					print(e.Message);
					print(e.StackTrace);
				}
			}
			print(line);
		}
	}
}