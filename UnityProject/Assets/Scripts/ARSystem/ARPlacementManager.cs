using UnityEngine;

public class ARPlacementManager : MonoBehaviour
{
    public GameObject objectToPlace;

    public void PlaceObject(Vector3 position)
    {
        Instantiate(objectToPlace, position, Quaternion.identity);
    }
}
