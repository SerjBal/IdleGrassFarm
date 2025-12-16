using System;
using Serjbal.Infrastructure.Services;
using UnityEngine;

namespace Serjbal
{
    public class Zone : MonoBehaviour, IZone
    {
        [SerializeField] private ItemType _grassType;
        [SerializeField] private Renderer _targetRenderer;
        [SerializeField] private Color _brushColor = Color.red;
        [SerializeField] private string _mapName = "_BaseMap";

        private float _brushRadius = 0.1f;
        private TexturePainter _texturePainter;
        private Texture2D _paintTexture;
        private int _mowedPixels;
        private MaterialPropertyBlock _propertyBlock;

        public Action<ItemType, int> OnMewed { get; set; }

        private void Start()
        {
            _texturePainter = new TexturePainter();
            _paintTexture = new Texture2D(1024, 1024, TextureFormat.RGBA32, false);
    
            Color[] colors = new Color[1024 * 1024];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = Color.black;
            }
            _paintTexture.SetPixels(colors);
            _paintTexture.Apply();

            _propertyBlock = new MaterialPropertyBlock();
            _propertyBlock.SetTexture(_mapName, _paintTexture);
            _targetRenderer.SetPropertyBlock(_propertyBlock);
        }

        public void Mow(Vector3 position)
        {
            RaycastHit hit;
            
            if (Physics.Raycast(position, Vector3.down, out hit, 100))
            {
                if (hit.collider.name == gameObject.name)
                {
                    Vector2 uv = hit.textureCoord;
                    var painted = _texturePainter.PaintTexture(_paintTexture, uv, _brushRadius, _brushColor);

                    if (_propertyBlock != null)
                    {
                        _propertyBlock.SetTexture(_mapName, _paintTexture);
                        _targetRenderer.SetPropertyBlock(_propertyBlock);
                    }

                    OnMewed?.Invoke(_grassType, painted);
                }
            }
        }
    }
}
