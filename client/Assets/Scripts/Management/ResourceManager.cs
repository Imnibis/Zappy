using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public GameObject orePrefab;
    public List<string> resources;

    public string GetResource(int id)
    {
        if (id >= resources.Count)
            return null;
        return resources[id];
    }
}
