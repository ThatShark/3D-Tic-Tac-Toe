using UnityEngine;
using UnityEngine.SceneManagement;
public class ScriptForCursor : MonoBehaviour {

    public Texture2D standard, o, x, triangle;
    public enum CursorState {
        O,
        X,
        Triangle,
        Default
    }
    public CursorState cursorState;

    void Start() {
        Cursor.visible = true;
        cursorState = CursorState.Default;
        DontDestroyOnLoad(gameObject);
    }

    void Update() {
        switch (cursorState) {
            case CursorState.O:
                Cursor.SetCursor(o, Vector2.zero, CursorMode.Auto);
                break;
            case CursorState.X:
                Cursor.SetCursor(x, Vector2.zero, CursorMode.Auto);
                break;
            case CursorState.Triangle:
                Cursor.SetCursor(triangle, Vector2.zero, CursorMode.Auto);
                break;
            default:
                Cursor.SetCursor(standard, Vector2.zero, CursorMode.Auto);
                break;
        }
    }
}
