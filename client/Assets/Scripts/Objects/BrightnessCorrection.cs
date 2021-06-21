using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Renderer))]
public class BrightnessCorrection : MonoBehaviour
{
    public Material material;
    [Range(-255, 255)] public float brightness;
    public bool refresh = false;

    void Start()
    {
        CreateNewMaterial();
    }

    void Update()
    {
        if (!Application.isPlaying && refresh) {
            refresh = false;
            CreateNewMaterial();
        }
    }

    void CreateNewMaterial()
    {
        Material newMaterial = new Material(material);
        Texture2D texture = (Texture2D) material.mainTexture;
        Texture2D newTexture = new Texture2D(texture.width, texture.height, texture.format, false);
        Color[] colors = texture.GetPixels();

        for (int i = 0; i < colors.Length; i++) {
            float newR = Mathf.Clamp01(((colors[i].r * 255) + brightness) / 255);
            float newG = Mathf.Clamp01(((colors[i].g * 255) + brightness) / 255);
            float newB = Mathf.Clamp01(((colors[i].b * 255) + brightness) / 255);
            colors[i] = new Color(newR, newG, newB, colors[i].a);
        }
        newTexture.SetPixels(colors);
        newTexture.Apply();
        newMaterial.mainTexture = newTexture;
        GetComponent<Renderer>().material = newMaterial;
    }
}
