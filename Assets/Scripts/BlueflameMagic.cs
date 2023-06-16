using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueflameMagic : MonoBehaviour
{
    public float destroyDuration = 3;
    public AudioClip burnUpSFX;
    public AudioClip impactSFX;
    public GameObject explosionFX;

    void Start()
    {
        AudioSource.PlayClipAtPoint(burnUpSFX, transform.position);
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
            other.GetComponent<EnemyHealth>().TakeDamage(LevelManager.blueFireballDamage);
            other.GetComponent<EnemyBehavior>().Alert();
        }

        GameObject explosion = Instantiate(explosionFX, transform.position, transform.rotation);
        Destroy(explosion, 2f);

        AudioSource.PlayClipAtPoint(impactSFX, transform.position);
        Destroy(gameObject, 0.05f);
    }
}