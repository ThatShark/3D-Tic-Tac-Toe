using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptForButton : MonoBehaviour {
    public GameManager gameManager;
    void Start() {
        gameManager = GameObject.Find("GameObject").GetComponent<GameManager>();
    }
    public void SwitchSceneTo(int sceneNumber) {
        SceneManager.LoadScene(sceneNumber);
    }

    public void QuitGame() {
        EditorApplication.isPlaying = false;
        Application.Quit();
    }

    public void ResetGame() {
        gameManager.ResetScene();
    }

    public void TriangleSkill () {
        
    }

    public void SpinSkill () {
        
    }

    public void UpsideDownSkill () {
        
    }

    
    public void SurrenderSkill () {
        gameManager.winner = ((gameManager.currentTurn == GameManager.Player.O) ? GameManager.Player.X : GameManager.Player.O);
        gameManager.currentTurn = GameManager.Player.Neither;
    }
}
