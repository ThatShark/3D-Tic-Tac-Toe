using UnityEngine;

public class ScriptForCamera : MonoBehaviour {
    //public float speed = 10;
    //public float sensitivity = 5;
    //public float cameraFaceX = 0;
    //public float cameraFaceY = 1.3f;

    //public GameObject Camera;

    //public Ray ray;
    //// public RaycastHit hit;
    //// Vector3 hitPoint = Vector3.zero;
    //Vector3 v = new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0);
    //void Start() {

    //}

    //void Update() {

    //    if (Input.GetMouseButton(0)) {
    //        if (v.x >= Screen.width) {
    //            v.x = 0.0f;
    //        } else {
    //            v.x += 1.0f;
    //        }

    //        ray = Camera.GetComponent<Camera>().ScreenPointToRay(v);
    //        cameraFaceX -= Input.GetAxis("Mouse Y") * sensitivity;
    //        cameraFaceY += Input.GetAxis("Mouse X") * sensitivity;
    //        this.transform.localRotation = Quaternion.Euler(cameraFaceX, cameraFaceY, 0);

    //    }

    //    if (Input.GetKey(KeyCode.W)) {
    //        this.transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);

    //    } else if (Input.GetKey(KeyCode.S)) {
    //        this.transform.Translate(Vector3.back * Time.deltaTime *
    //    speed, Space.Self);
    //    } else if (Input.GetKey(KeyCode.A)) {
    //        this.transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);
    //    } else if (Input.GetKey(KeyCode.D)) {
    //        this.transform.Translate(Vector3.right * Time.deltaTime * speed, Space.Self);
    //    } else if (Input.GetKey(KeyCode.Space)) {

    //        this.transform.Translate(Vector3.up * Time.deltaTime * speed, Space.Self
    //   );
    //    } else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
    //        this.transform.Translate(Vector3.down * Time.deltaTime * speed, Space.Self);
    //    }
    //}



    #region shark
    // 固定camera與正中央的距離
    // public Transform middleCube;
    // float offsetZ = 2f, offsetY = 1f;

    // void Start() {
    //     transform.rotation = Quaternion.identity;
    //     Vector3 pos = middleCube.position;
    //     pos.z -= offsetZ; // 後退 offsetZ 的距離
    //     transform.position = pos;
    // }

    // // 旋轉camera
    // void Update() {
    //     float mouse_dx = Input.GetAxis("Mouse X");
    //     float mouse_dy = Input.GetAxis("Mouse Y");
    //     if (Input.GetMouseButton(0)) {
    //         if (Mathf.Abs(mouse_dx) > 0 || Mathf.Abs(mouse_dy) > 0) {
    //             // 獲取camera的歐拉角  
    //             Vector3 currentCameraAngle = transform.rotation.eulerAngles;

    //             currentCameraAngle.x = Mathf.Repeat(currentCameraAngle.x + 180f, 360f) - 180f; // 永遠在 0 ~ 180
    //             currentCameraAngle.y += mouse_dx; // unity Y軸，往右為正，往左為負
    //             currentCameraAngle.x -= mouse_dy; // unity X軸，往上為負，往下為正

    //             Quaternion cameraRotation = Quaternion.identity;
    //             cameraRotation.eulerAngles = new Vector3(currentCameraAngle.x, currentCameraAngle.y, 0);
    //             transform.rotation = cameraRotation;

    //             // 重制camera位置
    //             Vector3 middlePosition = middleCube.position;
    //             Vector3 distance = cameraRotation * new Vector3(0f, 0f, offsetZ);
    //             transform.position = middlePosition - distance;

    //         }
    //     }

    // }
    #endregion
}
