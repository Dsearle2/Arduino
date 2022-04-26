using UnityEngine;
using Shapes;

public class RadarUI : MonoBehaviour {

    [SerializeField] private Disc pulse;
    private Color pulseColor;

    [SerializeField] private Transform indicatorTransform;
    [SerializeField] private Disc innerDisc, outerDisc;
    [SerializeField] private Line indicatorLeft, indicatorRight;

    [SerializeField] private ParticleSystem particleSystem;
    private ParticleSystemRenderer particleRenderer;
    private ParticleSystem.EmitParams emitParams;

    private void Awake() {
        pulseColor = pulse.ColorOuter;
        particleRenderer = particleSystem.GetComponent<ParticleSystemRenderer>();
    }

    private void OnEnable() {
        particleRenderer.enabled = true;
    }
    private void OnDisable() {
        particleRenderer.enabled = false;
    }

    public void SetPulseRadius(float radius, float fade) {
        pulse.Radius = radius;

        pulseColor.a = fade;
        pulse.ColorOuter = pulseColor;
    }
    public void SetPulseAngle(float angle, float angleRange) {
        pulse.AngRadiansStart = (angle - angleRange / 2f) * Mathf.Deg2Rad;
        pulse.AngRadiansEnd = (angle + angleRange / 2f) * Mathf.Deg2Rad;
    }

    public void SetIndicatorAngle(float angle) {
        indicatorTransform.rotation = Quaternion.Euler(0, 0, angle);
    }
    public void SetIndicatorRange(float radius, float angle) {
        float angleRadius = Mathf.Deg2Rad * angle / 2f;

        outerDisc.Radius = radius;
        
        outerDisc.AngRadiansStart = innerDisc.AngRadiansStart = angleRadius;
        outerDisc.AngRadiansEnd = innerDisc.AngRadiansEnd = -angleRadius;

        indicatorLeft.transform.localRotation = Quaternion.Euler(0, 0, -angle / 2f);
        indicatorRight.transform.localRotation = Quaternion.Euler(0, 0, angle / 2f);
    }

    public void CreatePing(Vector3 position) {
        emitParams.position = position;
        particleSystem.Emit(emitParams, 1);
    }


}