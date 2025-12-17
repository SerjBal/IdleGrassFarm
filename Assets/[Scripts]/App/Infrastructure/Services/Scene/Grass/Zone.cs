using System;
using UnityEngine;

namespace Serjbal
{
    public class Zone : MonoBehaviour, IZone
    {
        [SerializeField] private string _grassType;
        [SerializeField] private GrassSystem _grassSystem;
        [SerializeField] private string _mapName = "GrassMaskTex";
        [SerializeField] private int _texRes = 1024;
        
        private TexturePainter _texturePainter;
        private Texture2D _paintTexture;
        private int _mowedPixels;

        public Action<string, int> OnMewed { get; set; }

        private void Start()
        {
            _texturePainter = new TexturePainter();
            _paintTexture = CreateTexture();
            _grassSystem.PaintTexture = _paintTexture;
        }

        public void Mow(Vector3 position, float radius)
        {
            RaycastHit hit;
            
            if (Physics.Raycast(position, Vector3.down, out hit, 100))
            {
                if (hit.collider.name == gameObject.name)
                {
                    Vector2 uv = hit.textureCoord;
                    var painted = _texturePainter.PaintTexture(_paintTexture, uv, radius, Color.black);

                    _grassSystem.PaintTexture=_paintTexture;

                    OnMewed?.Invoke(_grassType, painted/(_texRes*4));
                }
            }
        }

        private Texture2D CreateTexture()
        {
            var texture = new Texture2D(_texRes, _texRes, TextureFormat.RGBA32, false);
            Color[] colors = new Color[_texRes * _texRes];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = Color.white;
            }
            texture.SetPixels(colors);
            texture.Apply();
            return texture;
        }
    }
}
