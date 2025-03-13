using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptForButton : MonoBehaviour{
    // Load game
    public void SwitchSceneTo(int sceneNumber) {
        SceneManager.LoadScene(sceneNumber);
    }

    // quit game
    public void QuitGame () {
        EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
// 且慢
