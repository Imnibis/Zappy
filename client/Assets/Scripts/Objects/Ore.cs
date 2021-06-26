using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour
{
    public ResourceManager resourceManager;
    public List<Resource> resources;
    public int[] quantities = new int[7] { 0, 0, 0, 0, 0, 0, 0 };

    private void Update()
    {
        bool hasResource = false;
        for (int i = 0; i < quantities.Length; i++) {
            foreach (Resource resource in resources) {
                if (resourceManager.resources[i] == resource.type &&
                    quantities[i] > 0) {
                    resource.Show();
                    hasResource = true;
                }
                else if (resourceManager.resources[i] == resource.type)
                    resource.Hide();
            }
        }
        if (hasResource)
            transform.GetChild(0).gameObject.SetActive(true);
        else
            transform.GetChild(0).gameObject.SetActive(false);
    }

    public void SetQuantities(int[] resourceQuantities)
    {
        if (quantities.Length != resourceQuantities.Length)
            return;
        quantities = resourceQuantities;
    }
    
    public int GetResourceQuantity(int id)
    {
        if (id >= quantities.Length)
            return -1;
        return quantities[id];
    }

    public void SetResourceQuantity(int id, int quantity)
    {
        if (id >= quantities.Length)
            return;
        quantities[id] = quantity;
    }

    public void AddResource(int id, int quantity)
    {
        if (id >= quantities.Length)
            return;
        SetResourceQuantity(id, GetResourceQuantity(id) + quantity);
    }
}
