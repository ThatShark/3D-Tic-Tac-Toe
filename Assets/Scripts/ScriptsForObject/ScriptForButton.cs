#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ScriptForButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    public UnityEvent onClick;
    private bool isPointerDown = false;

    public void SwitchSceneTo(string sceneName) {
        SceneHistoryManager.Instance.LoadScene(sceneName);
    }

    public void OnBackButtonClicked() {
        SceneHistoryManager.Instance.LoadPreviousScene();
    }

    public void QuitGame() {
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    public void OnPointerDown(PointerEventData eventData) {
        isPointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData) {
        if (isPointerDown) {
            onClick.Invoke();
            isPointerDown = false;
        }
    }
}