using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Shapes;

[HideMonoScript]
public class EngineController : MonoBehaviour {

    private static readonly Quaternion ROTATION_RIGHT = Quaternion.Euler(0, 0, 120), ROTATION_LEFT = Quaternion.Euler(0, 0, -120);

    [SerializeField] private EngineUI engineUI;

    [SerializeField, HideInInspector] private float rangeX, rangeY;
    [PropertyRange(0f, 3f), ShowInInspector] private float Range {
        get { return rangeY; }
        set {
            rangeY = value;
            rangeX = Mathf.Sqrt(3 * rangeY * rangeY / 4);
        }
    }

    [SerializeField, HideInInspector] private float horizontalPos;
    [ShowInInspector, PropertyRange("@-horizontalRange", "@horizontalRange")]
    public float HorizontalPos {
        get { return horizontalPos; }
        set {
            if (value > horizontalPos) {
                if (value > horizontalRange) {
                    Vector2 diff = Quaternion.Euler(0, 0, 60f) * Vector2.right * (value - horizontalRange);
                    verticalPos = Mathf.Clamp(verticalPos - diff.y, -rangeY / 2f, rangeY);
                    horizontalPos = horizontalRange;
                } else horizontalPos = value;
            } else {
                if (value < -horizontalRange) {
                    Vector2 diff = Quaternion.Euler(0, 0, -60f) * -Vector2.right * (-value - horizontalRange);
                    verticalPos = Mathf.Clamp(verticalPos - diff.y, -rangeY / 2f, rangeY);
                    horizontalPos = -horizontalRange;
                } else horizontalPos = value;
            }

            engineUI?.UpdatePanel(Position, Thrust, Turn, Stab);
        }
    }
    public float HorizontalPosDelta {
        set { HorizontalPos += value; }
    }

    [SerializeField, HideInInspector] private float verticalPos;
    [ShowInInspector, PropertyRange("@-rangeY/2f", "@verticalRange")]
    public float VerticalPos {
        get { return verticalPos; }
        set {
            if (value > verticalPos) {
                if (value > verticalRange) {
                    Vector2 diff = Quaternion.Euler(0, 0, Mathf.Sign(-horizontalPos) * 60f) * Vector2.up * (value - verticalRange);
                    verticalPos = Mathf.Clamp(verticalPos + diff.y, -rangeY / 2f, rangeY);
                    horizontalPos = Mathf.Sign(horizontalPos) * horizontalRange;
                } else verticalPos = value;
            } else verticalPos = Mathf.Max(value, -rangeY / 2f);

            engineUI?.UpdatePanel(Position, Thrust, Turn, Stab);
        }
    }
    public float VerticalPosDelta {
        set { VerticalPos += value; }
    }

    private float horizontalRange => rangeX * Mathf.InverseLerp(rangeY, -rangeY / 2f, verticalPos);
    private float verticalRange => (-rangeY / 2f) + (rangeY * 1.5f * Mathf.InverseLerp(rangeX, 0, Mathf.Abs(horizontalPos)));

    private Vector2 Position => new Vector2(horizontalPos, verticalPos);

    [ShowInInspector] public float Thrust {
        get {
            return Mathf.InverseLerp(-rangeY / 2f, rangeY, (ROTATION_RIGHT * Position).y);
        }
    }
    [ShowInInspector] public float Turn {
        get {
            return Mathf.InverseLerp(-rangeY / 2f, rangeY, (ROTATION_LEFT * Position).y);
        }
    }
    [ShowInInspector] public float Stab {
        get {
            return Mathf.InverseLerp(-rangeY / 2f, rangeY, verticalPos);
        }
    }

    //private void OnDrawGizmos() {
    //    Vector3 posC = new Vector3(0, rangeY);
    //    Vector3 posB = Quaternion.Euler(0, 0, -120) * posC;
    //    Vector3 posA = Quaternion.Euler(0, 0, 120) * posC;

    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(posA, posB);
    //    Gizmos.DrawLine(posB, posC);
    //    Gizmos.DrawLine(posC, posA);
    //}

}