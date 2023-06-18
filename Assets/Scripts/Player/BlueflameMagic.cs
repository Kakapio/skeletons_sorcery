using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueflameMagic : MonoBehaviour
{
    public float destroyDuration = 3;
    public AudioClip burnUpSFX;
    public AudioClip impactSFX;
    public GameObject explosionFX;
    
    private bool alreadyHit = false;

    void Start()
    {
        AudioSource.PlayClipAtPoint(burnUpSFX, transform.position, 0.2f);
        Destroy(gameObject, destroyDuration);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || alreadyHit)
            return;
        
        alreadyHit = true;
        
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>().TakeDamage(LevelManager.blueFireballDamage);
            other.GetComponent<EnemyBehavior>().Alert();
        }

        GameObject explosion = Instantiate(explosionFX, transform.position, transform.rotation);
        Destroy(explosion, 2f);

        AudioSource.PlayClipAtPoint(impactSFX, transform.position, 0.2f);
        Destroy(gameObject, 0.05f);
    }
}