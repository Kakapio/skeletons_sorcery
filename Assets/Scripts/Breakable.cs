using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public GameObject pieces;
    public float explosionForce = 100;
    public float explosionRadius = 5;
    public int points = 0;
    public string reason;
    public AudioClip breakSFX;

    bool hit = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Fireball"))
        {
            Break();
        }
    }

    public void Break()
    {
        if(!hit)
        {
            hit = true;
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

            if(points > 0)
            {
                FindObjectOfType<LevelManager>().UpdateScore(points, reason);
            }
        }
    }
}
