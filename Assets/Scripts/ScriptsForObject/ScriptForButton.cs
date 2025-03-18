using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ScriptForButton : MonoBehaviour {
    private GameManager gameManager;
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

    public void TriangleSkill() {

    }

    public void SpinSkill() {

    }

    public void UpsideDownSkill() {

    }

    public GameManager.Player nowTurn;
    public void SurrenderSkill() {
        if (nowTurn == GameManager.Player.O) {
            gameManager.winner = GameManager.Player.X;
        } else {
            gameManager.winner = GameManager.Player.O;
        }
        gameManager.currentTurn = GameManager.Player.Neither;
    }

    public void CheckIfSurrender() {
        nowTurn = gameManager.currentTurn;
        gameManager.currentTurn = GameManager.Player.Checking;
        gameManager.checkBox.SetActive(true);
    }
    public void CancelSurrender() {
        gameManager.currentTurn = nowTurn;
        gameManager.checkBox.SetActive(false);
        Debug.Log(gameManager.currentTurn);
    }
}
