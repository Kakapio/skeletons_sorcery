using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMagic : MonoBehaviour
{
    public float destroyDuration = 3;
    public AudioClip burnUpSFX;
    public AudioClip impactSFX;

    void Start()
    {
        AudioSource.PlayClipAtPoint(burnUpSFX, gameObject.transform.position);
        Destroy(gameObject, destroyDuration);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Enemy")) {
            // do enemy damage/behavior
            // for now just destroy
            other.GetComponentInParent<EnemyBehavior>().Dead();
        }

        // do magic explosion effect
        // for now just destroy
        AudioSource.PlayClipAtPoint(impactSFX, gameObject.transform.position);
        Destroy(gameObject, 0.05f);
    }
}
