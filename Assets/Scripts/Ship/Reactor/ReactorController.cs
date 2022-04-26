using Shapes;
using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;

public class ReactorController : MonoBehaviour {

    [SerializeField] private ReactorUI reactorUI;

    [TitleGroup("Inlet"), SerializeField] private AnimationCurve inletHeatCurve, inletPowerCurve, inletFuelCurve;

    [SerializeField, HideInInspector] private int activeInlets;
    [TitleGroup("Inlet"), PropertyRange(0, 8), ShowInInspector]
    public int ActiveInlets {
        get { return activeInlets; }
        set {
            activeInlets = Mathf.Clamp(value, 0, 8);
            reactorUI?.SetActiveInlets(activeInlets);
        }
    }
    public int ActiveInletsDelta {
        set { ActiveInlets += value; }
    }

    [TitleGroup("Fan"), SerializeField] private AnimationCurve fanCoolingCurve, fanEfficiencyCurve;

    [SerializeField, HideInInspector] private float fanSpeed;
    [TitleGroup("Fan"), ShowInInspector, PropertyRange(0f, 1f)]
    public float FanSpeed {
        get { return fanSpeed; }
        set {
            fanSpeed = Mathf.Clamp01(value);
            reactorUI?.SetFanSpeedIndicator(fanSpeed);
        }
    }
    public float FanSpeedDelta {
        set {
            fanSpeed += value;
        }
    }


    [TitleGroup("Values")]
    [SerializeField] private float maxHeat, maxPower, maxFuel;
    [SerializeField] private float heatGenerationBase = 1f, heatDissipationBase = 1f, powerGenerationBase = 1f, fuelBurnBase = 1f;

    private float heat = 0f;
    [ShowInInspector, PropertyRange(0, "@maxHeat")] public float Heat {
        get { return heat * maxHeat; }
        set { 
            heat = Mathf.Clamp01(value / maxHeat);
            reactorUI?.SetHeatIndicator(heat);
        }
    }

    private float power = 0f;
    [ShowInInspector, PropertyRange(0, "@maxPower")] public float Power {
        get { return power * maxPower; }
        set { 
            power = Mathf.Clamp01(value / maxPower);
            reactorUI?.SetPowerIndicator(power);
        }
    }
    
    private float fuel = 1f;
    [ShowInInspector, PropertyRange(0, "@maxFuel")] public float Fuel {
        get { return fuel * maxFuel; }
        set { 
            fuel = Mathf.Clamp01(value / maxFuel);
            reactorUI?.SetFuelIndicator(fuel);
        }
    }

    private void Update() {
        float inletRatio = (float) ActiveInlets / 8;

        Heat += (heatGenerationBase * inletHeatCurve.Evaluate(inletRatio) - heatDissipationBase * fanCoolingCurve.Evaluate(fanSpeed)) * Time.deltaTime;
        Power += powerGenerationBase * inletPowerCurve.Evaluate(inletRatio) * Time.deltaTime;
        Fuel -= fuelBurnBase * (inletFuelCurve.Evaluate(inletRatio) / fanEfficiencyCurve.Evaluate(FanSpeed)) * Time.deltaTime;
    }

}