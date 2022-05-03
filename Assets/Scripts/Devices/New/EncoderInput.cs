using Sirenix.OdinInspector;
using UnityEngine;

public class EncoderInput : ArduinoInput {

    [SerializeField] private int origin;
    private int raw, prevRaw;

    [ShowInInspector]
    public int Raw {
        get { return raw; }
        set {
            prevRaw = raw;
            raw = value;
            if (prevRaw != raw) OnChangedInvoke();
        }
    }

    [ShowInInspector]
    public int Value => raw - origin;

    [ShowInInspector]
    public int Delta => raw - prevRaw;

    public void ResetOrigin() {
        origin = raw;
    }

    public override void Parse(ref int index, string[] inputs) {
        if (int.TryParse(inputs[index++], out int encoderVal)) {
            Raw = encoderVal;
        }
    }

    public override T GetValue<T>() {
        return TryCast<T>(Value);
    }
    public override T GetDelta<T>() {
        return TryCast<T>(Delta);
    }
    public override T GetRaw<T>() {
        return TryCast<T>(Raw);
    }
    
}