using Fruits;
using Merdge;
using System.Collections.Generic;
using Tiles;
using UnityEngine;
using Zenject;

public class FruitPlacer : MonoBehaviour
{
    [SerializeField] private TileHolder _tileHolder;

    private Dragger _dragger;
    private Merdger _merdger;

    private IDraggable _currentDraggable;
    private Tile _startTile;
    private Tile _pointingTile;

    private List<Tile> _visitedTiles;

    [Inject]
    private void Construct(Dragger dragger, Merdger merdger)
    {
        _dragger = dragger;
        _merdger = merdger;
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
        draggable.StartDrag();
        _visitedTiles.Clear();
        _currentDraggable = draggable;
        _startTile = GetTileWithDraggable(draggable);
        _visitedTiles.Add(_startTile);
        _pointingTile = _startTile;
    }

    private void OnStoppedDragging(IDraggable draggable)
    {
        draggable.StopDrag();
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
            if (_merdger.IsReadyToMerdge(fruit.FruitData, tileFruit.FruitData))
            {
                draggable.ReceivePosition(_pointingTile.GetBildingPoint());
                PrintList(_visitedTiles);
                _visitedTiles.ForEach(tile => tile.ClearTile());
                _merdger.Merdge(_pointingTile, fruit, tileFruit);
            }
            else
            {
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

    private void SetBuildingToCurrentTile(Fruit fruit)
    {
        fruit.ReceivePosition(_visitedTiles[^1].GetBildingPoint());
        if (_visitedTiles[^1].TileContent == null)
        {
            _visitedTiles[^1].SetTileContent(fruit);
            //var tile = _islands.TilesBundle.Find(t => t.TileContent == building);
            //tile?.ClearBuilding();
            fruit.SetTile(_visitedTiles[^1]);
        }
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
