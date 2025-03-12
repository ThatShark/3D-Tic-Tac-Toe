using UnityEngine;

public class ScriptForCamera : MonoBehaviour{  
    void Start() {

    }

    void Update() {
        this.transform.LookAt(Vector3.zero);
        switch(Input.inputString){
            case "W":
                this.transform.Translate(Vector3.forward * Time.deltaTime, Space.Self);
                break;
            case "S":
                this.transform.Translate(Vector3.back * Time.deltaTime, Space.Self);
                break;
            case "A":
                this.transform.Translate(Vector3.left * Time.deltaTime, Space.Self);
                break;
            case "D":
                this.transform.Translate(Vector3.right * Time.deltaTime, Space.Self);
                break;
     }
        

        
    }

}
