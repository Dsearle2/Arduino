using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ScannerTarget : MonoBehaviour {

    [SerializeField] private ResourceRate[] resourceRates;

    private Vector2 wave;
    private float resourceTotal;
    [ShowInInspector, InlineButton("Start"), PropertyOrder(1)] private List<Content> contents;

    internal float Total {
        get {
            float sum = resourceRates.Sum(x => x.spawnRate);
            return sum > 0 ? sum : 1;
        }
    }
    [ShowInInspector, ListDrawerSettings(ShowIndexLabels = true), PropertyRange(0f, 1f)] public List<float> Rates {
        get {
            return new List<float>(resourceRates.Select(x => x.spawnRate / Total));
        }
    }

    public void Start() {
        wave = (UnityEngine.Random.insideUnitCircle + Vector2.one) / 2;

        contents = new List<Content>(6);
        float totalContents = 0;

        while (contents.Count < UnityEngine.Random.Range(1, 7)) {
            float rng = UnityEngine.Random.Range(0f, 1f);
            int resourceIndex = -1, index = 0;
            foreach (float rate in Rates) {
                if (rng < rate) {
                    resourceIndex = index;
                    break;
                } else rng -= rate;
                index++;
            }

            if (contents.Any(x => x.resource == resourceRates[resourceIndex].resource)) continue;

            Content newContent = new Content(resourceRates[resourceIndex]);
            totalContents += newContent.quantity;
            contents.Add(newContent);
        }

        for (int i = 0; i < contents.Count; i++) {
            Content content = contents[i];
            content.quantity /= totalContents;
            contents[i] = content;
        }
        contents.Sort((x, y) => (int)Mathf.Sign(y.quantity - x.quantity));
        
        Bounds bounds = GetComponent<SpriteRenderer>().bounds;
        resourceTotal = bounds.size.x * bounds.size.y;
    }
    public bool WaveMatch(Vector2 wave) {
        return (this.wave - wave).magnitude < 0.1f;
    }

    public enum Resource {
        Organics,
        Minerals,
        Metallics,
        Alloys,
        Geologics,

        Hydrocil, // Converts to Fuel
        Pyracite, // Generates Heat
        Radiqite, // Generates Radiation
        Noxite, // Consumes Health
        Cryolite, // Consumes Heat
        Ligherite, // Generates Light
        Voltanite, // Generates Power

    }
    [Serializable] public struct Content {

        public Resource resource;        
        public float quantity;

        public Content(ResourceRate rate) {
            resource = rate.resource;
            quantity = rate.Quantity;
        }

    }
    [Serializable] public struct ResourceRate {

        [SerializeField, HorizontalGroup("Rate"), HideLabel] internal Resource resource;
        [SerializeField, HorizontalGroup("Rate"), HideLabel, PropertyRange(0f, 1f)] internal float spawnRate;
        [SerializeField, HideLabel, MinMaxSlider(0f, 1f, ShowFields = true)] private Vector2 quantityRange;

        public float Quantity {
            get { return UnityEngine.Random.Range(quantityRange.x, quantityRange.y); }
        }
        
    }

}