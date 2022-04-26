using Shapes;
using Sirenix.OdinInspector;
using UnityEngine;

public class VehicleShield : MonoBehaviour {

    [SerializeField] private Disc discA, discB;

    [SerializeField] private float recoveryRate = 1f, maxHealth = 10f;
    [SerializeField, HideInInspector] private float distribution;

    [PropertyRange(-1, 1f), ShowInInspector]
    public float Distribution {
        get { return distribution; }
        set {
            float diff = (value - distribution) / 2f;
            distribution = value;

            discA.Thickness = Mathf.Lerp(0f, 1f, valA);
            discB.Thickness = Mathf.Lerp(0f, 1f, valB);

            float healthChange = diff < 0 ? Mathf.Min(Mathf.Abs(diff), HealthA) : -Mathf.Min(Mathf.Abs(diff), HealthB);
            HealthA -= healthChange;
            HealthB += healthChange;
        }
    }
    public float DistributionDelta {
        set { Distribution += value; }
    }

    public float valA => (1 + distribution) * 0.5f;
    public float valB => (1 - distribution) * 0.5f;

    private float healthA = 1f;
    public float HealthA {
        get { return healthA; }
        set {
            healthA = Mathf.Clamp(value, 0f, valA);
            discA.Radius = healthA / valA * (3f + 0.25f - valA / 2f);
        }
    }

    private float healthB = 1f;
    public float HealthB {
        get { return healthB; }
        set {
            healthB = Mathf.Clamp(value, 0f, valB);
            discB.Radius = healthB / valB * (3f + 0.25f - valB / 2f);
        }
    }

    private void Update() {
        float recovery = recoveryRate * Time.deltaTime;
        HealthA += recovery * valA;
        HealthB += recovery * valB;
    }

    [Button]
    private void ApplyDamage(int index, float damage) {
        damage /= maxHealth;
        if (index % 2 == 0) {
            HealthA -= damage;
        } else {
            HealthB -= damage;
        }
    }

}