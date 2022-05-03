using Shapes;
using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;
using System;

[HideMonoScript]
public class ScannerUI : MonoBehaviour {

    [SerializeField] private SpriteRenderer staticDisplay, dynamicDisplay;
    [SerializeField] private Line frequencyBar, amplitudeBar;

    [SerializeField, MinMaxSlider(0f, 12f, ShowFields = true)] private Vector2 frequencyRange;
    [SerializeField, MinMaxSlider(0f, 6f, ShowFields = true)] private Vector2 amplitudeRange;

    [SerializeField] private ContentIndicator[] contentLabels;

    public void UpdateDynamicWave(Vector2 wave) {
        Vector4 tiling = new Vector4(Mathf.Lerp(frequencyRange.y, frequencyRange.x, wave.x), Mathf.Lerp(amplitudeRange.y, amplitudeRange.x, wave.y));
        dynamicDisplay.sharedMaterial.SetVector("_Tiling", tiling);

        frequencyBar.End = Vector3.right * wave.x;
        amplitudeBar.End = Vector3.up * wave.y;
    }

    public void UpdateStaticWave(Vector2 wave) {
        Vector4 tiling = new Vector4(Mathf.Lerp(frequencyRange.y, frequencyRange.x, wave.x), Mathf.Lerp(amplitudeRange.y, amplitudeRange.x, wave.y));
        staticDisplay.sharedMaterial.SetVector("_Tiling", tiling);
    }

    public void UpdateTarget(ScannerTarget target) {
        
    }

    [Serializable]
    private struct ContentIndicator {
        
        public TMP_Text label;
        public Line bar;

        public void UpdateIndicator(ScannerTarget.Content content) {
            label.text = content.resource.ToString();
            bar.End = Vector3.up * content.quantity;
        }

    }

}