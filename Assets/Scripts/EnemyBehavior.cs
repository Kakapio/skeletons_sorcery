using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    public Transform enemyEyes;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 8f;
    public float attackRange = 2f;
    public float detectionRange = 20f;
    public float patrolRange = 10f;
    public int damageAmount = 20;
    public float enemyRotationSpeed = 10f;
    public float fieldOfView = 150f;
    public int deathScore = 1;
    public bool canSprint = true;
    public AudioClip attackSFX;
    public AudioClip hitSFX;
    public AudioClip deathSFX;

    Animator anim;
    bool isDead;
    bool attacking;
    bool alert;
    float distanceToPlayer;
    Vector3 startPos;
    NavMeshAgent agent;

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
        alert = false;
        distanceToPlayer = detectionRange + attackRange;
        startPos = transform.position;
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = attackRange;
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
        currentState = EnemyStates.Patrol;
    }

    void UpdatePatrolState()
    {
        anim.SetFloat("Speed_f", 0.3f);
        agent.speed = patrolSpeed;

        if(agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 point;
            if(GetNextPoint(startPos, patrolRange, out point))
            {
                agent.SetDestination(point);
            }
        }
        else if(IsPlayerInClearFOV() || alert)
        {
            currentState = EnemyStates.Chase;
        }

        FaceTarget(agent.destination);
    }

    void UpdateChaseState()
    {
        FaceTarget(playerTransform.position);
        agent.SetDestination(playerTransform.position);
        
        if(canSprint)
        {
            anim.SetFloat("Speed_f", 0.51f);
        }
        else
        {
            anim.SetFloat("Speed_f", 0.5f);
        }

        agent.speed = chaseSpeed;

        if(distanceToPlayer < attackRange)
        {
            anim.SetFloat("Speed_f", 0f);
            currentState = EnemyStates.Attack;
        }
        else if(distanceToPlayer > detectionRange && !alert)
        {
            anim.SetFloat("Speed_f", 0f);
            currentState = EnemyStates.Patrol;
        }
    }

    void UpdateAttackState()
    {
        if(!attacking)
        {
            if(distanceToPlayer > attackRange)
            {
                currentState = EnemyStates.Chase;
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
    }

    void UpdateDeadState()
    {
        if(!isDead)
        {
            ApplyVampireHeal();
            FindObjectOfType<LevelManager>().UpdateScore(deathScore, "Enemy Killed");
            //GetComponentInChildren<MeshCollider>().enabled = false;

            isDead = true;
            agent.SetDestination(transform.position);
            agent.speed = 0f;

            anim.SetInteger("MeleeType_int", -1);
            anim.SetFloat("Speed_f", 0f);

            anim.SetInteger("DeathType_int", 1);
            anim.SetBool("Death_b", true);

            AudioSource.PlayClipAtPoint(deathSFX, transform.position);
            Destroy(gameObject, 2f);
        }
    }

    bool GetNextPoint(Vector3 center, float range, out Vector3 target)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                target = hit.position;
                return true;
            }
        }
        target = Vector3.zero;
        return false;
    }

    bool IsPlayerInClearFOV()
    {
        RaycastHit hit;
        
        Vector3 directionToPlayer = (playerTransform.position + new Vector3(0, 1f, 0)) - enemyEyes.position;

        if(Vector3.Angle(directionToPlayer, enemyEyes.forward) <= fieldOfView)
        {
            if(Physics.Raycast(enemyEyes.position, directionToPlayer, out hit, detectionRange))
            {
                
                if(hit.collider.CompareTag("Player"))
                {
                    return true;
                }
            }
        }

        return false;
    }

    void ApplyVampireHeal()
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

    public void Alert()
    {
        alert = true;
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