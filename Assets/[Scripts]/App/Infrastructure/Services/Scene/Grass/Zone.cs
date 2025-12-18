using System;
using UnityEngine;

namespace Serjbal
{
    public class Zone : MonoBehaviour, IZone
    {
        [SerializeField] private string _grassType;
        [SerializeField] private GrassSystem _grassSystem;
        [SerializeField] private int _texRes = 1024;
        
        private TexturePainter _texturePainter;
        private Texture2D _paintTexture;
        private int _mowedPixels;

        public Action<string, int> OnMewed { get; set; }

        private void Start()
        {
            _texturePainter = new TexturePainter();
            _paintTexture = _texturePainter.CreateTexture(_texRes);
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

                    _grassSystem.PaintTexture = _paintTexture;

                    OnMewed?.Invoke(_grassType, painted/(_texRes*2));
                }
            }
        }
    }
}
