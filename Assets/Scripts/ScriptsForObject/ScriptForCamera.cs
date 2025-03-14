using UnityEngine;

public class ScriptForCamera : MonoBehaviour {
    public Transform target; // 旋轉中心目標
    private Camera c;
    public int maxView = 120; // 最大縮放距離
    public int minView = 10; // 最小縮放距離
    private float slideSpeed = 20f;

    private void Start() {
        c = this.GetComponent<Camera>();

    }
    private void LateUpdate() {
        float mouseX = Input.GetAxis("Mouse X"); // 獲取鼠標X軸增加量
        float mouseY = -Input.GetAxis("Mouse Y"); // 獲取鼠標Y軸增加量
        if (Input.GetMouseButton(1)) { // 右鍵轉動
            transform.RotateAround(target.transform.position, Vector3.up, mouseX * 5);
            transform.RotateAround(target.transform.position, transform.right, mouseY * 5);
        }
// 我剛剛有ㄒ
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