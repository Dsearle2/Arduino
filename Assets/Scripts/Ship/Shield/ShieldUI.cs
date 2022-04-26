using Shapes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldUI : MonoBehaviour {

    [SerializeField] private Disc indicator, limit;
    [SerializeField] private float padding = 10f;

    public void SetArc(Vector2 arc) {
        indicator.AngRadiansStart = (arc.x + padding) * Mathf.Deg2Rad;
        indicator.AngRadiansEnd = (arc.y - padding) * Mathf.Deg2Rad;
    }

    public void SetIndicator(float health, float distribution) {
        limit.Radius = distribution * 3.5f;
        indicator.Radius = (health / distribution) * (limit.Radius - 0.15f) - health / 2f;
        indicator.Thickness = health;

        //indicator.Thickness = Mathf.Min(distribution, health);
        //indicator.Radius = health / distribution * (3 + 0.25f - distribution / 2f);
    }

}