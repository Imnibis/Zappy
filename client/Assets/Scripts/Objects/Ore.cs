using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour
{
    public List<Resource> resources;

    public void ShowResources(Dictionary<string, int> resourceQuantities)
    {
        foreach (Resource resource in resources) {
            int quantity;
            if (resourceQuantities.TryGetValue(resource.type, out quantity) &&
                quantity != 0) resource.Show();
            else resource.Hide();
        }
    }
}
