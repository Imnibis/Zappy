using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggManager : MonoBehaviour
{
    public Map map;
    public Chatbox chatbox;
    public GameObject eggPrefab;
    public Dictionary<int, Egg> eggs = new Dictionary<int, Egg>();

    PlayerManager playerManager;

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    public void HandleNewEggPacket(string[] args)
    {
        if (!HandleNewEggPacketErrors(args))
            return;
        int id = int.Parse(args[0]);
        Player player = playerManager.players[int.Parse(args[1])];
        Vector2 mapPos = new Vector2(int.Parse(args[2]), int.Parse(args[3]));
        GameObject eggObject = Instantiate(eggPrefab);
        eggObject.transform.position = map.MapToWorldPos(mapPos, 0);
        Egg egg = eggObject.GetComponent<Egg>();
        egg.layingPlayer = player;
        player.SetEgg(egg);
        eggs.Add(id, egg);
    }

    bool HandleNewEggPacketErrors(string[] args)
    {
        if (args.Length != 4) {
            Debug.LogError("HandleNewEggPacketErrors: command must have 4 args, not " + args.Length);
            return false;
        }
        if (!PacketManager.ArgsAreInt(args)) {
            Debug.LogError("HandleNewEggPacketErrors: the all arguments must be ints");
            return false;
        }
        Player player;
        if (!playerManager.players.TryGetValue(int.Parse(args[1]), out player)) {
            Debug.LogError("HandleNewEggPacketErrors: invalid player");
            return false;
        }
        return true;
    }

    bool HandleEggPacketError(string[] args, string packetName)
    {
        if (args.Length != 1) {
            Debug.LogError(packetName + ": command must have 1 arg, not " + args.Length);
            return false;
        }
        if (!PacketManager.ArgsAreInt(args)) {
            Debug.LogError(packetName + ": the all arguments must be ints");
            return false;
        }
        Egg egg;
        if (!eggs.TryGetValue(int.Parse(args[0]), out egg)) {
            Debug.LogError(packetName + ": invalid egg");
            return false;
        }
        return true;
    }

    public void HandleEggHatchingPacket(string[] args)
    {
        if (!HandleEggPacketError(args, "HandleEggHatchingPacket"))
            return;
        Egg egg = eggs[int.Parse(args[0])];
        egg.Hatch();
        chatbox.SendGameMessage(egg.layingPlayer.ToString() + "'s egg is hatching!");
    }

    public void HandleEggPlayerConnectionPacket(string[] args)
    {
        if (!HandleEggPacketError(args, "HandleEggPlayerConnectionPacket"))
            return;
        Egg egg = eggs[int.Parse(args[0])];
        egg.StopHatching();
    }

    public void HandleEggDeathPacket(string[] args)
    {
        if (!HandleEggPacketError(args, "HandleEggDeathPacket"))
            return;
        int id = int.Parse(args[0]);
        Egg egg = eggs[id];
        eggs.Remove(id);
        Destroy(egg.gameObject);
    }
}