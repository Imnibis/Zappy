using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class Chatbox : MonoBehaviour
{
    public Color gameMessageColor;
    public Color serverMessageColor;
    
    TextMeshProUGUI tmp;
    Color defaultColor;

    void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        tmp.text = "";
        defaultColor = tmp.color;
    }

    void AddLine(string line)
    {
        tmp.text += "\n" + line;
    }

    public string SetColor(Color color, string message)
    {
        return "<color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" + message
            + "</color>";
    }

    string SetSender(string sender, Color color)
    {
        return SetColor(color, sender) + ": ";
    }

    public void SendGameMessage(string message)
    {
        AddLine(SetColor(gameMessageColor, message));
    }

    public void SendServerMessage(string message)
    {
        AddLine(SetSender("Server", serverMessageColor) + message);
    }

    public void PlayerSay(Player player, string message)
    {
        AddLine(SetSender(player.ToString(), player.team.color) + message);
    }
}
