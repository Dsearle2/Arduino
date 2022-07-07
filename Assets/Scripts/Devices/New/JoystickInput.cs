using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class JoystickInput : ArduinoInput {

    [SerializeField, HideInInspector, DisableContextMenu] internal AnalogInput joyX, joyY;
    
    [ShowInInspector, InlineEditor]
    private AnalogInput JoyX {
        get {
            if (joyX == null) {
                joyX = CreateInstance<AnalogInput>();
                joyX.name = "X";
            }
            return joyX;
        }
        set { }
    }

    [ShowInInspector, InlineEditor]
    private AnalogInput JoyY {
        get {
            if (joyY == null) {
                joyY = CreateInstance<AnalogInput>();
                joyY.name = "Y";
            }
            return joyY;
        }
        set { }
    }

    private Vector2 prevValue;

    public Vector2 Raw {
        get { return new Vector2(JoyY.Raw, JoyY.Raw); }
        set {
            prevValue = Value;
            JoyX.Raw = value.x;
            JoyY.Raw = value.y;
            if (Value != prevValue) OnChangedInvoke();
        }
    }

    public Vector2 Value {
        get { return new Vector2(JoyX.Value, JoyY.Value); }
    }

    public Vector2 Delta {
        get { return Value - prevValue; }
    }

    public override void Parse(ref int index, string[] inputs) {
        if (float.TryParse(inputs[index++], NumberStyles.Float, CultureInfo.InvariantCulture,  out float joyX) &&
            float.TryParse(inputs[index++], NumberStyles.Float, CultureInfo.InvariantCulture, out float joyY)) {
            Raw = new Vector2(joyX, joyY);
        }
    }

    public override T GetRaw<T>() {
        return TryCast<T>(Raw);
    }
    public override T GetValue<T>() {
        return TryCast<T>(Value);
    }
    public override T GetDelta<T>() {
        return TryCast<T>(Delta);
    }

    public override IEnumerable<ArduinoInput> Children {
        get {
            yield return JoyX;
            yield return JoyY;
        }
    }

}