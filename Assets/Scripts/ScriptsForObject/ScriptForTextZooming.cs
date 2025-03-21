using UnityEngine;
using System.Collections;
using TMPro;
public class TextScaleAnimation : MonoBehaviour {
    public TextMeshProUGUI textMeshPro; // ��r����
    public float targetFontSize = 50f;  // �̤j�r��j�p
    public float animationTime = 0.5f;  // �ʵe����ɶ�
    private float initialFontSize;      // ��l�r��j�p

    void Start() {
        initialFontSize = textMeshPro.fontSize; // �O����l�r��j�p
        StartCoroutine(AnimateFontSizeLoop());  // �}�l�L���`���ʵe
    }

    // Coroutine �Ψӹ�{�r��j�p�L���`���ʵe
    IEnumerator AnimateFontSizeLoop() {
        while (true) {
            // ��j�ʵe
            float elapsedTime = 0f;
            while (elapsedTime < animationTime) {
                textMeshPro.fontSize = Mathf.Lerp(initialFontSize, targetFontSize, elapsedTime / animationTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            textMeshPro.fontSize = targetFontSize;  // �T�O�̲צr��j�p�F��ؼ�

            // �Y�p�ʵe
            elapsedTime = 0f;
            while (elapsedTime < animationTime) {
                textMeshPro.fontSize = Mathf.Lerp(targetFontSize, initialFontSize, elapsedTime / animationTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}
