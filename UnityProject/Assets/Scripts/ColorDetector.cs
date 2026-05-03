using System.Collections;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ColorDetector : MonoBehaviour
{
    [Header("Referencias AR")]
    public ARCameraManager cameraManager; // arrastrar la AR Camera aquí

    [Header("Muestreo")]
    [Tooltip("Cada cuántos segundos se analiza la imagen.")]
    public float intervaloMuestreo = 0.5f;
    [Tooltip("De cada N píxeles, se toma 1. Más alto = más rápido.")]
    public int saltoPixeles = 16;
    [Tooltip("Reducción de resolución para ahorrar recursos.")]
    public int factorReduccion = 4;

    private float tiempo;
    private bool procesando;

    // Color predominante actual (otros módulos pueden leerlo)
    public Color ColorPredominante { get; private set; } = Color.white;

    // Evento para avisar a otros scripts
    public System.Action<Color> OnColorDetectado;

    void OnEnable()
    {
        if (cameraManager != null)
            cameraManager.frameReceived += OnFrameReceived;
    }

    void OnDisable()
    {
        if (cameraManager != null)
            cameraManager.frameReceived -= OnFrameReceived;
    }

    void OnFrameReceived(ARCameraFrameEventArgs eventArgs)
    {
        // Throttling: no procesamos cada frame
        tiempo += Time.deltaTime;
        if (tiempo < intervaloMuestreo || procesando) return;
        tiempo = 0f;

        ProcesarImagen();
    }

    void ProcesarImagen()
    {
        // Intentar obtener la imagen actual de la cámara AR
        if (!cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
            return;

        procesando = true;

        // Configurar la conversión: bajar resolución y formato RGBA
        var conversionParams = new XRCpuImage.ConversionParams
        {
            inputRect = new RectInt(0, 0, image.width, image.height),
            outputDimensions = new Vector2Int(
                image.width / factorReduccion,
                image.height / factorReduccion),
            outputFormat = TextureFormat.RGBA32,
            transformation = XRCpuImage.Transformation.None
        };

        // Reservar buffer y convertir
        int size = image.GetConvertedDataSize(conversionParams);
        var buffer = new NativeArray<byte>(size, Allocator.Temp);
        image.Convert(conversionParams, buffer);

        // CRÍTICO: liberar la imagen para evitar fugas de memoria
        image.Dispose();

        CalcularColorPromedio(buffer, conversionParams.outputDimensions);

        buffer.Dispose();
        procesando = false;
    }

    void CalcularColorPromedio(NativeArray<byte> buffer, Vector2Int dimensiones)
    {
        long r = 0, g = 0, b = 0;
        int count = 0;

        // El buffer es RGBA32 = 4 bytes por píxel
        int totalPixeles = dimensiones.x * dimensiones.y;

        for (int i = 0; i < totalPixeles; i += saltoPixeles)
        {
            int idx = i * 4;
            r += buffer[idx];
            g += buffer[idx + 1];
            b += buffer[idx + 2];
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
}