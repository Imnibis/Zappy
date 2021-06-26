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
    public Vector2 mapPosition;

    [Header("Settings")]
    public int HDRIntensity = 2;
    public PlayerManager playerManager;
    public GameManager gameManager;
    public Animator animator;
    public Light light;
    public MeshRenderer renderer;
    public Canvas canvas;
    public TextBubble textBubble;
    public PlayerEggLayAnimation eggLayAnimation;
    List<PickupHUD> pickupHUDPool = new List<PickupHUD>();

    public override string ToString()
    {
        return "<color=#" + ColorUtility.ToHtmlStringRGB(team.color) +
            ">Player #" + id + "</color>";
    }

    private void Awake()
    {
        inventory = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
    }

    public PickupHUD GetPickupHUD()
    {
        foreach (PickupHUD hud in pickupHUDPool)
            if (!hud.displayed)
                return hud;
        GameObject pickupHUDObject = Instantiate(playerManager.pickupHUDPrefab, canvas.transform);
        PickupHUD pickupHUD = pickupHUDObject.GetComponent<PickupHUD>();
        pickupHUDPool.Add(pickupHUD);
        return pickupHUD;
    }

    public void Say(string message)
    {
        textBubble.ShowText(message);
        playerManager.chatbox.PlayerSay(this, message);
    }

    public void SetEgg(Egg egg)
    {
        eggLayAnimation.egg = egg;
        eggLayAnimation.CheckIfEggShouldAppear();
    }


    public void LayEgg()
    {
        eggLayAnimation.eggShouldAppear = false;
        animator.SetFloat("TimeUnit", gameManager.timeUnit);
        animator.SetBool("LayEgg", true);
        StartCoroutine(DeactivateAnimatorLayEggVar());
    }

    IEnumerator DeactivateAnimatorLayEggVar()
    {
        yield return 0;
        animator.SetBool("LayEgg", false);
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