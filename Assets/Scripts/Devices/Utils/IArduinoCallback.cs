using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Events;

[HideLabel]
public interface IArduinoCallback {

    public void Invoke(ArduinoInput input, IArduinoGet getter);

}

namespace ArduinoCallback {

    [Serializable]
    public class Int : IArduinoCallback {
        public void Invoke(ArduinoInput input, IArduinoGet getter) => onValueChanged.Invoke(getter.Get<int>(input));
        [SerializeField, HideReferenceObjectPicker, HideLabel] private UnityEvent onValueChanged = new UnityEvent();
        [Serializable] private class UnityEvent : UnityEvent<int> {}
    }

    [Serializable]
    public class Float : IArduinoCallback {
        public void Invoke(ArduinoInput input, IArduinoGet getter) => onValueChanged.Invoke(getter.Get<float>(input));
        [SerializeField, HideReferenceObjectPicker, HideLabel] private UnityEvent onValueChanged = new UnityEvent();
        [Serializable] private class UnityEvent : UnityEvent<float> { }
    }

    [Serializable]
    public class FloatMul : IArduinoCallback {

        [SerializeField] private float multiplier = 1f;

        public void Invoke(ArduinoInput input, IArduinoGet getter) => onValueChanged.Invoke(multiplier * getter.Get<float>(input));
        [SerializeField, HideReferenceObjectPicker, HideLabel] private UnityEvent onValueChanged = new UnityEvent();
        [Serializable] private class UnityEvent : UnityEvent<float> { }
    }

    [Serializable]
    public class Bool : IArduinoCallback {
        public void Invoke(ArduinoInput input, IArduinoGet getter) => onValueChanged.Invoke(getter.Get<bool>(input));
        [SerializeField, HideReferenceObjectPicker, HideLabel] private UnityEvent onValueChanged = new UnityEvent();
        [Serializable] private class UnityEvent : UnityEvent<bool> { }
    }

    [Serializable]
    public class Vector2 : IArduinoCallback {
        public void Invoke(ArduinoInput input, IArduinoGet getter) => onValueChanged.Invoke(getter.Get<UnityEngine.Vector2>(input));
        [SerializeField, HideReferenceObjectPicker, HideLabel] private UnityEvent onValueChanged = new UnityEvent();
        [Serializable] private class UnityEvent : UnityEvent<UnityEngine.Vector2> { }
    }

    [Serializable]
    public class Vector3 : IArduinoCallback {
        public void Invoke(ArduinoInput input, IArduinoGet getter) => onValueChanged.Invoke(getter.Get<UnityEngine.Vector3>(input));
        [SerializeField, HideReferenceObjectPicker, HideLabel] private UnityEvent onValueChanged = new UnityEvent();
        [Serializable] private class UnityEvent : UnityEvent<UnityEngine.Vector3> { }
    }
    
}