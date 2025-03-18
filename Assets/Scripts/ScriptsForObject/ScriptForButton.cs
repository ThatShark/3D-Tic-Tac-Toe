using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ScriptForButton : MonoBehaviour {
    public GameManager gameManager;
    void Start() {
        gameManager = FindFirstObjectByType<GameManager>();
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

    public GameManager.Player nowTurn;
    public void SurrenderSkill () {
        gameManager.winner = ((nowTurn == GameManager.Player.O) ? GameManager.Player.X : GameManager.Player.O);
        gameManager.currentTurn = GameManager.Player.Neither;
    }

    public void Check() {
        nowTurn = gameManager.currentTurn;
        gameManager.currentTurn = GameManager.Player.Checking;
        gameManager.checkBox.SetActive(true);
    }
    public void Cancel () {
        gameManager.currentTurn = nowTurn;
        gameManager.checkBox.SetActive(false);
    }
}
