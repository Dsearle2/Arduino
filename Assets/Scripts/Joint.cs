using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Joint : MonoBehaviour {

    public Joint child;
    
    public void Rotate(float angle) {
        transform.Rotate(Vector3.forward, angle);
    }
    
    private void Awake() {
        
    }

    private void Update() {
        
    }

}
