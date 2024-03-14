using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera mainCamera; 
    public Camera secondaryCamera;

    public bool IsUsingMainCamera { get; private set; }

    void Start()
    {
        mainCamera.gameObject.SetActive(true);
        secondaryCamera.gameObject.SetActive(false);
    }

    public void ToggleCamera()
    {
        IsUsingMainCamera = !IsUsingMainCamera;
        
        mainCamera.gameObject.SetActive(IsUsingMainCamera);
        secondaryCamera.gameObject.SetActive(!IsUsingMainCamera);
    }
}
