using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcespearMagic : MonoBehaviour
{
    public float destroyDuration = 3;
    public AudioClip launchSFX; // A spear being thrown/launched.
    public AudioClip impactSFX; // Sound of flesh being pierced.
    public GameObject iceexplosionFX;

    private bool alreadyHit = false;

    void Start()
    {
        AudioSource.PlayClipAtPoint(launchSFX, transform.position, 0.5f);
        Destroy(gameObject, destroyDuration);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || alreadyHit || other.gameObject.CompareTag("Gate"))
            return;

        alreadyHit = true;
        
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>().TakeDamage(LevelManager.iceSpearDamage);
            other.GetComponent<EnemyBehavior>().Alert();
        }

        if(other.gameObject.CompareTag("Boss"))
        {
            other.GetComponent<BossBehavior>().TakeDamage(LevelManager.iceSpearDamage);
        }

        GameObject explosion = Instantiate(iceexplosionFX, transform.position, transform.rotation);
        Destroy(explosion, 2f);
        
        AudioSource.PlayClipAtPoint(impactSFX, transform.position, 1.5f);
        Destroy(gameObject, 0.05f);
    }
}
