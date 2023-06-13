using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBehavior : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<PlayerHealth>().Damage(100);
        }
        else if(other.gameObject.CompareTag("Enemy"))
        {
            FindObjectOfType<EnemyHealth>().TakeDamage(1000);
        }
    }
}
