using Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tiles
{
	public class TileHolder : MonoBehaviour
    {
        [SerializeField] private List<Tile> _tilesBundle;

        public List<Tile> TilesBundle => _tilesBundle;

        public event Action<Tile> TileBecameAvailable;
        public event Action<Tile, ITileContent> TileContentChanged;

        private void Awake()
        {
            _tilesBundle.ForEach(tile => tile.TileContentChanged += OnTileContentChanged);
        }

		private void OnDestroy()
		{
			_tilesBundle.ForEach(tile => tile.TileContentChanged -= OnTileContentChanged);
		}

		private void OnTileContentChanged(Tile tile, ITileContent tileContent)
        {
            this.Print($"content changed. Tile {tile.ID}, content: {tileContent}");

            TileContentChanged?.Invoke(tile, tileContent);
            if (tileContent == null)
            {
                TileBecameAvailable?.Invoke(tile);
            }
        }
    }
}