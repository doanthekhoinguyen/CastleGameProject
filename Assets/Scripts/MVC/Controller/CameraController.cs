using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform targetPosition; // Vị trí mà camera sẽ di chuyển đến
    public float moveSpeed = 5f; // Tốc độ di chuyển của camera

    // Phương thức này được gọi khi bạn muốn camera di chuyển đến vị trí mới
    public void MoveCameraToPosition()
    {
        // Bắt đầu coroutine để di chuyển camera
        StartCoroutine(MoveToPosition(targetPosition.position, targetPosition.rotation, moveSpeed));
    }

    private IEnumerator MoveToPosition(Vector3 targetPos, Quaternion targetRot, float speed)
    {
        while (Vector3.Distance(transform.position, targetPos) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, speed * Time.deltaTime);
            yield return null;
        }
    }

}
