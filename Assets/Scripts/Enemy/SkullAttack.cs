using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullAttack : MonoBehaviour
{
    public float activateDistance;
    public float speed = 30f;
    public float detonateTime = 1f;
    public GameObject explodeFX;
    public float range = 4f;
    public int damage = 5;
    public AudioClip explodeSFX;
    public AudioClip boneSFX;
    
    private Transform playerTransform;
    private float rotateSpeed = 2f;
    private Vector3 storedPlayerPos;
    private bool hasLockedOn;
    
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        InvokeRepeating("BoneAudio", 5, 3);
    }

    // Update is called once per frame
    void Update()
    {
        var step = speed * Time.deltaTime;
        if (Vector3.Distance(transform.position, playerTransform.position) <= activateDistance || hasLockedOn)
        {
            Debug.Log("Skull has locked on.");
            hasLockedOn = true;
            transform.position = Vector3.MoveTowards(transform.position, storedPlayerPos, step);
            Invoke("Explode", detonateTime);
            return;
        }
        
        FaceTarget(playerTransform.position);
        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, step);
        storedPlayerPos = playerTransform.position;
    }

    void Explode()
    {
        var hits = Physics.OverlapSphere(transform.position, range);
        foreach (var mob in hits)
        {
            Debug.Log("Hit a player...");
            if (mob.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
            {
                playerHealth.Damage(damage);
            }
        }
        
        AudioSource.PlayClipAtPoint(explodeSFX, transform.position, 0.8f);
        Instantiate(explodeFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    
    void FaceTarget(Vector3 target)
    {
        Vector3 directionToTarget = (target - transform.position);
        directionToTarget = directionToTarget.normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(
            transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
    }

    void BoneAudio()
    {
        AudioSource.PlayClipAtPoint(boneSFX, transform.position, 1f);
    }
}
