using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    public Slider healthSlider;

    public static bool isPlayerDead;

    int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        isPlayerDead = false;
    }

    public void Damage(int damageAmount)
    {
        if(currentHealth > 0)
        {
            currentHealth -= damageAmount;
            healthSlider.value = currentHealth;
        }

        if(currentHealth <= 0)
        {
            PlayerDies();
        }

        Debug.Log("Current health: " + currentHealth);
    }

    public void Heal(int healAmount)
    {
        currentHealth = Mathf.Clamp(currentHealth + healAmount, 0, maxHealth);
    }

    void PlayerDies()
    {
        Debug.Log("Player is dead...");

        //AudioSource.PlayClipAtPoint(deadSFX, transform.position);
        isPlayerDead = true;

        FindObjectOfType<LevelManager>().LevelLost();
    }
}
