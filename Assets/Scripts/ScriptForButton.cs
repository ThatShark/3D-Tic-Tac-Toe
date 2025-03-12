using UnityEngine;

public class ScriptForButton : MonoBehaviour{
    public Vector3 mousePosition = Input.mousePosition;

    void Start() {
        
    }

    void Update() {
       if(Input.GetMouseButtonDown(0)){
            if(mousePosition.x > 0 && mousePosition.x < 100 && mousePosition.y > 0 && mousePosition.y < 100){
                Debug.Log("Button Clicked");
            }
        }
        
    }
}
