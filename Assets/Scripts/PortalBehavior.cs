using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBehavior : MonoBehaviour
{
    public GameObject teleportParticles;
    public AudioClip teleportSFX;

    public static bool teleporting = false;
    
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Player") && !teleporting)
        {
            teleporting = true;
            GameObject particles = Instantiate(teleportParticles, other.transform.position, Quaternion.Euler(90, 0, 0));
            particles.transform.parent = other.transform;
            Invoke("TeleportAudio", 0.25f);
            FindObjectOfType<LevelManager>().LevelBeat();
        }
    }

    void TeleportAudio()
    {
        AudioSource.PlayClipAtPoint(teleportSFX, Camera.main.transform.position);
    }
}
