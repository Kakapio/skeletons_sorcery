using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 5;
    public float detectionRange = 20;
    public int damageAmount = 20;

    Animator anim;
    bool isDead;

    void Start()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float step = moveSpeed * Time.deltaTime;
        
        float distance = Vector3.Distance(transform.position, player.position);

        if(distance < detectionRange && !isDead)// && !PlayerHealth.isPlayerDead)
        {
            transform.LookAt(player);
            transform.position = Vector3.MoveTowards(transform.position, player.position, step);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !isDead)
        {
            //apply damage
            var playerHealth = other.GetComponent<PlayerHealth>();
            playerHealth.Damage(damageAmount);

            // animate hit
            anim.SetInteger("WeaponType_int", 12);
            anim.SetInteger("MeleeType_int", 1);
            Invoke("ResetAttack", 0.5f);
        }
    }

    private void ResetAttack()
    {
        anim.SetInteger("MeleeType_int", -1);
    }

    public void Dead()
    {
        anim.SetInteger("DeathType_int", 1);
        anim.SetBool("Death_b", true);
        isDead = true;
        Destroy(gameObject, 2f);
    }
}