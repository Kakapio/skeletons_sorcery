using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float startingHealth = 50;
    public float currentHealth;
    public Slider healthSlider;

    void Awake()
    {
        healthSlider = GetComponentInChildren<Slider>();
    }

    void Start()
    {
        startingHealth = startingHealth * (0.8f + 0.2f * PlayerPrefs.GetInt("difficulty", 0));
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
            GetComponent<EnemyBehavior>().Dead();
        }
    }

    private void OnTriggerEnter(Collider other) {
        
    }
}
