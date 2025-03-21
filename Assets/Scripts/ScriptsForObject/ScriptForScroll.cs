using UnityEngine;
using UnityEngine.UI;
public class ScriptForScroll : MonoBehaviour {
    public ScrollRect scrollRect;
    public float scrollSensitivity = 1f;  // 調整滾動速度
    void Update() {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput != 0f) {
            scrollRect.verticalNormalizedPosition += scrollInput * scrollSensitivity;
            scrollRect.verticalNormalizedPosition = Mathf.Clamp(scrollRect.verticalNormalizedPosition, 0f, 1f);
        }
    }
}
