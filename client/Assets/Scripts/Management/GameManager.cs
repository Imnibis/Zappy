using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Color> teamColors = new List<Color>();
    public Dictionary<string, Team> teams = new Dictionary<string, Team>();
    int timeUnit = 100;

    public void HandleTimeUnitPacket(string[] args)
    {
        int t;
        if (args.Length != 1) {
            Debug.LogError("HandleTimeUnitPacket: command must have 1 arg, not " + args.Length);
            return;
        }
        if (!int.TryParse(args[0], out t)) {
            Debug.LogError("HandleTimeUnitPacket: the time unit must be an int");
            return;
        }
        Debug.Log("Setting time unit to " + t);
        timeUnit = t;
    }

    public void HandleTeamNamePacket(string[] args)
    {
        Color color;

        if (args.Length != 1) {
            Debug.LogError("HandleTeamNamePacket: command must have 1 arg, not " + args.Length);
            return;
        }
        Debug.Log("Adding team " + args[0]);
        if (teams.Count < teamColors.Count)
            color = teamColors[teams.Count];
        else {
            int red = Random.Range(0, 256);
            int green = Random.Range(0, 256);
            int blue = Random.Range(0, 256);
            color = new Color(red, green, blue);
        }
        teams.Add(args[0], new Team(args[0], color));
    }
}