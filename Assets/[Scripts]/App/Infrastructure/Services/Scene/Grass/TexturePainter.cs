using UnityEngine;

namespace Serjbal
{
    public class TexturePainter
    {
        public int CalculateTexture(Texture2D texture, Vector2 point, float radius, Color color)
        {
            int pixelsPainted = 0;
            int centerX = Mathf.RoundToInt(point.x * texture.width);
            int centerY = Mathf.RoundToInt(point.y * texture.height);
            int pixelRadius = Mathf.RoundToInt(radius * Mathf.Min(texture.width, texture.height));

            int startX = Mathf.Max(0, centerX - pixelRadius);
            int endX = Mathf.Min(texture.width - 1, centerX + pixelRadius);
            int startY = Mathf.Max(0, centerY - pixelRadius);
            int endY = Mathf.Min(texture.height - 1, centerY + pixelRadius);

            float radiusSquared = pixelRadius * pixelRadius;

            for (int y = startY; y <= endY; y++)
            {
                for (int x = startX; x <= endX; x++)
                {
                    int dx = x - centerX;
                    int dy = y - centerY;
                
                    if (dx * dx + dy * dy <= radiusSquared)
                    {
                        Color currentColor = texture.GetPixel(x, y);
                        if (currentColor != color)
                        {
                            texture.SetPixel(x, y, color);
                            pixelsPainted++;
                        }
                    }
                }
            }
        
            return pixelsPainted;
        }
        
        public Texture2D CreateTexture(int resolution)
        {
            var texture = new Texture2D(resolution, resolution, TextureFormat.RGBA32, false);
            Color[] colors = new Color[resolution * resolution];
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