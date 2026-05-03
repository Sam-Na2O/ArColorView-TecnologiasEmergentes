using UnityEngine;

public class RoomColorController : MonoBehaviour
{
    public Material wallMaterial;
    public Material floorMaterial;
    public Material bedMaterial;
    public Material furnitureMaterial;

    public void ApplyColors(Color wall, Color floor, Color bed, Color furniture)
    {
        wallMaterial.color = wall;
        floorMaterial.color = floor;
        bedMaterial.color = bed;
        furnitureMaterial.color = furniture;
    }
}