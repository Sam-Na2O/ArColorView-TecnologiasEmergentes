using UnityEngine;
using UnityEngine.UI;

public class ColorDetector : MonoBehaviour
{
    [Header("UI (opcional)")]
    public RawImage cameraDisplay; // para previsualizar la cámara

    [Header("Muestreo")]
    [Tooltip("Cada cuántos segundos se analiza la imagen.")]
    public float intervaloMuestreo = 0.5f;
    [Tooltip("De cada N píxeles, se toma 1. Más alto = más rápido.")]
    public int saltoPixeles = 16;

    private WebCamTexture webcam;
    private float tiempo;

    // Color predominante actual (otros módulos pueden leerlo)
    public Color ColorPredominante { get; private set; }

    // Evento para avisar a otros scripts (UI, modelo 3D, etc.)
    public System.Action<Color> OnColorDetectado;

    void Start()
    {
        if (WebCamTexture.devices.Length == 0)
        {
            Debug.LogError("No se detectó cámara en el dispositivo.");
            return;
        }

        webcam = new WebCamTexture();
        if (cameraDisplay != null) cameraDisplay.texture = webcam;
        webcam.Play();
    }

    void Update()
    {
        if (webcam == null || !webcam.isPlaying || webcam.width < 100) return;

        tiempo += Time.deltaTime;
        if (tiempo >= intervaloMuestreo)
        {
            tiempo = 0f;
            CalcularColorPromedio();
        }
    }

    void CalcularColorPromedio()
    {
        Color32[] pixeles = webcam.GetPixels32();

        long r = 0, g = 0, b = 0;
        int count = 0;

        for (int i = 0; i < pixeles.Length; i += saltoPixeles)
        {
            r += pixeles[i].r;
            g += pixeles[i].g;
            b += pixeles[i].b;
            count++;
        }

        if (count == 0) return;

        ColorPredominante = new Color(
            (r / count) / 255f,
            (g / count) / 255f,
            (b / count) / 255f
        );

        OnColorDetectado?.Invoke(ColorPredominante);
    }

    void OnDisable()
    {
        if (webcam != null && webcam.isPlaying) webcam.Stop();
    }
}