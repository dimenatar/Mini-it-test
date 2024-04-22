using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tiles
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;

        private Color _defaultColor;

        public bool IsFree { get; set; } = true;

        public int ID { get; private set; }
        public ITileContent TileContent { get; private set; }

        public event Action<Tile, ITileContent> TileContentChanged;

        protected virtual void Awake()
        {
            _defaultColor = _renderer.material.color;
        }

        public void Initialise(int id)
        {
            ID = id;
        }

        public void SetTileContent(ITileContent tileContent)
        {
            if (TileContent != tileContent)
            {
                IsFree = false;
                TileContent = tileContent;
                TileContentChanged?.Invoke(this, tileContent);
            }
        }

        public void ClearTile()
        {
            IsFree = true;
            TileContent = null;
            TileContentChanged?.Invoke(this, null);
        }

        public Vector3 GetBildingPoint()
        {
            return transform.position;
        }

        public void Highlight(Color color)
        {
            _renderer.material.color = color;
        }

        public void StopHighlighting()
        {
            _renderer.material.color = _defaultColor;
        }

        public virtual bool IsAvailable()
        {
            return IsFree && gameObject.activeInHierarchy;
        }
    }
}