using Sirenix.OdinInspector;
using System;
using UnityEngine;

[HideMonoScript]
public class LightingController : MonoBehaviour {

    [SerializeField] private VehicleLight[] lights;
    [SerializeField] private LightState[] lightStates;

    [SerializeField, HideInInspector] private int setting;
    [PropertyRange(0, "@lightStates.Length - 1"), ShowInInspector]
    public int Setting {
        get { return setting; }
        set {
            setting = Mathf.Clamp(value, 0, lightStates.Length - 1);
            foreach (VehicleLight light in lights) {
                light.Intensity = lightStates[setting].intensity;
                light.Arc = lightStates[setting].arc;
                light.Radius = lightStates[setting].radius;
            }
        }
    }
    public int SettingDelta {
        set { Setting += value; }
    }

    private void Update() {
        
    }

    [Serializable]
    private struct LightState {
        
        [PropertyRange(0f, 1f)] public float intensity, arc, radius;
        public float powerUse;

    }
    
}