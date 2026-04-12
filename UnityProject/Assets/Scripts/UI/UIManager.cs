using UnityEngine;

public class UIManager : MonoBehaviour
{
    public MaterialController materialController;

    public void OnColorSelected(Color selectedColor)
    {
        materialController.ApplyColor(selectedColor);
    }
}
