using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

public class SceneHistoryManager : MonoBehaviour {
    public static SceneHistoryManager Instance;
    private Stack<string> sceneStack = new Stack<string>();

    public Animator transitionAnimator;
    public float transitionTime = 1f;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
    public string owo;
    void Update() {
        owo = (sceneStack.Count == 0) ? null : sceneStack.Peek();
    }

    public void LoadScene(string sceneName) {
        string currentScene = SceneManager.GetActiveScene().name;
        sceneStack.Push(currentScene);

        if (sceneName.Contains("Game")) {
            while (sceneStack.Count > 1) {
                sceneStack.Pop();
            }
        }

        StartCoroutine(LoadSceneWithTransition(sceneName));
    }

    public void LoadPreviousScene() {
        if (sceneStack.Count > 0) {
            string previousScene = sceneStack.Pop();
            StartCoroutine(LoadSceneWithTransition(previousScene));
        }
    }

    private IEnumerator LoadSceneWithTransition(string sceneName) {
        if (sceneName.Contains("Game")) {
            // 淡出
            transitionAnimator.SetTrigger("FadeOut");
            yield return new WaitForSeconds(transitionTime);
        }

        SceneManager.LoadScene(sceneName);

        if (sceneName.Contains("Game")) {
            // 淡入
            transitionAnimator.SetTrigger("FadeIn");
            yield return new WaitForSeconds(transitionTime);
        }
    }
}