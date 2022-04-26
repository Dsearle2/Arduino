using Shapes;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[HideMonoScript]
public class VehicleLight : MonoBehaviour {

    [SerializeField] private LightUI lightUI;

    [SerializeField] private Light2D light;

    [TitleGroup("Intensity"), SerializeField] private float maxIntensity = 1f;
    
    [SerializeField, HideInInspector] private float intensity = 1f;
    [TitleGroup("Intensity"), PropertyRange(0, 1f), ShowInInspector]
    public float Intensity {
        get { return intensity; }
        set {
            intensity = Mathf.Clamp01(value);
            light.intensity = intensity * maxIntensity;
            lightUI.SetIntensity(intensity);
        }
    }
    public float IntensityDelta {
        set {
            Intensity += value;
        }
    }
    

    [TitleGroup("Arc"), SerializeField, MinMaxSlider(0, 360f, ShowFields = true)] private Vector2 arcRange = new Vector2(60f, 150f);
    [TitleGroup("Arc"), SerializeField] private float arcEdge = 1.5f;
    
    [SerializeField, HideInInspector] private float arc = 0f;
    [TitleGroup("Arc"), PropertyRange(0, 1f), ShowInInspector] 
    public float Arc {
        get { return arc; }
        set {
            arc = Mathf.Clamp01(value);

            float arcAngle = Mathf.Lerp(arcRange.x, arcRange.y, arc);
            light.pointLightInnerAngle = arcAngle;
            light.pointLightOuterAngle = arcAngle * arcEdge;

            lightUI.SetArc(arcAngle * arcEdge);
        }
    }
    public float ArcDelta {
        set { Arc += value; }
    }


    [TitleGroup("Radius"), SerializeField, PropertyRange(0, 10f)] private float maxRadius = 5f;

    [SerializeField, HideInInspector] private float radius = 1f;
    [TitleGroup("Radius"), PropertyRange(0, 1f), ShowInInspector]
    public float Radius {
        get { return radius; }
        set {
            radius = Mathf.Clamp01(value);
            light.pointLightOuterRadius = radius * maxRadius;
            lightUI.SetRadius(radius);
        }
    }
    public float RadiusDelta {
        set { Radius += value; }
    }


    [TitleGroup("Rotation"), SerializeField] private bool invertRotation;
    [TitleGroup("Rotation"), SerializeField, MinMaxSlider(-180f, 180f, ShowFields = true)] private Vector2 rotationRange = new Vector2(-180f, 180f);
    
    [SerializeField, HideInInspector] private float rotation = 0.5f;
    [TitleGroup("Rotation"), PropertyRange(0, 1f), ShowInInspector]
    public float Rotation {
        get { return rotation; }
        set {
            rotation = Mathf.Clamp01(value);
            Quaternion localRot = Quaternion.Euler(0, 0, (invertRotation ? -1 : 1) - Mathf.Lerp(rotationRange.x, rotationRange.y, rotation));
            light.transform.localRotation = localRot;
            lightUI.SetRotation(localRot);
        }
    }
    public float RotationDelta {
        set {
            Rotation += value;
        }
    }


}