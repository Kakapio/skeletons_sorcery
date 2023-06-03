using System;
using System.Collections;
using System.Collections.Generic;
using Passive_Items;
using UnityEngine;

public class LootDropPickup : MonoBehaviour
{
    public string lootDrop;

    private void OnTriggerEnter(Collider other)
    {
        // Give the player the item they just touched.
        if (other.tag.Equals("Player"))
        {
            LootDropData lootDropData;
            switch (lootDrop)
            {
                case "VampireSoul":
                    lootDropData = new VampireSoul();
                    break;
                default:
                    throw new Exception("Invalid loot drop name provided.");
            }
            
            other.GetComponent<PlayerItems>().GiveItem(lootDropData);
            Debug.Log("Gave player vampire powers");
            Destroy(gameObject);
        }
    }
}
