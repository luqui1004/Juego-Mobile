using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] public Camera mainCamera;
    private Vector3 originalCameraPosition;
    private float originalCameraSize;

    [Header("Zoom Config")]
    [SerializeField] private float zoomSize = 4.3f;
    [SerializeField] private float zoomDuration = 0.8f;
    [SerializeField] private Vector3 zoomOffset = new Vector3(0f, -0.5f, 0f);
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera != null)
        {
            originalCameraPosition = mainCamera.transform.position;
            originalCameraSize = mainCamera.orthographicSize;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator CameraZoomIn()
    {
        float elapsed = 0f;
        float startSize = mainCamera.orthographicSize;
        Vector3 startPos = mainCamera.transform.position;
        Vector3 targetPos = originalCameraPosition + zoomOffset;

        while (elapsed < zoomDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / zoomDuration);
            mainCamera.orthographicSize = Mathf.Lerp(startSize, zoomSize, t);
            mainCamera.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }
    }

    public IEnumerator CameraZoomOut()
    {
        float elapsed = 0f;
        float startSize = mainCamera.orthographicSize;
        Vector3 startPos = mainCamera.transform.position;

        while (elapsed < zoomDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / zoomDuration);
            mainCamera.orthographicSize = Mathf.Lerp(startSize, originalCameraSize, t);
            mainCamera.transform.position = Vector3.Lerp(startPos, originalCameraPosition, t);
            yield return null;
        }

        mainCamera.orthographicSize = originalCameraSize;
        mainCamera.transform.position = originalCameraPosition;
    }

    public void death()
    {
        mainCamera.orthographicSize = originalCameraSize;
        mainCamera.transform.position = originalCameraPosition;
    }
}
