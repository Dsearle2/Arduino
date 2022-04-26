using Sirenix.OdinInspector;
using System;
using Uduino;
using UnityEngine;
using UnityEngine.Events;

public class SerialManager : MonoBehaviour {

    private static SerialManager instance;

    private Vector2 joyValues;
    public static Vector2 JoyValues => instance?.joyValues ?? Vector2.zero;

    [SerializeField] private Encoder[] encoders;
    public static int EncoderCount => instance?.encoders.Length ?? 0;
    public static Encoder GetEncoder(int index) => instance?.encoders[index];
    public static Encoder[] Encoders => instance?.encoders?? null;

    [SerializeField] private int buttonCount;
    private int buttonStates;
    private static bool GetButtonState(int index) => instance ? (instance.buttonStates & (1 << index)) > 0 : false;

    private event Action<int> onButtonDown;
    public static event Action<int> OnButtonDown {
        add { if (instance) instance.onButtonDown += value; }
        remove { if (instance) instance.onButtonDown -= value; }
    }

    private event Action<int, Encoder> onEncoderChanged;
    public static event Action<int, Encoder> OnEncoderChanged {
        add { if (instance) instance.onEncoderChanged += value; }
        remove { if (instance) instance.onEncoderChanged -= value; }
    }

    private bool initialized;

    private void Awake() {
        instance = this;
    }
    private void Start() {
        UduinoManager.Instance.OnDataReceived += HandleData;
    }

    [Button]
    public void SetLEDValue(int ledIndex, byte value) {
        UduinoManager.Instance.sendCommand("SetLED", ledIndex, value);
    }

    private void HandleData(string data, UduinoDevice board) {
        if (!initialized) {
            initialized = true;
            return;
        }

        string[] values = data.Split(',');
        if (values.Length == 5) {
            if (int.TryParse(values[0], out int buttonStates)) {
                this.buttonStates = buttonStates;
                for (int i = 0; i < buttonCount; i++) if (GetButtonState(i)) onButtonDown?.Invoke(i);
            }

            if (float.TryParse(values[1], out float joyX) && float.TryParse(values[2], out float joyY)) {
                joyValues = new Vector2(joyX, joyY);
            }

            for (int i = 3; i < values.Length; i++) {
                if (int.TryParse(values[i], out int encoderValue)) {
                    encoders[i].RawValue = encoderValue;
                    if (encoders[i].Delta > 0) onEncoderChanged?.Invoke(i, encoders[i]);
                }
            }
        }
    }

}