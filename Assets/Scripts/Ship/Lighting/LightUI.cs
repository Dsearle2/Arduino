using Shapes;
using Sirenix.OdinInspector;
using UnityEngine;

public class LightUI : MonoBehaviour {

    [SerializeField] private Disc indicator;
    [SerializeField, MinMaxSlider(0, 1f, ShowFields = true)] private Vector2 indicatorAlphaRange = new Vector2(0.1235f, 0.8f);

    public void SetIntensity(float intensity) {
        Color indicatorColor = indicator.Color;
        indicatorColor.a = Mathf.Lerp(indicatorAlphaRange.x, indicatorAlphaRange.y, intensity);
        indicator.Color = indicatorColor;
    }

    public void SetArc(float angle) {
        indicator.AngRadiansStart = -angle / 2f * Mathf.Deg2Rad;
        indicator.AngRadiansEnd = angle / 2f * Mathf.Deg2Rad;
    }

    public void SetRadius(float radius) {
        indicator.Radius = radius;
    }


    public void SetRotation(Quaternion rot) {
        indicator.transform.localRotation = rot;
    }

}