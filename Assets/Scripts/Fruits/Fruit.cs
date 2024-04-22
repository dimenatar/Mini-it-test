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

        [SerializeField] private float _flyHeight = 2f;
        [SerializeField] private float _flySpeed = 20f;

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
            transform.position = new Vector3(position.x, transform.position.y, position.z);
            transform.localPosition = new Vector3(transform.localPosition.x, _startHeight + position.y + _offset.y + _currentAdditiveHeight, transform.localPosition.z);
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
            float destinationHeight = _startHeight + _flyHeight;
            StopAllCoroutines();
            StartCoroutine(MoveToLocalY(destinationHeight, _flySpeed));
        }

        public void StopDrag()
        {
            StopAllCoroutines();
            StartCoroutine(MoveToLocalY(_startHeight, _flySpeed));
        }

        private IEnumerator MoveToLocalY(float destinationHeight, float speed)
        {
            if (transform.localPosition.y < destinationHeight)
            {
                while (transform.localPosition.y < destinationHeight)
                {

                    Vector3 position = transform.localPosition;
                    _currentAdditiveHeight = position.y + speed * Time.deltaTime;
                    transform.localPosition = new Vector3(transform.localPosition.x, _currentAdditiveHeight, transform.localPosition.z);
                    yield return null;
                }
            }
            else
            {
                while (transform.localPosition.y > destinationHeight)
                {
                    Vector3 position = transform.localPosition;
                    _currentAdditiveHeight = position.y - speed * Time.deltaTime;
                    transform.localPosition =  new Vector3 (transform.localPosition.x, _currentAdditiveHeight, transform.localPosition.z);
                    yield return null;
                }
                //_dropParticles.Play();
                Dropped?.Invoke(this);
            }
            transform.localPosition = new Vector3(transform.localPosition.x, destinationHeight, transform.localPosition.z);
        }
    }
}