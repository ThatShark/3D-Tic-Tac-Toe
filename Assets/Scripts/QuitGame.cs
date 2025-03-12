using UnityEditor;
using UnityEngine;

public class QuitGame : MonoBehaviour {
    // quit game
    public void Quit () {
        Application.Quit();
        EditorApplication.isPlaying = false;
    }
}
