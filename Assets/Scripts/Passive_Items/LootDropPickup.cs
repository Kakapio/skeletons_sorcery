using System;
using System.Collections;
using System.Collections.Generic;
using Passive_Items;
using UnityEngine;

public class LootDropPickup : MonoBehaviour
{
    public string lootDrop;
    public Sprite lootDropSprite;

    private float floatAmount = 0.0005f;
    private float rotateSpeed = 8;
    bool pickedUp = false;

    public void Update()
    {
        transform.Translate(new Vector3(0, Mathf.Sin(Time.realtimeSinceStartup) * floatAmount, 0));
        transform.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime, 0));
    }

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
                    Debug.Log("Gave player vampire powers");
                    break;
                case "ExampleLoot":
                    lootDropData = new ExampleLoot(lootDropSprite);
                    Debug.Log("Picked up example loot");
                    break;
                default:
                    throw new Exception("Invalid loot drop name provided.");
            }
            
            
            other.GetComponent<PlayerItems>().GiveItem(lootDropData);
            FindObjectOfType<LevelManager>().UpdateScore(2, "Loot Picked Up");
            FindObjectOfType<LootCount>().UpdateLootCounts();
            Destroy(gameObject);
        }
    }
}
