using Fruits;
using Merge;
using Scriptables;
using UserInput;

namespace Tiles
{
	public class TileHighlighter
    {
        private TilesColorsConfig _tileColorsConfig;
        private Dragger _dragger;
        private Merger _merger;

        private Fruit _currentFruit;
        private Tile _prevTile;

        public TileHighlighter(TilesColorsConfig tileColorsConfig, Merger merger)
        {
            _tileColorsConfig = tileColorsConfig;
            _merger = merger;
        }

        public void SetDragger(Dragger dragger)
        {
            if (_dragger)
            {
                _dragger.StartedDragging -= OnStartedDragging;
                _dragger.StoppedDragging -= OnStoppedDragging;
                _dragger.PointingTileChanged -= OnPointingTileChanged;
            }
            _dragger = dragger;
            _dragger.StartedDragging += OnStartedDragging;
            _dragger.StoppedDragging += OnStoppedDragging;
            _dragger.PointingTileChanged += OnPointingTileChanged;
        }

        private void OnPointingTileChanged(Tile tile)
        {
            _prevTile?.StopHighlighting();
            _prevTile = tile;
            if (tile.IsAvailable() || tile.TileContent == _currentFruit)
            {
                tile.Highlight(_tileColorsConfig.StartDragColor);
            }
            else
            {
                if (tile.TileContent is not Fruit tileFruit)
                {
                    tile.Highlight(_tileColorsConfig.NotReadyToMergeColor);
                }
                else
                {
                    if (_merger.IsReadyToMerge(_currentFruit.FruitData, tileFruit.FruitData))
                    {
                        tile.Highlight(_tileColorsConfig.ReadyToMergeColor);
                    }
                    else
                    {
                        tile.Highlight(_tileColorsConfig.NotReadyToMergeColor);
                    }
                }
            }
        }

        public void Dispose()
        {
            _dragger.StartedDragging -= OnStartedDragging;
            _dragger.StoppedDragging -= OnStoppedDragging;
            _dragger.PointingTileChanged -= OnPointingTileChanged;
        }

        private void OnStoppedDragging(IDraggable draggable)
        {
            if (draggable is Fruit fruit)
            {
                _prevTile?.StopHighlighting();
                _prevTile = null;
            }
        }

        private void OnStartedDragging(IDraggable draggable)
        {
            if (draggable is Fruit fruit)
            {
                _currentFruit = fruit;
                //fruit.Highlight(_tileColorsConfig.StartDrag);
            }
        }
    }
}