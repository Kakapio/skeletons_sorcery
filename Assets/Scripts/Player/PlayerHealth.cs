using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public Slider healthSlider;

    public static bool isPlayerDead;

    public static int currentHealth;
    public static int storedHealth = 100;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("difficulty", 0) == 0)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth = storedHealth;
        }

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        isPlayerDead = false;

        anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }

    public void Damage(int damageAmount)
    {
        if(!isPlayerDead)
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
        currentHealth = storedHealth;
        anim.SetInteger("Dead", Random.Range(1,3));
        FindObjectOfType<LevelManager>().LevelLost();
    }
}
