using Shapes;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[HideMonoScript]
public class ReactorUI : MonoBehaviour {

    [TitleGroup("Inlet"), SerializeField] private Rectangle[] inlets;

    [TitleGroup("Fan"), SerializeField] private Transform fanTransform;
    [TitleGroup("Fan"), SerializeField] private Disc fanSpeedIndicator;
    [TitleGroup("Fan"), SerializeField, MinMaxSlider(0f, 30f, ShowFields = true)] private Vector2 speedRange;
    [TitleGroup("Fan"), SerializeField, ShowInInspector] private float timeStep = 0.1f;
    [SerializeField, HideInInspector] private float angleStep;

    [SerializeField] private Rectangle heatIndicator, powerIndicator, fuelIndicator;


    private void Awake() {
        StartCoroutine(RotateFan());

        IEnumerator RotateFan() {
            while (enabled) {
                fanTransform.Rotate(Vector3.forward, angleStep);
                yield return new WaitForSeconds(timeStep);
            }
        }
    }

    public void SetActiveInlets(int activeInlets) {
        for (int i = 0; i < 8; i++) inlets[i].Type = i < activeInlets ? Rectangle.RectangleType.HardSolid : Rectangle.RectangleType.HardBorder;
    }

    public void SetFanSpeedIndicator(float fanSpeed) {
        fanSpeedIndicator.AngRadiansEnd = fanSpeed * 2 * Mathf.PI;
        angleStep = Mathf.Lerp(speedRange.x, speedRange.y, fanSpeed);
    }

    public void SetHeatIndicator(float heat) {
        heatIndicator.Width = heat;
    }
    public void SetPowerIndicator(float power) {
        powerIndicator.Width = power;
    }
    public void SetFuelIndicator(float fuel) {
        fuelIndicator.Width = fuel;
    }

}