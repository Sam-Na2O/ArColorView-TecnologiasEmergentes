using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorManager : MonoBehaviour
{
    [Header("Referencias")]
    public ColorDetector detector;
    public Renderer modelo3D;
    public Image colorPanelImage;

    [Header("Modo")]
    public bool modoEnVivo = false;

    public Color ColorBase { get; private set; }
    public List<Color> PaletaActual { get; private set; } = new List<Color>();

    public System.Action<List<Color>> OnPaletaActualizada;

    void Awake()
    {
        // 🔥 Toma automáticamente el ColorDetector del mismo objeto
        detector = GetComponent<ColorDetector>();
    }

    void Start()
    {
        if (detector == null)
        {
            Debug.LogError("ColorManager: falta asignar el ColorDetector.");
            return;
        }

        detector.OnColorDetectado += ManejarColorDetectado;
    }

    void ManejarColorDetectado(Color color)
    {
        if (modoEnVivo)
            ActualizarPaleta(color);
    }

    public void CapturarColor()
    {
        if (detector == null) return;

        ActualizarPaleta(detector.ColorPredominante);
    }

    void ActualizarPaleta(Color colorBase)
    {
        ColorBase = colorBase;
        PaletaActual = PaletteGenerator.GenerarPaleta(colorBase);

        if (modelo3D != null)
            modelo3D.material.color = ColorBase;

        if (colorPanelImage != null)
            colorPanelImage.color = ColorBase;

        OnPaletaActualizada?.Invoke(PaletaActual);

        Debug.Log($"Paleta generada. Color base: {ColorBase}");
    }

    void OnDestroy()
    {
        if (detector != null)
            detector.OnColorDetectado -= ManejarColorDetectado;
    }
}