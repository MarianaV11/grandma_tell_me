using UnityEngine;

public class OverlayCanvasSetup : MonoBehaviour
{
    [SerializeField] private Canvas overlayCanvas;

    void Start()
    {
        Camera mainCam = Camera.main;
        if (mainCam != null && overlayCanvas != null)
        {
            overlayCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            overlayCanvas.worldCamera = mainCam;
            Debug.Log("OverlayCanvas configurado com a MainCamera.");
        }
        else
        {
            Debug.LogWarning("MainCamera ou overlayCanvas não encontrados.");
        }
    }
}
