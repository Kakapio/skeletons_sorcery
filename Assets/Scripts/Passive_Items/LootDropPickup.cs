using System;
using System.Collections;
using System.Collections.Generic;
using Passive_Items;
using UnityEngine;

public class LootDropPickup : MonoBehaviour
{
    public string lootDrop;
    public Sprite lootDropSprite;
    public AudioClip pickupSFX;

    private bool pickedUp = false;

    private void OnTriggerEnter(Collider other)
    {
        // Give the player the item they just touched.
        if(other.tag.Equals("Player") && !pickedUp)
        {
            pickedUp = true;
            LootDropData lootDropData;
            switch(lootDrop)
            {
                case "VampireSoul":
                    lootDropData = new VampireSoul(lootDropSprite);
                    break;
                case "ExampleLoot":
                    lootDropData = new ExampleLoot(lootDropSprite);
                    break;
                case "CuckooFeather":
                    lootDropData = new CuckooFeather(lootDropSprite);
                    break;
                case "Blueflame":
                    lootDropData = new Blueflame(lootDropSprite);
                    break;
                default:
                    throw new Exception("Invalid loot drop name provided.");
            }
            
            var playerItems = other.GetComponent<PlayerItems>();
            if(!playerItems.HasItem(lootDrop))
            {
                PickupMessage(lootDrop);
            }
            playerItems.GiveItem(lootDropData);
            FindObjectOfType<LevelManager>().UpdateScore(2, "Loot Picked Up");
            FindObjectOfType<LootCount>().UpdateLootCounts();
            AudioSource.PlayClipAtPoint(pickupSFX, Camera.main.transform.position);

            Debug.Log($"Gave player {lootDrop} powers");
            
            Destroy(gameObject);
        }
    }

    void PickupMessage(string lootDrop)
    {
        string message;

        switch(lootDrop)
        {
            case "VampireSoul":
                message = "You found a Vampire Soul.\nVampire Souls heal you on kill";
                break;
            case "ExampleLoot":
                message = "You found Gold.\nGold gives you score";
                break;
            case "CuckooFeather":
                message = "You found a Cuckoo Feather.\nThese increase your jump height";
                break;
            case "Blueflame":
                message = "You found a Blueflame.\nBlueflames increase your damage";
                break;
            default:
                message = "invalid loot drop";
                break;
        }

        FindObjectOfType<LevelManager>().UpdateGameInfo(message, Color.white);
    }
}
