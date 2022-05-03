using UnityEngine;
using Sirenix.OdinInspector;

[HideMonoScript]
public class InstrumentController : SerializedMonoBehaviour {

    [SerializeField] private ArduinoAction[] inputEvents = new ArduinoAction[0];

    private void OnEnable() {
        foreach (ArduinoAction inputEvent in inputEvents) {
            inputEvent.Enable();
        }
    }

    private void OnDisable() {
        foreach (ArduinoAction inputEvent in inputEvents) {
            inputEvent.Disable();
        }
    }

}