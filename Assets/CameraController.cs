using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // 主角的位置
    public float smoothSpeed = 0.125f; // 摄像机移动的平滑度

    // 【新增260929】摄像机的移动边界
    public float minX = -15f;
    public float maxX = 15f;
    public float minY = -18f;
    public float maxY = 18f;

    void LateUpdate()
    {
        if (player != null)
        {
            // 计算期望的位置
            Vector3 desiredPosition = new Vector3(player.position.x, player.position.y, transform.position.z);

            // 【新增260929】限制摄像机的范围，防止拍到地图外面
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);

            // 平滑移动
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}