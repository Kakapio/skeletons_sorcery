using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttack : MonoBehaviour
{
    public float destroyDuration = 3;
    public AudioClip launchSFX;
    public AudioClip impactSFX;
    public GameObject explosionFX;
    public string magicID;
    
    private bool alreadyHit = false;

    void Start()
    {
        AudioSource.PlayClipAtPoint(launchSFX, transform.position, 0.2f);
        Destroy(gameObject, destroyDuration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || alreadyHit || other.gameObject.CompareTag("Gate"))
            return;
        
        alreadyHit = true;
        
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>().TakeDamage(FindObjectOfType<LevelManager>().GetDamage(magicID));
            other.GetComponent<EnemyBehavior>().Alert();
        }
        if (other.gameObject.CompareTag("Boss"))
        {
            other.GetComponent<BossBehavior>().DealDamage(FindObjectOfType<LevelManager>().GetDamage(magicID));
        }

        GameObject explosion = Instantiate(explosionFX, transform.position, transform.rotation);
        Destroy(explosion, 2f);

        AudioSource.PlayClipAtPoint(impactSFX, transform.position, 0.2f);
        Destroy(gameObject, 0.05f);
    }
}
