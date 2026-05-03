using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    [Header("Referencias")]
    public ColorDetector detector;
    public Renderer modelo3D; // del módulo de Persona 2 (puede ser null al inicio)

    [Header("Modo")]
    [Tooltip("Si está activo, la paleta se actualiza en vivo. Si no, solo cambia al llamar CapturarColor().")]
    public bool modoEnVivo = false;

    // Datos accesibles para otros módulos (UI, AR, etc.)
    public Color ColorBase { get; private set; }
    public List<Color> PaletaActual { get; private set; } = new List<Color>();

    // Evento para que la UI (Persona 4) se entere cuando hay paleta nueva
    public System.Action<List<Color>> OnPaletaActualizada;

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
        // Solo actualizamos automáticamente si está en modo en vivo
        if (modoEnVivo)
        {
            ActualizarPaleta(color);
        }
    }

    /// <summary>
    /// Llamar desde un botón de UI para capturar el color actual.
    /// </summary>
    public void CapturarColor()
    {
        if (detector == null) return;
        ActualizarPaleta(detector.ColorPredominante);
    }

    void ActualizarPaleta(Color colorBase)
    {
        ColorBase = colorBase;
        PaletaActual = PaletteGenerator.GenerarPaleta(colorBase);

        // Aplicar al modelo 3D si está asignado
        if (modelo3D != null)
        {
            modelo3D.material.color = ColorBase;
        }

        // Avisar a quien esté escuchando (UI, etc.)
        OnPaletaActualizada?.Invoke(PaletaActual);

        Debug.Log($"Paleta generada. Color base: {ColorBase}");
    }

    void OnDestroy()
    {
        if (detector != null)
            detector.OnColorDetectado -= ManejarColorDetectado;
    }
}