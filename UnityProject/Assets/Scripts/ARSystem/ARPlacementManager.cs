using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class ARPlacementManager : MonoBehaviour
{
    [Header("Objeto a colocar")]
    public GameObject objectToPlace;

    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private GameObject spawnedObject;
    private bool canPlace = false;

    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        // No permitir colocar hasta que UI lo indique
        if (!canPlace) return;

        // Detectar toque en pantalla
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            //codigo original
            // Lanzar raycast contra superficies detectadas
            if (raycastManager.Raycast(touch.position, hits, TrackableType.Planes))
            {
                Pose hitPose = hits[0].pose;

                // Solo crear un objeto
                if (spawnedObject == null)
                {
                    spawnedObject = Instantiate(objectToPlace, hitPose.position, hitPose.rotation);
                }
            }
            
            /*
            // Prueba en editor
            if (spawnedObject == null)
            {
                spawnedObject = Instantiate(objectToPlace, Vector3.zero, Quaternion.identity);
            }
            */
        }
    }

    // Método que se conecta desde el botón (NO desde UIManager)
    public void OnPaletteSelected()
    {
        Debug.Log("AR activado desde selección de paleta");
        EnablePlacement();
    }

    // Activa la colocación
    public void EnablePlacement()
    {
        canPlace = true;
    }

    // Opcional: reiniciar estado
    public void ResetPlacement()
    {
        canPlace = false;

        if (spawnedObject != null)
        {
            Destroy(spawnedObject);
            spawnedObject = null;
        }
    }
}