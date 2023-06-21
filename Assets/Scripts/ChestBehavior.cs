using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestBehavior : MonoBehaviour
{
    public GameObject[] possibleLoot;
    public AudioClip openSFX;
    public GameObject chestText;

    Transform player;
    Transform cameraTransform;
    Animator anim;
    bool open;

    // Start is called before the first frame update
    void Start()
    {
        chestText.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cameraTransform = Camera.main.transform;
        anim = GetComponentInChildren<Animator>();
        open = false;
    }
    
    public void SetText()
    {
        if(!open)
        {
            chestText.SetActive(true);
        }
    }

    public void RemoveText()
    {
        chestText.gameObject.SetActive(false);
    }

    public void OpenChest()
    {
        if(!open)
        {
            open = true;
            RemoveText();
            FindObjectOfType<LevelManager>().UpdateScore(5, "Chest Found");
            anim.SetTrigger("openChest");
            AudioSource.PlayClipAtPoint(openSFX, transform.position);
            Invoke("SpawnLoot", 0.5f);
        }
    }

    void SpawnLoot()
    {
        GameObject loot = possibleLoot.GetValue(Random.Range(0, possibleLoot.Length)) as GameObject;
        GameObject lootDrop = Instantiate(loot, transform.position, transform.rotation);
        lootDrop.transform.parent = transform;
    }
}
