using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalogInput : ArduinoInput {

    [SerializeField, PropertyRange(0f, 1f)] public float deadZone;
    private float raw;
    private float value, prevValue;

    [ShowInInspector]
    public float Raw {
        get { return raw; }
        set {
            raw = value;
            prevValue = this.value;
            this.value = Mathf.Sign(raw) * Mathf.InverseLerp(deadZone, 1f, Mathf.Abs(raw));
            if (this.value != prevValue) OnChangedInvoke();
        }
    }

    [ShowInInspector]
    public float Value {
        get { return value; }
    }

    [ShowInInspector]
    public float Delta {
        get { return value - prevValue; }
    }

    public override void Parse(ref int index, string[] inputs) {
        if (float.TryParse(inputs[index++], out float analogVal)) {
            Raw = analogVal;
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