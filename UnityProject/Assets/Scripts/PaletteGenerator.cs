using System.Collections.Generic;
using UnityEngine;

public static class PaletteGenerator
{
    public static List<Color> GenerarPaleta(Color colorBase)
    {
        List<Color> paleta = new List<Color>();

        paleta.Add(colorBase);                       // 0: base
        paleta.Add(Aclarar(colorBase, 0.25f));       // 1: claro
        paleta.Add(Aclarar(colorBase, 0.50f));       // 2: muy claro
        paleta.Add(Oscurecer(colorBase, 0.25f));     // 3: oscuro
        paleta.Add(Oscurecer(colorBase, 0.50f));     // 4: muy oscuro
        paleta.Add(Complementario(colorBase));       // 5: opuesto

        return paleta;
    }

    static Color Aclarar(Color c, float factor)
        => Color.Lerp(c, Color.white, factor);

    static Color Oscurecer(Color c, float factor)
        => Color.Lerp(c, Color.black, factor);

    static Color Complementario(Color c)
    {
        Color.RGBToHSV(c, out float h, out float s, out float v);
        h = (h + 0.5f) % 1f; // gira 180° en el círculo cromático
        return Color.HSVToRGB(h, s, v);
    }
}