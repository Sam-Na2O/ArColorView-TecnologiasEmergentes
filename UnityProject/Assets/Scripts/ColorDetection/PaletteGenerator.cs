using UnityEngine;
using System.Collections.Generic;

public class PaletteGenerator : MonoBehaviour
{
    public List<Color> GeneratePalette(Color baseColor)
    {
        List<Color> palette = new List<Color>();

        // Ejemplo simple: color más claro y más oscuro
        palette.Add(baseColor);
        palette.Add(baseColor * 0.8f);
        palette.Add(baseColor * 1.2f);

        return palette;
    }
}
