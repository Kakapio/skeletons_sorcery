using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable : MonoBehaviour
{
    public AudioClip collisionSFX;
    public int impactStrength = 100;

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.CompareTag("Fireball"))
        {
            Vector3 bounceDirection = collision.GetContact(0).normal;

            Rigidbody rb = GetComponent<Rigidbody>();

            rb.AddForce(bounceDirection * impactStrength);

            AudioSource.PlayClipAtPoint(collisionSFX, transform.position);
        }
    }
}
