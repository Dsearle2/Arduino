using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Arm : MonoBehaviour {

    [SerializeField] private Transform targetTransform;
    [SerializeField] private float speed = 10f;

    public Vector2 moveDir;

    public Vector2 MoveDir {
        get => moveDir;
        set => moveDir = value;
    }

    public float MoveX { 
        get { return moveDir.x; } 
        set { moveDir.x = value; }
    }
    public float MoveY {
        get { return moveDir.y; }
        set { moveDir.y = value; }
    }

    void Update() {
        Vector2 position = (Vector2)targetTransform.localPosition + moveDir * speed * Time.deltaTime;
        position = Vector2.ClampMagnitude(position, 3f);
        targetTransform.localPosition = position;
    }
}
