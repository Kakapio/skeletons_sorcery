using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionBehavior : MonoBehaviour
{
    public float reachDistance = 8f;

    GameObject chest;

    // Update is called once per frame
    void Update()
    {
        if(!PlayerHealth.isPlayerDead)
        {
            RaycastHit hit;

            if(Physics.Raycast(transform.position, transform.forward, out hit, reachDistance))
            {
                if(hit.collider.CompareTag("Chest"))
                {
                    chest = hit.collider.gameObject;
                    hit.collider.gameObject.GetComponent<ChestBehavior>().SetText();
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        hit.collider.gameObject.GetComponent<ChestBehavior>().OpenChest();
                    }
                }
                else
                {
                    RemoveChestText();
                }
            }
            else
            {
                RemoveChestText();
            }
        }
    }

    void RemoveChestText()
    {
        if(chest != null)
        {
            chest.GetComponent<ChestBehavior>().RemoveText();
        }
    }
}
