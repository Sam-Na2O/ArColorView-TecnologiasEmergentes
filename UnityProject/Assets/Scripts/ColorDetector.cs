using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.Collections;

public class ColorDetector : MonoBehaviour
{
    [Header("Referencias AR")]
    public ARCameraManager cameraManager;

    [Header("Muestreo")]
    public float intervaloMuestreo = 0.5f;
    public int saltoPixeles = 16;
    public int factorReduccion = 4;

    private float tiempo;
    private bool procesando;

    public Color ColorPredominante { get; private set; } = Color.white;
    public System.Action<Color> OnColorDetectado;

#if UNITY_EDITOR
    // --- VARIABLES PARA WEBCAM (SOLO EN PC) ---
    private WebCamTexture webCamTexture;
#endif

    void Start()
    {
#if UNITY_EDITOR
        webCamTexture = new WebCamTexture();
        webCamTexture.Play();
        
        // Mostrar el feed en el RawImage si está asignado
        if (previewWebcam != null)
            previewWebcam.texture = webCamTexture;
#endif
    }

    void OnEnable()
    {
#if !UNITY_EDITOR
        // Usar AR Foundation solo si NO estamos en la compu (cuando esté en el celular)
        if (cameraManager != null)
            cameraManager.frameReceived += OnFrameReceived;
#endif
    }

    void OnDisable()
    {
#if !UNITY_EDITOR
        if (cameraManager != null)
            cameraManager.frameReceived -= OnFrameReceived;
#endif
    }

    void Update()
    {
#if UNITY_EDITOR
        // Lógica para la Webcam en PC
        if (webCamTexture == null || !webCamTexture.isPlaying) return;

        tiempo += Time.deltaTime;
        if (tiempo < intervaloMuestreo || procesando) return;
        tiempo = 0f;

        ProcesarImagenWebCam();
#endif
    }

#if !UNITY_EDITOR
    // --- LÓGICA DE CELULAR (AR FOUNDATION) ---
    void OnFrameReceived(ARCameraFrameEventArgs eventArgs)
    {
        tiempo += Time.deltaTime;
        if (tiempo < intervaloMuestreo || procesando) return;
        tiempo = 0f;
        ProcesarImagenAR();
    }

    void ProcesarImagenAR()
    {
        if (!cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image)) return;
        procesando = true;

        var conversionParams = new XRCpuImage.ConversionParams
        {
            inputRect = new RectInt(0, 0, image.width, image.height),
            outputDimensions = new Vector2Int(image.width / factorReduccion, image.height / factorReduccion),
            outputFormat = TextureFormat.RGBA32,
            transformation = XRCpuImage.Transformation.None
        };

        int size = image.GetConvertedDataSize(conversionParams);
        var buffer = new NativeArray<byte>(size, Allocator.Temp);
        image.Convert(conversionParams, buffer);
        image.Dispose();

        CalcularColorPromedio(buffer, conversionParams.outputDimensions);
        buffer.Dispose();
        procesando = false;
    }

    void CalcularColorPromedio(NativeArray<byte> buffer, Vector2Int dimensiones)
    {
        long r = 0, g = 0, b = 0;
        int count = 0;
        int totalPixeles = dimensiones.x * dimensiones.y;

        for (int i = 0; i < totalPixeles; i += saltoPixeles)
        {
            int idx = i * 4;
            r += buffer[idx];
            g += buffer[idx + 1];
            b += buffer[idx + 2];
            count++;
        }

        if (count > 0)
        {
            ColorPredominante = new Color((r / count) / 255f, (g / count) / 255f, (b / count) / 255f);
            OnColorDetectado?.Invoke(ColorPredominante);
        }
    }
#endif

#if UNITY_EDITOR
    // --- LÓGICA DE PC (WEBCAM) ---
    void ProcesarImagenWebCam()
    {
        procesando = true;
        
        Color32[] pixeles = webCamTexture.GetPixels32();
        long r = 0, g = 0, b = 0;
        int count = 0;

        for (int i = 0; i < pixeles.Length; i += saltoPixeles)
        {
            r += pixeles[i].r;
            g += pixeles[i].g;
            b += pixeles[i].b;
            count++;
        }

        if (count > 0)
        {
            ColorPredominante = new Color((r / count) / 255f, (g / count) / 255f, (b / count) / 255f);
            OnColorDetectado?.Invoke(ColorPredominante);
        }

        procesando = false;
    }
#endif

#if UNITY_EDITOR
    [Header("Preview Editor (solo para pruebas en PC)")]
    public UnityEngine.UI.RawImage previewWebcam; // opcional, solo editor
#endif

}