using UnityEngine;
public class ScriptForCamera : MonoBehaviour {
    public Transform target; // 旋轉中心目標
    private Camera c;
    public int maxView = 120; // 最大縮放距離
    public int minView = 50; // 最小縮放距離
    private float slideSpeed = 20f;

    private float currentPitch = 0f; // 記錄垂直旋轉角度
    private float currentYaw = 0f; // 記錄水平旋轉角度

    private float distanceToTarget; // 相機與目標之間的距離

    private void Start() {
        transform.LookAt(target);
        c = this.GetComponent<Camera>();
        c.fieldOfView = 80f;
        distanceToTarget = Vector3.Distance(transform.position, target.position);
    }

    private void LateUpdate() {
        float mouseX = Input.GetAxis("Mouse X"); // 獲取鼠標X軸增量
        float mouseY = -Input.GetAxis("Mouse Y"); // 獲取鼠標Y軸增量

        if (Input.GetMouseButton(1)) { // 右鍵旋轉
            // 更新水平和垂直旋轉角度
            currentYaw += mouseX * 5f; // 水平旋轉
            currentPitch -= mouseY * 5f; // 垂直旋轉
            currentPitch = Mathf.Clamp(currentPitch, -85f, 85f); // 限制垂直旋轉角度

            // 計算新的相機位置並更新
            Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
            Vector3 direction = rotation * Vector3.back; // 使用 `Vector3.back` 來定義從相機到目標的方向
            transform.position = target.position + direction * distanceToTarget; // 根據距離調整相機位置

            transform.LookAt(target); // 讓相機始終指向目標
        }

        // 滾輪縮放
        float mouseCenter = Input.GetAxis("Mouse ScrollWheel");
        if (mouseCenter < 0) { // 滾輪縮小
            if (c.fieldOfView <= maxView) {
                c.fieldOfView += 10 * slideSpeed * Time.deltaTime;
            }
        } else if (mouseCenter > 0) { // 滾輪放大
            if (c.fieldOfView >= minView) {
                c.fieldOfView -= 10 * slideSpeed * Time.deltaTime;
            }
        }
    }
}