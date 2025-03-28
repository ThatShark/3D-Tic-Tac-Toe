using UnityEngine;

public class RotateAroundPoint : MonoBehaviour {
    public float rotationSpeed = 50f; // degree per seccond
    public Vector3 centerPoint = Vector3.zero;

    void Update() {
        transform.RotateAround(centerPoint, Vector3.up, rotationSpeed * Time.deltaTime);
    }
}