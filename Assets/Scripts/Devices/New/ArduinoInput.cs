using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class ArduinoInput : ScriptableObject {

    protected void OnChangedInvoke() => OnValueChanged?.Invoke(this);
    public event Action<ArduinoInput> OnValueChanged;

    public abstract void Parse(ref int index, string[] inputs);

    public abstract T GetRaw<T>();
    public abstract T GetValue<T>();
    public abstract T GetDelta<T>();

    public virtual IEnumerable<ArduinoInput> Children {
        get {
            yield break;
        }
    }

    public static T TryCast<T>(object data) {
        if (data is T) return (T)data;
        try {
            return (T)Convert.ChangeType(data, typeof(T));
        } catch (InvalidCastException) {
            return default(T);
        }
    }

}