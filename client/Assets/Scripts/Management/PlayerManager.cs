using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerManager : MonoBehaviour
{
    public Map map;
    public GameObject playerPrefab;
    public Dictionary<int, Player> players = new Dictionary<int, Player>();

    GameManager gameManager;

    private void Start()
    {
        gameManager = GetComponent<GameManager>();
    }

    public bool HandlePlayerPacketError(string[] args, string packetName, int argNb)
    {
        if (args.Length != argNb) {
            Debug.LogError(packetName + ": command must have " + argNb +
                " arg" + (argNb != 1 ? "s" : "") + ", not" + args.Length);
            return false;
        }
        if (!PacketManager.ArgsAreInt(args)) {
            Debug.LogError(packetName + ": all args must be ints");
            return false;
        }
        Player player;
        if (!players.TryGetValue(int.Parse(args[0]), out player)) {
            Debug.LogError(packetName + ": invalid player");
            return false;
        }
        return true;
    }

    public void HandleNewPlayerPacket(string[] args)
    {
        if (!HandleNewPlayerPacketErrors(args))
            return;
        Vector2 mapPos = new Vector2(int.Parse(args[1]), int.Parse(args[2]));
        Orientation orientation = (Orientation) int.Parse(args[3]);
        int level = int.Parse(args[4]);
        GameObject playerObject = Instantiate(playerPrefab);
        playerObject.transform.position = map.MapToWorldPos(mapPos, 0);
        playerObject.transform.rotation = map.OrientationToQuaternion(orientation);
        Player player = playerObject.GetComponent<Player>();
        player.id = int.Parse(args[0]);
        player.level = level;
        player.team = gameManager.teams[args[5]];
        player.SetColor(player.team.color);
        players.Add(player.id, player);
    }

    public bool HandleNewPlayerPacketErrors(string[] args)
    {
        if (args.Length != 6) {
            Debug.LogError("HandleNewPlayerPacket: command must have 6 args, not " + args.Length);
            return false;
        }
        List<string> intArgs = new List<string>();
        for (int i = 0; i < args.Length - 1; i++)
            intArgs.Add(args[i]);
        if (!PacketManager.ArgsAreInt(intArgs.ToArray())) {
            Debug.LogError("HandleNewPlayerPacket: the first 5 arguments must be ints");
            return false;
        }
        Team team;
        if (!gameManager.teams.TryGetValue(args[5], out team)) {
            Debug.LogError("HandleNewPlayerPacket: invalid team");
            return false;
        }
        return true;
    }

    public void HandlePlayerPositionPacket(string[] args)
    {
        if (!HandlePlayerPacketError(args, "HandlePlayerPositionPacket", 4))
            return;
        Player player = players[int.Parse(args[0])];
        Vector2 mapPos = new Vector2(int.Parse(args[1]), int.Parse(args[2]));
        Orientation orientation = (Orientation) int.Parse(args[3]);
        player.transform.position = map.MapToWorldPos(mapPos, 0);
        player.transform.rotation = map.OrientationToQuaternion(orientation);
    }

    public void HandlePlayerLevelPacket(string[] args)
    {
        if (!HandlePlayerPacketError(args, "HandlePlayerLevelPacket", 2))
            return;
        Player player = players[int.Parse(args[0])];
        player.level = int.Parse(args[1]);
    }

    public void HandlePlayerDeathPacket(string[] args)
    {
        if (!HandlePlayerPacketError(args, "HandlePlayerDeathPacket", 1))
            return;
        Player player = players[int.Parse(args[0])];
        Destroy(player.gameObject);
        players.Remove(int.Parse(args[0]));
    }

    public void HandlePlayerInventoryPacket(string[] args)
    {
        if (!HandlePlayerPacketError(args, "HandlePlayerInventoryPacket", 10))
            return;
        Player player = players[int.Parse(args[0])];
        // TODO: continue inventory management
    }

    void Update()
    {
        
    }
}
