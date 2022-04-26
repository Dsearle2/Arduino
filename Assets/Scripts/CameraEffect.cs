using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CameraEffect : MonoBehaviour {

    public Material material;

    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        if (material == null) Graphics.Blit(source, destination);
        else {
            Graphics.Blit(source, destination, material);
        }
    }

}