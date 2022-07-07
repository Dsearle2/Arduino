using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Arm : MonoBehaviour {

    [SerializeField] private Transform targetTransform;
    [SerializeField] private float speed = 10f;

    [SerializeField] private Camera gameCam;
    [SerializeField] private float pixelWidthOffset;

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
        //Vector2 targetPos = gameCam.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
        //targetTransform.position = targetPos;

        moveDir = Vector2.zero;
        if (UnityEngine.Input.GetKey(KeyCode.UpArrow)) moveDir += (Vector2)transform.up;
        if (UnityEngine.Input.GetKey(KeyCode.DownArrow)) moveDir -= (Vector2)transform.up;
        if (UnityEngine.Input.GetKey(KeyCode.RightArrow)) moveDir += (Vector2)transform.right;
        if (UnityEngine.Input.GetKey(KeyCode.LeftArrow)) moveDir -= (Vector2)transform.right;
        moveDir.Normalize();

        Vector2 position = (Vector2)targetTransform.localPosition + moveDir * speed * Time.deltaTime;
        position = Vector2.ClampMagnitude(position, 3f);
        targetTransform.localPosition = position;
    }
}
