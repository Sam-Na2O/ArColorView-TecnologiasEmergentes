using UnityEngine;

public class ColorDetector : MonoBehaviour
{
    public Texture2D cameraImage;

    public Color GetAverageColor()
    {
        Color[] pixels = cameraImage.GetPixels();
        float r = 0, g = 0, b = 0;

        foreach (Color pixel in pixels)
        {
            r += pixel.r;
            g += pixel.g;
            b += pixel.b;
        }

        int total = pixels.Length;

        return new Color(r / total, g / total, b / total);
    }
}
