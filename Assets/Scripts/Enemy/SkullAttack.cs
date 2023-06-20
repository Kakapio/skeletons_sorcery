using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullAttack : MonoBehaviour
{
    public float activateDistance;
    private Transform playerTransform;
    private float rotateSpeed = 2f;
    
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        FaceTarget(playerTransform.position);
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
