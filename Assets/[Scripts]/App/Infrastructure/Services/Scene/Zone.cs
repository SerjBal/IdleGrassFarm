using System;
using Serjbal.Infrastructure.Services;
using UnityEngine;

namespace Serjbal
{
    public class Zone : MonoBehaviour, IZone
    {
        [SerializeField] private ItemType _grassType;
        [SerializeField] private Renderer _targetRenderer;
        [SerializeField] private string _mapName = "_BaseMap";

        private float _startMawRadius = 0.1f;
        private float _finalMawRadius = 0.1f;
        private TexturePainter _texturePainter;
        private Texture2D _paintTexture;
        private int _mowedPixels;
        private MaterialPropertyBlock _propertyBlock;

        public Action<ItemType, int> OnMewed { get; set; }

        private void Start()
        {
            _texturePainter = new TexturePainter();
            _paintTexture = CreateTexture();
            
            _propertyBlock = new MaterialPropertyBlock();
            _propertyBlock.SetTexture(_mapName, _paintTexture);
            _targetRenderer.SetPropertyBlock(_propertyBlock);
        }

        public void SetMowRadius(int upgradeLevel)
        {
            _finalMawRadius = _startMawRadius * upgradeLevel;
        }

        public void Mow(Vector3 position)
        {
            RaycastHit hit;
            
            if (Physics.Raycast(position, Vector3.down, out hit, 100))
            {
                if (hit.collider.name == gameObject.name)
                {
                    Vector2 uv = hit.textureCoord;
                    var painted = _texturePainter.PaintTexture(_paintTexture, uv, _finalMawRadius, Color.white);

                    if (_propertyBlock != null)
                    {
                        _propertyBlock.SetTexture(_mapName, _paintTexture);
                        _targetRenderer.SetPropertyBlock(_propertyBlock);
                    }

                    OnMewed?.Invoke(_grassType, painted);
                }
            }
        }

        private Texture2D CreateTexture()
        {
            var texture = new Texture2D(1024, 1024, TextureFormat.RGBA32, false);
            Color[] colors = new Color[1024 * 1024];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = Color.black;
            }
            texture.SetPixels(colors);
            texture.Apply();
            return texture;
        }
    }
}
