using UnityEngine;

public class ScriptForCamera : MonoBehaviour {
    public Transform target; // 旋轉中心目標
    private Camera c;
    public int maxView = 120; // 最大縮放距離
    public int minView = 50; // 最小縮放距離
    private float slideSpeed = 20f;

    private void Start() {
        c = this.GetComponent<Camera>();

    }
    private void LateUpdate() {
        transform.LookAt(target);
        float mouseX = Input.GetAxis("Mouse X"); // 獲取鼠標X軸增加量
        float mouseY = -Input.GetAxis("Mouse Y"); // 獲取鼠標Y軸增加量
        if (Input.GetMouseButton(1)) { // 右鍵轉動
            QuaternionRotateAround(target.transform.position, Vector3.up, mouseX * 5);
            QuaternionRotateAround(target.transform.position, Vector3.right, mouseY * 5);
        }

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

    void QuaternionRotateAround(Vector3 center, Vector3 axis, float angle) {
        Vector3 pos = transform.position;
        Quaternion rot = Quaternion.AngleAxis(angle, axis);
        Vector3 dir = pos - center; // 計算從圓心指向camera的朝向向量
        dir = rot * dir; // 旋轉此向量 
        transform.position = center + dir; // 移動camera位置 
        var myRot = transform.rotation;
        transform.rotation *= Quaternion.Inverse(myRot) * rot * myRot; // 設置角度
    }
}