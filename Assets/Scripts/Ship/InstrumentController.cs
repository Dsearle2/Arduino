using System;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using System.Collections.Generic;

public class InstrumentController : SerializedMonoBehaviour {

    [SerializeField] private UnityEvent[] OnButton;
    [SerializeField] private Encoder.Event[] OnEncoder = new Encoder.Event[0];

    private void OnEnable() {
        SerialManager.OnButtonDown += OnButtonDown;
        SerialManager.OnEncoderChanged += OnEncoderChanged;
        //foreach (Encoder encoder in SerialManager.Encoders) encoder.ResetOrigin();
    }

    private void OnDisable() {
        SerialManager.OnButtonDown -= OnButtonDown;
        SerialManager.OnEncoderChanged -= OnEncoderChanged;
    }

    private void OnButtonDown(int index) {
        if (index < OnButton.Length) OnButton[index].Invoke();
    }

    private void OnEncoderChanged(int index, Encoder encoder) {
        if (index < OnEncoder.Length) OnEncoder[index].Invoke(encoder);
    }

}