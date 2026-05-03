using UnityEngine;
using System.Collections.Generic;

public class RoomColorController : MonoBehaviour
{
    public Material wallMaterial;
    public Material floorMaterial;
    public Material bedMaterial;
    public Material furnitureMaterial;

    public void AplicarPaleta(List<Color> paleta)
    {
        if (paleta == null || paleta.Count < 6) return;

        wallMaterial.color = paleta[0];      // base
        floorMaterial.color = paleta[3];     // oscuro
        bedMaterial.color = paleta[1];       // claro
        furnitureMaterial.color = paleta[4]; // muy oscuro
    }
}