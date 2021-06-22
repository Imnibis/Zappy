using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Team
{
    public string name;
    public Color color;

    public Team(string name, Color color)
    {
        this.name = name;
        this.color = color;
    }
}
