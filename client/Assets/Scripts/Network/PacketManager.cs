using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PacketCallback
{
    public string packet;
    public UnityEvent<string[]> callback;
}

public class PacketManager : MonoBehaviour
{
    public UnityEvent<string> packetSendCallback;
    public List<PacketCallback> packetCallbacks = new List<PacketCallback>();
    public List<string> packetRecieveQueue = new List<string>();

    public void Update()
    {
        lock (packetRecieveQueue) {
            foreach (string command in packetRecieveQueue) {
                InvokeCallback(command);
            }
            packetRecieveQueue.Clear();
        }
    }

    public static bool ArgsAreInt(string[] args)
    {
        int dummy;

        foreach (string arg in args) {
            if (!int.TryParse(arg, out dummy))
                return false;
        }
        return true;
    }

    public bool PacketHasCallback(string command)
    {
        int spaceIndex = command.IndexOf(' ');
        string packet;
        if (spaceIndex != -1)
            packet = command.Substring(0, spaceIndex);
        else
            packet = command;
        return packetCallbacks.Exists(x => x.packet.Equals(packet));
    }

    public void InvokeCallback(string command)
    {
        int spaceIndex = command.IndexOf(' ');
        string packet;
        string[] args;
        if (spaceIndex != -1) {
            packet = command.Substring(0, spaceIndex);
            if (spaceIndex < command.Length - 1) {
                args = command.Substring(spaceIndex + 1,
                    command.Length - (spaceIndex + 1)).Split(' ');
            }
            else args = new string[0];
        }
        else {
            packet = command;
            args = new string[0];
        }
        InvokeCallback(packet, args);
    }

    public void InvokeCallback(string packet, string[] args)
    {
        lock (packetCallbacks) {
            foreach (PacketCallback callback in packetCallbacks) {
                if (callback.packet.Equals(packet)) {
                    callback.callback.Invoke(args);
                }
            }
        }
    }

    public void AddRecievedPacketToQueue(string command)
    {
        lock (packetRecieveQueue) {
            packetRecieveQueue.Add(command);
        }
    }

    public void SendPacket(string packet, string[] args)
    {
        string fullCommand;
        if (args.Length == 0) fullCommand = packet;
        else fullCommand = packet + " " + string.Join(" ", args);
        SendPacket(fullCommand);
    }

    public void SendPacket(string fullCommand)
    {
        fullCommand += '\n';
        packetSendCallback.Invoke(fullCommand);
    }
}
