using UnityEngine;
using UnityEngine.UI;
public class ScriptForScroll : MonoBehaviour {
    public ScrollRect scrollRect;
    public float scrollSensitivity = 1f;  // �վ�u���F�ӫ�
    void Update() {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput != 0f) {
            scrollRect.verticalNormalizedPosition += scrollInput * scrollSensitivity;
            scrollRect.verticalNormalizedPosition = Mathf.Clamp(scrollRect.verticalNormalizedPosition, 0f, 1f);
        }
    }
}
