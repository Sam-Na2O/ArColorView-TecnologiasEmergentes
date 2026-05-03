using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaletteUI : MonoBehaviour
{
    [Header("Referencias")]
    public ColorManager colorManager;
    public Image[] swatches; // 6 cuadritos de la UI

    void Start()
    {
        if (colorManager == null)
        {
            Debug.LogError("PaletteUI: falta asignar el ColorManager.");
            return;
        }

        colorManager.OnPaletaActualizada += MostrarPaleta;
    }

    void MostrarPaleta(List<Color> paleta)
    {
        for (int i = 0; i < swatches.Length && i < paleta.Count; i++)
        {
            if (swatches[i] != null)
                swatches[i].color = paleta[i];
        }
    }

    void OnDestroy()
    {
        if (colorManager != null)
            colorManager.OnPaletaActualizada -= MostrarPaleta;
    }
}