using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullAttack : MonoBehaviour
{
    public float activateDistance;
    public float speed = 30f;
    public float detonateTime = 1f;
    public GameObject explodeFX;
    
    private Transform playerTransform;
    private float rotateSpeed = 2f;
    private Vector3 storedPlayerPos;
    private bool hasLockedOn;
    
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
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
}
