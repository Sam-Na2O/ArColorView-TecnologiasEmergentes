using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI General")]
    public TextMeshProUGUI topText;
    public GameObject detectButton;

    [Header("Panel de color detectado")]
    public GameObject colorPanel;
    public Image colorPreview;
    public TextMeshProUGUI colorCodeText;

    [Header("Panel de paletas")]
    public GameObject palettePanel;
    public Image[] paletteImages; // Asignar 4 imágenes desde Unity

    [Header("Panel de modelo")]
    public GameObject modelPanel;

    void Start()
    {
        ShowInitial();
    }

    // Estado inicial
    public void ShowInitial()
    {
        topText.text = "Apunta a una superficie y detecta el color";

        detectButton.SetActive(true);
        colorPanel.SetActive(false);
        palettePanel.SetActive(false);
        modelPanel.SetActive(false);
    }

    // Botón detectar
    public void OnDetectPressed()
    {
        topText.text = "Detectando color...";
        detectButton.SetActive(false);

        // Persona 1 — Captura de cámara y datos de color
        // Aquí se debe iniciar la detección real del color desde la cámara.
        // Cuando se obtenga el color, se debe llamar a OnColorDetected(...)
    }

    // Recibir color detectado
    public void OnColorDetected(Color detectedColor, string hexCode)
    {
        topText.text = "Color detectado";

        colorPanel.SetActive(true);

        colorPreview.color = detectedColor;
        colorCodeText.text = hexCode;

        // Persona 2 — Modelo tridimensional y materiales
        // Aquí se puede iniciar la generación de la paleta a partir del color detectado
    }

    // Mostrar paleta generada
    public void ShowPalette(Color[] colors)
    {
        topText.text = "Paletas recomendadas";

        colorPanel.SetActive(false);
        palettePanel.SetActive(true);

        for (int i = 0; i < paletteImages.Length && i < colors.Length; i++)
        {
            paletteImages[i].color = colors[i];
        }

        // Persona 2 — Modelo tridimensional y materiales
        // Este método debe ser llamado cuando ya se tenga la paleta generada
    }

    // Botón seleccionar paleta
    public void OnSelectPalette()
    {
        topText.text = "Modelo generado";

        palettePanel.SetActive(false);
        modelPanel.SetActive(true);

        // Persona 2 — Modelo tridimensional y materiales
        // Aquí se deben aplicar los colores al modelo 3D

        // Persona 3 — Realidad aumentada y escena tridimensional
        // Aquí se debe mostrar o posicionar el modelo en la escena
    }

    // Botón terminar
    public void RestartApp()
    {
        modelPanel.SetActive(false);
        ShowInitial();

        // Todos
        // Aquí se puede limpiar estado o reiniciar datos si es necesario
    }
}
