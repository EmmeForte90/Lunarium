using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public int maxItems = 20;

    public void AddItem(Item item)
    {
        if (items.Count < maxItems)
        {
            items.Add(item);
            Debug.Log("Item added to inventory: " + item.Name);
        }
        else
        {
            Debug.Log("Inventory is full!");
        }
    }

    public void RemoveItem(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            Debug.Log("Item removed from inventory: " + item.Name);
        }
        else
        {
            Debug.Log("Item not found in inventory.");
        }
    }
}


