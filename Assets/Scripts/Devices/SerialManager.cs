using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using Uduino;
using UnityEngine;

[HideMonoScript]
public class SerialManager : SerializedMonoBehaviour {

    private static SerialManager instance;
    public static SerialManager Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<SerialManager>();
            }
            return instance;
        }
    }

    [SerializeField] private Input[] inputs;

    private bool initialized;

    private void Awake() {
        instance = this;
    }
    private void Start() {
        UduinoManager.Instance.OnDataReceived += HandleData;
    }
    private void Update() {
        if (!UduinoManager.Instance.isConnected() && UduinoManager.Instance.ManagerState != UduinoManagerState.Discovering) UduinoManager.Instance.DiscoverPorts();
    }
    
    [Button] public static void SetLEDValue(int ledIndex, byte value) {
        UduinoManager.Instance.sendCommand("SetLED", ledIndex, value);
    }
    [Button]
    public static void SetLEDValues(byte r, byte g, byte b) {
        UduinoManager.Instance.sendCommand("SetLEDs", r, g, b);
    }

    private void HandleData(string data, UduinoDevice board) {
        if (!initialized) {
            initialized = true;
            return;
        }

        string[] inputValues = data.Split(',');
        int inputIndex = 0;
        for (int i = 0; i < inputs.Length; i++) {
            try {
                inputs[i].Object.Parse(ref inputIndex, inputValues);
            } catch (IndexOutOfRangeException) {
                Debug.LogWarning("Incorrect number of values received from Arduino");
                break;
            }
        }
    }

    private static IList<ValueDropdownItem<ArduinoInput>> Inputs {
        get {
            ValueDropdownList<ArduinoInput> list = new ValueDropdownList<ArduinoInput>();
            foreach (Input input in Instance.inputs) AddInput(input.Object);
            return list;

            void AddInput(ArduinoInput input, string path = "") {
                string newPath = path + input.name;
                list.Add(new ValueDropdownItem<ArduinoInput>(newPath, input));
                foreach (ArduinoInput childInput in input.Children) AddInput(childInput, newPath + "/");
            }
        }
    }

}

[Serializable, HideReferenceObjectPicker]
public abstract class Input {

    public abstract ArduinoInput Object { get; }
    
    public abstract class Base<T> : Input where T : ArduinoInput {

        [SerializeField, HideInInspector] protected T input;
        [ShowInInspector, InlineEditor, LabelText("@GetType().Name"), CustomContextMenu("Rename", "@Name = \"\"")] private T Input {
            get { return input; }
            set { }
        }

        public override ArduinoInput Object => Input;
        
        public Base() => input = ScriptableObject.CreateInstance<T>();

        [ShowInInspector, ShowIf("@Name == \"\""), DelayedProperty, PropertyOrder(-1)]
        private string Name {
            get { return input.name; }
            set { input.name = value; }
        }

    }

    public class Button : Base<ButtonInput> { }
    public class ButtonMask : Base<ButtonMaskInput> { }
    public class Analog : Base<AnalogInput> { }
    public class Joystick : Base<JoystickInput> { }
    public class Encoder : Base<EncoderInput> { }

}