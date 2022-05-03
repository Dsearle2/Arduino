using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[InlineProperty, HideReferenceObjectPicker]
public class ArduinoAction {

    [SerializeField, ValueDropdown("@SerialManager.Inputs", HideChildProperties = true), HideLabel, HorizontalGroup("Target")] private ArduinoInput input;
    [SerializeField, TypeFilter("@ArduinoUtils.GetTypes<IArduinoGet>()"), HorizontalGroup("Target")] private IArduinoGet get;
    [SerializeField, TypeFilter("@ArduinoUtils.GetTypes<IArduinoCallback>()")] private IArduinoCallback callback;

    public void Enable() {
        if (input != null) input.OnValueChanged += Invoke;
    }
    public void Disable() {
        if (input != null) input.OnValueChanged -= Invoke;
    }

    private void Invoke(ArduinoInput input) {
        if (input != null) callback.Invoke(input, get);
    }

}