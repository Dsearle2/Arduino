using Sirenix.OdinInspector;

public class ButtonInput : ArduinoInput {

    private int raw, prevRaw;

    [ShowInInspector]
    public int Raw {
        get { return raw; }
        set {
            prevRaw = raw;
            raw = value;
            if (Delta) OnChangedInvoke();
        }
    }

    [ShowInInspector]
    public bool Value {
        get { return Raw > 0; }
    }

    [ShowInInspector]
    public bool Delta {
        get { return raw - prevRaw > 0; }
    }

    public override void Parse(ref int index, string[] inputs) {
        if (int.TryParse(inputs[index++], out int buttonState)) {
            Raw = buttonState;
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