using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstrumentController : MonoBehaviour {

    [SerializeField] private ArduinoEventSystem[] instruments;

    private ArduinoEventSystem eventSystem;

    private void Awake() {
        eventSystem = GetComponent<ArduinoEventSystem>();
    }

    private int selectedInstrument;
    public int Selection {
        get { return selectedInstrument; }
        set {
            int newInstrumentIndex = (value + instruments.Length) % instruments.Length;
            instruments[selectedInstrument].gameObject.SetActive(false);
            selectedInstrument = newInstrumentIndex;
            instruments[selectedInstrument].gameObject.SetActive(true);
        }
    }
    public int SelectionDelta {
        set { Selection += value; }
    }
     
    public void SetSelectedInstrumentActive(bool active) {
        eventSystem.enabled = !active;
        instruments[selectedInstrument].enabled = active;
    }

}
