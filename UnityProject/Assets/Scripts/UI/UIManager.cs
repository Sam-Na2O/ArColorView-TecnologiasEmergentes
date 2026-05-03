using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    private ColorManager colorManager;

    [Header("UI General")]
    public TextMeshProUGUI topText;
    public GameObject detectButton;

    [Header("Panel de color detectado")]
    public GameObject colorPanel;
    public Image colorPreview;
    public TextMeshProUGUI colorCodeText;

    [Header("Panel de paletas")]
    public GameObject palettePanel;
    public Image[] paletteImages;

    [Header("Panel de modelo")]
    public GameObject modelPanel;

    void Awake()
    {
        colorManager = GetComponent<ColorManager>();
    }

    void Start()
    {
        ShowInitial();

        if (colorManager != null)
        {
            colorManager.OnPaletaActualizada += MostrarResultadoDeteccion;
        }
        else
        {
            Debug.LogError("UIManager: No se encontró ColorManager");
        }
    }

    void OnDestroy()
    {
        if (colorManager != null)
        {
            colorManager.OnPaletaActualizada -= MostrarResultadoDeteccion;
        }
    }

    public void ShowInitial()
    {
        topText.text = "Apunta a una superficie y detecta el color";

        detectButton.SetActive(true);
        colorPanel.SetActive(false);
        palettePanel.SetActive(false);
        modelPanel.SetActive(false);
    }

    public void OnDetectPressed()
    {
        topText.text = "Detectando color...";
        detectButton.SetActive(false);

        if (colorManager != null)
        {
            colorManager.CapturarColor();
        }
    }

    void MostrarResultadoDeteccion(List<Color> paleta)
    {
        Debug.Log("UIManager recibió la paleta");

        topText.text = "Color detectado";

        detectButton.SetActive(false);
        colorPanel.SetActive(true);

        palettePanel.SetActive(false);
        modelPanel.SetActive(false);

        if (paleta != null && paleta.Count > 0)
        {
            colorPreview.color = paleta[0];
            colorCodeText.text = "#" + ColorUtility.ToHtmlStringRGB(paleta[0]);
        }
    }

    public void OnSelectColor()
    {
        Debug.Log("Botón seleccionar presionado");

        topText.text = "Paletas recomendadas";

        colorPanel.SetActive(false);
        palettePanel.SetActive(true);

        if (colorManager == null)
        {
            Debug.LogError("ColorManager no está asignado");
            return;
        }

        if (colorManager.PaletaActual == null || colorManager.PaletaActual.Count == 0)
        {
            Debug.LogError("La paleta está vacía");
            return;
        }

        for (int i = 0; i < paletteImages.Length && i < colorManager.PaletaActual.Count; i++)
        {
            paletteImages[i].color = colorManager.PaletaActual[i];
        }
    }

    public void RestartApp()
    {
        modelPanel.SetActive(false);
        ShowInitial();
    }
}
