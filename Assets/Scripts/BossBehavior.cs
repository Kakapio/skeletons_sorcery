using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.UI;

enum BossPhase
{
    Melee,
    Summon,
    Attack,
    Dead
}
public class BossBehavior : MonoBehaviour
{
    public float bossRotationSpeed = 10f;
    public int activateDistance = 80;
    public int damageAmount = 10;
    public float attackRange = 3f;
    public float chaseSpeed = 4f;
    public float summonFrequency = 60f;
    public float skullAttackFrequency = 15f;
    public GameObject enemySummon;
    public GameObject skullAttack;
    public GameObject[] summonLocations;
    public AudioClip hitSFX;
    public AudioClip attackSFX;
    public AudioClip summonSFX;
    public AudioClip deathSFX;
    public int currentHealth = 1000;
    public GameObject healthbar;

    private BossPhase bossPhase = BossPhase.Summon;
    private NavMeshAgent agent;
    private Transform playerTransform;
    private float distanceToPlayer;
    private bool attacking;
    private Animator anim;
    private bool bossActive = false;
    private Slider healthSlider;
    private MeshCollider meshCollider;
    bool isDead = false;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        meshCollider = GetComponent<MeshCollider>();
        playerTransform = GameObject.FindWithTag("Player").transform;
        healthbar.SetActive(false);
        healthSlider = healthbar.GetComponentInChildren<Slider>();
        healthSlider.maxValue = currentHealth;
        healthSlider.value = currentHealth;
        InvokeRepeating("SkullRangedAttack", skullAttackFrequency * 2, skullAttackFrequency);
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
                anim.SetFloat("Speed_f", 0.3f);
                FaceTarget(playerTransform.position);
                agent.SetDestination(playerTransform.position);
                agent.speed = chaseSpeed;
                
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
            
            case BossPhase.Dead:
                anim.SetFloat("Speed_f", 0f);
                agent.speed = 0;
                break;
        }
    }

    void SkullRangedAttack()
    {
        var spawnPosition = transform.position;
        spawnPosition.y += 7;
        spawnPosition.z += 4;

        Instantiate(skullAttack, spawnPosition, Quaternion.identity);
        Instantiate(skullAttack, new Vector3(spawnPosition.x * -1, spawnPosition.y, spawnPosition.z), Quaternion.identity);
        Debug.Log("Boss is doing ranged skull attack.");
    }

    private void PlaySummonAudio()
    {
    }

    private void CheckBossActivation()
    {
        if (distanceToPlayer <= activateDistance && !bossActive)
        {
            bossActive = true;
            GameObject.FindWithTag("Gate").GetComponent<BoxCollider>().isTrigger = false;
            healthbar.SetActive(true);
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

<<<<<<< Updated upstream
    public void TakeDamage(int damageAmount)
    {
        if(currentHealth > 0)
        {
            currentHealth -= damageAmount;
            healthSlider.value = currentHealth;
        }

        if(currentHealth <= 0)
        {
            Dead();
        }
    }

    void Dead()
    {
        if(!isDead)
        {
            healthbar.SetActive(false);
            
            FindObjectOfType<LevelManager>().UpdateScore(50, "Boss Killed");

            isDead = true;
            agent.SetDestination(transform.position);
            agent.speed = 0f;

            anim.SetInteger("MeleeType_int", -1);
            anim.SetFloat("Speed_f", 0f);

            anim.SetInteger("DeathType_int", 1);
            anim.SetBool("Death_b", true);

            AudioSource.PlayClipAtPoint(deathSFX, transform.position);

            FindObjectOfType<LevelManager>().LevelBeat();
        }
=======
    public void DealDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
            bossPhase = BossPhase.Dead;
        healthbar.GetComponent<Slider>().value = currentHealth;
>>>>>>> Stashed changes
    }
}
