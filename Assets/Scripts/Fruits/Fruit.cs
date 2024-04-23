using System;
using System.Collections;
using Tiles;
using UnityEngine;

namespace Fruits
{
	public class Fruit : MonoBehaviour, IDraggable, ITileContent
    {
        [SerializeField] private FruitData _fruitData;
        [SerializeField] private Collider _collider;
        [SerializeField] private Vector3 _offset;

        private float _startHeight;
        private float _currentAdditiveHeight;

        public FruitData FruitData => _fruitData;
        public Collider Collider => _collider;
        public Tile Tile { get; private set; }
        public Bounds Bounds => _collider.bounds;
        public Vector3 Offset => _offset;

        public event Action<Fruit> Destroyed;
        public event Action<Fruit> Dropped;

        private void Start()
        {
            _startHeight = transform.localPosition.y;
        }

        private void OnDestroy()
        {
            Destroyed?.Invoke(this);
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