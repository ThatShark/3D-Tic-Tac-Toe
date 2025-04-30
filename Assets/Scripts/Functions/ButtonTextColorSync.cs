using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonTextColorSync : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler {
    private TMP_Text targetText;
    private Button button;

    private bool isPointerOver = false;
    private bool isPointerDown = false;

    void Awake() {
        button = GetComponent<Button>();
        targetText = GetComponentInChildren<TMP_Text>();
        UpdateTextColor(GetCurrentColor());
    }

    public void OnPointerEnter(PointerEventData eventData) {
        isPointerOver = true;
        UpdateTextColor(GetCurrentColor());
    }

    public void OnPointerExit(PointerEventData eventData) {
        isPointerOver = false;
        UpdateTextColor(GetCurrentColor());
    }

    public void OnPointerDown(PointerEventData eventData) {
        isPointerDown = true;
        UpdateTextColor(GetCurrentColor());
    }

    public void OnPointerUp(PointerEventData eventData) {
        UpdateTextColor(GetCurrentColor());
        isPointerDown = false;
    }

    public void OnSelect(BaseEventData eventData) {
        UpdateTextColor(GetCurrentColor());
    }

    public void OnDeselect(BaseEventData eventData) {
        UpdateTextColor(GetCurrentColor());
    }

    private void UpdateTextColor(Color color) {
        targetText.color = color;
    }

    private Color GetCurrentColor() {
        if (button == null)
            return Color.white;

        ColorBlock cb = button.colors;

        if (!button.IsInteractable())
            return cb.disabledColor;

        if (isPointerDown)
            return cb.pressedColor;

        if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == gameObject)
            return cb.selectedColor;

        if (isPointerOver)
            return cb.highlightedColor;

        return cb.normalColor;
    }

    
}