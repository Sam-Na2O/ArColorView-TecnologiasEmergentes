using UnityEngine;
using System.Collections.Generic;

public class RoomColorController : MonoBehaviour
{
    public Material wallMaterial;
    public Material floorMaterial;
    public Material bedMaterial;
    public Material furnitureMaterial;

    private ColorManager colorManager;

    void Start()
    {
        colorManager = FindObjectOfType<ColorManager>();
    }

    public void AplicarPaleta()
    {
        if (colorManager == null) return;

        List<Color> paleta = colorManager.PaletaActual;

        if (paleta == null || paleta.Count < 4) return;

        wallMaterial.color = paleta[0];
        floorMaterial.color = paleta[1];
        bedMaterial.color = paleta[2];
        furnitureMaterial.color = paleta[3];
    }
}