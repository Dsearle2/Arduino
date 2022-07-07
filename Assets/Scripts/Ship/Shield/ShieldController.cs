using UnityEngine;
using Shapes;
using Sirenix.OdinInspector;
using System;

public class ShieldController : MonoBehaviour {
   
    [SerializeField] private Shield[] shields;

    [SerializeField] private float recoveryRate = 1f;
    [SerializeField] internal float maxHealth = 10f;

    private void Awake() {
        for (int i = 0; i < shields.Length; i++) {
            shields[i].Health = 1f;
        }
    }

    private void FixedUpdate() {
        float recovery = (recoveryRate / maxHealth) * Time.fixedDeltaTime;
        for (int i = 0; i < shields.Length; i++) shields[i].Health += recovery;
    }

    [SerializeField, HideInInspector] private int selector;
    [PropertyRange(0, "@shields.Length - 1"), ShowInInspector]
    public int Selector {
        get { return selector; }
        set {
            selector = (value + shields.Length) % shields.Length;
        }
    }
    public int SelectorDelta {
        set { Selector += value; }
    }

    [PropertyRange(0f, 1f), ShowInInspector]
    public float Distribution {
        get { return selector < shields.Length ? shields[selector].Distribution : 0; }
        set {
            if (value < shields[selector].Health) {
                float charge = shields[selector].Health - value;

                int fullShieldCount = 0;
                while (charge > Mathf.Epsilon && fullShieldCount < shields.Length - 1) {
                    float perShieldCharge = charge / (shields.Length - 1 - fullShieldCount);
                    for (int i = 0; i < shields.Length; i++) {
                        if (i == selector || Mathf.Approximately(shields[i].Health, shields[i].Distribution)) {
                            fullShieldCount++;
                            continue;
                        }
                            
                        float deltaCharge = Mathf.Min(shields[i].Distribution - shields[i].Health, perShieldCharge);
                        shields[i].Health += deltaCharge;
                        charge -= deltaCharge;
                        if (Mathf.Approximately(shields[i].Health, shields[i].Distribution)) fullShieldCount++;
                    }
                }
            }

            shields[selector].Distribution = value;
        }
    }
    public float DistributionDelta {
        set { Distribution += value; }
    }

    //[Button] private void ApplyDamage(int index, float damage) {
    //    damage /= maxHealth;
    //    if (index % 2 == 0) {
    //        HealthA -= damage;
    //    } else {
    //        HealthB -= damage;
    //    }
    //}

    [Serializable]
    public class Shield {

        [SerializeField] private ShieldUI shieldUI;
        [SerializeField] private PointEffector2D effector;

        [SerializeField, HideInInspector] private Vector2 arc;
        [MinMaxSlider(-360f, 360f, ShowFields = true), ShowInInspector]  public Vector2 Arc {
            get { return arc; }
            set {
                arc = value;
                shieldUI?.SetArc(arc);
            }
        }

        [SerializeField, HideInInspector] private float distribution;
        [PropertyRange(0f, 1f), ShowInInspector] public float Distribution {
            get { return distribution; }
            set {
                distribution = Mathf.Clamp01(value);
                Health = Health;
                shieldUI?.SetIndicator(health, distribution);
            }
        }

        private float health = 1f;
        [ShowInInspector] public float Health {
            get { return health; }
            set {
                health = Mathf.Clamp(value, 0f, distribution);
                //disc.Radius = health / valA * (3f + 0.25f - valA / 2f);
                effector.forceMagnitude = health * 2f;
                shieldUI?.SetIndicator(health, Distribution);
            }
        }

    }

}