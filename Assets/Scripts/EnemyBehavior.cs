using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyBehavior : MonoBehaviour
{
    public enum EnemyStates
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Dead
    }

    public EnemyStates currentState;

    public Transform playerTransform;
    public float moveSpeed = 5;
    public float attackRange = 2f;
    public float detectionRange = 20;
    public int damageAmount = 20;
    public float enemyRotationSpeed = 10;
    public AudioClip attackSFX;
    public AudioClip hitSFX;
    public AudioClip deathSFX;
    
    Animator anim;
    bool isDead;
    bool attacking;
    float distanceToPlayer;

    void Start()
    {
        if(playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        currentState = EnemyStates.Idle;

        anim = gameObject.GetComponent<Animator>();
        isDead = false;
        attacking = false;
        distanceToPlayer = detectionRange + attackRange;
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        switch(currentState)
        {
            case EnemyStates.Idle:
                UpdateIdleState();
                break;
            case EnemyStates.Patrol:
                UpdatePatrolState();
                break;
            case EnemyStates.Chase:
                UpdateChaseState();
                break;
            case EnemyStates.Attack:
                UpdateAttackState();
                break;
            case EnemyStates.Dead:
                UpdateDeadState();
                break;
        }
    }

    void UpdateIdleState()
    {
        if(distanceToPlayer < detectionRange)
        {  
            currentState = EnemyStates.Chase;
        }
    }

    void UpdatePatrolState()
    {
        //implement patrolling
    }

    void UpdateChaseState()
    {
        FaceTarget(playerTransform.position);
        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
        
        anim.SetFloat("Speed_f", 0.51f);

        if(distanceToPlayer < attackRange)
        {
            anim.SetFloat("Speed_f", 0f);
            currentState = EnemyStates.Attack;
        }
        else if(distanceToPlayer > detectionRange)
        {
            anim.SetFloat("Speed_f", 0f);
            currentState = EnemyStates.Idle;
        }
    }

    void UpdateAttackState()
    {
        if(!attacking)
        {
            if(distanceToPlayer > attackRange)
            {
                currentState = EnemyStates.Idle;
            }
            else
            {
                attacking = true;

                // animate hit
                anim.SetInteger("WeaponType_int", 12);
                anim.SetInteger("MeleeType_int", 1);
                
                Invoke("SwingSound", 0.25f);
                Invoke("CheckAttackHit", 0.5f);
                Invoke("ResetAttack", 1.2f);
            }
        }
    }

    void UpdateDeadState()
    {
        if(!isDead)
        {
            ApplyVampireHeal();

            isDead = true;
            anim.SetInteger("MeleeType_int", -1);
            anim.SetFloat("Speed_f", 0f);

            anim.SetInteger("DeathType_int", 1);
            anim.SetBool("Death_b", true);

            AudioSource.PlayClipAtPoint(deathSFX, transform.position);
            Destroy(gameObject, 2f);
        }
    }

    private void ApplyVampireHeal()
    {
        var player = GameObject.FindWithTag("Player");
        if (player.GetComponent<PlayerItems>().HasItem("VampireSoul"))
        {
            Debug.Log($"Player has {player.GetComponent<PlayerItems>().GetItem("VampireSoul").Count} vampire souls");
            var vampireItem = player.GetComponent<PlayerItems>().GetItem("VampireSoul");
            player.GetComponent<PlayerHealth>().Heal(1 * vampireItem.Count);
        }
    }

    private void SwingSound()
    {
        AudioSource.PlayClipAtPoint(attackSFX, transform.position);
    }

    private void CheckAttackHit()
    {
        if(distanceToPlayer < attackRange + 1)
        {
            // apply damage
            var playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
            playerHealth.Damage(damageAmount);
            AudioSource.PlayClipAtPoint(hitSFX, Camera.main.transform.position);
        }
    }

    private void ResetAttack()
    {
        anim.SetInteger("MeleeType_int", -1);
        attacking = false;
    }

    public void Dead()
    {
        currentState = EnemyStates.Dead;
    }

    public void DeadByFire()
    {
        //update death by fire effect to dissolve enemy using dissolve material from unity fx pack
        //for now just use usual death
        Dead();
    }

    void FaceTarget(Vector3 target)
    {
        Vector3 directionToTarget = (target - transform.position);
        directionToTarget.y = 0;
        directionToTarget = directionToTarget.normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(
            transform.rotation, lookRotation, enemyRotationSpeed * Time.deltaTime);
    }
}