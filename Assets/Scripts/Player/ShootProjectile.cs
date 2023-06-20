using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

enum PlayerProjectile
{
    Fireball,
    Icespear,
    Venombomb
}

public class ShootProjectile : MonoBehaviour
{
    public GameObject fireballPrefab;
    public GameObject bluefireballPrefab;
    public GameObject icespearPrefab;
    public GameObject venomBombPrefab;
    public float projectileSpeed = 50;
    public AudioClip projectileSFX;
    public float fireballCooldown = 0.7f;
    public float icespearCooldown = 1f;
    public float venomBombCooldown = 5f;

    Animator anim;
    PlayerProjectile playerProjectile = PlayerProjectile.Fireball;
    float timeSinceLastShoot;

    void Start()
    {
        anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        PollWeaponSwap();
        HandleShoot();
        timeSinceLastShoot += Time.deltaTime;
    }

    private void PollWeaponSwap()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            playerProjectile = PlayerProjectile.Fireball;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            playerProjectile = PlayerProjectile.Icespear;
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            playerProjectile = PlayerProjectile.Venombomb;
    }

    private void HandleShoot()
    {
        // Do not allow firing when cooldown has not been met.

        if (Input.GetButtonDown("Fire1") && CooldownMet())
        {
            timeSinceLastShoot = 0;
            
            anim.SetInteger("Attack", UnityEngine.Random.Range(1, 3));
            Invoke("AttackDelay", 0.15f);
            Invoke("ResetAttackAnim", 0.5f);
        }
    }

    void AttackDelay()
    {
        var shotPosition = CalculateShotPosition(out var mouseWorldPosition);
        Vector3 aimDirection = (mouseWorldPosition - shotPosition).normalized;
        Quaternion aimRotation = Quaternion.LookRotation(aimDirection, Vector3.up);

        GameObject projectile = Instantiate(GetProjectilePrefab(),
                shotPosition, aimRotation);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        rb.AddForce(aimDirection * projectileSpeed, ForceMode.VelocityChange);

        projectile.transform.SetParent(
                GameObject.FindGameObjectWithTag("ProjectileParent").transform);
    }

    void ResetAttackAnim()
    {
        anim.SetInteger("Attack", 0);
    }

    private bool CooldownMet()
    {
        switch (playerProjectile)
        {
            case PlayerProjectile.Fireball:
                return timeSinceLastShoot >= fireballCooldown;
            case PlayerProjectile.Icespear:
                return timeSinceLastShoot >= icespearCooldown;
            case PlayerProjectile.Venombomb:
                return timeSinceLastShoot >= venomBombCooldown;
            default:
                throw new Exception("Unsupported weapon projectile used.");
        }
    }

    private Vector3 CalculateShotPosition(out Vector3 mouseWorldPosition)
    {
        Vector3 shotPosition = transform.position + Camera.main.transform.forward + new Vector3(0, 1.5f, 0);

        mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity))
        {
            mouseWorldPosition = raycastHit.point;
        }
        else
        {
            mouseWorldPosition = Camera.main.transform.position + Camera.main.transform.forward * 20;
        }

        return shotPosition;
    }

    private GameObject GetProjectilePrefab()
    {
        switch (playerProjectile)
        {
            case PlayerProjectile.Fireball:
                return fireballPrefab;
            case PlayerProjectile.Icespear:
                return icespearPrefab;
            case PlayerProjectile.Venombomb:
                return venomBombPrefab;
            default:
                throw new Exception("Unsupported weapon projectile used.");
        }
    }
}
