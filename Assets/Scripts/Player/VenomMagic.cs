using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VenomMagic : MonoBehaviour
{
    public float activateDistance = 2f;
    public float speed = 30f;
    public float detonateTime = 0.1f;
    public float range = 10;
    public GameObject explodeFX;
    public AudioClip castSFX;
    public AudioClip impactSFX;
    
    private Transform targetEnemy;
    private float rotateSpeed = 2f;
    
    void Start()
    {
        AudioSource.PlayClipAtPoint(castSFX, transform.position, 0.5f);
        List<GameObject> enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        enemies.Add(GameObject.FindWithTag("Boss"));
        targetEnemy = enemies[0].transform;

        for (int i = 1; i < enemies.Count; i++)
        {
            if (Vector3.Distance(transform.position, enemies[i].transform.position) <
                Vector3.Distance(transform.position, targetEnemy.position))
            {
                targetEnemy = enemies[i].transform;
            }
        }

        targetEnemy.position =
            new Vector3(targetEnemy.position.x, targetEnemy.position.y + 4, targetEnemy.position.z); //offset to body;
    }

    // Update is called once per frame
    void Update()
    {
        var step = speed * Time.deltaTime;
        if (Vector3.Distance(transform.position, targetEnemy.position + new Vector3(0, 2, 0)) <= activateDistance)
        {
            Invoke("Explode", detonateTime);
        }
        
        FaceTarget(targetEnemy.position);
        transform.position = Vector3.MoveTowards(transform.position, targetEnemy.position + new Vector3(0, 2, 0), step);
    }
    
    void Explode()
    {
        var hits = Physics.OverlapSphere(transform.position, range);
        foreach (var enemy in hits)
        {
            Debug.Log("Hit an enemy...");
            if (enemy.TryGetComponent<EnemyHealth>(out EnemyHealth enemHealth))
            {
                enemHealth.TakeDamage(LevelManager.venomBombDamage);
            }
            if (enemy.TryGetComponent<BossBehavior>(out BossBehavior boss))
            {
                boss.DealDamage(LevelManager.venomBombDamage);
            }
        }
        
        AudioSource.PlayClipAtPoint(impactSFX, transform.position, 0.8f);
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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Registered hit with {other.name}");
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Gate"))
            return;

        Explode();
    }
}
