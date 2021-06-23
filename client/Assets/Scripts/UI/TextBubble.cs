using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextBubble : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float disappearDelay = 3f;
    
    public void ShowText(string message)
    {
        text.text = message;
        gameObject.SetActive(true);
        StartCoroutine(DisappearAfterDelay());
    }

    IEnumerator DisappearAfterDelay()
    {
        yield return new WaitForSecondsRealtime(disappearDelay);
        gameObject.SetActive(false);
    }
}
