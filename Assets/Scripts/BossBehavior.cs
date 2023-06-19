using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

enum BossPhase
{
    Melee,
    Summon,
    Attack
}
public class BossBehavior : MonoBehaviour
{
    public float bossRotationSpeed = 10f;
    public int activateDistance = 80;
    public int damageAmount = 10;
    public float attackRange = 3f;
    public float chaseSpeed = 4f;
    public float summonFrequency = 60f;
    public GameObject enemySummon;
    public GameObject[] summonLocations;
    public AudioClip hitSFX;
    public AudioClip attackSFX;
    public AudioClip summonSFX;
    
    private BossPhase bossPhase = BossPhase.Summon;
    private int currentHealth = 1000;
    private NavMeshAgent agent;
    private Transform playerTransform;
    private float distanceToPlayer;
    private bool attacking;
    private Animator anim;
    private bool bossActive = false;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        agent.speed = chaseSpeed;
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        CheckBossActivation();
        
        if (bossActive == false)
            return;
        
        switch (bossPhase)
        {
            case BossPhase.Melee:
                anim.SetFloat("Speed_f", 0.7f);
                FaceTarget(playerTransform.position);
                agent.SetDestination(playerTransform.position);
                
                if(distanceToPlayer < attackRange)
                {
                    anim.SetFloat("Speed_f", 0f);
                    bossPhase = BossPhase.Attack;
                }
                break;
            case BossPhase.Summon:
                PlaySummonAudio();
                    for (int i = 0; i < summonLocations.Length; i++)
                    {
                        Debug.Log($"Summoning enemy {i}...");
                        var enemy = Instantiate(enemySummon, summonLocations[i].transform.position, Quaternion.identity);
                        enemy.GetComponent<EnemyBehavior>().detectionRange = 100000;
                        enemy.GetComponent<EnemyBehavior>().currentState = EnemyBehavior.EnemyStates.Chase;
                    }
                    
                    bossPhase = BossPhase.Melee;
                    Invoke("ResetSummonTimer", summonFrequency);
                break;
            case BossPhase.Attack:
                if(!attacking)
                {
                    if(distanceToPlayer > attackRange)
                    {
                        bossPhase = BossPhase.Melee;
                    }
                    else
                    {
                        attacking = true;
                        agent.SetDestination(transform.position);
                        agent.speed = 0f;

                        // animate hit
                        anim.SetInteger("WeaponType_int", 12);
                        anim.SetInteger("MeleeType_int", 1);
                
                        Invoke("SwingSound", 0.25f);
                        Invoke("CheckAttackHit", 0.5f);
                        Invoke("ResetAttack", 1.2f);
                    }
                }
                break;
        }
    }

    private void PlaySummonAudio()
    {
    }

    private void CheckBossActivation()
    {
        if (distanceToPlayer <= activateDistance)
        {
            bossActive = true;
            GameObject.FindWithTag("Gate").GetComponent<BoxCollider>().isTrigger = false;
        }
    }

    void ResetSummonTimer()
    {
        Debug.Log("Resetting summon timer.");
        bossPhase = BossPhase.Summon;
    }
    
    void SwingSound()
    {
        AudioSource.PlayClipAtPoint(attackSFX, transform.position);
    }

    void CheckAttackHit()
    {
        if(distanceToPlayer < attackRange + 1)
        {
            // apply damage
            var playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
            playerHealth.Damage(damageAmount);
            AudioSource.PlayClipAtPoint(hitSFX, Camera.main.transform.position);
        }
    }

    void ResetAttack()
    {
        anim.SetInteger("MeleeType_int", -1);
        attacking = false;
    }
    
    void FaceTarget(Vector3 target)
    {
        Vector3 directionToTarget = (target - transform.position);
        directionToTarget.y = 0;
        directionToTarget = directionToTarget.normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(
            transform.rotation, lookRotation, bossRotationSpeed * Time.deltaTime);
    }
}
