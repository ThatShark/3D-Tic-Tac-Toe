using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ScriptForButton : MonoBehaviour {  
    public void SwitchSceneTo(int sceneNumber) {
        SceneManager.LoadScene(sceneNumber);
    }

    public void QuitGame() {
        EditorApplication.isPlaying = false;
        Application.Quit();
    }
}