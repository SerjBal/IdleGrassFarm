using System;
using Serjbal.Infrastructure.Services;
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
                    var oldPercent = _currentPercent;
                    var oldЗPixels = _paintTexture.GetPixels();

                    PaintTexture(radius, hit, out var paintedPercents);
                    _currentPercent += paintedPercents;

                    if (_currentPercent >= 1)
                    {
                        var result = Mathf.FloorToInt(_currentPercent);
                        
                        if (DI.GetService<IInventory>().CheckLimit(new ItemPrice(_grassType, result)))
                        {
                            _currentPercent -= result;
                            OnMewed?.Invoke(new ItemPrice(_grassType, result));
                        }
                        else
                        {
                            _currentPercent = oldPercent;
                            _paintTexture.SetPixels(oldЗPixels);
                            _paintTexture.Apply();
                            _grassSystem.PaintTexture = _paintTexture;
                        }
                    }
                }
            }
        }
        
        private void PaintTexture(float radius, RaycastHit hit, out float inPercents)
        {
            var painted = _texturePainter.CalculateTexture(_paintTexture, hit.textureCoord, radius, Color.black);
            _grassSystem.PaintTexture = _paintTexture;

            inPercents = painted / _totalPixels * 100f;
        }
    }
}
