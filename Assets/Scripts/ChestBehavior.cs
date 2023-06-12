using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBehavior : MonoBehaviour
{
    public GameObject loot;
    public AudioClip openSFX;

    Transform player;
    Animator anim;
    bool open;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponentInChildren<Animator>();
        open = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, player.position) < 5 && !open)
        {
            open = true;
            FindObjectOfType<LevelManager>().ChestFound();
            anim.SetTrigger("openChest");
            AudioSource.PlayClipAtPoint(openSFX, transform.position);
            Invoke("SpawnLoot", 0.5f);
        }
    }

    void SpawnLoot()
    {
        GameObject lootDrop = Instantiate(loot, transform.position, transform.rotation);
        lootDrop.transform.parent = transform;
    }
}
