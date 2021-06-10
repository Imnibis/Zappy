using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Map : MonoBehaviour
{
    public Camera camera;
    public Vector2Int size;
    public PacketManager packetManager;
    public ResourceManager resourceManager;

    MeshFilter meshFilter;
    MeshRenderer renderer;

    public void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        renderer = GetComponent<MeshRenderer>();
    }

    public void HandleMapSizePacket(string[] args)
    {
        Vector2Int size;
        if (args.Length != 2) {
            Debug.LogError("HandleMapSizePacket: command must have 2 args, not " + args.Length);
            return;
        }
        if (!packetManager.ArgsAreInt(args)) {
            Debug.LogError("HandleMapSizePacket: both args must be ints");
            return;
        }
        size = new Vector2Int(int.Parse(args[0]), int.Parse(args[1]));
        Debug.Log(string.Format("Setting map size to ({0}, {1})", size.x, size.y));
        SetMapSize(size);
    }

    public void HandleTileContentPacket(string[] args)
    {
        Vector2 pos = new Vector2();
        int i = 0;
        int tileResources = 0;
        if (!HandleTileContentPacketErrors(args)) return;
        foreach (string arg in args) {
            if (i == 0)
                pos.x = int.Parse(arg);
            else if (i == 1)
                pos.y = int.Parse(arg);
            else {
                int quantity = int.Parse(arg);
                if (quantity == 0) {
                    i++;
                    continue;
                }
                GameObject resourcePrefab = resourceManager.GetResource(i - 2);
                if (resourcePrefab == null) {
                    Debug.LogError("Unknown resource id " + (i++ - 2));
                    continue;
                }
                GameObject resourceObject = Instantiate(resourcePrefab, transform);
                resourceObject.transform.position = MapToWorldPos(pos, (float) tileResources / 2 + 0.5f);
                tileResources++;
            }
            i++;
        }
    }

    bool HandleTileContentPacketErrors(string[] args)
    {
        if (args.Length != 9) {
            Debug.LogError("HandleTileContentPacket: command must have 9 args, not " + args.Length);
            return false;
        }
        if (!packetManager.ArgsAreInt(args)) {
            Debug.LogError("HandleTileContentPacket: all args must be ints");
            return false;
        }
        return true;
    }

    

    public void SetMapSize(Vector2Int size)
    {
        this.size = size;
        UpdateMapMesh();
        camera.transform.position = new Vector3(camera.transform.position.x,
            camera.transform.position.y, size.y / 2 + 8);
    }

    void UpdateMapMesh()
    {
        Mesh mesh = new Mesh();
        Vector2 halfSize = size / 2;

        Vector3[] vertices = new Vector3[] {
            new Vector3(-halfSize.x, 0, -halfSize.y),
            new Vector3(halfSize.x, 0, -halfSize.y),
            new Vector3(-halfSize.x, 0, halfSize.y),
            new Vector3(halfSize.x, 0, halfSize.y)
        };
        int[] triangles = new int[] {
            3, 1, 0, 2, 3, 0
        };
        Vector3[] normals = new Vector3[] {
            new Vector3(0, 1, 0),
            new Vector3(0, 1, 0),
            new Vector3(0, 1, 0),
            new Vector3(0, 1, 0)
        };
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++) {
            uvs[i] = new Vector2(vertices[i].x / (size.x / 2), vertices[i].z / (size.y / 2));
        }
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.SetNormals(normals);
        mesh.SetUVs(0, uvs);
        meshFilter.mesh = mesh;
    }

    Vector3 MapToWorldPos(Vector2 mapPos, float height)
    {
        Vector2 halfSize = size / 2;

        return new Vector3(mapPos.x - halfSize.x + 0.5f, height, mapPos.y - halfSize.y + 0.5f);
    }
}
