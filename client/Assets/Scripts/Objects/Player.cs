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
    public PlayerManager playerManager;
    public Light light;
    public MeshRenderer renderer;
    public Canvas canvas;
    public TextBubble textBubble;

    public override string ToString()
    {
        return "<color=#" + ColorUtility.ToHtmlStringRGB(team.color) +
            ">Player #" + id + "</color>";
    }

    private void Awake()
    {
        inventory = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
    }

    public void Say(string message)
    {
        textBubble.ShowText(message);
        playerManager.chatbox.PlayerSay(this, message);
    }

    public void SetWorldCamera(Camera camera)
    {
        canvas.worldCamera = camera;
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