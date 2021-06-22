using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player info")]
    public int id;
    public int level;
    public Team team;
    public int[] inventory;

    [Header("Settings")]
    public int HDRIntensity = 2;
    public Light light;
    public MeshRenderer renderer;

    private void Awake()
    {
        inventory = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
    }

    public void SetColor(Color color)
    {
        light.color = color;
        Material mat = renderer.material;
        Material newMat = new Material(mat);
        newMat.color = color;
        newMat.SetColor("_EmissionColor", color * HDRIntensity);
        renderer.material = newMat;
    }
}