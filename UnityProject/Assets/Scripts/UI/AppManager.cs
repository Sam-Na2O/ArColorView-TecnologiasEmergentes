using UnityEngine;
using System.Collections.Generic;

public class AppManager : MonoBehaviour
{
    public ColorDetector colorDetector;
    public PaletteGenerator paletteGenerator;
    public UIManager uiManager;

    public void ProcessImage()
    {
        Color detectedColor = colorDetector.GetAverageColor();

        List<Color> palette = paletteGenerator.GeneratePalette(detectedColor);

        // Aquí se enviarían los colores a la UI
        Debug.Log("Color detectado y paleta generada");
    }
}
