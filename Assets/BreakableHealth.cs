using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreakableHealth : MonoBehaviour
{
    public GameObject pieces;
    public float explosionForce = 100;
    public float explosionRadius = 5;
    public int points = 0;
    public string reason;
    public AudioClip breakSFX;
    public int startingHealth = 50;
    public int currentHealth;
    public Slider healthSlider;

    bool destroyed = false;

    void Awake()
    {
        healthSlider = GetComponentInChildren<Slider>();
    }

    void Start()
    {
        currentHealth = startingHealth;
        healthSlider.maxValue = startingHealth;
        healthSlider.value = currentHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        if(currentHealth > 0)
        {
            currentHealth -= damageAmount;
            healthSlider.value = currentHealth;
        }

        if(currentHealth <= 0)
        {
            Break();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Fireball"))
        {
            TakeDamage(LevelManager.fireballDamage);
        }
    }

    public void Break()
    {
        if(!destroyed)
        {
            destroyed = true;
            Transform current = gameObject.transform;

            GameObject broken = Instantiate(
                pieces, current.position, current.rotation);

            Rigidbody[] rbPieces = broken.GetComponentsInChildren<Rigidbody>();

            foreach(Rigidbody rb in rbPieces)
            {
                rb.AddExplosionForce(explosionForce, current.position, explosionRadius);
            }
            
            AudioSource.PlayClipAtPoint(breakSFX, transform.position);

            Destroy(gameObject);
            Destroy(broken, 2);
            
            if(points > 0)
            {
                FindObjectOfType<LevelManager>().UpdateScore(points, reason);
            }
        }
    }
}
