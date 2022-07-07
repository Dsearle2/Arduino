using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[HideMonoScript]
public class ScannerController : MonoBehaviour {

    [SerializeField] private ScannerUI scannerUI;

    [SerializeField] private float scannerRange = 8;
    [ShowInInspector] private ScannerTarget target;

    [Button] private void Test() {
        target.Start();
        scannerUI.UpdateTarget(target);
    }

    [SerializeField, HideInInspector] private Vector2 wave;
    [ShowInInspector, PropertyRange(0f, 1f)]
    public float Frequency {
        get { return wave.x; }
        set {
            wave.x = Mathf.Clamp01(value);
            scannerUI?.UpdateDynamicWave(wave);
        }
    }
    public float FrequencyDelta {
        set { Frequency += value; }
    }

    [ShowInInspector, PropertyRange(0f, 1f)]
    public float Amplitude {
        get { return wave.y; }
        set {
            wave.y = Mathf.Clamp01(value);
            scannerUI?.UpdateDynamicWave(wave);
        }
    }
    public float AmplitudeDelta {
        set { Amplitude += value; }
    }

    private void Update() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.forward, scannerRange);
        if (hit && hit.rigidbody.TryGetComponent(out target)) {
            scannerUI.UpdateTarget(target);
        }
    }

}