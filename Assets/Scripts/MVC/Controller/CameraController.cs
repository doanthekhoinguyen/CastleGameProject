using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera mainCamera; // Camera chính mà bạn sử dụng ban đầu
    public Camera secondaryCamera; // Camera thứ hai mà bạn muốn chuyển đổi sang

    private bool isUsingMainCamera = true; // Biến kiểm tra xem camera nào đang được sử dụng

    void Start()
    {
        // Khi bắt đầu, đảm bảo rằng mainCamera được bật và secondaryCamera được tắt
        mainCamera.gameObject.SetActive(true);
        secondaryCamera.gameObject.SetActive(false);
    }

    public void ToggleCamera()
    {
        isUsingMainCamera = !isUsingMainCamera; // Đảo giá trị để chuyển đổi camera

        // Dựa vào giá trị của isUsingMainCamera để bật/tắt camera tương ứng
        mainCamera.gameObject.SetActive(isUsingMainCamera);
        secondaryCamera.gameObject.SetActive(!isUsingMainCamera);
    }
}
