using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItems : MonoBehaviour
{
    private List<LootDropData> items = new List<LootDropData>();
    
    /// <summary>
    /// Give the player a given item.
    /// </summary>
    /// <param name="item"></param>
    public void GiveItem(LootDropData item)
    {
        bool found = false;
        // Already have this item, increase the count instead.
        foreach (var loot in items)
        {
            if (loot.ItemName.Equals(item.ItemName))
            {
                Debug.Log("Incremented count");
                loot.IncrementCount();
                found = true;
            }
        }
        
        // Don't already have it, add it.
        if (!found)
            items.Add(item);
    }

    /// <summary>
    /// Returns true if the player has a given item.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool HasItem(string item)
    {
        foreach (var loot in items)
        {
            if (loot.ItemName.Equals(item))
                return true;
        }

        return false;
    }

    public LootDropData GetItem(string item)
    {
        foreach (var loot in items)
        {
            if (loot.ItemName.Equals(item))
                return loot;
        }

        return null;
    }
}
