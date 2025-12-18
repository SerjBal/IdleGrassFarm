using System;
using Unity.Mathematics;
using UnityEngine;

namespace Serjbal
{
    public class Zone : MonoBehaviour, IZone
    {
        [SerializeField] private ItemType _grassType;
        [SerializeField] private GrassSystem _grassSystem;
        [SerializeField] private int _texRes = 1024;
        
        private TexturePainter _texturePainter;
        private Texture2D _paintTexture;
        private float _currentPercent;
        private float _totalPixels;
        public Action<ItemPrice> OnMewed { get; set; }

        private void Start()
        {
            _texturePainter = new TexturePainter();
            _paintTexture = _texturePainter.CreateTexture(_texRes);
            _grassSystem.PaintTexture = _paintTexture;
            _totalPixels = _texRes * _texRes;
        }

        public void Mow(Vector3 position, float radius)
        {
            RaycastHit hit;
            
            if (Physics.Raycast(position, Vector3.down, out hit, 10))
            {
                if (hit.collider.name == gameObject.name)
                {
                    PaintTexture(radius, hit, out var inPercents);

                    _currentPercent += inPercents;
                    if (_currentPercent >= 1)
                    {
                        var result = Mathf.FloorToInt(_currentPercent);
                        OnMewed?.Invoke(new ItemPrice(_grassType, result));
                        _currentPercent -= result;
                    }
                }
            }
        }

        private void PaintTexture(float radius, RaycastHit hit, out float inPercents)
        {
            var painted = _texturePainter.PaintTexture(_paintTexture, hit.textureCoord, radius, Color.black);
            _grassSystem.PaintTexture = _paintTexture;
            inPercents = painted / _totalPixels * 100f;
        }
    }
}
