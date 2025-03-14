using UnityEngine;

public class ButtonClick : MonoBehaviour {
    void OnMouseDown() { // clicking button
        if (GameManager.Instance != null) { 
            GameManager.Instance.ButtonWasClicked(); // Calling GameManager's function "ButtonWasClicked"
            print("Button has been clicked");
        }
    }
}