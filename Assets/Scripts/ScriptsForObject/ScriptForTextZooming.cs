using UnityEngine;
using System.Collections;
using TMPro;

public class TextScaleAnimation : MonoBehaviour {
    public TextMeshProUGUI textMeshPro; // 文字物件
    public float targetFontSize = 50f;  // 最大字體大小
    public float animationTime = 0.5f;  // 動畫持續時間
    private float initialFontSize;      // 初始字體大小

    void Start() {
        initialFontSize = textMeshPro.fontSize; // 記錄初始字體大小
        StartCoroutine(AnimateFontSizeLoop());  // 開始無限循環動畫
    }

    // Coroutine 用來實現字體大小無限循環動畫
    IEnumerator AnimateFontSizeLoop() {
        while (true) {
            // 放大動畫
            float elapsedTime = 0f;
            while (elapsedTime < animationTime) {
                textMeshPro.fontSize = Mathf.Lerp(initialFontSize, targetFontSize, elapsedTime / animationTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            textMeshPro.fontSize = targetFontSize;  // 確保最終字體大小達到目標

            // 縮小動畫
            elapsedTime = 0f;
            while (elapsedTime < animationTime) {
                textMeshPro.fontSize = Mathf.Lerp(targetFontSize, initialFontSize, elapsedTime / animationTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}
