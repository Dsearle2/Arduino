using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

[HideMonoScript]
public class RadarController : MonoBehaviour {

    [SerializeField] private RadarUI radarUI;

    [SerializeField, HideInInspector] private float angle;
    [PropertyRange(0f, 360f), ShowInInspector] 
    public float Angle {
        get { return angle; }
        set {
            angle = (value + 360f) % 360f;
            radarUI?.SetIndicatorAngle(angle);
        }
    }
    public float AngleDelta {
        set { Angle += value; }
    }

    [SerializeField, HideInInspector] private float arc;
    [PropertyRange(0f, 1f), ShowInInspector]
    public float Arc {
        get { return arc; }
        set {
            arc = Mathf.Clamp01(value);
            radarUI?.SetIndicatorRange(DetectionRadius / rangeMax, Mathf.Lerp(arcRange.x, arcRange.y, arc));
        }
    }
    public float ArcDelta {
        set { Arc += value; }
    }

    [SerializeField] private float rangeMax = 300f, rangeMin;
    [SerializeField, MinMaxSlider(0f, 360f, ShowFields = true)] private Vector2 arcRange;
    [SerializeField] private float fadeRange = 50f;
    [SerializeField] private float rangeSpeed = 300f;
    
    [SerializeField] private ContactFilter2D radarLayerMask;

    private float DetectionRadius => Mathf.Lerp(rangeMin, rangeMax, 1 - Arc);

    private float range;
    private float curAngle, curAngleRange, curDetectionRadius;

    private HashSet<Collider2D> pingedColliders;
    private List<RaycastHit2D> raycastHits;

    private void Awake() {      
        pingedColliders = new HashSet<Collider2D>();
        raycastHits = new List<RaycastHit2D>();
    }

    private void Update() {
        range += rangeSpeed * Time.deltaTime;
        if (range > 1.0f) {
            range = 0.1f;
            curAngle = -transform.rotation.eulerAngles.z + angle;
            curAngleRange = Mathf.Lerp(arcRange.x, arcRange.y, Arc);
            curDetectionRadius = DetectionRadius;
            pingedColliders.Clear();

            radarUI?.SetPulseAngle(curAngle, curAngleRange);
        }

        
        float worldRange = range * rangeMax;
        if (worldRange <= curDetectionRadius) {
            int hitCount = Physics2D.CircleCast(transform.position, range * rangeMax, Vector2.zero, radarLayerMask, raycastHits, 0f);
            for (int i = 0; i < hitCount; i++) {
                RaycastHit2D raycastHit = raycastHits[i];
                if (raycastHit.collider != null) {
                    if (Mathf.Abs(Mathf.DeltaAngle(curAngle, Vector2.SignedAngle(Vector2.right, raycastHit.point - (Vector2)transform.position))) > curAngleRange / 2f) continue;

                    if (!pingedColliders.Contains(raycastHit.collider)) {
                        pingedColliders.Add(raycastHit.collider);
                        radarUI?.CreatePing(transform.InverseTransformPoint(raycastHit.point).normalized * range);
                    }
                }
            }

            //if (worldRange > fadeRange * curDetectionRadius) pulseColor.a = Mathf.InverseLerp(curDetectionRadius, fadeRange * curDetectionRadius, worldRange);
            //else pulseColor.a = 1f;
            //pulse.ColorOuter = pulseColor;

            float fade = worldRange > fadeRange * curDetectionRadius ? Mathf.InverseLerp(curDetectionRadius, fadeRange * curDetectionRadius, worldRange) : 1f;
            radarUI?.SetPulseRadius(range, fade);
        }
    }

}