using UnityEngine;
public class ScriptForSpinningWord : MonoBehaviour {
    public float rotationSpeed = 50f; // degree per second

    void Update() {
        Quaternion rotation = Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0);
        transform.rotation = transform.rotation * rotation;
    }
}