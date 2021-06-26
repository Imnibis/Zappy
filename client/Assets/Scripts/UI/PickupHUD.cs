using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickupHUD : MonoBehaviour
{
    public int quantity;
    public string resourceName;
    public AnimationCurve animationCurve;
    public float from;
    public float to;
    public float duration;
    public bool displayed = false;

    RectTransform rt;
    TextMeshProUGUI tmp;
    float time = 0;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        tmp = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Display()
    {
        if (quantity == 0)
            return;
        displayed = true;
        time = 0;
        rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, from);
        tmp.enabled = true;
        tmp.text = "";
        tmp.text += quantity < 0 ? '-' : '+';
        tmp.text += quantity;
        tmp.text += " " + CapitalizeFirstLetter(resourceName);
    }

    string CapitalizeFirstLetter(string str)
    {
        if (str.Length == 0) return str;
        else if (str.Length == 1) return str.ToUpper();
        string newStr = "";
        newStr += char.ToUpper(str[0]);
        newStr += str.Substring(1).ToLower();
        return newStr;
    }

    private void Update()
    {
        if (time >= duration) {
            displayed = false;
            tmp.enabled = false;
            return;
        }
        float currentHeight = animationCurve.Evaluate(time / duration) * (to - from) + from;
        rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, currentHeight);
        time += Time.deltaTime;
    }
}
