using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public List<GameObject> resources;

    public GameObject GetResource(int id)
    {
        if (id >= resources.Count)
            return null;
        return resources[id];
    }
}
