using Shapes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineUI : MonoBehaviour {

    [SerializeField] private Transform marker;
    [SerializeField] private Line thrustBar, turnBar, stabBar;

    public void UpdatePanel(Vector2 position, float thrust, float turn, float stab) {
        marker.position = position;
        thrustBar.End = Vector2.right * 2f * thrust;
        turnBar.End = Vector2.right * 2f * turn;
        stabBar.End = Vector2.right * 2f * stab;
    }

}