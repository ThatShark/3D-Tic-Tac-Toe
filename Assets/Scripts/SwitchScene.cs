using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    // Load game
    public void SwitchSceneTo(int sceneNumber) {
        SceneManager.LoadScene(sceneNumber);
    }
}
