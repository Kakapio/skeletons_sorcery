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
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        isPlayerDead = false;

        anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
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
        Debug.Log($"Healing player by {healAmount}");
        Debug.Log("Current health: " + currentHealth);
        currentHealth = Mathf.Clamp(currentHealth + healAmount, 0, maxHealth);
        healthSlider.value = currentHealth;
    }

    void PlayerDies()
    {
        //AudioSource.PlayClipAtPoint(deadSFX, transform.position);
        isPlayerDead = true;
        anim.SetInteger("Dead", Random.Range(1,3));
        FindObjectOfType<LevelManager>().LevelLost();
    }
}
