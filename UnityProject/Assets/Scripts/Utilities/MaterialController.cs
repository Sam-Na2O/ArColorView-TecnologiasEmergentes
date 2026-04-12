using UnityEngine;

public class MaterialController : MonoBehaviour
{
    public Renderer wallRenderer;

    public void ApplyColor(Color newColor)
    {
        wallRenderer.material.color = newColor;
    }
}
