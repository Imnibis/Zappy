using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerManager : MonoBehaviour
{
    public Camera camera;
    public Map map;
    public Chatbox chatbox;
    public GameObject playerPrefab;
    public GameObject incantationPrefab;
    public GameObject pickupHUDPrefab;
    public Dictionary<int, Player> players = new Dictionary<int, Player>();
    public Dictionary<Vector2, List<Incantation>> incantations = new Dictionary<Vector2, List<Incantation>>();

    GameManager gameManager;
    ResourceManager resourceManager;

    private void Start()
    {
        gameManager = GetComponent<GameManager>();
        resourceManager = GetComponent<ResourceManager>();
    }

    public bool HandlePlayerPacketError(string[] args, string packetName, int argNb, bool variableArgs = false)
    {
        if (!variableArgs && args.Length != argNb) {
            Debug.LogError(packetName + ": command must have " + argNb +
                " arg" + (argNb != 1 ? "s" : "") + ", not " + args.Length);
            return false;
        } else if (variableArgs && args.Length < argNb) {
            Debug.LogError(packetName + ": command must have at least " + argNb +
                " arg" + (argNb != 1 ? "s" : "") + ", not " + args.Length);
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
        playerObject.transform.rotation = Map.OrientationToQuaternion(orientation);
        Player player = playerObject.GetComponent<Player>();
        player.id = int.Parse(args[0]);
        player.level = level;
        player.team = gameManager.teams[args[5]];
        player.gameManager = gameManager;
        player.playerManager = this;
        player.mapPosition = mapPos;
        player.SetColor(player.team.color);
        player.SetWorldCamera(camera);
        players.Add(player.id, player);
        chatbox.SendGameMessage(player.ToString() + " joined the game");
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
        player.transform.rotation = Map.OrientationToQuaternion(orientation);
        player.mapPosition = mapPos;
    }

    public void HandlePlayerLevelPacket(string[] args)
    {
        if (!HandlePlayerPacketError(args, "HandlePlayerLevelPacket", 2))
            return;
        Player player = players[int.Parse(args[0])];
        player.level = int.Parse(args[1]);
        chatbox.SendGameMessage(player.ToString() + " reached level " + player.level + " !");
    }

    public void HandlePlayerInventoryPacket(string[] args)
    {
        if (!HandlePlayerPacketError(args, "HandlePlayerInventoryPacket", 10))
            return;
        Player player = players[int.Parse(args[0])];
        int[] inventory = new int[7];
        for (int i = 3; i < args.Length; i++)
            inventory[i - 3] = int.Parse(args[i]);
        player.inventory = inventory;
    }

    public void HandlePlayerExpulsionPacket(string[] args)
    {
        if (!HandlePlayerPacketError(args, "HandlePlayerExpulsionPacket", 1))
            return;
        int id = int.Parse(args[0]);
        Player player = players[id];
        chatbox.SendServerMessage(player.ToString() + " got kicked out");
        //RemovePlayer(id);
    }

    public void HandlePlayerBroadcastPacket(string[] args)
    {
        if (args.Length < 2) {
            Debug.LogError("HandlePlayerBroadcastPacket: command must have at least 2 args, not " + args.Length);
            return;
        }
        Player player;
        if (!players.TryGetValue(int.Parse(args[0]), out player)) {
            Debug.LogError("HandlePlayerBroadcastPacket: invalid player");
            return;
        }
        string message = "";
        for (int i = 1; i < args.Length; i++) {
            if (i != 1) message += "";
            message += args[i];
        }
        player.Say(message);
    }

    public void HandlePlayerIncantationCreatePacket(string[] args)
    {
        if (!HandlePlayerIncantationCreatePacketError(args)) return;
        GameObject incantationObject = Instantiate(incantationPrefab);
        Vector2 mapPos = new Vector2(int.Parse(args[0]), int.Parse(args[1]));
        incantationObject.transform.position = map.MapToWorldPos(mapPos, 0);
        Incantation inc = incantationObject.GetComponent<Incantation>();
        inc.level = int.Parse(args[2]);
        inc.initiatingPlayer = players[int.Parse(args[3])];
        string broadcast = inc.initiatingPlayer.ToString() + " began a level " +
            inc.level + " incantation";
        for (int i = 4; i < args.Length; i++) {
            if (i != 4 && i == args.Length - 1)
                broadcast += "and ";
            else if (i != 4)
                broadcast += ", ";
            else broadcast += " with ";
            Player player = players[int.Parse(args[i])];
            broadcast += player.ToString();
            inc.involvedPlayers.Add(player);
        }
        broadcast += ".";
        chatbox.SendGameMessage(broadcast);
        if (incantations.ContainsKey(mapPos))
            incantations[mapPos].Add(inc);
        else incantations.Add(mapPos, new List<Incantation>() { inc });
    }

    public bool HandlePlayerIncantationCreatePacketError(string[] args)
    {
        if (args.Length < 4) {
            Debug.LogError("HandlePlayerIncantationCreatePacket: command must have at least 4 args, not " + args.Length);
            return false;
        }
        if (!PacketManager.ArgsAreInt(args)) {
            Debug.LogError("HandlePlayerIncantationCreatePacket: all args must be ints");
            return false;
        }
        for (int i = 4; i < args.Length; i++) {
            Player player;
            if (!players.TryGetValue(int.Parse(args[i]), out player)) {
                Debug.LogError("HandlePlayerIncantationCreatePacket: invalid player #" + args[i]);
                return false;
            }
        }
        return true;
    }

    public void HandlePlayerIncantationEndPacket(string[] args)
    {
        if (args.Length != 3) {
            Debug.LogError("HandlePlayerIncantationEndPacket: command must have 3 args, not " + args.Length);
            return;
        }
        if (!PacketManager.ArgsAreInt(args)) {
            Debug.LogError("HandlePlayerIncantationEndPacket: all args must be ints");
            return;
        }
        Vector2 mapPos = new Vector2(int.Parse(args[0]), int.Parse(args[1]));
        bool success = args[2] == "1";
        if (!incantations.ContainsKey(mapPos)) {
            Debug.LogError("HandlePlayerIncantationEndPacket: no incantation at this position");
            return;
        }
        List<Incantation> posIncantations = incantations[mapPos];
        Incantation incantation = posIncantations[0];
        string broadcast = incantation.initiatingPlayer.ToString() + "'s incantation ";
        if (success) broadcast += "was a triumph!";
        else broadcast += "failed miserably";
        chatbox.SendGameMessage(broadcast);
        incantations[mapPos].RemoveAt(0);
        if (incantations[mapPos].Count == 0)
            incantations.Remove(mapPos);
        Destroy(incantation.gameObject);
    }

    public void HandlePlayerLayEggPacket(string[] args)
    {
        if (!HandlePlayerPacketError(args, "HandlePlayerLayEggPacket", 1))
            return;
        Player player = players[int.Parse(args[0])];
        player.LayEgg();
        chatbox.SendGameMessage(player.ToString() + " laid an egg!");
    }

    public void HandlePlayerDropResourcePacket(string[] args)
    {
        if (!HandlePlayerPacketError(args, "HandlePlayerDropResourcePacket", 2))
            return;
        Player player = players[int.Parse(args[0])];
        int resourceID = int.Parse(args[1]);
        Vector2 mapPos = player.mapPosition;
        if (player.inventory[resourceID] == 0) return;
        PickupHUD pickupHUD = player.GetPickupHUD();
        pickupHUD.quantity = -1;
        pickupHUD.resourceName = resourceManager.resources[resourceID];
        pickupHUD.Display();
        map.GetOre(mapPos).AddResource(resourceID, 1);
        player.inventory[resourceID] -= 1;
    }

    public void HandlePlayerGatherResourcePacket(string[] args)
    {
        if (!HandlePlayerPacketError(args, "HandlePlayerGatherResourcePacket", 2))
            return;
        Player player = players[int.Parse(args[0])];
        int resourceID = int.Parse(args[1]);
        Vector2 mapPos = player.mapPosition;
        Ore ore = map.GetOre(mapPos);
        int quantity = ore.GetResourceQuantity(resourceID);
        if (quantity == 0) return;
        PickupHUD pickupHUD = player.GetPickupHUD();
        pickupHUD.quantity = 1;
        pickupHUD.resourceName = resourceManager.resources[resourceID];
        pickupHUD.Display();
        player.inventory[resourceID] += 1;
        ore.AddResource(resourceID, -1);
    }

    public void HandlePlayerDeathPacket(string[] args)
    {
        if (!HandlePlayerPacketError(args, "HandlePlayerDeathPacket", 1))
            return;
        int id = int.Parse(args[0]);
        Player player = players[id];
        chatbox.SendGameMessage(player.ToString() + " died");
        RemovePlayer(id);
    }

    void RemovePlayer(int id)
    {
        Player player = players[id];
        Destroy(player.gameObject);
        players.Remove(id);
    }

    void Update()
    {
        
    }
}
