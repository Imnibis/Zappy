using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Map : MonoBehaviour
{
    public Vector2Int size;

    MeshFilter meshFilter;
    MeshRenderer renderer;

    public void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        renderer = GetComponent<MeshRenderer>();
    }

    public void HandleMapSizePacket(string[] args)
    {
        int x;
        int y;
        foreach (string arg in args) {
            Debug.LogError(arg);
        }
        if (args.Length != 2) {
            Debug.LogError("HandleMapSizePacket: command must have 2 args, not " + args.Length);
            return;
        }
        if (!int.TryParse(args[0], out x) || !int.TryParse(args[1], out y)) {
            Debug.LogError("HandleMapSizePacket: both args must be ints");
            return;
        }
        Debug.Log(string.Format("Setting map size to ({0}, {1})", x, y));
        SetMapSize(new Vector2Int(x, y));
    }

    public void SetMapSize(Vector2Int size)
    {
        this.size = size;
        UpdateMapMesh();
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
            new Vector3(0, 1, 0)
        };
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++) {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.SetNormals(normals);
        mesh.SetUVs(0, uvs);
        meshFilter.mesh = mesh;
    }
}
