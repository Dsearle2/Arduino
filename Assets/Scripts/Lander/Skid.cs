using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skid : MonoBehaviour {

    private SpriteRenderer renderer;
    private Collider2D collider;
    private HingeJoint2D joint;

    private void Awake() {
        renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();

        joint = collider.attachedRigidbody.gameObject.AddComponent<HingeJoint2D>();
        joint.enabled = false;
        joint.anchor = transform.localPosition;
        joint.enableCollision = true;
    }

    private void OnDisable() {
        joint.connectedBody = null;
        joint.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (enabled && !joint.enabled) {
            joint.connectedBody = collision.rigidbody;
            joint.enabled = true;
        }
    }

    public void SetMode(VehicleController.SkidSettings settings) {
        renderer.color = settings.color;
        collider.sharedMaterial = settings.material;
        enabled = settings.gripEnabled;
    }

}