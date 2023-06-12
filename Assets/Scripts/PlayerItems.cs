using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItems : MonoBehaviour
{
    public static List<LootDropData> items = new List<LootDropData>();
    
    /// <summary>
    /// Give the player a given item.
    /// </summary>
    /// <param name="item"></param>
    public void GiveItem(LootDropData item)
    {
        if(HasItem(item.ItemName))
            foreach (var loot in items)
            {
                if (loot.ItemName.Equals(item.ItemName))
                {
                    loot.IncrementCount();
                    return;
                }
            }
        else
        {
            items.Add(item);
        }
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
