using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcespearMagic : MonoBehaviour
{
    public float destroyDuration = 3;
    public AudioClip launchSFX; // A spear being thrown/launched.
    public AudioClip impactSFX; // Sound of flesh being pierced.

    void Start()
    {
        AudioSource.PlayClipAtPoint(launchSFX, transform.position);
        Destroy(gameObject, destroyDuration);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>().TakeDamage(LevelManager.iceSpearDamage);
            other.GetComponent<EnemyBehavior>().Alert();
        }

        AudioClip impact = Instantiate(impactSFX, transform.position, transform.rotation);
        Destroy(impact, 2f);
    }
}
