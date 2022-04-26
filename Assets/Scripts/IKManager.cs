using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKManager : MonoBehaviour {

    public Transform root, end;
    public Transform target;

    public float threshhold = 0.05f;
    public float rate = 5.0f;
    public int steps = 20;

    private float CalculateSlope(Transform joint) {
        float deltaTheta = 0.01f;
        float distance1 = Vector2.Distance(end.position, target.position);

        joint.Rotate(Vector3.forward, deltaTheta);

        float distance2 = Vector2.Distance(end.position, target.position);

        joint.Rotate(Vector3.forward, -deltaTheta);

        return (distance2 - distance1) / deltaTheta;
    }

    private void Update() {
        for (int i = 0; i < steps; i++) {
            if (Vector3.Distance(end.position, target.position) > threshhold) {
                Transform current = root;
                while (current != null) {
                    float slope = CalculateSlope(current);
                    current.Rotate(Vector3.forward, -slope * rate * Time.deltaTime);
                    current = current.childCount > 0 ? current.GetChild(0) : null;
                }
            }
        }
    }
}
