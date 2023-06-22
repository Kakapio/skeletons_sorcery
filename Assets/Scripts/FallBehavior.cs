using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallBehavior : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().Damage(1000);
        }
    }
}
