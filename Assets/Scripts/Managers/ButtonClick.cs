using UnityEngine;

public class ButtonClick : MonoBehaviour {
    public string buttonID;
    void OnMouseDown() {

        if (GameManager.Instance != null) {
            GameManager.Instance.CubeWasClicked(); // Calling GameManager's function "ButtonWasClicked"

            print($"Button{buttonID} has been clicked");
        }
    }
}