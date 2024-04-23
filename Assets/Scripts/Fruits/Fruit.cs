using Extensions;
using System;
using Tiles;
using TMPro;
using UnityEngine;

namespace Fruits
{
	public class Fruit : MonoBehaviour, IDraggable, ITileContent
    {
        [SerializeField] private TextMeshPro _text;
        [SerializeField] private Collider _collider;
        [SerializeField] private Vector3 _offset;

        public FruitData FruitData { get; private set; }
        public Collider Collider => _collider;
        public Tile Tile { get; private set; }
        public Bounds Bounds => _collider.bounds;
        public Vector3 Offset => _offset;

        public event Action<Fruit> Destroyed;
        public event Action<Fruit> Dropped;

        private void OnDestroy()
        {
            Destroyed?.Invoke(this);
        }

        public void SetFruitData(FruitData data)
        {
            FruitData = data;
            _text.SetText(data.GlobalLevel);
        }

        public void ReceivePosition(Vector3 position)
        {
            transform.position = position + _offset;
        }

        public void SetTile(Tile tile)
        {
            //transform.position = tile.GetBildingPoint() + _offset;
            ReceivePosition(tile.GetBildingPoint());
            transform.SetParent(tile.transform);
            Tile = tile;
        }

        public void StartDrag()
        {

        }

        public void StopDrag()
        {

        }
    }
}