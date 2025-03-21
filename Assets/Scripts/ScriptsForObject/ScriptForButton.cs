using UnityEditor; // 匯出時要刪掉這行
using UnityEngine;
using UnityEngine.SceneManagement;
public class ScriptForButton : MonoBehaviour {  
    public void SwitchSceneTo(int sceneNumber) {
        SceneManager.LoadScene(sceneNumber);
    }

    public void QuitGame() {
        EditorApplication.isPlaying = false; // 匯出時要刪掉這行
        Application.Quit();
    }
}