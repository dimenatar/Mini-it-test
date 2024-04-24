using DG.Tweening;
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
        [SerializeField] private Vector3 _offset;

        [SerializeField] private float _scaleInDuration = 0.3f;
        [SerializeField] private float _scaleOutDuration = 0.3f;
        [SerializeField] private Ease _scaleInEase;
        [SerializeField] private Ease _scaleOutEase;

        public FruitData FruitData { get; private set; }
        public Tile Tile { get; private set; }
        public Vector3 Offset => _offset;

        public event Action<Fruit> Destroyed;

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
            transform.SetParent(tile.transform);
            Tile = tile;
        }

        public void ScaleIn()
        {
            transform.ScaleIn(_scaleInEase, _scaleInDuration);
        }

        public void ScaleOut()
        {
            transform.ScaleOut(_scaleOutDuration, _scaleOutEase);
        }
    }
}