using UnityEngine;
using Sirenix.OdinInspector;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class VehicleController : MonoBehaviour {

    [SerializeField] private float baseMass, baseDrag;
    
    [SerializeField] private float springStrength, springDamper;
    [SerializeField, Range(-90f, 90f)] private float targetRot;

    [PropertySpace(10)]
    [SerializeField, HideInInspector] private EngineMode _engineMode;
    [ShowInInspector] private EngineMode engineMode {
        get { return _engineMode; }
        set {
            _engineMode = value;
            switch (_engineMode) {
                case EngineMode.Off:
                    rigidbody.mass = baseMass;
                    rigidbody.drag = baseDrag;
                    rigidbody.gravityScale = 1f;
                    break;
                case EngineMode.Fuel:
                    rigidbody.mass = baseMass * massGenerator.value;
                    rigidbody.drag = baseDrag;
                    rigidbody.gravityScale = 1f;
                    break;
                case EngineMode.Electric:
                    rigidbody.mass = baseMass;
                    rigidbody.drag = baseDrag * decelerator.value;
                    rigidbody.gravityScale = 1f - antiGravity.value;
                    break;
                default:
                    break;
            }
        }
    }
    [SerializeField, MinMaxSlider(0, 10)] private Vector2 engineVibration;

    [TitleGroup("Fuel Engine")]
    [SerializeField] private VehicleFeature fuelThrust;
    [SerializeField] private VehicleFeature stabilizer;
    [SerializeField] private VehicleFeature massGenerator;

    [TitleGroup("Electric Engine", HorizontalLine = true)]
    [SerializeField] private VehicleFeature electricThrust;
    [SerializeField] private VehicleFeature antiGravity;
    [SerializeField] private VehicleFeature decelerator;

    [SerializeField, HideInInspector] private SkidMode _skidMode;
    [ShowInInspector, TitleGroup("Skids", HorizontalLine = true)] public SkidMode skidMode {
        get {
            return _skidMode;
        }
        set {
            _skidMode = value;
            SkidSettings modeSetting = modeSettings[(int)_skidMode];
            foreach (Skid skid in skids) skid.SetMode(modeSetting);
        }
    }
    [SerializeField, TitleGroup("Skids")] private SkidSettings[] modeSettings;
    [SerializeField, TitleGroup("Skids")] private Skid[] skids;

    private Rigidbody2D rigidbody;

    private void Awake() {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        skidMode = _skidMode;
    }

    private void Update() {
        
    }

    private void FixedUpdate() {
        if (engineMode != EngineMode.Off) {
            ApplyUprightForce();
            ApplyEngineForce();
        }
    }

    private void ApplyUprightForce() {
        float toGoal = Mathf.DeltaAngle(-targetRot, rigidbody.rotation);

        float rotRadians = -toGoal * Mathf.Deg2Rad;

        rigidbody.AddTorque((rotRadians * springStrength) - (rigidbody.angularVelocity * springDamper));
    }
    private void ApplyEngineForce() {
        switch (engineMode) {
            case EngineMode.Fuel:
                rigidbody.mass = baseMass * massGenerator.value;
                //rigidbody.AddForce((SerialManager.JoyValues * fuelThrust.value) - Physics2D.gravity);
                rigidbody.AddForce((UnityEngine.Random.insideUnitCircle * UnityEngine.Random.Range(engineVibration.x, engineVibration.y) * Time.fixedDeltaTime) / stabilizer.value, ForceMode2D.Impulse);
                return;
            case EngineMode.Electric:
                rigidbody.drag = baseDrag * decelerator.value;
                rigidbody.gravityScale = 1f - antiGravity.value;
                //rigidbody.AddForce(SerialManager.JoyValues * electricThrust.value);
                return;
            default:
                return;
        }
    }


    private enum EngineMode {
        Off,
        Fuel,
        Electric
    }

    public enum SkidMode {
        Default,
        Slick,
        Grip
    }


    [Serializable]
    private struct Vector6 {
        
        [SerializeField, HorizontalGroup, HideLabel]
        private float val0, val1, val2, val3, val4, val5;

        public float this[int index] {
            get {
                switch (index) {
                    default:
                    case 0: return val0;
                    case 1: return val1;
                    case 2: return val2;
                    case 3: return val3;
                    case 4: return val4;
                    case 5: return val5;
                }
            }
        }

    }

    [Serializable]
    private struct Vector5 {

        [SerializeField, HorizontalGroup, HideLabel]
        private float val0, val1, val2, val3, val4;

        public float this[int index] {
            get {
                switch (index) {
                    default:
                    case 0: return 0;
                    case 1: return val0;
                    case 2: return val1;
                    case 3: return val2;
                    case 4: return val3;
                    case 5: return val4;
                }
            }
        }

    }

    [Serializable]
    private struct VehicleFeature {

        [PropertyRange(0, 5)] public int index;
        
        [SerializeField] private Vector6 values;
        [SerializeField] private Vector5 soundSignatures, heatSignatures, electricSignatures;

        [ShowInInspector, HorizontalGroup("Value"), LabelWidth(9)] public float value => values[index];
        [ShowInInspector, HorizontalGroup("Value"), LabelWidth(9)] public float sound => soundSignatures[index];
        [ShowInInspector, HorizontalGroup("Value"), LabelWidth(9)] public float heat => heatSignatures[index];
        [ShowInInspector, HorizontalGroup("Value"), LabelWidth(9)] public float electric => electricSignatures[index];

    }

    [Serializable]
    public struct SkidSettings {

        public Color color;
        public PhysicsMaterial2D material;
        public bool gripEnabled;

    }

}