using UnityEditor;
using UnityEngine;

public class QuitGame : MonoBehaviour{
    // quit game
    public void Quit () {
        EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
