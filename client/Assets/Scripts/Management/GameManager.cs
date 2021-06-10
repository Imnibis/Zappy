using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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
}
