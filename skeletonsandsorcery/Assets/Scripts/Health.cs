using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Health : MonoBehaviour
{
    public int CurrentHealth { get; private set; }
    
    [SerializeField]
    private int maxHealth;

    public event Action OnDeath;

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = maxHealth;
    }

    public void Damage(int amount)
    {
        CurrentHealth -= amount;

        if (CurrentHealth <= 0)
            OnDeath?.Invoke();
    }

    public void Heal(int amount)
    {
        CurrentHealth = Math.Clamp(CurrentHealth + amount, 0, maxHealth);
    }
}
