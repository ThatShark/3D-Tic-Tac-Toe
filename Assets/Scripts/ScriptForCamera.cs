using UnityEngine;

public class ScriptForCamera : MonoBehaviour{  
    public int speed = 10;
    public int sensitivity = 5;

    public float cameraFaceX = 0;
    public float cameraFaceY = 1.3f;
    // int count = 0;

    void Start() {

    }

    void Update() {

        cameraFaceX += Input.GetAxis("Mouse Y") * -1 * sensitivity;
        cameraFaceY += Input.GetAxis("Mouse X") * sensitivity;
        this.transform.eulerAngles = new Vector3(cameraFaceX, cameraFaceY, 0);

        if(Input.GetKey(KeyCode.W)){
            this.transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);
        }else if(Input.GetKey(KeyCode.S)){
            this.transform.Translate(Vector3.back * Time.deltaTime * speed, Space.Self);
        }else if(Input.GetKey(KeyCode.A)){
            this.transform.Translate(Vector3.left * Time.deltaTime * speed, Space.Self);
        }else if(Input.GetKey(KeyCode.D)){
                this.transform.Translate(Vector3.right * Time.deltaTime * speed, Space.Self);            }else if(Input.GetKey(KeyCode.Space)){
            this.transform.Translate(Vector3.up * Time.deltaTime * speed, Space.Self);
        }else if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
            this.transform.Translate(Vector3.down * Time.deltaTime * speed, Space.Self);
        }
    }

}
