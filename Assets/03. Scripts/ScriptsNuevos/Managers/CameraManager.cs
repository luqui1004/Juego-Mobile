using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [Header("Camera")]
    [SerializeField] private Camera mainCamera;
    private Vector3 originalPos;
    private float originalSize;

    [Header("Zoom Config")]
    [SerializeField] private float zoomSize = 4.3f;
    [SerializeField] private float zoomDuration = 0.8f;
    [SerializeField] private Vector3 zoomOffset = new Vector3(0f, -0.5f, 0f);

    private Coroutine zoomCoroutine;
    private Coroutine shakeCoroutine;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        originalPos = mainCamera.transform.position;
        originalSize = mainCamera.orthographicSize;
    }

    public void DoZoomIn()
    {
        if (zoomCoroutine != null)
            StopCoroutine(zoomCoroutine);

        zoomCoroutine = StartCoroutine(CameraZoomIn());
    }

    public void DoZoomOut()
    {
        if (zoomCoroutine != null)
            StopCoroutine(zoomCoroutine);

        zoomCoroutine = StartCoroutine(CameraZoomOut());
    }

    public void DoShake(float intensity, float duration)
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(ShakeRoutine(intensity, duration));
    }

    public void ResetCameraInstant()
    {
        if (zoomCoroutine != null)
            StopCoroutine(zoomCoroutine);
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        mainCamera.orthographicSize = originalSize;
        mainCamera.transform.position = originalPos;
    }

    private IEnumerator CameraZoomIn()
    {
        float elapsed = 0f;
        float startSize = mainCamera.orthographicSize;
        Vector3 startPos = mainCamera.transform.position;
        Vector3 targetPos = originalPos + zoomOffset;

        while (elapsed < zoomDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / zoomDuration);

            mainCamera.orthographicSize = Mathf.Lerp(startSize, zoomSize, t);
            mainCamera.transform.position = Vector3.Lerp(startPos, targetPos, t);

            yield return null;
        }
    }

    private IEnumerator CameraZoomOut()
    {
        float elapsed = 0f;
        float startSize = mainCamera.orthographicSize;
        Vector3 startPos = mainCamera.transform.position;

        while (elapsed < zoomDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / zoomDuration);

            mainCamera.orthographicSize = Mathf.Lerp(startSize, originalSize, t);
            mainCamera.transform.position = Vector3.Lerp(startPos, originalPos, t);

            yield return null;
        }

        mainCamera.orthographicSize = originalSize;
        mainCamera.transform.position = originalPos;
    }

    private IEnumerator ShakeRoutine(float intensity, float duration)
    {
        // Guardamos la posición ACTUAL de la cámara al iniciar el shake
        Vector3 startPos = mainCamera.transform.position;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * intensity;
            float y = Random.Range(-1f, 1f) * intensity;

            // Shake RELATIVO Y CONTROLADO
            mainCamera.transform.position = startPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Al terminar, volver EXACTAMENTE al lugar correcto
        mainCamera.transform.position = startPos;
    }
}
