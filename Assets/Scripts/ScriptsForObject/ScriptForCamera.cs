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
    public Transform target; // 旋轉中心目標
    private float mouse_x;
    private float mouse_y;
    private void LateUpdate() {
        float mouse_x = Input.GetAxis("Mouse X"); // 獲取鼠標X軸增加量
        float mouse_y = -Input.GetAxis("Mouse Y"); // 獲取鼠標Y軸增加量
        if (Input.GetMouseButton(1)) { 
            transform.RotateAround(target.transform.position, Vector3.up, mouse_x * 5); 
            transform.RotateAround(target.transform.position, transform.right, mouse_y * 5); 
        } 
    }
    #endregion
}
